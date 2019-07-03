using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal static class ReorderableBlobList
    {
        internal static (List<T> list, int selected, T clicked, bool reordered) Draw<T>(List<T> list, int selected, int maxRowWidth, Func<T, GUIContent> blobContentFunc, Func<T, Color> getColorFunc, Func<T, GUIStyle> getStyleFunc, Func<T, GUIStyle> getSelectedStyleFunc)
        {
            (List<T> list, int selected, T clicked, bool reordered) result = (list, selected, default, false);

            if (list == null || list.Count == 0)
                return result;

            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = (ReorderableBlobListState<T>)GUIUtility.GetStateObject(typeof(ReorderableBlobListState<T>), controlId);

            var renderedList = state.IsDragging ? state.TempList : list;

            var blobIndex = 0;
            var currentLine = new BlobLine();
            while (blobIndex < renderedList.Count)
            {
                var element = renderedList[blobIndex++];
                var style = default(GUIStyle);
                //if (state.IsDragging)
                //    style = state.CachedStyleList[blobIndex - 1];
                //else
                    style = (state.IsDragging ? state.CachedSelectedList[blobIndex - 1] : result.selected == blobIndex - 1) ? getSelectedStyleFunc(element) : getStyleFunc(element);
                var content = blobContentFunc(element);
                var color = getColorFunc(element);

                if (currentLine.State == BlobState.DidntStarted)
                {
                    GUILayout.BeginHorizontal();
                    currentLine.State = BlobState.Started;
                }
                
                var blobSize = style.CalcSize(content);
                var estimatedLinePosition = Mathf.CeilToInt(currentLine.LinePosition + blobSize.x);
                if (estimatedLinePosition > maxRowWidth && currentLine.BlobsCount > 0) //if BlobsCount == 0 and we already exceeded max width, we must draw anyway to avoid infinite loop
                {
                    currentLine.State = BlobState.Populated;
                    blobIndex--; //rewinding index since we didn't draw that element yet
                }

                if (currentLine.State != BlobState.Populated)
                {
                    currentLine.BlobsCount++;
                    currentLine.LinePosition = estimatedLinePosition;
                    var blobControlId = GUIUtility.GetControlID(FocusType.Passive);
                    var position = GUILayoutUtility.GetRect(content, style);

                    switch (Event.current.type)
                    {
                        case EventType.Repaint:
                            if (!(state.IsDragging && state.DraggedIndex == blobIndex - 1))
                                drawBlob(color, content, style, position, blobControlId);
                            break;
                        case EventType.MouseDown:
                            if (Event.current.button == 0 && position.Contains(Event.current.mousePosition))
                            {
                                GUIUtility.hotControl = blobControlId;

                                state.DraggedIndex = blobIndex - 1;
                                state.IsDragging = true;
                                state.Content = content;
                                state.Style = new GUIStyle(style);
                                state.Color = color;
                                state.Position = position;

                                //state.CachedStyleList = new List<GUIStyle>();
                                state.CachedSelectedList = new List<bool>();
                                state.TempList = new List<T>();
                                for (int i = 0; i < list.Count; i++)
                                {
                                    var tempElement = list[i];
                                    var tempStyle = result.selected == i ? getSelectedStyleFunc(tempElement) : getStyleFunc(tempElement);
                                    state.TempList.Add(tempElement);
                                    //state.CachedStyleList.Add(new GUIStyle(tempStyle));
                                    state.CachedSelectedList.Add(result.selected == i);
                                }
                            }
                            break;
                        case EventType.MouseLeaveWindow:
                        case EventType.DragExited:
                        case EventType.MouseUp:
                            if (GUIUtility.hotControl == blobControlId)
                            {
                                if (position.Contains(Event.current.mousePosition))
                                {
                                    result.clicked = element;
                                    result.selected = blobIndex - 1;
                                }
                                else if (state.IsDragging)
                                {
                                    result.clicked = state.TempList[state.DraggedIndex];
                                    //result.selected = state.DraggedIndex;
                                    result.selected = state.CachedSelectedList.IndexOf(true);
                                }

                                if (state.IsDragging)
                                {
                                    result.reordered = state.TempList == null ? false : isReordered(result.list, state.TempList);
                                    result.list = state.TempList;
                                }
                                state.Purge();
                                GUI.FocusControl(null);
                                Event.current.Use();
                                GUIUtility.hotControl = 0;
                            }
                            GUI.changed = true;
                            break;
                    }
                    if (Event.current.isMouse)
                    {
                        if (state.IsDragging && position.Contains(Event.current.mousePosition))
                        {
                            if (state.DraggedIndex != blobIndex - 1)
                            {
                                var draggedSelected = state.CachedSelectedList[state.DraggedIndex];
                                state.CachedSelectedList.RemoveAt(state.DraggedIndex);

                                var draggedEntry = renderedList[state.DraggedIndex];
                                renderedList.RemoveAt(state.DraggedIndex);
                                state.DraggedIndex = blobIndex - 1;
                                renderedList.Insert(state.DraggedIndex, draggedEntry);

                                state.CachedSelectedList.Insert(state.DraggedIndex, draggedSelected);
                            }
                        }
                        if (GUIUtility.hotControl == blobControlId)
                        {
                            state.DraggingPosition = Event.current.mousePosition - state.Position.size / 2f;

                            GUI.changed = true;
                        }
                    }
                }
                
                if (currentLine.State == BlobState.Populated || blobIndex == renderedList.Count)
                {
                    GUILayout.EndHorizontal();
                    currentLine = new BlobLine();
                }
            }

            if (state.IsDragging && Event.current.type == EventType.Repaint)
            {
                var draggedControlId = GUIUtility.GetControlID(FocusType.Passive);
                var draggedPosition = new Rect(state.DraggingPosition, state.Position.size);
                drawBlob(state.Color, state.Content, state.Style, draggedPosition, draggedControlId);
            }

            return result;
        }

        private static bool isReordered<T>(List<T> original, List<T> possiblyReordered)
        {
            for (int i = 0; i < original.Count; i++)
            {
                if (!original[i].Equals(possiblyReordered[i]))
                    return true;
            }
            return false;
        }

        private static void drawBlob(Color color, GUIContent content, GUIStyle style, Rect position, int blobControlId)
        {
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            style.Draw(position, content, blobControlId);
            GUI.backgroundColor = originalColor;
        }
    }
}

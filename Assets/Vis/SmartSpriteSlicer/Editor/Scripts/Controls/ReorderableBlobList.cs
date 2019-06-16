using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal static class ReorderableBlobList
    {
        internal static List<T> Draw<T>(List<T> list, int maxRowWidth, Func<T, GUIContent> blobContentFunc, Func<T, Color> getColorFunc, GUIStyle blobsStyle)
        {
            var result = list;
            //var controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (list == null || list.Count == 0)
                return result;

            var blobIndex = 0;
            var currentLine = new BlobLine();
            while (blobIndex < list.Count)
            {
                var element = list[blobIndex++];
                var content = blobContentFunc(element);
                var color = getColorFunc(element);

                if (currentLine.State == BlobState.DidntStarted)
                {
                    GUILayout.BeginHorizontal();
                    currentLine.State = BlobState.Started;
                }
                
                var blobSize = blobsStyle.CalcSize(content);
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
                    var position = GUILayoutUtility.GetRect(content, blobsStyle);

                    switch (Event.current.type)
                    {
                        case EventType.Repaint:
                            var originalColor = GUI.backgroundColor;
                            GUI.backgroundColor = color;
                            blobsStyle.Draw(position, content, blobControlId);
                            GUI.backgroundColor = originalColor;
                            break;
                    }
                }

                if (currentLine.State == BlobState.Populated || blobIndex == list.Count)
                {
                    GUILayout.EndHorizontal();
                    currentLine = new BlobLine();
                }
            }

            return result;
        }
    }
}

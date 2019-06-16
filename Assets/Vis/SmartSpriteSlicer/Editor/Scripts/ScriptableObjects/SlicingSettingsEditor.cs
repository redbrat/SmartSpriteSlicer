using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    [CustomEditor(typeof(SlicingSettings))]
    public class SlicingSettingsEditor : Editor
    {
        private static Dictionary<object, int> _currentEditedChunks = new Dictionary<object, int>();
        private static Dictionary<object, int> _currentFocusedControl = new Dictionary<object, int>();
        private static int _controlPtr;

        private GUISkin _skin;
        private GUIStyle _chunksPanelStyle;
        private GUIStyle _chunkEditPanelStyle;

        public override void OnInspectorGUI()
        {
            if (_skin == null)
                _skin = SmartSpriteSlicerWindow.loadGuiSkin();
            if (_chunksPanelStyle == null)
                _chunksPanelStyle = _skin.GetStyle(ChunksView.ChunksPanelStyleName);
            if (_chunkEditPanelStyle == null)
                _chunkEditPanelStyle = _skin.GetStyle(ChunksView.ChunkEditPanelStyleName);

            RenderSlicingSettingsGUI(this, target as SlicingSettings, _chunksPanelStyle, _chunkEditPanelStyle);
        }

        public static void RenderSlicingSettingsGUI(object sender, SlicingSettings target, GUIStyle chunksPanelStyle, GUIStyle chunkEditPanelStyle)
        {
            if (!_currentEditedChunks.ContainsKey(sender))
                _currentEditedChunks.Add(sender, default);

            var chunks = target.Chunks;

            EditorGUILayout.LabelField($"Chunks");
            EditorGUILayout.BeginHorizontal(chunksPanelStyle);
            for (int i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                var originalColor = GUI.backgroundColor;
                GUI.backgroundColor = chunk._color;
                if (GUILayout.Button($"{chunk._size.x}x{chunk._size.y}"))
                {
                    _currentEditedChunks[sender] = chunk.Id;
                    Debug.Log($"Choosed: {chunk.Id}");
                }
                GUI.backgroundColor = originalColor;
            }
            if (GUILayout.Button(new GUIContent("+", "Create new chunk"), GUILayout.Width(34f)))
            {
                var defaultSize = Vector2Int.one * 64;
                if (chunks.Count > 0)
                    defaultSize = chunks[chunks.Count - 1]._size;
                chunks.Add(new SpriteChunk(chunks.Count, defaultSize));
            }
            EditorGUILayout.EndHorizontal();

            var targetChunkIndex = chunks.FindIndex(c => c.Id == _currentEditedChunks[sender]);
            if (targetChunkIndex >= 0)
            {
                var chunk = chunks[targetChunkIndex];
                EditorGUILayout.BeginVertical(chunkEditPanelStyle);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Width:"));
                GUI.SetNextControlName("Width");
                var newWidth = EditorGUILayout.IntField(chunk.Size.x);
                var id = GUIUtility.GetControlID(FocusType.Keyboard);
                var te = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), id - 1);
                if (te != null)
                {
                    Debug.Log($"te.selectIndex = {te.text}");
                }
                if (!_currentFocusedControl.ContainsKey(sender))
                    _currentFocusedControl.Add(sender, id);
                else
                {
                    if (_currentFocusedControl[sender] != id && _controlPtr == _currentFocusedControl[sender])//Layout changed
                        GUI.FocusControl("Width");
                    _currentFocusedControl[sender] = id;
                }
                if (newWidth != chunk.Size.x)
                {
                    _controlPtr = id;
                    Undo.RecordObject(target, "Chunk width changed");
                    chunks[targetChunkIndex] = chunk.SetSize(new Vector2Int(newWidth, chunk.Size.y));
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUI.SetNextControlName("Height");
                EditorGUILayout.LabelField(new GUIContent("Height:"));
                var newHeight = EditorGUILayout.IntField(chunk.Size.y);
                if (newHeight != chunk.Size.y)
                {
                    Undo.RecordObject(target, "Chunk height changed");
                    chunks[targetChunkIndex] = chunk.SetSize(new Vector2Int(chunk.Size.x, newHeight));
                }
                EditorGUILayout.EndHorizontal();

                //var newSize = EditorGUILayout.Vector2IntField(new GUIContent("Size:"), chunk._size);
                ////var control = GetLastControlId();
                ////GUIUtility.hotControl = control;
                //if (!_currentFocusedControl.ContainsKey(sender))
                //    _currentFocusedControl[sender] = GetLastControlId();


                //if (newSize != chunk._size)
                //{
                //    Debug.Log("Focused: " + GUI.Control());
                //    Undo.RecordObject(target, "Chunk size changed");
                //    chunks[targetChunkIndex] = chunk.SetSize(newSize);
                //}
                var newColor = EditorGUILayout.ColorField(new GUIContent("Color:"), chunk._color);
                if (newColor != chunk._color)
                {
                    Undo.RecordObject(target, "Chunk color changed");
                    chunks[targetChunkIndex] = chunk.SetColor(newColor);
                }

                if (GUILayout.Button(new GUIContent($"Delete", "Remove chunk and all groups containing it")))
                {
                    chunks.RemoveAt(targetChunkIndex);
                }
                EditorGUILayout.EndVertical();
            }

            if (GUILayout.Button("Set Focus"))
                GUI.FocusControl("Size");
            //var isPossibleFocusChange = Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp;
            //if (!isPossibleFocusChange && GUIUtility.hotControl != _currentFocusedControl[sender])
            //{
            //    GUIUtility.hotControl = _currentFocusedControl[sender];
            //    Event.current.Use();
            //}
        }

        public static int GetLastControlId()
        {
            var getLastControlId = typeof(EditorGUIUtility).GetField("s_LastControlID", BindingFlags.Static | BindingFlags.NonPublic);
            if (getLastControlId != null)
                return (int)getLastControlId.GetValue(null);
            return 0;
        }
    }
}

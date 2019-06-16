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
        private const int _maxButtonsPerRow = 4;
        private static Dictionary<object, int> _currentEditedChunks = new Dictionary<object, int>();

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
            EditorGUILayout.BeginVertical(chunksPanelStyle);
            var buttonsCount = chunks.Count + 1;
            var currentButtonIndex = 0;
            while(currentButtonIndex < buttonsCount)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < _maxButtonsPerRow; i++)
                {
                    if (currentButtonIndex == buttonsCount - 1)
                    {
                        if (GUILayout.Button(new GUIContent("+", "Create new chunk"), GUILayout.Width(34f)))
                        {
                            var defaultSize = Vector2Int.one * 64;
                            if (chunks.Count > 0)
                                defaultSize = chunks[chunks.Count - 1]._size;
                            chunks.Add(new SpriteChunk(chunks.Count, defaultSize));
                        }
                        currentButtonIndex++;
                        i = _maxButtonsPerRow;
                    }
                    else
                    {
                        var chunk = chunks[currentButtonIndex++];
                        var originalColor = GUI.backgroundColor;
                        GUI.backgroundColor = chunk._color;
                        if (GUILayout.Button($"{chunk._size.x}x{chunk._size.y}", GUILayout.MinWidth(80f)))
                            _currentEditedChunks[sender] = chunk.Id;
                        GUI.backgroundColor = originalColor;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            //EditorGUILayout.BeginHorizontal();
            //for (int i = 0; i < chunks.Count; i++)
            //{
            //    var chunk = chunks[i];
            //    var originalColor = GUI.backgroundColor;
            //    GUI.backgroundColor = chunk._color;
            //    if (GUILayout.Button($"{chunk._size.x}x{chunk._size.y}", GUILayout.MinWidth(80f)))
            //        _currentEditedChunks[sender] = chunk.Id;
            //    GUI.backgroundColor = originalColor;
            //}
            //if (GUILayout.Button(new GUIContent("+", "Create new chunk"), GUILayout.Width(34f)))
            //{
            //    var defaultSize = Vector2Int.one * 64;
            //    if (chunks.Count > 0)
            //        defaultSize = chunks[chunks.Count - 1]._size;
            //    chunks.Add(new SpriteChunk(chunks.Count, defaultSize));
            //}
            //EditorGUILayout.EndHorizontal();

            var targetChunkIndex = chunks.FindIndex(c => c.Id == _currentEditedChunks[sender]);
            if (targetChunkIndex >= 0)
            {
                var chunk = chunks[targetChunkIndex];
                EditorGUILayout.BeginVertical(chunkEditPanelStyle);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Width:"));
                var newWidth = EditorGUILayout.IntField(chunk.Size.x);
                if (newWidth != chunk.Size.x)
                {
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

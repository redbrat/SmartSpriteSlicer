using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal class ChunksView : LayoutViewBase
    {
        private const int _maxButtonsPerRow = 4;

        internal const string ChunksPanelStyleName = "ChunksPanel";
        internal const string ChunkEditPanelStyleName = "ChunkEditPanel";
        internal const string ChunkButtonStyleName = "ChunkButton";
        internal const string ChunkButtonPressedStyleName = "ChunkButtonPressed";

        private readonly GUIStyle _chunksPanelStyle;
        private readonly GUIStyle _chunkEditPanelStyle;
        private readonly GUIStyle _chunkButtonStyle;
        private readonly GUIStyle _chunkButtonPressedStyle;

        public ChunksView(SpriteEditorProWindow model) : base(model)
        {
            _chunksPanelStyle = _model.Skin.GetStyle(ChunksPanelStyleName);
            _chunkEditPanelStyle = _model.Skin.GetStyle(ChunkEditPanelStyleName);
            _chunkButtonStyle = _model.Skin.GetStyle(ChunkButtonStyleName);
            _chunkButtonPressedStyle = _model.Skin.GetStyle(ChunkButtonPressedStyleName);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var chunks = _model.SlicingSettings.Chunks;

            EditorGUILayout.LabelField(new GUIContent($"<b>Chunks</b>", "Here you can edit chunks"), _model.RichTextStyle);
            EditorGUILayout.BeginVertical(_chunksPanelStyle);
            var buttonsCount = chunks.Count + 1;
            var currentButtonIndex = 0;
            while (currentButtonIndex < buttonsCount)
            {
                EditorGUILayout.BeginHorizontal();
                for (int i = 0; i < _maxButtonsPerRow; i++)
                {
                    if (currentButtonIndex == buttonsCount - 1)
                    {
                        if (DragableButton.Draw(new GUIContent("<color=#000000>+</color>", "Create new chunk"), _chunkButtonStyle, false, GUILayout.Width(34f)) == DraggableButtonResult.Clicked)
                        {
                            var defaultSize = Vector2Int.one * 64;
                            if (chunks.Count > 0)
                                defaultSize = chunks[chunks.Count - 1].Size;
                            chunks.Add(new SpriteChunk(_model.SlicingSettings.Chunks.Count == 0 ? 1 : _model.SlicingSettings.Chunks.OrderByDescending(c => c.Id).First().Id + 1, defaultSize));
                        }
                        currentButtonIndex++;
                        i = _maxButtonsPerRow;
                    }
                    else
                    {
                        var chunk = chunks[currentButtonIndex++];
                        var originalColor = GUI.backgroundColor;
                        GUI.backgroundColor = chunk.Color;
                        GUI.SetNextControlName($"Chunk_{currentButtonIndex - 1}");
                        _chunkButtonStyle.normal.textColor = chunk.TextColor;
                        _chunkButtonPressedStyle.normal.textColor = chunk.TextColor;
                        var draggableButtonResult = DragableButton.Draw(new GUIContent(chunk.GetHumanFriendlyName()), _model.EditedChunkId == chunk.Id ? _chunkButtonPressedStyle : _chunkButtonStyle, true, GUILayout.MinWidth(80f));
                        switch (draggableButtonResult)
                        {
                            case DraggableButtonResult.None:
                                break;
                            case DraggableButtonResult.Clicked:
                                if (_model.EditedChunkId == chunk.Id)
                                    _model.EditedChunkId = default;
                                else
                                    _model.EditedChunkId = chunk.Id;
                                GUI.FocusControl(default);
                                break;
                            case DraggableButtonResult.Droped:
                                var newGroupId = 1;
                                if (_model.SlicingSettings.ChunkGroups.Count > 0)
                                    newGroupId = _model.SlicingSettings.ChunkGroups.OrderByDescending(c => c.Id).First().Id + 1;
                                Undo.RecordObject(_model.SlicingSettings, "Group added");
                                _model.SlicingSettings.ChunkGroups.Add(new SpriteGroup(newGroupId, chunk.Id));
                                _model.Repaint();
                                EditorUtility.SetDirty(_model.SlicingSettings);
                                break;
                            default:
                                break;
                        }
                        GUI.backgroundColor = originalColor;
                    }

                    if (buttonsCount == 1)
                        EditorGUILayout.LabelField(new GUIContent($"<i><color=#000000>Create some chunks with \"+\" button.</color></i>"), _model.RichTextStyle);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            var targetChunkIndex = chunks.FindIndex(c => c.Id == _model.EditedChunkId);
            if (targetChunkIndex >= 0)
            {
                var chunk = chunks[targetChunkIndex];
                EditorGUILayout.BeginVertical(_chunkEditPanelStyle);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Width:"));
                var newWidth = EditorGUILayout.IntField(chunk.Size.x);
                if (newWidth != chunk.Size.x)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk width changed");
                    chunks[targetChunkIndex] = chunk.SetSize(new Vector2Int(newWidth, chunk.Size.y));
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUI.SetNextControlName("Height");
                EditorGUILayout.LabelField(new GUIContent("Height:"));
                var newHeight = EditorGUILayout.IntField(chunk.Size.y);
                if (newHeight != chunk.Size.y)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk height changed");
                    chunks[targetChunkIndex] = chunk.SetSize(new Vector2Int(chunk.Size.x, newHeight));
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                EditorGUILayout.EndHorizontal();

                var newName = EditorGUILayout.TextField(new GUIContent($"Name:"), chunk.Name);
                if (newName != chunk.Name)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk name changed");
                    chunks[targetChunkIndex] = chunk.SetName(newName);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                var newColor = EditorGUILayout.ColorField(new GUIContent("Color:"), chunk.Color);
                if (newColor != chunk.Color)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk color changed");
                    chunks[targetChunkIndex] = chunk.SetColor(newColor);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                var newTextColor = EditorGUILayout.ColorField(new GUIContent("Text Color:"), chunk.TextColor);
                if (newTextColor != chunk.TextColor)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk's text color changed");
                    chunks[targetChunkIndex] = chunk.SetTextColor(newTextColor);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                if (GUILayout.Button(new GUIContent($"Delete", "Remove chunk and all groups that contain it")) &&
                    (!_model.SlicingSettings.ChunkGroups.Where(g => g.ChunkId == chunks[targetChunkIndex].Id).Any() || EditorUtility.DisplayDialog("Warning!", "This action will delete all groups containing that chunk. Are you sure you want to delete it?", "Yes", "No")))
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk deleted");
                    var deletedChunk = chunks[targetChunkIndex];
                    for (int i = 0; i < _model.SlicingSettings.ChunkGroups.Count; i++)
                    {
                        var group = _model.SlicingSettings.ChunkGroups[i];
                        if (group.ChunkId == deletedChunk.Id)
                            _model.SlicingSettings.ChunkGroups.RemoveAt(i--);
                    }
                    chunks.RemoveAt(targetChunkIndex);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                EditorGUILayout.EndVertical();
            }
        }
    }
}
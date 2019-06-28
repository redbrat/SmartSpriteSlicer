using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ScriptableSlicingBlobsView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _panelDragAcceptanceStyle;
        private readonly GUIStyle _blobStyle;
        private readonly GUIStyle _selectedBlobStyle;

        public ScriptableSlicingBlobsView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsMainPanel");
            _panelDragAcceptanceStyle = model.Skin.GetStyle("GroupsMainPanelDragAcceptence");
            _blobStyle = model.Skin.GetStyle("BlobStyle");
            _selectedBlobStyle = model.Skin.GetStyle("SelectedBlobStyle");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginVertical(DragableButton.IsDragging && DragableButton.ReadyToDrop ? _panelDragAcceptanceStyle : _panelStyle);
            if (_model.SlicingSettings.ChunkGroups.Count == 0)
                EditorGUILayout.LabelField(new GUIContent($"<i><color=#888888>No nodes found added.</color></i>"), _model.RichTextStyle);
            else
            {
                var reorderableListResult = ReorderableBlobList.Draw(_model.SlicingSettings.ScriptableNodes, _model.SelectedNodeIndex, (int)WindowWidth - 30, getBlobContent, getBlobColor, getBlobStyle, getSelectedBlobStyle);
                if (reorderableListResult.reordered)
                    Undo.RecordObject(_model.SlicingSettings, $"Scriptable nodes reordered");
                _model.SlicingSettings.ScriptableNodes = reorderableListResult.list;
                if (reorderableListResult.reordered)
                {
                    if (_model.SelectedNodeIndex >= 0)
                        _model.SelectedNodeIndex = reorderableListResult.selected;
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                else
                    _model.SelectedNodeIndex = reorderableListResult.selected;
                if (reorderableListResult.clicked.Id != 0 && !reorderableListResult.reordered)
                    onGroupClick(reorderableListResult.clicked);
            }
            EditorGUILayout.EndVertical();
        }

        private Color getBlobColor(ScriptableNode node)
        {
            switch (node.Type)
            {
                case ScriptableNodeType.Text:
                    return new Color(0, 1, 1, 0.5f);
                default:
                    return Color.white * 0.95f;
            }
        }

        private GUIContent getBlobContent(ScriptableNode node)
        {
            switch (node.Type)
            {
                case ScriptableNodeType.Text:
                    if (string.IsNullOrEmpty(node.Pattern))
                        return new GUIContent($"<color=#000000>Text: <i>None</i></color>");
                    else
                        return new GUIContent($"<color=#000000>Text: {node.Pattern}</color>");
                case ScriptableNodeType.EndOfLine:
                    return new GUIContent($"<color=#000000><i>End of line</i></color>");
                case ScriptableNodeType.Name:
                    return new GUIContent($"<color=#000000><b>Name</b></color>");
                case ScriptableNodeType.Group:
                    return new GUIContent($"<color=#000000><b>Group</b></color>");
                case ScriptableNodeType.X:
                    return new GUIContent($"<color=#000000><b>X</b></color>");
                case ScriptableNodeType.Y:
                    return new GUIContent($"<color=#000000><b>Y</b></color>");
                case ScriptableNodeType.Width:
                    return new GUIContent($"<color=#000000><b>Width</b></color>");
                case ScriptableNodeType.Height:
                    return new GUIContent($"<color=#000000><b>Height</b></color>");
                case ScriptableNodeType.PivotX:
                    return new GUIContent($"<color=#000000><b>Pivot X</b></color>");
                case ScriptableNodeType.PivotY:
                    return new GUIContent($"<color=#000000><b>Pivot Y</b></color>");
                default:
                    throw new ApplicationException($"Unknown node type: {node.Type}");
            }
        }

        private GUIStyle getSelectedBlobStyle(ScriptableNode node) => formatStyle(_selectedBlobStyle, node);
        private GUIStyle getBlobStyle(ScriptableNode node) => formatStyle(_blobStyle, node);
        private GUIStyle formatStyle(GUIStyle style, ScriptableNode node)
        {
            switch (node.Type)
            {
                case ScriptableNodeType.Text:
                case ScriptableNodeType.EndOfLine:
                    style.normal.textColor = node.TextColor;
                    break;
                default:
                    style.normal.textColor = Color.white * 0.25f;
                    break;
            }
            return style;
        }

        private void onGroupClick(ScriptableNode node)
        {
            if (_model.EditedNodeId == node.Id)
            {
                _model.EditedNodeId = 0;
                _model.SelectedNodeIndex = -1;
            }
            else
                _model.EditedNodeId = node.Id;
        }

        private GUIContent getBlobContent(SpriteChunk chunk) => new GUIContent($"{chunk.GetHumanFriendlyName()}");
    }
}
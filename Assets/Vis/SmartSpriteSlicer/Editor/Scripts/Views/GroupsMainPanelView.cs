using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsMainPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _panelDragAcceptanceStyle;
        private readonly GUIStyle _blobStyle;

        public GroupsMainPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsMainPanel");
            _panelDragAcceptanceStyle = model.Skin.GetStyle("GroupsMainPanelDragAcceptence");
            _blobStyle = model.Skin.GetStyle("BlobStyle");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginVertical(DragableButton.IsDragging && DragableButton.ReadyToDrop ? _panelDragAcceptanceStyle : _panelStyle);
            if (_model.SlicingSettings.ChunkGroups.Count == 0)
            {
                if (_model.SlicingSettings.Chunks.Count == 0)
                    EditorGUILayout.LabelField(new GUIContent($"<i><color=#888888>Groups panel empty.</color></i>"), _model.RichTextStyle);
                else
                    EditorGUILayout.LabelField(new GUIContent($"<i><color=#888888>Drag'n'Drop some chunks here to create a group.</color></i>"), _model.RichTextStyle);
            }
            else
                _model.SlicingSettings.ChunkGroups = ReorderableBlobList.Draw(_model.SlicingSettings.ChunkGroups, SmartSpriteSlicerWindow.MaxContolPanelWidth - 30, group => getBlobContent(_model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First()), group => _model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First().Color, onGroupClick, _blobStyle);
            EditorGUILayout.EndVertical();
        }

        private void onGroupClick(SpriteGroup group)
        {
            if (GroupsView.EditedGroupId == group.Id)
                GroupsView.EditedGroupId = 0;
            else
                GroupsView.EditedGroupId = group.Id;
        }

        private GUIContent getBlobContent(SpriteChunk chunk) => new GUIContent($"{chunk.GetHumanFriendlyName()}");
    }
}
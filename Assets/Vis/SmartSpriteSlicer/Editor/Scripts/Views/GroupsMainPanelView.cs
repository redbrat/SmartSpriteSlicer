﻿using System.Collections.Generic;
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
                EditorGUILayout.LabelField(new GUIContent($"Start by dragging some chunks here..."));
            else
            {
                _model.SlicingSettings.ChunkGroups = ReorderableBlobList.Draw(_model.SlicingSettings.ChunkGroups, 300, group => getBlobContent(_model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First()), group => _model.SlicingSettings.Chunks.Where(chunk => chunk.Id == group.ChunkId).First().Color, _blobStyle);
            }
            EditorGUILayout.EndVertical();
        }

        private GUIContent getBlobContent(SpriteChunk chunk)
        {
            return new GUIContent($"{chunk.Size.x}x{chunk.Size.y}");
        }
    }
}
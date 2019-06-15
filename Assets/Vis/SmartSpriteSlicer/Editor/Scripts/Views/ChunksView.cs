using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ChunksView : LayoutViewBase
    {
        private readonly GUIStyle _chunksPanelStyle;
        private readonly GUIStyle _chunkEditPanelStyle;

        private Guid _currentEditedChunk;

        public ChunksView(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksPanelStyle = _model.Skin.GetStyle("ChunksPanel");
            _chunkEditPanelStyle = _model.Skin.GetStyle("ChunkEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var chunks = _model.SlicingSettings.Chunks;

            EditorGUILayout.LabelField($"Chunks");
            EditorGUILayout.BeginHorizontal(_chunksPanelStyle);
            for (int i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                if (GUILayout.Button($"{chunk.Size.x}x{chunk.Size.y}"))
                    _currentEditedChunk = chunk.Id;
            }
            EditorGUILayout.EndHorizontal();

            var targetChunkIndex = chunks.FindIndex(c => c.Id == _currentEditedChunk);
            if (targetChunkIndex >= 0)
            {
                var chunk = chunks[targetChunkIndex];
                EditorGUILayout.BeginHorizontal(_chunkEditPanelStyle);
                var newSize = EditorGUILayout.Vector2IntField(new GUIContent("Size:"), chunk.Size);
                if (newSize != chunk.Size)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Chunk size changed");
                    chunks[targetChunkIndex] = chunk.SetSize(newSize);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
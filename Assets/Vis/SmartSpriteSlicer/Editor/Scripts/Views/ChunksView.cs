using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ChunksView : LayoutViewBase
    {
        internal const string ChunksPanelStyleName = "ChunksPanel";
        internal const string ChunkEditPanelStyleName = "ChunkEditPanel";
        internal const string ChunkButtonStyleName = "ChunkButton";

        private readonly GUIStyle _chunksPanelStyle;
        private readonly GUIStyle _chunkEditPanelStyle;
        private readonly GUIStyle _chunkButtonStyle;

        public ChunksView(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksPanelStyle = _model.Skin.GetStyle(ChunksPanelStyleName);
            _chunkEditPanelStyle = _model.Skin.GetStyle(ChunkEditPanelStyleName);
            _chunkButtonStyle = _model.Skin.GetStyle(ChunkButtonStyleName);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            SlicingSettingsEditor.RenderSlicingSettingsGUI(this, _model.SlicingSettings, _chunksPanelStyle, _chunkEditPanelStyle, _chunkButtonStyle);
        }
    }
}
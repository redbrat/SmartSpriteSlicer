using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ChunksView : LayoutViewBase
    {
        internal const string ChunksPanelStyleName = "ChunksPanel";
        internal const string ChunkEditPanelStyleName = "ChunkEditPanel";

        private readonly GUIStyle _chunksPanelStyle;
        private readonly GUIStyle _chunkEditPanelStyle;

        public ChunksView(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksPanelStyle = _model.Skin.GetStyle(ChunksPanelStyleName);
            _chunkEditPanelStyle = _model.Skin.GetStyle(ChunkEditPanelStyleName);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            SlicingSettingsEditor.RenderSlicingSettingsGUI(this, _model.SlicingSettings, _chunksPanelStyle, _chunkEditPanelStyle);
        }
    }
}
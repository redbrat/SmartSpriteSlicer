namespace Vis.SpriteEditorPro
{
    internal class ScriptableSlicingPreviewView : LayoutViewBase
    {
        private readonly LayoutViewBase _top;
        private readonly LayoutViewBase _center;
        private readonly LayoutViewBase _bottom;

        public ScriptableSlicingPreviewView(SpriteEditorProWindow model) : base(model)
        {
            _top = new ScriptableSlicingPreviewTopView(model);
            _center = new ScriptableSlicingPreviewCenterView(model);
            _bottom = new ScriptableSlicingPreviewBottomView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _top.OnGUILayout();
            _center.OnGUILayout();
            if (!string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText) &&
                _model.SlicingSettings.HasWholeSetOfNodes() &&
                _model.SlicingSettings.HasAllNodesSeparated())
                _bottom.OnGUILayout();
        }
    }
}
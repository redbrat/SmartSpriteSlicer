namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private LayoutViewBase _topPanelView;
        private LayoutViewBase _chunksView;
        private LayoutViewBase _groupsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _topPanelView = new TopPanelView(model);
            _chunksView = new ChunksView(model);
            _groupsView = new GroupsView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _topPanelView.OnGUILayout();
            _chunksView.OnGUILayout();
            _groupsView.OnGUILayout();
        }
    }
}
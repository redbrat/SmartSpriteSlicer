using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelView : ViewBase
    {
        private GUIStyle _backgroundStyle;

        private ControlPanelWindow _subWindow;

        public ControlPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _backgroundStyle = model.Skin.GetStyle("ControlPanel");
            _subWindow = new ControlPanelWindow(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            _model.BeginWindows();
            _model.ControlPanelRect = GUILayout.Window(0, _model.ControlPanelRect, _subWindow.WindowContentCallback, new GUIContent("Layout Settings"));
            if (_model.ControlPanelRect.x < 0)
                _model.ControlPanelRect.x = 0;
            if (_model.ControlPanelRect.y < 0)
                _model.ControlPanelRect.y = 0;
            if (_model.ControlPanelRect.width != SmartSpriteSlicerWindow.MaxConhtolPanelWidth)
                _model.ControlPanelRect.width = SmartSpriteSlicerWindow.MaxConhtolPanelWidth;
            _model.EndWindows();
        }
    }
}

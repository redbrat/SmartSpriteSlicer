using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class PreviewSpriteView : ViewBase
    {
        private readonly GUIStyle _backgroundStyle;
        private readonly SpritePreviewWindow _subWindow;

        public PreviewSpriteView(SmartSpriteSlicerWindow model) : base(model)
        {
            _backgroundStyle = model.Skin.GetStyle("PreviewSprite");
            _subWindow = new SpritePreviewWindow(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            //var use = false;
            //if (Event.current.type == EventType.MouseUp && _model.PreviewWindowRect.Contains(Event.current.mousePosition))
            //    use = true;

            if (_model.PreviewedArea == null)
                return;

            //_model.BeginWindows();
            _subWindow.WindowPosition = _model.PreviewWindowRect;
            _model.PreviewWindowRect = GUILayout.Window(1154, _model.PreviewWindowRect, _subWindow.WindowContentCallback, new GUIContent("Preview sprite"));
            if (_model.PreviewWindowRect.x < 0)
                _model.PreviewWindowRect.x = 0;
            if (_model.PreviewWindowRect.y < 0)
                _model.PreviewWindowRect.y = 0;
            if (_model.PreviewWindowRect.width != SmartSpriteSlicerWindow.MaxPreviewWindowRect)
                _model.PreviewWindowRect.width = SmartSpriteSlicerWindow.MaxPreviewWindowRect;
            var yMax = _model.PreviewWindowRect.height;
            _model.PreviewWindowRect.height = 0;
            //_model.EndWindows();

            var windowRect = _model.PreviewWindowRect;
            windowRect.yMax = yMax;
            if (Event.current.type == EventType.MouseUp && windowRect.Contains(Event.current.mousePosition))
                Event.current.Use();

            //if (use)
            //{
            //    Event.current.Use();
            //    Debug.Log($"USED!!!");
            //}
        }
    }
}
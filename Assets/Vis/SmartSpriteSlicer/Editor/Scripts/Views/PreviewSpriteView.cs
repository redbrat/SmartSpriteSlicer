using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class PreviewSpriteView : ViewBase
    {
        private readonly SpritePreviewWindow _subWindow;

        public PreviewSpriteView(SmartSpriteSlicerWindow model) : base(model)
        {
            _subWindow = new SpritePreviewWindow(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            if (_model.PreviewedArea == null)
                return;

            _subWindow.WindowPosition = _model.PreviewWindowRect;
            var futureSpriteName = _model.PreviewName;
            if (!_model.SlicingSettings.UseCustomSpriteName)
                futureSpriteName = $"{_model.Texture.name}{futureSpriteName}";
            _model.PreviewWindowRect = GUILayout.Window(1, _model.PreviewWindowRect, _subWindow.WindowContentCallback, new GUIContent($"Preview: {futureSpriteName}"));
            if (_model.PreviewWindowRect.x < 0)
                _model.PreviewWindowRect.x = 0;
            if (_model.PreviewWindowRect.y < 0)
                _model.PreviewWindowRect.y = 0;
            if (_model.PreviewWindowRect.width != SmartSpriteSlicerWindow.MaxPreviewWindowWidth)
                _model.PreviewWindowRect.width = SmartSpriteSlicerWindow.MaxPreviewWindowWidth;
            if (_model.PreviewWindowRect.x >= _model.position.width - _subWindow.WindowWorkaroundRect.width)
                _model.PreviewWindowRect.x = _model.position.width - _subWindow.WindowWorkaroundRect.width;
            if (_model.PreviewWindowRect.y >= _model.position.height - 16)
                _model.PreviewWindowRect.y = _model.position.height - 16;
            var yMax = _model.PreviewWindowRect.height;
            _model.PreviewWindowRect.height = 0;

            var windowRect = _model.PreviewWindowRect;
            windowRect.yMax = yMax;
            if (Event.current.type == EventType.MouseUp && windowRect.Contains(Event.current.mousePosition))
                Event.current.Use();
        }
    }
}
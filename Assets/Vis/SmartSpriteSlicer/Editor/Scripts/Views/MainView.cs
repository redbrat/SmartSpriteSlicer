using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class MainView : ViewBase
    {
        private readonly BackgroundView _background;
        private readonly TextureView _image;
        private readonly ControlPanelView _controlPanel;
        private readonly PreviewSpriteView _previewSpriteView;
        private readonly ForegroundView _foreground;

        public MainView(SmartSpriteSlicerWindow model) : base(model)
        {
            _background = new BackgroundView(model);
            _image = new TextureView(model);
            _controlPanel = new ControlPanelView(model);
            _previewSpriteView = new PreviewSpriteView(model);
            _foreground = new ForegroundView(model);
        }
        
        public override void OnGUI(Rect position)
        {
            var textureRatio = (float)_model.Texture.width / _model.Texture.height;
            var screenRatio = position.width / position.height;
            var fitX = 0f;
            var fitY = 0f;
            var fitWidth = 0f;
            var fitHeight = 0f;
            if (screenRatio < textureRatio)
            {
                fitWidth = position.width;
                fitHeight = fitWidth / textureRatio;
                fitX = 0;
                fitY = (position.height - fitHeight) / 2f;
            }
            else
            {
                fitHeight = position.height;
                fitWidth = fitHeight * textureRatio;
                fitY = 0;
                fitX = (position.width - fitWidth) / 2f;
            }
            _model.TextureRect = new Rect(Mathf.RoundToInt(fitX), Mathf.RoundToInt(fitY), Mathf.RoundToInt(fitWidth), Mathf.RoundToInt(fitHeight));

            _background.OnGUI(position);
            _image.OnGUI(position);

            _model.BeginWindows();
            _controlPanel.OnGUI(position);
            _previewSpriteView.OnGUI(position);
            _model.EndWindows();

            _foreground.OnGUI(position);
        }
    }
}

using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class MainView : ViewBase
    {
        private readonly BackgroundView _background;
        private readonly TextureView _image;
        private readonly ControlPanelView _controlPanel;
        private readonly PreviewSpriteView _previewSpriteView;

        public MainView(SmartSpriteSlicerWindow model) : base(model)
        {
            _background = new BackgroundView(model);
            _image = new TextureView(model);
            _controlPanel = new ControlPanelView(model);
            _previewSpriteView = new PreviewSpriteView(model);
        }
        
        public override void OnGUI(Rect position)
        {
            _background.OnGUI(position);
            _image.OnGUI(position);
            _controlPanel.OnGUI(position);
        }
    }
}

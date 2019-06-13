using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class MainView : ViewBase
    {
        private BackgroundView _background;
        private TextureView _image;
        private ControlPanelView _controlPanel;
        private PreviewSpriteView _previewSpriteView;

        public MainView(SmartSpriteSlicer model) : base(model)
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
        }
    }
}

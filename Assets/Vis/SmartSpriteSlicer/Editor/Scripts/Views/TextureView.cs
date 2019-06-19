using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class TextureView : ViewBase
    {
        private readonly AreasView _areas;

        public TextureView(SmartSpriteSlicerWindow model) : base(model)
        {
            _areas = new AreasView(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

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
            var rect = new Rect(fitX, fitY, fitWidth, fitHeight);
            GUI.DrawTexture(rect, _model.Texture, ScaleMode.StretchToFill);

            _areas.OnGUI(rect);
        }
    }
}
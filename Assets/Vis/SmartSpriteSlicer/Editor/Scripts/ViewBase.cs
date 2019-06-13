using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public abstract class ViewBase
    {
        protected readonly SmartSpriteSlicer _model;

        public ViewBase(SmartSpriteSlicer model)
        {
            _model = model;
        }

        public virtual void OnGUI(Rect position)
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
            var rect = new Rect(fitX, fitY, fitWidth, fitHeight);
            GUI.DrawTexture(rect, _model.Texture, ScaleMode.StretchToFill);
        }
    }
}

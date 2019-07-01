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

            GUI.DrawTexture(_model.TextureRect, _model.Texture, ScaleMode.StretchToFill);
            _areas.OnGUI(_model.TextureRect);
        }
    }
}
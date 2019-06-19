using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class SpritePreviewWindow : LayoutViewBase
    {
        public SpritePreviewWindow(SmartSpriteSlicerWindow model) : base(model)
        {
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var area = _model.PreviewedArea.Value;
            var texture = _model.Texture;
            var localArea = area;
            localArea.x -= _model.TextureRect.x;
            localArea.y -= _model.TextureRect.y;
            var ratio = area.width / area.height;
            var textureRect = new Rect(WindowPosition.position, new Vector2(WindowPosition.width, WindowPosition.width / ratio));
            var rect = GUILayoutUtility.GetRect(textureRect.width, textureRect.height);
            var textureSubRect = new Rect(localArea.x / _model.TextureRect.width, localArea.y / _model.TextureRect.height, area.width / _model.TextureRect.width, area.height / _model.TextureRect.height);
            textureSubRect.y = 1f - textureSubRect.y - textureSubRect.height;
            GUI.DrawTexture(rect, _model.PreviewBackground);
            GUI.DrawTextureWithTexCoords(rect, texture, textureSubRect);
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class SpritePreviewWindow : LayoutViewBase
    {
        private readonly GUIStyle _previewSpriteStyle;
        public SpritePreviewWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _previewSpriteStyle = _model.Skin.GetStyle($"PreviewSprite");
        }

        public Rect WindowWorkaroundRect;

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var area = _model.PreviewedArea.Value;
            var scaledPart = area.position - _model.TextureRect.position;
            var scaledArea = new Rect(_model.TextureRect.position + Vector2.Scale(_model.TextureScale, scaledPart), Vector2.Scale(_model.TextureScale, area.size));
            area = scaledArea;

            var texture = _model.Texture;
            var localArea = area;
            localArea.x -= _model.TextureRect.x;
            localArea.y -= _model.TextureRect.y;
            var ratio = area.width / area.height;
            var textureRect = new Rect(WindowPosition.position, new Vector2(WindowPosition.width, WindowPosition.width / ratio));
            var rect = GUILayoutUtility.GetRect(textureRect.width, textureRect.height);
            var textureSubRect = new Rect(localArea.x / _model.TextureRect.width, localArea.y / _model.TextureRect.height, area.width / _model.TextureRect.width, area.height / _model.TextureRect.height);
            textureSubRect.y = 1f - textureSubRect.y - textureSubRect.height;
            GUI.DrawTextureWithTexCoords(rect, _previewSpriteStyle.normal.background, new Rect(0, 0, rect.width / _previewSpriteStyle.normal.background.width, rect.height / _previewSpriteStyle.normal.background.height));
            GUI.DrawTextureWithTexCoords(rect, texture, textureSubRect);

            var newIterationMode = (SpriteIterationMode)EditorGUILayout.EnumPopup(new GUIContent($"Iteration Mode:", $"You can iterate through sprites with right and left arrow buttons. This option allows you to choose what do you want to iterate through."), _model.IterationMode);
            if (newIterationMode != _model.IterationMode)
            {
                Undo.RecordObject(_model, $"Iteration mode changed");
                _model.IterationMode = newIterationMode;
                EditorUtility.SetDirty(_model);
            }

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    var lastRect = GUILayoutUtility.GetLastRect();
                    WindowWorkaroundRect = WindowPosition;
                    WindowWorkaroundRect.height = lastRect.y + lastRect.height * 2f - WindowWorkaroundRect.y + 100; //Don't know why but height is calculated wrong here and needed to be defined this cumbersome way.
                    WindowWorkaroundRect.position -= WindowPosition.position;
                    break;
                case EventType.MouseDown:
                case EventType.MouseUp:
                    if (WindowWorkaroundRect.Contains(Event.current.mousePosition))
                        Event.current.Use();
                    break;
            }
        }
    }
}

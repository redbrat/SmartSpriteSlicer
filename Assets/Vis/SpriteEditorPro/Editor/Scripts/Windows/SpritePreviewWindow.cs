using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class SpritePreviewWindow : LayoutViewBase
    {
        public static bool Extracted;
        public static GUIStyle _previewSpriteStyle;

        public SpritePreviewWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _previewSpriteStyle = _model.Skin.GetStyle($"PreviewSprite");
        }

        public Rect WindowWorkaroundRect;

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            DrawPreview(WindowPosition, _model);

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

        public static void DrawPreview(Rect windowRect, SmartSpriteSlicerWindow model)
        {
            var area = model.PreviewedArea.Value;
            var scaledPart = area.position - model.TextureRect.position;
            area = new Rect(model.TextureRect.position + Vector2.Scale(model.TextureScale, scaledPart), Vector2.Scale(model.TextureScale, area.size));

            var texture = model.Texture;
            var localArea = area;
            localArea.x -= model.TextureRect.x;
            localArea.y -= model.TextureRect.y;
            var ratio = area.width / area.height;
            var textureRect = new Rect(windowRect.position, new Vector2(windowRect.width, windowRect.width / ratio));
            var rect = GUILayoutUtility.GetRect(textureRect.width, textureRect.height);
            var textureSubRect = new Rect(localArea.x / model.TextureRect.width, localArea.y / model.TextureRect.height, area.width / model.TextureRect.width, area.height / model.TextureRect.height);
            textureSubRect.y = 1f - textureSubRect.y - textureSubRect.height;
            GUI.DrawTextureWithTexCoords(rect, _previewSpriteStyle.normal.background, new Rect(0, 0, rect.width / _previewSpriteStyle.normal.background.width, rect.height / _previewSpriteStyle.normal.background.height));
            GUI.DrawTextureWithTexCoords(rect, texture, textureSubRect);

            var pivot = model.PreviewedPivotPoint.Value - model.PreviewedArea.Value.position;
            pivot = rect.position + pivot * rect.width / _previewSpriteStyle.normal.background.width;
            var worldPos = new Vector3(pivot.x, pivot.y, 0);
            var newWorldPos = DraggableDisc.Draw(worldPos, Vector3.back, 4f, Color.gray * 0.5f);
            Handles.BeginGUI();
            Handles.color = new Color(0, 0, 0, 0.4f);
            Handles.DrawSolidDisc(worldPos, Vector3.back, 5);
            Handles.color = new Color(1, 1, 1, 0.4f);
            Handles.DrawSolidDisc(worldPos, Vector3.back, 2);
            Handles.color = new Color(1, 1, 1, 1);
            Handles.DrawSolidDisc(worldPos, Vector3.back, 1);
            Handles.EndGUI();

            if (model.ControlPanelTab == ControlPanelTabs.ManualSlicing)
            {
                var newIterationMode = (SpriteIterationMode)EditorGUILayout.EnumPopup(new GUIContent($"Iteration Mode:", $"You can iterate through sprites with right and left arrow buttons. This option allows you to choose what do you want to iterate through."), model.IterationMode);
                if (newIterationMode != model.IterationMode)
                {
                    Undo.RecordObject(model, $"Iteration mode changed");
                    model.IterationMode = newIterationMode;
                    EditorUtility.SetDirty(model);
                }
            }
        }
    }
}

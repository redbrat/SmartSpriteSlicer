using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ForegroundView : ViewBase
    {
        public ForegroundView(SmartSpriteSlicerWindow model) : base(model) { }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            var rect = new Rect(Vector2.zero, position.size);
            //Handles.BeginGUI();
            //var ctrlId = GUIUtility.GetControlID(FocusType.Passive);
            //Handles.DrawSolidRectangleWithOutline(rect, Color.red * 0.3f, Color.black);
            //Handles.EndGUI();

            if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
            {
                _model.PreviewedAreaControlId = null;
                _model.PreviewedArea = null;
                GUI.changed = true;
                Event.current.Use();
            }
        }
    }
}
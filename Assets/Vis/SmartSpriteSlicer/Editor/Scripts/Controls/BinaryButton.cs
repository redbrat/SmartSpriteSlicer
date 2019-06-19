using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public static class BinaryButton
    {
        public static (bool clicked, bool pressed) Draw(GUIContent content, Rect rect, Color outlineColor, Color color0, Color color1, float fadingAmountPerFrame = 0.1f)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = (BinaryButtonState)GUIUtility.GetStateObject(typeof(BinaryButtonState), controlId);
            var result = (clicked: false, pressed: state.Pressed);

            Handles.BeginGUI();
            var targetColor = state.Pressed ? color1 : color0;
            var nonTargetColor = state.Pressed ? color0 : color1;
            var color = Color.Lerp(nonTargetColor, targetColor, state.Value);
            Handles.DrawSolidRectangleWithOutline(rect, color, outlineColor);
            Handles.EndGUI();

            switch (Event.current.type)
            {
                case EventType.Repaint:
                    if (state.Value != 1f)
                    {
                        state.Value = Mathf.Clamp01(state.Value + fadingAmountPerFrame);
                        GUI.changed = true;
                    }
                    break;
                case EventType.MouseDown:
                    if (rect.Contains(Event.current.mousePosition))
                        GUIUtility.hotControl = controlId;
                    break;
                case EventType.MouseLeaveWindow:
                case EventType.DragExited:
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        if (rect.Contains(Event.current.mousePosition))
                            result.clicked = true;
                        state.Pressed = !state.Pressed;
                        result.pressed = state.Pressed;
                        state.Value = 1 - state.Value;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
            }

            return result;
        }
    }
}

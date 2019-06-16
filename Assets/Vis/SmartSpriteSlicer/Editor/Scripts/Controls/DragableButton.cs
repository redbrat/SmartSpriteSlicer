using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public static class DragableButton
    {
        public static bool IsDragging;
        public static Rect DraggingPosition;
        public static GUIContent DraggingContent;
        public static GUIStyle DraggingStyle;
        public static Color DragableColor;

        public static bool Draw(GUIContent content, GUIStyle style, bool isDragable, params GUILayoutOption[] options)
        {
            var result = false;
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var position = GUILayoutUtility.GetRect(content, style, options);
            switch (Event.current.GetTypeForControl(controlId))
            {
                case EventType.Repaint:
                    style.Draw(position, content, controlId);
                    break;
                case EventType.MouseDown:
                    if (isDragable)
                        IsDragging = false;
                    if (Event.current.button == 0 && position.Contains(Event.current.mousePosition))
                        GUIUtility.hotControl = controlId;
                    break;
                case EventType.MouseLeaveWindow:
                case EventType.DragExited:
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == controlId)
                    {
                        GUIUtility.hotControl = 0;
                        if (position.Contains(Event.current.mousePosition))
                            result = true;
                        if (isDragable)
                            IsDragging = false;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;
            }
            if (isDragable && Event.current.isMouse && GUIUtility.hotControl == controlId)
            {
                IsDragging = true;
                DraggingPosition = position;
                DraggingPosition.x = Event.current.mousePosition.x - DraggingPosition.width * 0.5f;
                DraggingPosition.y = Event.current.mousePosition.y - DraggingPosition.height * 0.5f;
                DraggingContent = new GUIContent($"{content.text} +", content.tooltip);
                DraggingStyle = style;
                DragableColor = GUI.backgroundColor;
                Event.current.Use();
            }
            return result;
        }
    }
}

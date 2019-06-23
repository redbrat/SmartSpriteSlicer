using UnityEditor;
using UnityEngine;

public static class DraggableDisc
{
    public static Vector3 Draw(Vector3 position, Vector3 normal, float radius, Color color)
    {
        var ctrlId = GUIUtility.GetControlID(FocusType.Passive);
        var state = (DraggableDiscState)GUIUtility.GetStateObject(typeof(DraggableDiscState), ctrlId);

        switch (Event.current.type)
        {
            case EventType.MouseDown:
                {
                    var rect = getRect(position, radius);
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        GUIUtility.hotControl = ctrlId;
                        state.IsDragging = true;
                        Event.current.Use();
                    }
                }
                break;
            case EventType.MouseLeaveWindow:
            case EventType.DragExited:
            case EventType.MouseUp:
                if (GUIUtility.hotControl == ctrlId)
                {
                    GUIUtility.hotControl = 0;
                    state.IsDragging = false;

                    GUI.changed = true;
                    Event.current.Use();
                }
                break;
            case EventType.Repaint:
                {
                    Handles.BeginGUI();
                    var originalColor = Handles.color;
                    Handles.color = makeOpaque(Color.white - color);
                    Handles.DrawWireDisc(position, Vector3.back, 4f);
                    Handles.color = color;
                    Handles.DrawSolidDisc(position, Vector3.back, 3f);
                    Handles.color = originalColor;
                    Handles.EndGUI();

                    var rect = getRect(position, radius);
                    EditorGUIUtility.AddCursorRect(rect, MouseCursor.Pan);
                }
                break;
        }

        if (Event.current.isMouse && state.IsDragging && GUIUtility.hotControl == ctrlId)
        {
            position = Event.current.mousePosition;
            GUI.changed = true;
        }
        return position;
    }
    private static Color makeOpaque(Color color) => new Color(color.r, color.g, color.b, 1f);
    private static Rect getRect(Vector3 position, float radius) => new Rect(position.x - radius, position.y - radius, radius * 2, radius * 2);
}

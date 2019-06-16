using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public static class DragableButton
    {
        public static bool Draw(GUIContent content, GUIStyle style, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(content, style, options);
            return GUI.Button(position, content, style);
        }
    }
}

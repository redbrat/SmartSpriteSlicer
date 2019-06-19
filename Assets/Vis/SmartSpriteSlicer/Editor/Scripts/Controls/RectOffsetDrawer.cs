using UnityEditor;
using UnityEngine;
namespace Vis.SmartSpriteSlicer
{
    public static class RectOffsetDrawer
    {
        public static RectOffset Draw(GUIContent content, RectOffset value)
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = (RectOffsetState)GUIUtility.GetStateObject(typeof(RectOffsetState), controlId);
            state.Unfolded = EditorGUILayout.Foldout(state.Unfolded, content);
            if (state.Unfolded)
            {
                var newValue = EditorGUILayout.IntField(new GUIContent("Left:"), value.left);
                if (newValue != value.left)
                {
                    value.left = newValue;
                }
                newValue = EditorGUILayout.IntField(new GUIContent("Right:"), value.right);
                if (newValue != value.right)
                {
                    value.right = newValue;
                }
                newValue = EditorGUILayout.IntField(new GUIContent("Top:"), value.top);
                if (newValue != value.top)
                {
                    value.top = newValue;
                }
                newValue = EditorGUILayout.IntField(new GUIContent("Bottom:"), value.bottom);
                if (newValue != value.bottom)
                {
                    value.bottom = newValue;
                }
            }
            return value;
        }
    }
}

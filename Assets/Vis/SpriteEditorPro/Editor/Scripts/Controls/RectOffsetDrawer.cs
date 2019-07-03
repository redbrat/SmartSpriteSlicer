using UnityEditor;
using UnityEngine;
namespace Vis.SpriteEditorPro
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
                value = new RectOffset(value.left, value.right, value.top, value.bottom);
                value.left = EditorGUILayout.IntField(new GUIContent("Left:"), value.left);
                value.right = EditorGUILayout.IntField(new GUIContent("Right:"), value.right);
                value.top = EditorGUILayout.IntField(new GUIContent("Top:"), value.top);
                value.bottom = EditorGUILayout.IntField(new GUIContent("Bottom:"), value.bottom);
            }
            return value;
        }
    }
}

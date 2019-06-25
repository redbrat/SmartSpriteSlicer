using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public static class RadioButtonsGroup
    {
        public static T DrawEnum<T>(T value, Func<T, bool, RadioButtonType, GUIStyle> styleFunc, Func<T, bool, RadioButtonType, GUIContent> contentFunc) where T : Enum
        {
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            var state = (RadioButtonsGroupState<T>)GUIUtility.GetStateObject(typeof(RadioButtonsGroupState<T>), controlId);

            EditorGUILayout.BeginHorizontal();
            var enumNames = typeof(T).GetEnumNames();
            var enumValues = typeof(T).GetEnumValues();
            for (int i = 0; i < enumNames.Length; i++)
            {
                var type = RadioButtonType.Middle;
                if (i == 0)
                    type = RadioButtonType.Left;
                else if (i == enumNames.Length - 1)
                    type = RadioButtonType.Left;
                var currentValue = (T)enumValues.GetValue(i);
                var selected = currentValue.Equals(value);
                var style = styleFunc(currentValue, selected, type);
                var content = contentFunc(currentValue, selected, type);
                if (GUILayout.Button(content, style))
                {
                    state.Selected = currentValue;
                    value = currentValue;
                }
            }
            EditorGUILayout.EndHorizontal();
            return value;
        }
    }
}

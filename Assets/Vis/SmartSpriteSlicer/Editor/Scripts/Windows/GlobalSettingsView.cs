using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GlobalSettingsView : LayoutViewBase
    {
        private bool _unfolded;

        public GlobalSettingsView(SmartSpriteSlicerWindow model) : base(model)
        {
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var content = new GUIContent($"<b>Global Settings</b>");
            var rect = GUILayoutUtility.GetRect(content, _model.RichTextStyle);
            EditorGUILayout.BeginHorizontal();
            var triangleRect = rect;
            triangleRect.width = 10;
            triangleRect.y += 2;
            triangleRect.height -= 2;
            rect.x += triangleRect.width;
            _unfolded = EditorGUI.Foldout(triangleRect, _unfolded, GUIContent.none);
            EditorGUI.LabelField(rect, content, _model.RichTextStyle);
            EditorGUILayout.EndHorizontal();
            if (_unfolded)
            {
                EditorGUI.indentLevel++;
                var newAnchor = (LayoutAnchor)EditorGUILayout.EnumPopup(new GUIContent($"Anchor:"), _model.SlicingSettings.LayoutAnchor);
                if (newAnchor != _model.SlicingSettings.LayoutAnchor)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Global anchor changed");
                    _model.SlicingSettings.LayoutAnchor = newAnchor;
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                var newOffset = EditorGUILayout.Vector2IntField(new GUIContent($"Offset:"), _model.SlicingSettings.Offset);
                if (newOffset != _model.SlicingSettings.Offset)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Global offset changed");
                    _model.SlicingSettings.Offset = newOffset;
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
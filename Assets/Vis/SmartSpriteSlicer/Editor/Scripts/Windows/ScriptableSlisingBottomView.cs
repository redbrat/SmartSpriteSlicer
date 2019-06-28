using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class ScriptableSlisingBottomView : LayoutViewBase
    {
        private readonly GUIStyle _previewTextStyle;
        private readonly GUIStyle _panelStyle;

        public ScriptableSlisingBottomView(SmartSpriteSlicerWindow model) : base(model)
        {
            _previewTextStyle = model.Skin.GetStyle("ScriptableSlicePreviewText");
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            EditorGUILayout.LabelField(_model.SlicingSettings.ScriptabeSlicingTestText, _previewTextStyle, GUILayout.MinHeight(60f), GUILayout.MaxHeight(80f));
            EditorGUILayout.EndHorizontal();
        }
    }
}
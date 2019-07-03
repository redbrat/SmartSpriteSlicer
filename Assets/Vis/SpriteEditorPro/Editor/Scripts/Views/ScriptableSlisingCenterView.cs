using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public class ScriptableSlicingPreviewCenterView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;

        public ScriptableSlicingPreviewCenterView(SpriteEditorProWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsMainPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            var newTestText = EditorGUILayout.TextArea(_model.SlicingSettings.ScriptabeSlicingTestText, GUILayout.MinHeight(60f), GUILayout.MaxHeight(80f));
            if (newTestText != _model.SlicingSettings.ScriptabeSlicingTestText)
            {
                Undo.RecordObject(_model.SlicingSettings, $"Scriptable slicing test text changed");
                _model.SlicingSettings.ScriptabeSlicingTestText = /*newTestText.Length >= 256 ? newTestText.Substring(0, 256) : */newTestText;
                _model.SlicingSettings.UpdateScriptableSlicingLayoutHash();
                _model.Repaint();
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
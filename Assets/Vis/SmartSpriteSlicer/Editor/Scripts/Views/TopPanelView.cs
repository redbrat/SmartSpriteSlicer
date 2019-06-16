using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class TopPanelView : LayoutViewBase
    {
        private readonly GUIStyle _topPanelStyle;

        public TopPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _topPanelStyle = _model.Skin.GetStyle("ControlTopPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_topPanelStyle);
            EditorGUILayout.LabelField(new GUIContent($"Preset:"), GUILayout.Width(40f));
            var newPreset = (Preset)EditorGUILayout.ObjectField(_model.SlicingSettingsPreset, typeof(Preset), false, GUILayout.Width(140f));
            if (newPreset != _model.SlicingSettingsPreset)
            {
                _model.SlicingSettingsPreset = newPreset;
            }
            if (_model.SlicingSettingsPreset != null)
            {
                if (!_model.SlicingSettingsPreset.DataEquals(_model.SlicingSettings))
                {
                    EditorGUILayout.LabelField(new GUIContent($"*modified"), GUILayout.Width(60));
                    if (GUILayout.Button(new GUIContent($"Save", "Save values to preset")))
                    {
                        if (Selection.activeObject == _model.SlicingSettingsPreset)
                            Selection.activeObject = null; //There's an issue when preset we're saving into is opened in inspector: saving wouldn't work, inspector somehow overrides it's values with it's own. So we just close inspector in that case in order to be able to save.
                        Undo.RecordObject(_model.SlicingSettingsPreset, "Preset values updated");
                        _model.SlicingSettingsPreset.UpdateProperties(_model.SlicingSettings);
                    }
                    if (GUILayout.Button(new GUIContent($"Reset", "Apply preset values")))
                    {
                        _model.SlicingSettingsPreset.ApplyTo(_model.SlicingSettings);
                    }
                }
            }
            if (GUILayout.Button(new GUIContent($"Save as...")))
            {
                var absolutePath = EditorUtility.SaveFilePanel($"Save preset as", string.Empty, $"SpriteSlicePreset", "preset");
                var relativePath = $"Assets{absolutePath.Substring(Application.dataPath.Length)}";
                var preset = new Preset(_model.SlicingSettings);
                AssetDatabase.CreateAsset(preset, relativePath);
                AssetDatabase.SaveAssets();
                _model.SlicingSettingsPreset = preset;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
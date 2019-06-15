using System.IO;
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
            var newPreset = EditorGUILayout.ObjectField(_model.SlicingSettingsPreset, typeof(Preset), false, GUILayout.Width(140f));
            if (_model.SlicingSettingsPreset != null)
            {
                if (!_model.SlicingSettingsPreset.DataEquals(_model.SlicingSettings))
                {
                    EditorGUILayout.LabelField(new GUIContent($"*modified"), GUILayout.Width(60));
                    if (GUILayout.Button(new GUIContent($"Save")))
                    {
                        _model.SlicingSettingsPreset.UpdateProperties(_model.SlicingSettings);
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
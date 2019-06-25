using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class TopPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _extractWindowsButtonStyle;

        private float _validWidthCache;

        public TopPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = _model.Skin.GetStyle("ControlTopPanel");
            _extractWindowsButtonStyle = _model.Skin.GetStyle("ExtractWindowsButtonStyle");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            //EditorGUILayout.BeginHorizontal();
            //extract
            var rect = GUILayoutUtility.GetRect(ReservedHeight, float.MaxValue, ReservedHeight, ReservedHeight);
            if (rect.width > 0)
                _validWidthCache = rect.width;
            Debug.Log($"rect = {rect}. ReservedHeight = {ReservedHeight}");
            rect.height = ReservedHeight;
            rect.width = _validWidthCache;
            var leftRect = rect;
            leftRect.width = ReservedHeight;
            var rightRect = rect;
            rightRect.x += ReservedHeight;
            rightRect.width -= ReservedHeight;
            Debug.Log($"rightRect = {rightRect}");
            if (GUI.Button(leftRect, new GUIContent(_extractWindowsButtonStyle.active.background, $"Extract windows")))
                //if (GUILayout.Button(new GUIContent(_extractWindowsButtonStyle.active.background, $"Extract windows"), GUILayout.MinWidth(ReservedHeight), GUILayout.MinHeight(ReservedHeight)))
                Debug.Log($"asdasdas");
            GUILayout.BeginArea(rightRect);
            GUILayout.Button($"adasds");
            EditorGUILayout.BeginVertical(_panelStyle);
            EditorGUILayout.BeginHorizontal();
            //first row
            EditorGUILayout.LabelField(new GUIContent($"<i><size=14>Preset:</size></i>", "Saved layout info"), _model.RichTextStyle, GUILayout.Width(58f));
            var newPreset = (Preset)EditorGUILayout.ObjectField(_model.SlicingSettingsPreset, typeof(Preset), false, GUILayout.Width(128f));
            if (newPreset != _model.SlicingSettingsPreset)
            {
                _model.SlicingSettingsPreset = newPreset;
            }
            if (_model.SlicingSettingsPreset != null)
            {
                if (!_model.SlicingSettingsPreset.DataEquals(_model.SlicingSettings))
                {
                    EditorGUILayout.LabelField(new GUIContent($"*modified"), _model.RichTextStyle, GUILayout.Width(68f));
                    if (GUILayout.Button(new GUIContent($"Save", "Save values to preset")))
                    {
                        if (Selection.activeObject == _model.SlicingSettingsPreset)
                            Selection.activeObject = null; //There's an issue when preset we're saving into is opened in inspector: saving wouldn't work, inspector somehow overrides it's values with it's own. So we just close inspector in that case in order to be able to save.
                        Undo.RecordObject(_model.SlicingSettingsPreset, "Preset values updated");
                        _model.SlicingSettingsPreset.UpdateProperties(_model.SlicingSettings);
                        EditorUtility.SetDirty(_model.SlicingSettingsPreset);
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
                var relativePath = $"Assets/{absolutePath.Substring(Application.dataPath.Length)}";
                Debug.Log($"relativePath = {relativePath}");
                var preset = new Preset(_model.SlicingSettings);
                AssetDatabase.CreateAsset(preset, relativePath);
                AssetDatabase.SaveAssets();
                _model.SlicingSettingsPreset = preset;
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            //second row

            if (GUILayout.Button(new GUIContent($"Apply", $"Slice texture")))
                _model.Slice();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            //EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();


            //EditorGUILayout.BeginHorizontal(_panelStyle);
            //EditorGUILayout.LabelField(new GUIContent($"<i><size=14>Preset:</size></i>", "Saved layout info"), _model.RichTextStyle, GUILayout.Width(58f));
            //var newPreset = (Preset)EditorGUILayout.ObjectField(_model.SlicingSettingsPreset, typeof(Preset), false, GUILayout.Width(128f));
            //if (newPreset != _model.SlicingSettingsPreset)
            //{
            //    _model.SlicingSettingsPreset = newPreset;
            //}
            //if (_model.SlicingSettingsPreset != null)
            //{
            //    if (!_model.SlicingSettingsPreset.DataEquals(_model.SlicingSettings))
            //    {
            //        EditorGUILayout.LabelField(new GUIContent($"*modified"), _model.RichTextStyle, GUILayout.Width(68f));
            //        if (GUILayout.Button(new GUIContent($"Save", "Save values to preset")))
            //        {
            //            if (Selection.activeObject == _model.SlicingSettingsPreset)
            //                Selection.activeObject = null; //There's an issue when preset we're saving into is opened in inspector: saving wouldn't work, inspector somehow overrides it's values with it's own. So we just close inspector in that case in order to be able to save.
            //            Undo.RecordObject(_model.SlicingSettingsPreset, "Preset values updated");
            //            _model.SlicingSettingsPreset.UpdateProperties(_model.SlicingSettings);
            //            EditorUtility.SetDirty(_model.SlicingSettingsPreset);
            //        }
            //        if (GUILayout.Button(new GUIContent($"Reset", "Apply preset values")))
            //        {
            //            _model.SlicingSettingsPreset.ApplyTo(_model.SlicingSettings);
            //        }
            //    }
            //}
            //if (GUILayout.Button(new GUIContent($"Save as...")))
            //{
            //    var absolutePath = EditorUtility.SaveFilePanel($"Save preset as", string.Empty, $"SpriteSlicePreset", "preset");
            //    var relativePath = $"Assets/{absolutePath.Substring(Application.dataPath.Length)}";
            //    Debug.Log($"relativePath = {relativePath}");
            //    var preset = new Preset(_model.SlicingSettings);
            //    AssetDatabase.CreateAsset(preset, relativePath);
            //    AssetDatabase.SaveAssets();
            //    _model.SlicingSettingsPreset = preset;
            //}
            //EditorGUILayout.EndHorizontal();

            //if (!_model.SlicingSettings.HaveChunkGroups())
            //    return;
            //EditorGUILayout.BeginHorizontal();

            //if (GUILayout.Button(new GUIContent($"Apply", $"Slice texture")))
            //    _model.Slice();
            //EditorGUILayout.EndHorizontal();
        }
    }
}
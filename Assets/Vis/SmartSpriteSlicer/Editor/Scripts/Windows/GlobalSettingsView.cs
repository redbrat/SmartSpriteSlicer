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

            var newUseCustomSpriteName = EditorGUILayout.Toggle(new GUIContent($"Use custom name:", _model.SlicingSettings.UseCustomSpriteName ? $"First part of final sprite name will be custom: \"{_model.SlicingSettings.CustomName}\"" : $"First part of final sprite will be tha same as original file's name (\"{_model.Texture.name}\")"), _model.SlicingSettings.UseCustomSpriteName);
            if (newUseCustomSpriteName != _model.SlicingSettings.UseCustomSpriteName)
            {
                Undo.RecordObject(_model.SlicingSettings, "Global naming changed");
                _model.SlicingSettings.UseCustomSpriteName = newUseCustomSpriteName;
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            if (newUseCustomSpriteName)
            {
                var newCustomName = EditorGUILayout.TextField(new GUIContent($"Custom name:"), _model.SlicingSettings.CustomName);
                if (newCustomName != _model.SlicingSettings.CustomName)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Global naming changed");
                    _model.SlicingSettings.CustomName = newCustomName;
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }

            var newNamePartsSeparator = EditorGUILayout.TextField(new GUIContent($"Name parts separator:", $"Your final sprites will be named like <global>{_model.SlicingSettings.NamePartsSeparator}<group>{_model.SlicingSettings.NamePartsSeparator}{{index}}"), _model.SlicingSettings.NamePartsSeparator);
            if (newNamePartsSeparator != _model.SlicingSettings.NamePartsSeparator)
            {
                Undo.RecordObject(_model.SlicingSettings, "Global naming changed");
                _model.SlicingSettings.NamePartsSeparator = newNamePartsSeparator;
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

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

            var newPivotPoint = (PivotPoint)EditorGUILayout.EnumPopup(new GUIContent($"Pivot Point"), _model.SlicingSettings.GlobalPivotPoint);
            if (newPivotPoint != _model.SlicingSettings.GlobalPivotPoint)
            {
                Undo.RecordObject(_model.SlicingSettings, "Global pivot point changed");
                _model.SlicingSettings.GlobalPivotPoint = newPivotPoint;
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            if (newPivotPoint == PivotPoint.Absolute)
            {
                var newAbsolutePivot = EditorGUILayout.Vector2IntField(new GUIContent($"Absolute Pivot:"), _model.SlicingSettings.GlobalAbsolutePivot);
                if (newAbsolutePivot != _model.SlicingSettings.GlobalAbsolutePivot)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Global absolute pivot changed");
                    _model.SlicingSettings.GlobalAbsolutePivot = newAbsolutePivot;
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }

            var newGroupsDependentEditing = !EditorGUILayout.Toggle(new GUIContent($"Groups independent editing:", $"If false - changes to preceding groups will cause changes to positions of further groups"), !_model.SlicingSettings.GroupsDependentEditing);
            if (newGroupsDependentEditing != _model.SlicingSettings.GroupsDependentEditing)
            {
                Undo.RecordObject(_model.SlicingSettings, "Groups dependent editing setting changed");
                _model.SlicingSettings.GroupsDependentEditing = newGroupsDependentEditing;
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
        }
    }
}
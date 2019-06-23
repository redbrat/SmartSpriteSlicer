using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupEditPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;

        public GroupEditPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var groupIndex = default(int);
            var group = default(SpriteGroup);
            for (int i = 0; i < _model.SlicingSettings.ChunkGroups.Count; i++)
            {
                if (_model.SlicingSettings.ChunkGroups[i].Id == GroupsView.EditedGroupId)
                {
                    groupIndex = i;
                    group = _model.SlicingSettings.ChunkGroups[i];
                    break;
                }
            }

            EditorGUILayout.BeginVertical(_panelStyle);
            var newTimes = EditorGUILayout.IntField(new GUIContent($"Times:"), group.Times);
            if (newTimes != group.Times)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group times changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetTimes(newTimes);
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                var newNaming = EditorGUILayout.Toggle(new GUIContent($"Modify name", "Modify final sprite name"), group.Naming);
                if (newNaming != group.Naming)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetNaming(newNaming);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
                if (newNaming)
                {
                    var newUseCustomName = !EditorGUILayout.Toggle(new GUIContent($"Use chunk name", $"Final sprite name will include chunk name ({_model.SlicingSettings.Chunks.Where(c => c.Id == group.ChunkId).First().GetHumanFriendlyName()})"), !group.UseCustomName);
                    if (newUseCustomName != group.UseCustomName)
                    {
                        Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                        _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetUseCustomName(newUseCustomName);
                        EditorUtility.SetDirty(_model.SlicingSettings);
                    }

                    if (newUseCustomName)
                    {
                        var newCustomName = EditorGUILayout.TextField(new GUIContent($"Custom group name", "Final sprite will include this name"), group.CustomName);
                        if (newCustomName != group.CustomName)
                        {
                            Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                            _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetCustomName(newCustomName);
                            EditorUtility.SetDirty(_model.SlicingSettings);
                        }
                    }
                }

                var newUseGroupPivotPointSettings = !EditorGUILayout.Toggle(new GUIContent($"Use global pivot point:"), !group.UseGroupPivotPointSettings);
                if (newUseGroupPivotPointSettings != group.UseGroupPivotPointSettings)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group new use global pivot point settings changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetUseGroupPivotPointSettings(newUseGroupPivotPointSettings);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                if (newUseGroupPivotPointSettings)
                {
                    var newPivotPoint = (PivotPoint)EditorGUILayout.EnumPopup(new GUIContent($"Pivot Point"), group.PivotPoint);
                    if (newPivotPoint != group.PivotPoint)
                    {
                        Undo.RecordObject(_model.SlicingSettings, "Group pivot point changed");
                        _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetPivotPoint(newPivotPoint);
                        EditorUtility.SetDirty(_model.SlicingSettings);
                    }

                    if (newPivotPoint == PivotPoint.Absolute)
                    {
                        var newAbsolutePivot = EditorGUILayout.Vector2IntField(new GUIContent($"Absolute Pivot:"), group.AbsolutePivot);
                        if (newAbsolutePivot != group.AbsolutePivot)
                        {
                            Undo.RecordObject(_model.SlicingSettings, "Group absolute pivot changed");
                            _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetAbsolutePivot(newAbsolutePivot);
                            EditorUtility.SetDirty(_model.SlicingSettings);
                        }
                    }
                }
            }

            var offsetAlias = "Offset";
            if (group.Flavor == SpriteGroupFlavor.EmptySpace)
                offsetAlias = "Size";
            else if (group.Flavor == SpriteGroupFlavor.EndOfLine)
                offsetAlias = "Additional Offset";

            var newOffset = EditorGUILayout.Vector2IntField(new GUIContent($"{offsetAlias}:"), group.Offset);
            if (newOffset != group.Offset)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group offset changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetOffset(newOffset);
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                var newIndividualMargin = RectOffsetDrawer.Draw(new GUIContent($"Individual Margin:"), group.IndividualMargin);
                if (newIndividualMargin != group.IndividualMargin)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group individual margin changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetIndividualMargin(newIndividualMargin);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }

            var friendlyName = "group";
            if (group.Flavor == SpriteGroupFlavor.EndOfLine)
                friendlyName = "end of line";
            else if (group.Flavor == SpriteGroupFlavor.EmptySpace)
                friendlyName = "empty space";
            if (GUILayout.Button(new GUIContent($"Delete {friendlyName}")))
            {
                Undo.RecordObject(_model.SlicingSettings, $"{friendlyName} deleted");
                _model.SlicingSettings.ChunkGroups.RemoveAt(groupIndex);
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            EditorGUILayout.EndVertical();
        }
    }
}
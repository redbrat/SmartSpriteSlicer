using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    internal class GroupEditPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;

        public GroupEditPanelView(SpriteEditorProWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var groupInfo = _model.SlicingSettings.GetGroupInfoById(_model.EditedGroupId);
            var groupIndex = groupInfo.index;
            var group = groupInfo.group;

            EditorGUILayout.BeginVertical(_panelStyle);
            var newTimes = EditorGUILayout.IntField(new GUIContent($"Times:"), group.Times);
            if (newTimes != group.Times)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group times changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetTimes(newTimes);
                _model.Repaint();
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                //var newNaming = EditorGUILayout.Toggle(new GUIContent($"Modify name", "Modify final sprite name"), group.Naming);
                //if (newNaming != group.Naming)
                //{
                //    Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                //    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetNaming(newNaming);
                //    EditorUtility.SetDirty(_model.SlicingSettings);
                //}
                //if (newNaming)
                //{
                var newUseCustomName = !EditorGUILayout.Toggle(new GUIContent($"Use chunk name", $"If true final sprite name will include chunk name ({_model.SlicingSettings.Chunks.Where(c => c.Id == group.ChunkId).First().GetHumanFriendlyName()})"), !group.UseCustomName);
                if (newUseCustomName != group.UseCustomName)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetUseCustomName(newUseCustomName);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                if (newUseCustomName)
                {
                    var newCustomName = EditorGUILayout.TextField(new GUIContent($"Custom group name", "Final sprite will include this name"), group.CustomName);
                    if (newCustomName != group.CustomName)
                    {
                        Undo.RecordObject(_model.SlicingSettings, "Group naming setting changed");
                        _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetCustomName(newCustomName);
                        _model.Repaint();
                        EditorUtility.SetDirty(_model.SlicingSettings);
                    }
                }
                //}

                var newUseGroupPivotPointSettings = !EditorGUILayout.Toggle(new GUIContent($"Use global pivot point:"), !group.UseGroupPivotPointSettings);
                if (newUseGroupPivotPointSettings != group.UseGroupPivotPointSettings)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group new use global pivot point settings changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetUseGroupPivotPointSettings(newUseGroupPivotPointSettings);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                if (newUseGroupPivotPointSettings)
                {
                    var newPivotPoint = (PivotPoint)EditorGUILayout.EnumPopup(new GUIContent($"Pivot Point"), group.PivotPoint);
                    if (newPivotPoint != group.PivotPoint)
                    {
                        Undo.RecordObject(_model.SlicingSettings, "Group pivot point changed");
                        _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetPivotPoint(newPivotPoint);
                        _model.Repaint();
                        EditorUtility.SetDirty(_model.SlicingSettings);
                    }

                    if (newPivotPoint == PivotPoint.Absolute)
                    {
                        var newAbsolutePivot = EditorGUILayout.Vector2IntField(new GUIContent($"Absolute Pivot:"), group.AbsolutePivot);
                        if (newAbsolutePivot != group.AbsolutePivot)
                        {
                            Undo.RecordObject(_model.SlicingSettings, "Group absolute pivot changed");
                            _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetAbsolutePivot(newAbsolutePivot);
                            _model.Repaint();
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
                if (!_model.SlicingSettings.GroupsDependentEditing && 
                    groupIndex < _model.SlicingSettings.ChunkGroups.Count - 1 &&
                    group.Flavor == SpriteGroupFlavor.Group)
                {
                    var nextGroup = _model.SlicingSettings.ChunkGroups[groupIndex + 1];
                    if (newOffset.x != group.Offset.x)
                    {
                        var compensatedOffset = nextGroup.Offset;
                        compensatedOffset.x -= newOffset.x - group.Offset.x;
                        _model.SlicingSettings.ChunkGroups[groupIndex + 1] = nextGroup.SetOffset(compensatedOffset);
                    }
                    else
                    {
                        var compensatedOffset = nextGroup.Offset;
                        compensatedOffset.y -= newOffset.y - group.Offset.y;
                        _model.SlicingSettings.ChunkGroups[groupIndex + 1] = nextGroup.SetOffset(compensatedOffset);
                    }
                }
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetOffset(newOffset);
                _model.Repaint();
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                var newIndividualMargin = RectOffsetDrawer.Draw(new GUIContent($"Individual Margin:"), group.IndividualMargin);
                if (newIndividualMargin != group.IndividualMargin)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group individual margin changed");
                    if (!_model.SlicingSettings.GroupsDependentEditing && groupIndex < _model.SlicingSettings.ChunkGroups.Count - 1)
                    {
                        var nextGroup = _model.SlicingSettings.ChunkGroups[groupIndex + 1];
                        if (group.Direction == LayoutDirection.Horizontal && newIndividualMargin.left != group.IndividualMargin.left || newIndividualMargin.right != group.IndividualMargin.right)
                        {
                            var compensatedOffset = nextGroup.Offset;
                            var amountBigger = group.IndividualMargin.left - newIndividualMargin.left - newIndividualMargin.right + group.IndividualMargin.right;
                            var multiplier = group.Direction == LayoutDirection.Horizontal ? group.Times : 1;
                            compensatedOffset.x += amountBigger * multiplier;
                            _model.SlicingSettings.ChunkGroups[groupIndex + 1] = nextGroup.SetOffset(compensatedOffset);
                        }
                        else if (group.Direction == LayoutDirection.Vertical)
                        {
                            var compensatedOffset = nextGroup.Offset;
                            var amountBigger = group.IndividualMargin.top - newIndividualMargin.top - newIndividualMargin.bottom + group.IndividualMargin.bottom;
                            var multiplier = group.Direction == LayoutDirection.Vertical ? group.Times : 1;
                            compensatedOffset.y += amountBigger * multiplier;
                            _model.SlicingSettings.ChunkGroups[groupIndex + 1] = nextGroup.SetOffset(compensatedOffset);
                        }
                    }
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetIndividualMargin(newIndividualMargin);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }

            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                var newDirection = (LayoutDirection)EditorGUILayout.EnumPopup(new GUIContent($"Group direction:"), group.Direction);
                if (newDirection != group.Direction)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group direction changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetDirection(newDirection);
                    _model.Repaint();
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }

            var friendlyName = "group";
            if (group.Flavor == SpriteGroupFlavor.EndOfLine)
                friendlyName = "end of line";
            else if (group.Flavor == SpriteGroupFlavor.EmptySpace)
                friendlyName = "empty space";
            if (GUILayout.Button(new GUIContent($"Delete {friendlyName}")) && EditorUtility.DisplayDialog($"Warning", "Are you sure you want to delete group?", "Yes", "No"))
                _model.RemoveGroupAt(groupIndex);
            EditorGUILayout.EndVertical();
        }
    }
}
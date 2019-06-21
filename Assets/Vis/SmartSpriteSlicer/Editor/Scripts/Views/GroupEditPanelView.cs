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
            //var newFlavor = (SpriteGroupFlavor)EditorGUILayout.EnumPopup(new GUIContent($"Group type:"), group.Flavor);
            //if (newFlavor != group.Flavor)
            //{
            //    Undo.RecordObject(_model.SlicingSettings, "Group type changed");
            //    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetFlavor(newFlavor);
            //    EditorUtility.SetDirty(_model.SlicingSettings);
            //}
            if (group.Flavor == SpriteGroupFlavor.Group)
            {
                var newDirection = (LayoutDirection)EditorGUILayout.EnumPopup(new GUIContent($"Direction:"), group.Direction);
                if (newDirection != group.Direction)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Group direction changed");
                    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetDirection(newDirection);
                    EditorUtility.SetDirty(_model.SlicingSettings);
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
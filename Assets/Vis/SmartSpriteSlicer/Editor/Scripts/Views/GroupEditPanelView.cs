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
            var newFlavor = (SpriteGroupFlavor)EditorGUILayout.EnumPopup(new GUIContent($"Group type:"), group.Flavor);
            if (newFlavor != group.Flavor)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group type changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetFlavor(newFlavor);
            }
            var newDirection = (LayoutDirection)EditorGUILayout.EnumPopup(new GUIContent($"Direction:"), group.Direction);
            if (newDirection != group.Direction)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group direction changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetDirection(newDirection);
            }
            var newOffset = EditorGUILayout.Vector2IntField(new GUIContent($"Offset:"), group.Offset);
            if (newOffset != group.Offset)
            {
                Undo.RecordObject(_model.SlicingSettings, "Group offset changed");
                _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetOffset(newOffset);
            }
            //var newIndividualMargin = (new GUIContent($"Individual Margin:"), group.IndividualMargin);
            //if (newIndividualMargin != group.IndividualMargin)
            //{
            //    Undo.RecordObject(_model.SlicingSettings, "Group individual margin changed");
            //    _model.SlicingSettings.ChunkGroups[groupIndex] = group.SetIndividualMargin(newIndividualMargin);
            //}
            EditorGUILayout.EndVertical();
        }
    }
}
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
            switch (newFlavor)
            {
                case SpriteGroupFlavor.Group:

                    break;
                case SpriteGroupFlavor.Eof:
                    break;
                case SpriteGroupFlavor.Space:
                    break;
                default:
                    break;
            }
            EditorGUILayout.EndVertical();
        }
    }
}
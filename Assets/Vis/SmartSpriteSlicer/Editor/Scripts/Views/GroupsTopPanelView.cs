using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsTopPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _buttonsStyle;

        public GroupsTopPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsTopPanel");
            _buttonsStyle = model.Skin.GetStyle("GroupsTopPanelButton");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            if (GUILayout.Button(new GUIContent($"Add end of line"), _buttonsStyle, GUILayout.MaxWidth(100f)))
            {

            }
            if (GUILayout.Button(new GUIContent($"Add empty space"), _buttonsStyle, GUILayout.MaxWidth(120f)))
            {

            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsView : LayoutViewBase
    {
        public GroupsView(SmartSpriteSlicerWindow model) : base(model)
        {
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.LabelField(new GUIContent($"Groups", $"Here you can edit groups of chunks"));
        }
    }
}
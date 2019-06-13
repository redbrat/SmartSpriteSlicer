using UnityEditor;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        public ControlPanelWindow(SmartSpriteSlicer model) : base(model) { }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.LabelField($"Hello world!");
        }
    }
}
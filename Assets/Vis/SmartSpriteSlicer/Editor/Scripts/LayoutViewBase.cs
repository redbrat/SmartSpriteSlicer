using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class LayoutViewBase : ViewBase
    {
        public LayoutViewBase(SmartSpriteSlicerWindow model) : base(model) { }

        public override sealed void OnGUI(Rect position) {}

        public void WindowContentCallback(int index)
        {
            GUI.DragWindow(new Rect(0, 0, 10000, 20));
            OnGUILayout();
        }

        public virtual void OnGUILayout()
        {

        }
    }
}

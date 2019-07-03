using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public class LayoutViewBase : ViewBase
    {
        public LayoutViewBase(SpriteEditorProWindow model) : base(model) { }

        public Rect WindowPosition;
        public float ReservedHeight;
        public float WindowWidth;

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

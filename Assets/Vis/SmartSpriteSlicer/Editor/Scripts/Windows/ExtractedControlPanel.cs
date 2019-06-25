using UnityEditor;

namespace Vis.SmartSpriteSlicer
{
    public class ExtractedControlPanel : EditorWindow
    {
        private void OnGUI()
        {
            if (ControlPanelWindow.Extracted)
                ControlPanelWindow.DrawControlPanel(position.width);
            Repaint();
        }
    }
}

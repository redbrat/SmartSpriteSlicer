using UnityEditor;

namespace Vis.SpriteEditorPro
{
    public class ExtractedControlPanelWIndow : EditorWindow
    {
        private bool _opened;
        private void OnGUI()
        {
            if (ControlPanelWindow.Extracted)
            {
                _opened = true;
                ControlPanelWindow.DrawControlPanel(position.width);
                Repaint();
            }
            else if (_opened)
            {
                _opened = false;
                Close();
            }
        }
    }
}

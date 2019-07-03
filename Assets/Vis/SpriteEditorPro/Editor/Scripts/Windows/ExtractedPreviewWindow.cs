using UnityEditor;

namespace Vis.SpriteEditorPro
{
    public class ExtractedPreviewWindow : EditorWindow
    {
        public SpriteEditorProWindow Model;
        private bool _opened;

        private void OnGUI()
        {
            if (Model.PreviewedArea != null)
            {
                if (SpritePreviewWindow.Extracted)
                {
                    _opened = true;
                    SpritePreviewWindow.DrawPreview(position, Model);
                    Repaint();
                }
                else if (_opened)
                {
                    _opened = false;
                    Close();
                }
            }
            else if (_opened) 
            {
                _opened = false;
                Close();
            }
        }
    }
}

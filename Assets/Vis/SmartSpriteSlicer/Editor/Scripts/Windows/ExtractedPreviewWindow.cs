using UnityEditor;

namespace Vis.SmartSpriteSlicer
{
    public class ExtractedPreviewWindow : EditorWindow
    {
        public SmartSpriteSlicerWindow Model;
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

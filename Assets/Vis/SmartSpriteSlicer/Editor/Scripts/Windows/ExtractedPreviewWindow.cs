using UnityEditor;

namespace Vis.SmartSpriteSlicer
{
    public class ExtractedPreviewWindow : EditorWindow
    {
        public SmartSpriteSlicerWindow Model;

        private void OnGUI()
        {
            if (SpritePreviewWindow.Extracted)
                SpritePreviewWindow.DrawPreview(position, Model);
            Repaint();
        }
    }
}

using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SmartSpriteSlicer : EditorWindow
    {
        [NonSerialized]
        public int BackgroundCellSize = 40;

        [NonSerialized]
        public Texture2D Texture;
        [NonSerialized]
        public TextureImporter Importer;

        private MainView _view;

        internal void Initialize(Texture2D sprite, TextureImporter importer)
        {
            Texture = sprite;
            Importer = importer;

            _view = new MainView(this);
        }

        private void OnGUI()
        {
            manageDragAndDrop();

            _view?.OnGUI(position);
        }

        private void manageDragAndDrop()
        {
            if ((Event.current.type == EventType.DragUpdated ||
                Event.current.type == EventType.DragExited) &&
                DragAndDrop.objectReferences != null &&
                DragAndDrop.objectReferences.Length == 1 &&
                EntryPoints.ValidateObjectIsEditableSprite(DragAndDrop.objectReferences[0]))
            {
                if (Event.current.type == EventType.DragUpdated)
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                else if (Event.current.type == EventType.DragExited)
                    Initialize(DragAndDrop.objectReferences[0] as Texture2D, EntryPoints.GetTextureImporter(DragAndDrop.objectReferences[0]));
            }
        }
    }
}

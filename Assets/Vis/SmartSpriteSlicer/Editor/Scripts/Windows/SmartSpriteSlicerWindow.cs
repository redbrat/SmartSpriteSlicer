using System;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SmartSpriteSlicerWindow : EditorWindow
    {
        private const string _dbPointerName = "SmartSpriteSlicerDbPointer";
        private const string _slicingSettingsName = "SlicingSettings.asset";

        /// <summary>
        /// Background tile size
        /// </summary>
        public int BackgroundTileSize = 40;

        /// <summary>
        /// Local rect of control panel window
        /// </summary>
        public Rect ControlPanelRect = new Rect(100, 100, 240, 360);

        /// <summary>
        /// Current slicing settings
        /// </summary>
        public SlicingSettings SlicingSettings;

        /// <summary>
        /// Currently selected slicing settings preset
        /// </summary>
        public Preset SlicingSettingsPreset;

        [NonSerialized]
        public GUISkin Skin;
        [NonSerialized]
        public Texture2D Texture;
        [NonSerialized]
        public TextureImporter Importer;

        private MainView _view;

        internal void Initialize(Texture2D sprite, TextureImporter importer)
        {
            Texture = sprite;
            Importer = importer;
            Skin = loadGuiSkin();

            SlicingSettings = getSlicingSettings();

            _view = new MainView(this);
        }

        private void OnDestroy()
        {
            DestroyImmediate(SlicingSettings);
        }

        private void OnGUI()
        {
            manageDragAndDrop();

            _view?.OnGUI(position);
        }

        internal static GUISkin loadGuiSkin() => Resources.Load<GUISkin>("Vis/SmartSpriteSlicer/SmartSpriteSlicer");

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

        private SlicingSettings getSlicingSettings()
        {
            var dbPointerGuids = AssetDatabase.FindAssets(_dbPointerName);
            if (dbPointerGuids.Length == 0)
                throw new ApplicationException($"[{nameof(SmartSpriteSlicerWindow)}] Asset installation corrupted. Try reimport asset from AssetStore!");
            var dbPointerPath = AssetDatabase.GUIDToAssetPath(dbPointerGuids[0]);
            var slicingSettingsPath = Path.Combine(dbPointerPath.Substring(0, dbPointerPath.Length - Path.GetFileName(dbPointerPath).Length), _slicingSettingsName);
            var instance = AssetDatabase.LoadAssetAtPath<SlicingSettings>(slicingSettingsPath);
            if (instance == null)
            {
                instance = CreateInstance<SlicingSettings>();
                AssetDatabase.CreateAsset(instance, slicingSettingsPath);
                AssetDatabase.SaveAssets();
                instance = AssetDatabase.LoadAssetAtPath<SlicingSettings>(slicingSettingsPath);
            }
            return instance;
        }
    }
}

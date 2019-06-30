using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SmartSpriteSlicerWindow : EditorWindow
    {
        public const int MaxContolPanelWidth = 460;
        public const int MaxPreviewWindowWidth = 300;
        public int EditedGroupId;
        private const string _dbPointerName = "SmartSpriteSlicerDbPointer";
        private const string _slicingSettingsName = "SlicingSettings.asset";

        internal readonly string ControlPanelCaption = "Control Panel";

        public Vector2 TextureScale => new Vector2(TextureRect.width / Texture.width, TextureRect.height / Texture.height);
        public Texture2D WhiteTexture { get; private set; }
        public Texture2D BlackTexture { get; private set; }

        public string GetPreviewTitle()
        {
            var result = PreviewName;
            if (!SlicingSettings.UseCustomSpriteName)
                result = $"{Texture.name}{result}";
            return $"Preview: {result}";
        }

        public Rect Position => position;

        /// <summary>
        /// Background tile size
        /// </summary>
        public int BackgroundTileSize = 40;

        /// <summary>
        /// Local rect of control panel window
        /// </summary>
        public Rect ControlPanelRect = new Rect(100, 100, MaxContolPanelWidth, 0);

        /// <summary>
        /// Local rect of control panel window
        /// </summary>
        public Rect PreviewWindowRect = new Rect(100, 100, MaxPreviewWindowWidth, 0);

        /// <summary>
        /// Current slicing settings
        /// </summary>
        public SlicingSettings SlicingSettings;

        /// <summary>
        /// Currently selected slicing settings preset
        /// </summary>
        public Preset SlicingSettingsPreset;

        public GUISkin Skin
        {
            get
            {
                if (_skin == null)
                    _skin = loadGuiSkin();
                return _skin;
            }
        }
        private GUISkin _skin;
        public GUIStyle RichTextStyle => Skin.GetStyle($"RichText");

        [NonSerialized]
        public Texture2D Texture;
        [NonSerialized]
        public TextureImporter Importer;

        private MainView _view;
        public Rect TextureRect;

        public int EditedChunkId;

        public List<int> IterableCtrlIds = new List<int>();
        public List<Rect> IterableAreas = new List<Rect>();
        public List<Vector2Int> IterablePivotPoints = new List<Vector2Int>();
        public Dictionary<int, int> IterableCtrlIdsToGroupsIds = new Dictionary<int, int>();

        public SpriteIterationMode IterationMode;

        public string PreviewName;
        public int? PreviewedAreaControlId;
        public Rect? PreviewedArea;
        public Vector2Int? PreviewedPivotPoint;
        public int? PreviewedGlobalIndex;
        public SpriteGroup? PreviewGroup;
        public SpriteChunk? PreviewChunk;
        public int SelectedGroupIndex;

        public int SelectedNodeIndex;
        public int EditedNodeId;
        public ControlPanelTabs ControlPanelTab;

        public void Initialize(Texture2D sprite, TextureImporter importer)
        {
            Texture = sprite;
            Importer = importer;

            BlackTexture = new Texture2D(1, 1);
            BlackTexture.SetPixel(0, 0, Color.black);
            BlackTexture.Apply();

            WhiteTexture = new Texture2D(1, 1);
            WhiteTexture.SetPixel(0, 0, Color.white);
            WhiteTexture.Apply();

            SlicingSettings = getSlicingSettings();

            _view = new MainView(this);
        }

        private void OnDestroy()
        {
            DestroyImmediate(BlackTexture);
        }

        private void OnGUI()
        {
            manageDragAndDrop();

            if (_view == null)
                GUI.Box(new Rect(Vector2Int.zero, position.size), new GUIContent($"Drag'n'drop your sprite here"), _isDraggingOverWindow ? Skin.GetStyle("PlaceholderActive") : Skin.GetStyle("Placeholder"));
            else
                _view?.OnGUI(position);
        }

        public static GUISkin loadGuiSkin() => Resources.Load<GUISkin>("Vis/SmartSpriteSlicer/SmartSpriteSlicer");

        private bool _isDraggingOverWindow;
        private void manageDragAndDrop()
        {
            if ((Event.current.type == EventType.DragUpdated ||
                Event.current.type == EventType.DragExited) &&
                DragAndDrop.objectReferences != null &&
                DragAndDrop.objectReferences.Length == 1 &&
                EntryPoints.ValidateObjectIsEditableSprite(DragAndDrop.objectReferences[0]))
            {
                if (Event.current.type == EventType.DragUpdated)
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    _isDraggingOverWindow = true;
                }
                else if (Event.current.type == EventType.DragExited)
                {
                    var localPos = new Rect(Vector2Int.zero, position.size);
                    if (localPos.Contains(Event.current.mousePosition))
                        Initialize(DragAndDrop.objectReferences[0] as Texture2D, EntryPoints.GetTextureImporter(DragAndDrop.objectReferences[0]));
                    _isDraggingOverWindow = false;
                }
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

        public void Slice()
        {
            Importer.spriteImportMode = SpriteImportMode.Single;
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(Texture), ImportAssetOptions.Default);
            Importer.spriteImportMode = SpriteImportMode.Multiple;
            var sprites = new List<SpriteMetaData>();

            switch (ControlPanelTab)
            {
                case ControlPanelTabs.ManualSlicing:
                    {
                        var globalName = Texture.name;
                        if (SlicingSettings.UseCustomSpriteName)
                            globalName = SlicingSettings.CustomName;

                        var layout = new Layout(SlicingSettings, new Rect(Vector2Int.zero, new Vector2Int(Texture.width, Texture.height)));
                        //var layout = new Layout(SlicingSettings, new Rect(position.position, TextureRect.size));
                        foreach (var area in layout)
                        {
                            var groupName = area.chunk.GetHumanFriendlyName();
                            if (area.group.UseCustomName)
                                groupName = area.group.CustomName;

                            var name = $"{globalName}{SlicingSettings.NamePartsSeparator}{groupName}{SlicingSettings.NamePartsSeparator}{area.groupIndex}";

                            var flippedYRect = area.position;
                            flippedYRect.y = Texture.height - area.position.y - area.position.height;
                            var flippedYPivot = area.pivotPoint;
                            flippedYPivot.y = Texture.height - area.pivotPoint.y;
                            var normalizedPivot = (flippedYPivot - flippedYRect.position) / flippedYRect.size;

                            sprites.Add(new SpriteMetaData()
                            {
                                name = name,
                                rect = flippedYRect,
                                alignment = (int)SpriteAlignment.Custom,
                                pivot = normalizedPivot
                            });
                        }
                    }
                    break;
                case ControlPanelTabs.ScriptableSclicing:
                    {
                        var layout = new ScriptableLayout(SlicingSettings, new Rect(Vector2Int.zero, TextureRect.position));
                        foreach (var area in layout)
                        {
                            var flippedYRect = area.position;
                            flippedYRect.y = Texture.height - area.position.y - area.position.height;
                            var flippedYPivot = area.pivotPoint;
                            flippedYPivot.y = Texture.height - area.pivotPoint.y;
                            var normalizedPivot = (flippedYPivot - flippedYRect.position) / flippedYRect.size;

                            sprites.Add(new SpriteMetaData()
                            {
                                name = area.name,
                                rect = flippedYRect,
                                alignment = (int)SpriteAlignment.Custom,
                                pivot = normalizedPivot
                            });
                        }
                    }
                    break;
                default:
                    break;
            }

            Importer.spritesheet = sprites.ToArray();
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(Texture), ImportAssetOptions.Default);
        }

        public void RemoveGroupAt(int groupIndex)
        {
            var group = SlicingSettings.ChunkGroups[groupIndex];
            var groupsFriendlyName = "Group";
            if (group.Flavor == SpriteGroupFlavor.EndOfLine)
                groupsFriendlyName = "End of line";
            else if (group.Flavor == SpriteGroupFlavor.EmptySpace)
                groupsFriendlyName = "Empty space";
            Undo.RecordObject(SlicingSettings, $"{groupsFriendlyName} deleted");
            SlicingSettings.ChunkGroups.RemoveAt(groupIndex);
            Repaint();
            EditorUtility.SetDirty(SlicingSettings);
        }
    }
}

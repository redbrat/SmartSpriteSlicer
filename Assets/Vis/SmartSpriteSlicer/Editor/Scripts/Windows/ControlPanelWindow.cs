using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private readonly LayoutViewBase _topPanelView;

        public static LayoutViewBase ChunksView;
        public static LayoutViewBase GlobalSettingsView;
        public static LayoutViewBase GroupsView;
        public static ViewBase DraggableButtonsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _topPanelView = new TopPanelView(model);

            ChunksView = new ChunksView(model);
            GlobalSettingsView = new GlobalSettingsView(model);
            GroupsView = new GroupsView(model);
            DraggableButtonsView = new DraggableButtonView(model);
        }

        private Rect _topPanelViewPosition;
        public static bool Extracted;

        private EditorWindow _windowInstanceCache;

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var reserve = GUILayoutUtility.GetRect(1, _model.SlicingSettings.HaveChunkGroups() ? 60 : 30);
            _topPanelView.ReservedHeight = reserve.height;
            if (Event.current.type == EventType.Repaint)
                _topPanelViewPosition = reserve;

            if (!Extracted)
            {
                if (_windowInstanceCache != null)
                    _windowInstanceCache.Close();
                DrawControlPanel(SmartSpriteSlicerWindow.MaxContolPanelWidth);
            }
            else if (_windowInstanceCache == null)
                _windowInstanceCache = EditorWindow.GetWindow(typeof(ExtractedControlPanel), true, "Control Panel");

            if (Event.current.type == EventType.Repaint)
                _topPanelViewPosition.width = GUILayoutUtility.GetLastRect().width;
            if (_topPanelViewPosition == Rect.zero)
                GUI.changed = true;

            GUILayout.BeginArea(_topPanelViewPosition);
            _topPanelView.OnGUILayout();
            GUILayout.EndArea();
        }

        public static void DrawControlPanel(float windowWidth)
        {
            ChunksView.OnGUILayout();
            EditorGUILayout.Space();
            GlobalSettingsView.OnGUILayout();
            GroupsView.WindowWidth = windowWidth;
            GroupsView.OnGUILayout();

            DraggableButtonsView.OnGUI(Rect.zero);
        }
    }
}
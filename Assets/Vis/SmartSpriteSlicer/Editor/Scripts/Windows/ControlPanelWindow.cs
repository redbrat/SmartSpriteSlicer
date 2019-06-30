using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private readonly LayoutViewBase _topPanelView;

        public static LayoutViewBase TabsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _topPanelView = new TopPanelView(model);

            TabsView = new ControlPanelTabsView(model);
        }

        private Rect _topPanelViewPosition;
        public static bool Extracted;

        private EditorWindow _windowInstanceCache;

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var reserve = GUILayoutUtility.GetRect(1, /*_model.SlicingSettings.HaveChunkGroups() ? 60 : 30*/60);
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
            {
                _windowInstanceCache = EditorWindow.GetWindow(typeof(ExtractedControlPanelWIndow), true, _model.ControlPanelCaption);
                _model.Focus();
            }

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
            TabsView.WindowWidth = windowWidth;
            TabsView.OnGUILayout();
        }
    }
}
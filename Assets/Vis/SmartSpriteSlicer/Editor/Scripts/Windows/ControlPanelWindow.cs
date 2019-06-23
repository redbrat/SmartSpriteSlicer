using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private readonly LayoutViewBase _topPanelView;
        private readonly LayoutViewBase _chunksView;
        private readonly LayoutViewBase _globalSettingsView;
        private readonly LayoutViewBase _groupsView;
        private readonly ViewBase _draggableButtonsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksView = new ChunksView(model);
            _globalSettingsView = new GlobalSettingsView(model);
            _groupsView = new GroupsView(model);
            _topPanelView = new TopPanelView(model);
            _draggableButtonsView = new DraggableButtonView(model);
        }

        private Rect _topPanelViewPosition;

        public override void OnGUILayout()
        {
            base.OnGUILayout();
            
            var reserve = GUILayoutUtility.GetRect(1, _model.SlicingSettings.HaveChunkGroups() ? 60 : 30);
            if (Event.current.type == EventType.Repaint)
                _topPanelViewPosition = reserve;

            _chunksView.OnGUILayout();
            EditorGUILayout.Space();
            _globalSettingsView.OnGUILayout();
            _groupsView.OnGUILayout();

            if (Event.current.type == EventType.Repaint)
                _topPanelViewPosition.width = GUILayoutUtility.GetLastRect().width;
            if (_topPanelViewPosition == Rect.zero)
                GUI.changed = true;
            GUILayout.BeginArea(_topPanelViewPosition);
            _topPanelView.OnGUILayout();
            GUILayout.EndArea();

            _draggableButtonsView.OnGUI(Rect.zero);
        }
    }
}
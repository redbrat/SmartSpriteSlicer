using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private LayoutViewBase _topPanelView;
        private LayoutViewBase _chunksView;
        private LayoutViewBase _groupsView;
        private ViewBase _draggableButtonsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksView = new ChunksView(model);
            _groupsView = new GroupsView(model);
            _topPanelView = new TopPanelView(model);
            _draggableButtonsView = new DraggableButtonView(model);
        }

        private Rect _topPanelViewPosition;

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var reserve = GUILayoutUtility.GetRect(1, 28);
            if (Event.current.type == EventType.Repaint)
                _topPanelViewPosition = reserve;

            _chunksView.OnGUILayout();
            EditorGUILayout.Space();
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
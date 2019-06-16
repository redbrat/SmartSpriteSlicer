using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelWindow : LayoutViewBase
    {
        private LayoutViewBase _topPanelView;
        private LayoutViewBase _chunksView;
        private LayoutViewBase _groupsView;

        public ControlPanelWindow(SmartSpriteSlicerWindow model) : base(model)
        {
            _chunksView = new ChunksView(model);
            _groupsView = new GroupsView(model);
            _topPanelView = new TopPanelView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _chunksView.OnGUILayout();
            _groupsView.OnGUILayout();
            EditorGUILayout.Space();
            _topPanelView.OnGUILayout();

            manageDrag();
        }

        private void manageDrag()
        {
            switch(Event.current.type)
            {
                case EventType.DragUpdated:
                    var focusedControl = GUI.GetNameOfFocusedControl();
                    Debug.Log($"focusedControl = {focusedControl}");
                    break;
            }
        }
    }
}
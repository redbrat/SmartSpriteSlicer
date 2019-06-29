using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ControlPanelTabsView : LayoutViewBase
    {
        private readonly GUIStyle _radioButtonDefaultStyle;
        private readonly GUIStyle _radioButtonDefaultSelectedStyle;

        public readonly LayoutViewBase ChunksView;
        public readonly LayoutViewBase GlobalSettingsView;
        public readonly LayoutViewBase ScriptableSlicingView;
        public readonly LayoutViewBase GroupsView;
        public readonly ViewBase DraggableButtonsView;

        public ControlPanelTabsView(SmartSpriteSlicerWindow model) : base(model)
        {
            _radioButtonDefaultStyle = model.Skin.GetStyle("RadioButtonDefault");
            _radioButtonDefaultSelectedStyle = model.Skin.GetStyle("RadioButtonSelectedDefault");

            ChunksView = new ChunksView(model);
            GlobalSettingsView = new GlobalSettingsView(model);
            ScriptableSlicingView = new ScriptableSlicingView(model);
            GroupsView = new GroupsView(model);
            DraggableButtonsView = new DraggableButtonView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var newTab = RadioButtonsGroup.DrawEnum(_model.ControlPanelTab, styleFunc, contentFunc);
            if (newTab != _model.ControlPanelTab)
                _model.ControlPanelTab = newTab;

            switch (newTab)
            {
                case ControlPanelTabs.ManualSlicing:
                    GlobalSettingsView.OnGUILayout();

                    ChunksView.OnGUILayout();
                    EditorGUILayout.Space();
                    GroupsView.WindowWidth = WindowWidth;
                    GroupsView.OnGUILayout();

                    DraggableButtonsView.OnGUI(Rect.zero);
                    break;
                case ControlPanelTabs.ScriptableSclicing:
                    ScriptableSlicingView.WindowWidth = WindowWidth;
                    ScriptableSlicingView.OnGUILayout();
                    break;
                default:
                    break;
            }
        }

        private GUIContent contentFunc(ControlPanelTabs tab, bool selected, RadioButtonType buttonType)
        {
            switch (tab)
            {
                case ControlPanelTabs.ManualSlicing:
                    return new GUIContent($"<size=16>Manual Slicing</size>");
                case ControlPanelTabs.ScriptableSclicing:
                    return new GUIContent($"<size=16>Scriptable Slicing</size>");
                default:
                    throw new ApplicationException($"Unknown tab type: {tab}");
            }
        }

        private GUIStyle styleFunc(ControlPanelTabs tab, bool selected, RadioButtonType buttonType) => selected ? _radioButtonDefaultSelectedStyle : _radioButtonDefaultStyle;
    }
}
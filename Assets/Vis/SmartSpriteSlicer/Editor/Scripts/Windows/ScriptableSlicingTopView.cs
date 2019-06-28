using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ScriptableSlicingTopView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _buttonsStyle;

        public ScriptableSlicingTopView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsTopPanel");
            _buttonsStyle = model.Skin.GetStyle("GroupsTopPanelButton");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            if (GUILayout.Button(new GUIContent($"Add node"), _buttonsStyle, GUILayout.MaxWidth(100f)))
            {
                Undo.RecordObject(_model.SlicingSettings, "End of line added");
                _model.SlicingSettings.ScriptableNodes.Add(new ScriptableNode(_model.SlicingSettings.GetNextNodeId()));
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
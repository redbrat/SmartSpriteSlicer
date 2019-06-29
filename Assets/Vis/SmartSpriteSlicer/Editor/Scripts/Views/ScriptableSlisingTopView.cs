using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class ScriptableSlisingTopView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _buttonsStyle;

        public ScriptableSlisingTopView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsTopPanel");
            _buttonsStyle = model.Skin.GetStyle("GroupsTopPanelButton");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginVertical(_panelStyle);

            EditorGUILayout.LabelField(new GUIContent($"<b>Test</b>"), _model.RichTextStyle);

            if (!string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText) &&
                _model.SlicingSettings.ScriptableNodes.Count > 0)
            {
                var messageType = MessageType.Error;
                var haveText = !string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText);
                var wholeSet = _model.SlicingSettings.HasWholeSetOfNodes();
                var hasAllThingsNicelySeparated = _model.SlicingSettings.HasAllNodesSeparated();

                var tooltip = default(string);
                if (!haveText)
                    tooltip = $"You must have some text to test";
                else if (!wholeSet)
                    tooltip = $"You must have the whole set of required nodes: {_model.SlicingSettings.GetListOfNodesRequired()}";
                else if (!hasAllThingsNicelySeparated)
                    tooltip = $"You must have all your special nodes separated.";
                else
                {
                    var deepTestPassed = _model.SlicingSettings.NodesDeepTestPassed();
                    if (deepTestPassed)
                    {
                        tooltip = "You pattern runs successfully on provided text.";
                        messageType = MessageType.Info;
                    }
                    else
                        tooltip = "You pattern doesn't run successfully on provided text.";
                }

                EditorGUILayout.HelpBox(tooltip, messageType);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
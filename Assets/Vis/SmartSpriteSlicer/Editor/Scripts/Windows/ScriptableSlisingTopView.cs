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

            EditorGUILayout.BeginHorizontal(_panelStyle);

            EditorGUILayout.LabelField(new GUIContent($"<b>Text</b>", "Enter text here"), _model.RichTextStyle);

            var haveText = !string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText);
            var wholeSet = _model.SlicingSettings.HasWholeSetOfNodes();
            var hasAllThingsNicelySeparated = _model.SlicingSettings.HasAllNodesSeparated();
            GUI.enabled = wholeSet && haveText && hasAllThingsNicelySeparated;

            var tooltip = default(string);
            if (!haveText)
                tooltip = $"You must have some text to test";
            else if (!wholeSet)
                tooltip = $"You must have the whole set of required nodes: {_model.SlicingSettings.GetListOfNodesRequired()}";
            else if (!hasAllThingsNicelySeparated)
                tooltip = $"You must have all your special nodes separated.";
            else
                tooltip = "Test your pattern on provided text.";

            if (GUILayout.Button(new GUIContent($"Test", tooltip)))
            {

            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }
    }
}
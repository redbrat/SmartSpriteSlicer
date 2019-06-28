using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ScriptableSlicingEditView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;

        public ScriptableSlicingEditView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            var nodeInfo = _model.SlicingSettings.GetNodeInfoById(_model.EditedNodeId);
            var nodeIndex = nodeInfo.index;
            var node = nodeInfo.node;

            EditorGUILayout.BeginVertical(_panelStyle);

            var availableTypes = _model.SlicingSettings.GetAvailableNodeTypes();
            if (!availableTypes.Contains(node.Type))
                availableTypes.Insert(0, node.Type);
            var availableTypeNames = availableTypes.Select(t => t.ToString()).ToList();
            var selectedIndex = availableTypes.IndexOf(node.Type);
            var newSelectedIndex = EditorGUILayout.Popup(new GUIContent($"Type:"), selectedIndex, availableTypeNames.ToArray());
            if (newSelectedIndex != selectedIndex)
            {
                Undo.RecordObject(_model.SlicingSettings, "Scriptable node type changed");
                _model.SlicingSettings.ScriptableNodes[nodeIndex] = node.SetType(availableTypes[newSelectedIndex]);
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            if (node.Type == ScriptableNodeType.Text)
            {
                var newText = EditorGUILayout.TextField(new GUIContent($"Text:"), node.Pattern);
                if (newText != node.Pattern)
                {
                    Undo.RecordObject(_model.SlicingSettings, "Scriptable node text changed");
                    _model.SlicingSettings.ScriptableNodes[nodeIndex] = node.SetPattern(newText);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                //var newColor = EditorGUILayout.ColorField(new GUIContent($"Color:"), node.Color);
                //if (newColor != node.Color)
                //{
                //    Undo.RecordObject(_model.SlicingSettings, "Scriptable node color changed");
                //    _model.SlicingSettings.ScriptableNodes[nodeIndex] = node.SetColor(newColor);
                //    EditorUtility.SetDirty(_model.SlicingSettings);
                //}

                //var newTextColor = EditorGUILayout.ColorField(new GUIContent($"Text Color:"), node.TextColor);
                //if (newTextColor != node.TextColor)
                //{
                //    Undo.RecordObject(_model.SlicingSettings, "Scriptable node text color changed");
                //    _model.SlicingSettings.ScriptableNodes[nodeIndex] = node.SetTextColor(newTextColor);
                //    EditorUtility.SetDirty(_model.SlicingSettings);
                //}
            }

            if (GUILayout.Button($"Delete node"))
            {
                Undo.RecordObject(_model.SlicingSettings, "Scriptable node deleted");
                _model.SlicingSettings.ScriptableNodes.RemoveAt(nodeIndex);
                EditorUtility.SetDirty(_model.SlicingSettings);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
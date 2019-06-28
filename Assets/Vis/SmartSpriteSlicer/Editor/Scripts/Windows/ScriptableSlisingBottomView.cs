using System;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class ScriptableSlisingBottomView : LayoutViewBase
    {
        private readonly GUIStyle _previewTextStyle;
        private readonly GUIStyle _panelStyle;

        public ScriptableSlisingBottomView(SmartSpriteSlicerWindow model) : base(model)
        {
            _previewTextStyle = model.Skin.GetStyle("ScriptableSlicePreviewText");
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            var text = _model.SlicingSettings.ScriptableNodes.Count > 0 ? getColorizedValidatedText(_model.SlicingSettings.ScriptabeSlicingTestText) : _model.SlicingSettings.ScriptabeSlicingTestText;
            EditorGUILayout.LabelField(text, _previewTextStyle, GUILayout.MinHeight(100f), GUILayout.MaxHeight(120f));
            EditorGUILayout.EndHorizontal();
        }

        private string getColorizedValidatedText(string text)
        {
            var report = new ScriptableLayoutReport();
            var layout = new ScriptableLayout(_model.SlicingSettings, Rect.zero, report);
            foreach (var item in layout) ;
            for (int i = report.Chunks.Count - 1; i >= 0; i--)
            {
                var chunk = report.Chunks[i];
                var hexColorString = ColorUtility.ToHtmlStringRGB(chunk.Color);
                if (!chunk.SuccessfullyParsed)
                    text = text.Insert(chunk.StopIndex, $"</i></b>");
                text = text.Insert(chunk.StopIndex, $"</color>");
                text = text.Insert(chunk.StartIndex, $"<color=#{hexColorString}>");
                if (!chunk.SuccessfullyParsed)
                    text = text.Insert(chunk.StartIndex, $"<b><i>");
            }
            return text;
        }
    }
}
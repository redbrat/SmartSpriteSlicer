using System;
using System.Text;
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

        private (int hash, string text) _colorizedTextCache;
        private string getColorizedValidatedText(string text)
        {
            if (_colorizedTextCache.hash != _model.SlicingSettings.ScriptableSlicingLayoutHash || _colorizedTextCache.text == null)
            {
                var report = new ScriptableLayoutReport();
                var layout = new ScriptableLayout(_model.SlicingSettings, Rect.zero, report);
                foreach (var item in layout) ;

                if (report.Chunks.Count > 0) 
                {
                    var sb = new StringBuilder();
                    var currentChunkIndex = 0;
                    for (int i = 0; i < text.Length; i++)
                    {
                        var chunk = currentChunkIndex < report.Chunks.Count ? report.Chunks[currentChunkIndex] : null;
                        if (chunk != null)
                        {
                            if (chunk.StartIndex == i)
                            {
                                var hexColorString = ColorUtility.ToHtmlStringRGB(chunk.Color);

                                sb.Append($"<color=#{hexColorString}>");
                                if (!chunk.SuccessfullyParsed)
                                    sb.Append($"<b><i>");
                            }
                            if (chunk.StopIndex == i || text.Length == i + 1)
                            {
                                if (!chunk.SuccessfullyParsed)
                                    sb.Append($"</i></b>");
                                sb.Append($"</color>");
                                currentChunkIndex++;
                                if (text.Length > i + 1)
                                    i--;
                                continue;
                            }
                        }
                        sb.Append(text[i]);
                    }
                    text = sb.ToString();
                }
                _colorizedTextCache = (_model.SlicingSettings.ScriptableSlicingLayoutHash, text);
            }
            return _colorizedTextCache.text;
        }
    }
}
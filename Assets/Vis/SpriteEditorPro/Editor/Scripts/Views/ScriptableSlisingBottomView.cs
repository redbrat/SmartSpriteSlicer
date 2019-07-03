using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public class ScriptableSlicingPreviewBottomView : LayoutViewBase
    {
        private readonly GUIStyle _previewTextStyle;
        private readonly GUIStyle _panelStyle;

        private Vector2 _scrollPosition;

        public ScriptableSlicingPreviewBottomView(SpriteEditorProWindow model) : base(model)
        {
            _previewTextStyle = model.Skin.GetStyle("ScriptableSlicePreviewText");
            _panelStyle = model.Skin.GetStyle("GroupsEditPanel");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();
            EditorGUILayout.BeginHorizontal(_panelStyle);
            var text = _model.SlicingSettings.ScriptableNodes.Count > 0 ? getColorizedValidatedText(_model.SlicingSettings.ScriptabeSlicingTestText) : _model.SlicingSettings.ScriptabeSlicingTestText;
            if (_model.PreviewedGlobalIndex.HasValue && _model.PreviewedGlobalIndex < _colorizedTextCache.report.Chunks.Count)
            {
                var firstChunkIndex = _model.PreviewedGlobalIndex.Value * _model.SlicingSettings.ScriptableNodes.Count;
                var lastChunkIndex = firstChunkIndex + _model.SlicingSettings.ScriptableNodes.Count - 1;
                var firstReportedChunk = _colorizedTextCache.report.Chunks[firstChunkIndex];
                var lastReportedChunk = _colorizedTextCache.report.Chunks[lastChunkIndex];
                var size = _previewTextStyle.CalcSize(new GUIContent(text));
                _scrollPosition.y = (firstReportedChunk.EnrichedStartIndex / (float)text.Length) * size.y;
                text = text.Insert(lastReportedChunk.EnrichedStopIndex, "</size>").Insert(firstReportedChunk.EnrichedStartIndex, "<size=24>");
            }
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.MinHeight(100f), GUILayout.MaxHeight(300f));
            //Debug.LogError($"_scrollPosition = {_scrollPosition}");
            //Debug.LogError($"size = {size}");

            EditorGUILayout.TextArea(text, _previewTextStyle, /*GUILayout.MinHeight(100f), GUILayout.MaxHeight(300f),*/ GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        private (int hash, string text, ScriptableLayoutReport report) _colorizedTextCache;
        private string getColorizedValidatedText(string text)
        {
            if (_colorizedTextCache.hash != _model.SlicingSettings.ScriptableSlicingLayoutHash || _colorizedTextCache.text == null)
            {
                var report = new ScriptableLayoutReport();
                var layout = new ScriptableLayout(_model.SlicingSettings, Rect.zero, report);
                foreach (var item in layout) ;

                var enrichedOffset = 0;
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

                                chunk.EnrichedStartIndex += enrichedOffset;
                                var openColorTag = $"<color=#{hexColorString}>";
                                enrichedOffset += openColorTag.Length;
                                sb.Append(openColorTag);
                                if (!chunk.SuccessfullyParsed)
                                {
                                    var failedOpenTags = $"<b><i>";
                                    sb.Append(failedOpenTags);
                                    enrichedOffset += failedOpenTags.Length;
                                }
                            }
                            if (chunk.StopIndex == i || text.Length == i + 1)
                            {
                                if (!chunk.SuccessfullyParsed)
                                {
                                    var failedCloseTags = $"</i></b>";
                                    sb.Append(failedCloseTags);
                                    enrichedOffset += failedCloseTags.Length;
                                }
                                var closeColorTag = $"</color>";
                                sb.Append(closeColorTag);
                                enrichedOffset += closeColorTag.Length;
                                chunk.EnrichedStopIndex += enrichedOffset;
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
                _colorizedTextCache = (_model.SlicingSettings.ScriptableSlicingLayoutHash, text, report);
            }
            return _colorizedTextCache.text;
        }
    }
}
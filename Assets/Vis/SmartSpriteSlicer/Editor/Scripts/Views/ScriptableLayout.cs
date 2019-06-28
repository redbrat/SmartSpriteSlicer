using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal struct ScriptableLayout : IEnumerable<(int globalIndex, string name, Rect position, Rect localPosition, Vector2Int pivotPoint, Vector2Int localPivotPoint)>
    {
        private const char _endOfLineChar = '\n';

        private readonly SlicingSettings _slicingSettings;
        private readonly Rect _position;
        private readonly ScriptableLayoutReport _report;

        public ScriptableLayout(SlicingSettings slicingSettings, Rect position, ScriptableLayoutReport report = null)
        {
            _slicingSettings = slicingSettings;
            _position = position;
            _report = report;
        }

        public IEnumerator<(int globalIndex, string name, Rect position, Rect localPosition, Vector2Int pivotPoint, Vector2Int localPivotPoint)> GetEnumerator()
        {
            var text = _slicingSettings.ScriptabeSlicingTestText;
            var nodes = _slicingSettings.ScriptableNodes;
            var nodeIndex = 0;

            var sb = new StringBuilder();

            var textChunks = new List<ScriptableNodeTypeTextChunk>();
            var name = default(ScriptableNodeTypeTextChunk);
            var group = default(ScriptableNodeTypeTextChunk);
            var x = default(ScriptableNodeTypeTextChunk);
            var y = default(ScriptableNodeTypeTextChunk);
            var width = default(ScriptableNodeTypeTextChunk);
            var height = default(ScriptableNodeTypeTextChunk);
            var pivotX = default(ScriptableNodeTypeTextChunk);
            var pivotY = default(ScriptableNodeTypeTextChunk);

            var globalIndex = 0;
            var textChunkStartIndex = 0;

            for (int i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var currentNode = nodes[nodeIndex];
                ScriptableNode? nextNode = nodeIndex >= nodes.Count - 1 ? null : (ScriptableNode?)nodes[nodeIndex + 1];

                switch (currentNode.Type)
                {
                    case ScriptableNodeType.EndOfLine:
                        sb.Append(currentChar);
                        if (currentChar == _endOfLineChar)
                        {
                            textChunks.Add(new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, ScriptableNodeType.EndOfLine, true, sb.ToString() ));
                            textChunkStartIndex = 0;

                            nodeIndex++;
                            sb.Clear();
                        }
                        break;
                    case ScriptableNodeType.Text:
                        sb.Append(currentChar);
                        if (sb.ToString() == currentNode.Pattern)
                        {
                            textChunks.Add(new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, ScriptableNodeType.Text, true, sb.ToString() ));
                            textChunkStartIndex = 0;

                            nodeIndex++;
                            sb.Clear();
                        }
                        break;
                    default:
                        sb.Append(currentChar);
                        if (nextIsSeparator(text, nextNode, i) || i >= text.Length - 1)
                        {
                            nodeIndex++;
                            switch (currentNode.Type)
                            {
                                case ScriptableNodeType.Name:
                                    name = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(name);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.Group:
                                    group = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(group);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.X:
                                    x = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(x);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.Y:
                                    y = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(y);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.Width:
                                    width = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(width);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.Height:
                                    height = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(height);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.PivotX:
                                    pivotX = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(pivotX);
                                    textChunkStartIndex = 0;
                                    break;
                                case ScriptableNodeType.PivotY:
                                    pivotY = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, i, currentNode.Type, false, sb.ToString());
                                    textChunks.Add(pivotY);
                                    textChunkStartIndex = 0;
                                    break;
                            }
                            sb.Clear();
                        }
                        break;
                }

                if (nodeIndex >= nodes.Count)
                {
                    nodeIndex = 0;

                    int numX;
                    if (int.TryParse(x.Text, out numX))
                        x.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    int numY;
                    if (int.TryParse(y.Text, out numY))
                        y.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    int numWidth;
                    if (int.TryParse(width.Text, out numWidth))
                        width.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    int numHeight;
                    if (int.TryParse(height.Text, out numHeight))
                        height.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    int numPivotX;
                    if (pivotX == null || string.IsNullOrEmpty(pivotX.Text))
                        numPivotX = 0;
                    else if (int.TryParse(pivotX.Text, out numPivotX))
                        pivotX.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    int numPivotY;
                    if (pivotY == null || string.IsNullOrEmpty(pivotY.Text))
                        numPivotY = 0;
                    else if (int.TryParse(pivotY.Text, out numPivotY))
                        pivotY.SuccessfullyParsed = true;
                    else if (_report != null)
                        _report.ParsingFailed = true;

                    var localPosition = new Rect(numX, numY, numWidth, numHeight);
                    var position = new Rect(_position.position + localPosition.position, localPosition.size);
                    var localPivotPoint = new Vector2Int(numPivotX, numPivotY);
                    var pivotPoint = toVector2Int(_position.position) + localPivotPoint;

                    localPivotPoint += toVector2Int(localPosition.position);
                    pivotPoint += toVector2Int(localPosition.position);

                    var fullName = string.Empty;
                    if (name == default)
                        fullName = globalIndex.ToString();
                    else
                        fullName = name.Text;
                    if (group != default)
                        fullName = $"{group}{_slicingSettings.NamePartsSeparator}{fullName}";
                    
                    if (_report == null || !_report.ParsingFailed)
                        yield return (globalIndex++, fullName, position, localPosition, pivotPoint, localPivotPoint);

                    x = default;
                    y = default;
                    width = default;
                    height = default;
                    pivotX = default;
                    pivotY = default;
                    name = default;
                    group = default;
                }
            }
        }

        private bool nextIsSeparator(string text, ScriptableNode? node, int index)
        {
            if (!node.HasValue)
                return false;
            var value = node.Value;
            if (!(value.Type == ScriptableNodeType.EndOfLine || value.Type == ScriptableNodeType.Text))
                return false;
            if (text.Length <= index + 1)
                return true;
            if (value.Type == ScriptableNodeType.EndOfLine)
                return text[index + 1] == _endOfLineChar;
            if (string.IsNullOrEmpty(value.Pattern))
                return true; //Не уверен, что тут не false
            for (int i = index + 1; i < text.Length; i++)
            {
                var localIndex = i - index - 1;
                if (value.Pattern.Length <= localIndex)
                    return true; //Если достигли конца текстовой ноды без ошибок присылаем труе
                if (text[i] != value.Pattern[localIndex])
                    return false;
            }
            return true; //Если достигли конца текста без ошибок присылаем труе
        }

        private Vector2Int toVector2Int(Vector2 value) => new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
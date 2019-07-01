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
                var nextTextIndex = i + 1;
                var currentChar = text[i];
                var currentNode = nodes[nodeIndex];
                ScriptableNode? nextNode = nodeIndex >= nodes.Count - 1 ? null : (ScriptableNode?)nodes[nodeIndex + 1];

                switch (currentNode.Type)
                {
                    case ScriptableNodeType.EndOfLine:
                        sb.Append(currentChar);
                        if (currentChar == _endOfLineChar)
                        {
                            textChunks.Add(new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, ScriptableNodeType.EndOfLine, sb.ToString(), true));
                            textChunkStartIndex = nextTextIndex;

                            nodeIndex++;
                            sb.Clear();
                        }
                        break;
                    case ScriptableNodeType.Text:
                        if (string.IsNullOrEmpty(currentNode.Pattern))
                        {
                            nodeIndex++;
                            i--;
                            break;
                        }
                        sb.Append(currentChar);
                        if (stringCoincide(sb.ToString(), currentNode.Pattern))
                        {
                            textChunks.Add(new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, ScriptableNodeType.Text, sb.ToString(), true));
                            textChunkStartIndex = nextTextIndex;

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
                                    name = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), true);
                                    textChunks.Add(name);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.Group:
                                    group = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), true);
                                    textChunks.Add(group);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.X:
                                    x = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(x);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.Y:
                                    y = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(y);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.Width:
                                    width = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(width);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.Height:
                                    height = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(height);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.PivotX:
                                    pivotX = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(pivotX);
                                    textChunkStartIndex = nextTextIndex;
                                    break;
                                case ScriptableNodeType.PivotY:
                                    pivotY = new ScriptableNodeTypeTextChunk(currentNode.Color, textChunkStartIndex, nextTextIndex, currentNode.Type, sb.ToString(), false);
                                    textChunks.Add(pivotY);
                                    textChunkStartIndex = nextTextIndex;
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
                    var pivotPoint = _position.position.ToVector2Int() + localPivotPoint;

                    localPivotPoint += localPosition.position.ToVector2Int();
                    pivotPoint += localPosition.position.ToVector2Int();

                    string fullName;
                    if (name == default)
                        fullName = globalIndex.ToString();
                    else
                        fullName = name.Text;
                    if (group != default)
                        fullName = $"{group.Text}{_slicingSettings.NamePartsSeparator}{fullName}";

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
                if (_report != null)
                {
                    _report.Chunks.AddRange(textChunks);
                    if (_report.ParsingFailed)
                        break;
                }
                textChunks.Clear();
            }
        }

        private bool stringCoincide(string what, string with)
        {
            if (what.Length < with.Length)
                return false;
            var j = what.Length - 1;
            for (int i = with.Length - 1; i >= 0; i--)
                if (with[i] != what[j--])
                    return false;
            return true;
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
                return false; //Не уверен, что тут не true
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
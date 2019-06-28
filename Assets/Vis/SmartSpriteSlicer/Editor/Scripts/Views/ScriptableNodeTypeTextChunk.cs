using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class ScriptableNodeTypeTextChunk
    {
        public readonly ScriptableNodeType Type;
        public readonly int StartIndex;
        public readonly int StopIndex;
        public readonly Color Color;
        public readonly string Text;

        public bool SuccessfullyParsed;

        public ScriptableNodeTypeTextChunk(Color color, int startIndex, int stopIndex, ScriptableNodeType type, bool endOfLine, string text, bool successfullyParsed = false)
        {
            Color = color;
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Type = type;
            Text = text;
            SuccessfullyParsed = successfullyParsed;
        }
    }
}
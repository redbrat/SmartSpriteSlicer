using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public class ScriptableNodeTypeTextChunk
    {
        public readonly ScriptableNodeType Type;
        public readonly int StartIndex;
        public readonly int StopIndex;
        public readonly Color Color;
        public readonly string Text;

        public int EnrichedStartIndex;
        public int EnrichedStopIndex;
        public bool SuccessfullyParsed;

        public PivotPointAnchor PivotPointAnchor;
        public Vector2Int CustomPivotPointAnchor;
        public PivotDirection PivotDirection;

        public ScriptableNodeTypeTextChunk(Color color, int startIndex, int stopIndex, ScriptableNodeType type, string text, bool successfullyParsed = false)
        {
            Color = color;
            StartIndex = startIndex;
            StopIndex = stopIndex;
            Type = type;
            Text = text;
            SuccessfullyParsed = successfullyParsed;
            EnrichedStartIndex = startIndex;
            EnrichedStopIndex = stopIndex;
        }
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vis.SpriteEditorPro
{
    [Serializable]
    public struct ScriptableNode
    {
        public int Id => _id;
        [SerializeField]
        private int _id;
        public ScriptableNodeType Type => _type;
        [SerializeField]
        private ScriptableNodeType _type;
        public Color Color => _color;
        [SerializeField]
        private Color _color;
        public Color TextColor => _textColor;
        [SerializeField]
        private Color _textColor;
        public string Pattern => _pattern;
        [SerializeField]
        private string _pattern;
        public PivotPointAnchor PivotAnchor => _pivotAnchor;
        [SerializeField]
        private PivotPointAnchor _pivotAnchor;
        public Vector2Int CustomAnchor => _customAnchor;
        [SerializeField]
        private Vector2Int _customAnchor;
        public PivotDirection PivotDirection => _pivotDirection;
        [SerializeField]
        private PivotDirection _pivotDirection;

        public ScriptableNode(int id)
        {
            _id = id;
            _color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _textColor = Color.white;
            _type = default;
            _pattern = default;
            _pivotAnchor = default;
            _customAnchor = default;
            _pivotDirection = default;
        }

        public ScriptableNode(int id, ScriptableNodeType type, Color color, Color textColor, string pattern, PivotPointAnchor pivotAnchor, Vector2Int customAnchor, PivotDirection pivotDirection)
        {
            _id = id;
            _type = type;
            _color = color;
            _textColor = textColor;
            _pattern = pattern;
            _pivotAnchor = pivotAnchor;
            _customAnchor = customAnchor;
            _pivotDirection = pivotDirection;
        }

        internal ScriptableNode SetType(ScriptableNodeType type) => new ScriptableNode(_id, type, _color, _textColor, _pattern, _pivotAnchor, _customAnchor, _pivotDirection);
        internal ScriptableNode SetColor(Color color) => new ScriptableNode(_id, _type, color, _textColor, _pattern, _pivotAnchor, _customAnchor, _pivotDirection);
        internal ScriptableNode SetTextColor(Color textColor) => new ScriptableNode(_id, _type, _color, textColor, _pattern, _pivotAnchor, _customAnchor, _pivotDirection);
        internal ScriptableNode SetPattern(string pattern) => new ScriptableNode(_id, _type, _color, _textColor, pattern, _pivotAnchor, _customAnchor, _pivotDirection);
        internal ScriptableNode SetPivotAnchor(PivotPointAnchor pivotAnchor) => new ScriptableNode(_id, _type, _color, _textColor, _pattern, pivotAnchor, _customAnchor, _pivotDirection);
        internal ScriptableNode SetCustomAnchor(Vector2Int customAnchor) => new ScriptableNode(_id, _type, _color, _textColor, _pattern, _pivotAnchor, customAnchor, _pivotDirection);
        internal ScriptableNode SetPivotDirection(PivotDirection pivotDirection) => new ScriptableNode(_id, _type, _color, _textColor, _pattern, _pivotAnchor, _customAnchor, pivotDirection);

        public override int GetHashCode()
        {
            var result = _id.GetHashCode();
            result += _type.GetHashCode();
            result += _color.GetHashCode();
            result += _textColor.GetHashCode();
            if (_pattern != null)
                result += _pattern.GetHashCode();
            result += _pivotAnchor.GetHashCode();
            result += _customAnchor.GetHashCode();
            result += _pivotDirection.GetHashCode();
            return result;
        }
    }
}
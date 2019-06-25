using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vis.SmartSpriteSlicer
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

        public ScriptableNode(int id)
        {
            _id = id;
            _color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            _textColor = Color.white;
            _type = default;
            _pattern = default;
        }

        public ScriptableNode(int id, ScriptableNodeType type, Color color, Color textColor, string pattern)
        {
            _id = id;
            _type = type;
            _color = color;
            _textColor = textColor;
            _pattern = pattern;
        }

        internal ScriptableNode SetType(ScriptableNodeType type) => new ScriptableNode(_id, type, _color, _textColor, _pattern);
        internal ScriptableNode SetColor(Color color) => new ScriptableNode(_id, _type, color, _textColor, _pattern);
        internal ScriptableNode SetTextColor(Color textColor) => new ScriptableNode(_id, _type, _color, textColor, _pattern);
        internal ScriptableNode SetPattern(string pattern) => new ScriptableNode(_id, _type, _color, _textColor, pattern);
    }
}
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public struct SpriteChunk
    {
        [SerializeField]
        private int _id;
        public int Id => _id;
        [SerializeField]
        private Color _color;
        public Color Color => _color;
        [SerializeField]
        private Vector2Int _size;
        public Vector2Int Size => _size;

        public SpriteChunk(int id, Vector2Int size)
        {
            _id = id;
            _size = size;
            _color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        public SpriteChunk(int id, Vector2Int size, Color color)
        {
            _id = id;
            _size = size;
            _color = color;
        }

        public SpriteChunk SetSize(Vector2Int newSize) => new SpriteChunk(_id, newSize, _color);
        public SpriteChunk SetColor(Color newColor) => new SpriteChunk(_id, _size, newColor);
    }
}

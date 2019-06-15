using System;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public struct SpriteChunk
    {
        public readonly Guid Id;
        public readonly Vector2Int Size;

        public SpriteChunk(Vector2Int size) : this()
        {
            Size = size;
        }

        public SpriteChunk(Guid id, Vector2Int size)
        {
            Id = id;
            Size = size;
        }

        public SpriteChunk SetSize(Vector2Int newSize) => new SpriteChunk(Id, newSize);
    }
}

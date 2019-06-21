using System.Collections.Generic;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SlicingSettings : ScriptableObject
    {
        public Vector2Int Offset;
        //public LayoutDirection LayoutDirection;
        public LayoutAnchor LayoutAnchor;
        public List<SpriteChunk> Chunks = new List<SpriteChunk>();
        public List<SpriteGroup> ChunkGroups = new List<SpriteGroup>();
    }
}

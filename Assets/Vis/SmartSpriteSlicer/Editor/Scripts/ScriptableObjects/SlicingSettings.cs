using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SlicingSettings : ScriptableObject
    {
        public bool UseCustomSpriteName;
        public string CustomName = "sprite";
        public string NamePartsSeparator = "-";
        public Vector2Int Offset;
        public LayoutAnchor LayoutAnchor;
        public List<SpriteChunk> Chunks = new List<SpriteChunk>();
        public List<SpriteGroup> ChunkGroups = new List<SpriteGroup>();

        public int GetNextChunkId()
        {
            if (Chunks.Count == 0)
                return 1;
            return Chunks.OrderByDescending(c => c.Id).First().Id + 1;
        }

        public int GetNextGroupId()
        {
            if (ChunkGroups.Count == 0)
                return 1;
            return ChunkGroups.OrderByDescending(g => g.Id).First().Id + 1;
        }

        public Vector2Int GetLastGroupOffset()
        {
            var result = default(Vector2Int);
            if (ChunkGroups.Count > 0 && ChunkGroups.Where(g => g.Flavor == SpriteGroupFlavor.Group).Any())
            {
                for (int i = ChunkGroups.Count - 1; i >= 0; i--)
                {
                    var group = ChunkGroups[i];
                    if (group.Flavor == SpriteGroupFlavor.Group)
                    {
                        result = Chunks.Where(c => c.Id == group.ChunkId).First().Size;
                        break;
                    }
                }
            }
            return result;
        }
    }
}

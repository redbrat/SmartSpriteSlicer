using System;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public struct SpriteGroup
    {
        /// <summary>
        /// Type of group
        /// </summary>
        public SpriteGroupFlavor Flavor;
        /// <summary>
        /// Chunk id of a chunk group
        /// </summary>
        public int ChunkId;
        /// <summary>
        /// Times that chunk is used in that group
        /// </summary>
        public int Times;
    }
}

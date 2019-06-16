using System;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public struct SpriteGroup
    {
        public int Id => _id;
        [SerializeField]
        private int _id;
        /// <summary>
        /// Type of group
        /// </summary>
        public SpriteGroupFlavor Flavor => _flavor;
        [SerializeField]
        private SpriteGroupFlavor _flavor;
        /// <summary>
        /// Chunk id of a chunk group
        /// </summary>
        public int ChunkId => _chunkId;
        [SerializeField]
        private int _chunkId;
        /// <summary>
        /// Times that chunk is used in that group
        /// </summary>
        public int Times => _times;
        [SerializeField]
        private int _times;

        /// <summary>
        /// Ctor with only nessessary properties
        /// </summary>
        /// <param name="id"></param>
        public SpriteGroup(int id)
        {
            _id = id;
            _flavor = default;
            _chunkId = default;
            _times = default;
        }

        /// <summary>
        /// Default ctor fot chunk groups
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chunkId"></param>
        public SpriteGroup(int id, int chunkId)
        {
            _id = id;
            _chunkId = chunkId;
            _flavor = SpriteGroupFlavor.Group;
            _times = 1;
        }

        /// <summary>
        /// Ctor with full set of features
        /// </summary>
        /// <param name="id"></param>
        /// <param name="chunkId"></param>
        /// <param name="flavor"></param>
        /// <param name="times"></param>
        public SpriteGroup(int id, int chunkId, SpriteGroupFlavor flavor, int times)
        {
            _id = id;
            _chunkId = chunkId;
            _flavor = flavor;
            _times = times;
        }
    }
}

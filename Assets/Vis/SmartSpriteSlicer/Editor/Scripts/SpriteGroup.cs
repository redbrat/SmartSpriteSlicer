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

        public Vector2Int Offset => _offset;
        [SerializeField]
        private Vector2Int _offset;
        public LayoutDirection Direction => _direction;
        [SerializeField]
        private LayoutDirection _direction;
        public RectOffset IndividualMargin => _individualMargin;
        [SerializeField]
        private RectOffset _individualMargin;

        /// <summary>
        /// Ctor with only nessessary properties
        /// </summary>
        public SpriteGroup(int id)
        {
            _id = id;
            _flavor = default;
            _chunkId = default;
            _times = default;
            _offset = default;
            _direction = default;
            _individualMargin = new RectOffset();
        }

        /// <summary>
        /// Default ctor fot chunk groups
        /// </summary>
        public SpriteGroup(int id, int chunkId)
        {
            _id = id;
            _chunkId = chunkId;
            _flavor = SpriteGroupFlavor.Group;
            _times = 1;
            _offset = default;
            _direction = default;
            _individualMargin = new RectOffset();
        }

        /// <summary>
        /// Default ctor fot non-chunks groups
        /// </summary>
        public SpriteGroup(int id, SpriteGroupFlavor flavor, Vector2Int offset = default)
        {
            _id = id;
            _chunkId = 0;
            _flavor = flavor;
            _times = 1;
            _offset = offset;
            _direction = default;
            _individualMargin = new RectOffset();
        }

        /// <summary>
        /// Ctor with full set of features
        /// </summary>
        public SpriteGroup(int id, int chunkId, SpriteGroupFlavor flavor, int times, Vector2Int offset, LayoutDirection direction, RectOffset individualMargin)
        {
            _id = id;
            _chunkId = chunkId;
            _flavor = flavor;
            _times = times;
            _offset = offset;
            _direction = direction;
            _individualMargin = individualMargin;
        }

        internal SpriteGroup SetChunk(int chunkId) => new SpriteGroup(_id, chunkId, _flavor, _times, _offset, _direction, _individualMargin);
        internal SpriteGroup SetTimes(int times) => new SpriteGroup(_id, _chunkId, _flavor, times, _offset, _direction, _individualMargin);
        internal SpriteGroup SetFlavor(SpriteGroupFlavor flavor) => new SpriteGroup(_id, _chunkId, flavor, _times, _offset, _direction, _individualMargin);
        internal SpriteGroup SetOffset(Vector2Int offset) => new SpriteGroup(_id, _chunkId, _flavor, _times, offset, _direction, _individualMargin);
        internal SpriteGroup SetDirection(LayoutDirection direction) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, direction, _individualMargin);
        internal SpriteGroup SetIndividualMargin(RectOffset individualMargin) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, individualMargin);
    }
}

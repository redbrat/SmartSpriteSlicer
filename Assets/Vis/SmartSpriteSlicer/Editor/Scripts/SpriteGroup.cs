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

        public bool Naming => _naming;
        [SerializeField]
        private bool _naming;
        public bool UseCustomName => _useCustomName;
        [SerializeField]
        private bool _useCustomName;
        public string CustomName => _customName;
        [SerializeField]
        private string _customName;

        public PivotPoint PivotPoint => _pivotPoint;
        [SerializeField]
        private PivotPoint _pivotPoint;
        public Vector2Int AbsolutePivot => _absolutePivot;
        [SerializeField]
        private Vector2Int _absolutePivot;

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
            _naming = default;
            _useCustomName = default;
            _customName = default;
            _pivotPoint = default;
            _absolutePivot = default;
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
            _naming = default;
            _useCustomName = default;
            _customName = default;
            _pivotPoint = default;
            _absolutePivot = default;
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
            _naming = default;
            _useCustomName = default;
            _customName = default;
            _pivotPoint = default;
            _absolutePivot = default;
        }

        /// <summary>
        /// Ctor with full set of features
        /// </summary>
        public SpriteGroup(int id, int chunkId, SpriteGroupFlavor flavor, int times, Vector2Int offset, LayoutDirection direction, RectOffset individualMargin, bool naming, bool useCustomName, string customName, PivotPoint pivotPoint, Vector2Int absolutePivot)
        {
            _id = id;
            _chunkId = chunkId;
            _flavor = flavor;
            _times = times;
            _offset = offset;
            _direction = direction;
            _individualMargin = individualMargin;
            _naming = naming;
            _useCustomName = useCustomName;
            _customName = customName;
            _pivotPoint = pivotPoint;
            _absolutePivot = absolutePivot;
        }

        internal SpriteGroup SetChunk(int chunkId) => new SpriteGroup(_id, chunkId, _flavor, _times, _offset, _direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetTimes(int times) => new SpriteGroup(_id, _chunkId, _flavor, times, _offset, _direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetFlavor(SpriteGroupFlavor flavor) => new SpriteGroup(_id, _chunkId, flavor, _times, _offset, _direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetOffset(Vector2Int offset) => new SpriteGroup(_id, _chunkId, _flavor, _times, offset, _direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetDirection(LayoutDirection direction) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetIndividualMargin(RectOffset individualMargin) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, individualMargin, _naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetNaming(bool naming) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, _individualMargin, naming, _useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetUseCustomName(bool useCustomName) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, _individualMargin, _naming, useCustomName, _customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetCustomName(string customName) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, _individualMargin, _naming, _useCustomName, customName, _pivotPoint, _absolutePivot);
        internal SpriteGroup SetPivotPoint(PivotPoint pivotPoint) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, _individualMargin, _naming, _useCustomName, _customName, pivotPoint, _absolutePivot);
        internal SpriteGroup SetAbsolutePivot(Vector2Int absolutePivot) => new SpriteGroup(_id, _chunkId, _flavor, _times, _offset, _direction, _individualMargin, _naming, _useCustomName, _customName, _pivotPoint, absolutePivot);
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public class SlicingSettings : ScriptableObject
    {
        private readonly ScriptableNodeType[] _requiredSetOfNodes = new ScriptableNodeType[] { ScriptableNodeType.X, ScriptableNodeType.Y, ScriptableNodeType.Width, ScriptableNodeType.Height };

        public bool UseCustomSpriteName;
        public string CustomName = "sprite";
        public string NamePartsSeparator = "-";
        public Vector2Int Offset;
        public LayoutAnchor LayoutAnchor;
        public List<SpriteChunk> Chunks = new List<SpriteChunk>();
        public List<SpriteGroup> ChunkGroups = new List<SpriteGroup>();
        public List<ScriptableNode> ScriptableNodes = new List<ScriptableNode>();
        public PivotPoint GlobalPivotPoint;
        public Vector2Int GlobalAbsolutePivot;
        public bool GroupsDependentEditing;

        public string ScriptabeSlicingTestText;

        public int GetNextChunkId()
        {
            if (Chunks.Count == 0)
                return 1;
            return Chunks.OrderByDescending(c => c.Id).First().Id + 1;
        }

        internal bool HasWholeSetOfNodes()
        {
            for (int i = 0; i < _requiredSetOfNodes.Length; i++)
                if (!ScriptableNodes.Where(n => n.Type == _requiredSetOfNodes[i]).Any())
                    return false;
            return true;
        }

        internal bool HasAllNodesSeparated()
        {
            var seenSeparator = false;
            var separatorWasOnStart = false;
            for (int i = 0; i < ScriptableNodes.Count; i++)
            {
                var node = ScriptableNodes[i];
                if ((node.Type == ScriptableNodeType.Text && !string.IsNullOrEmpty(node.Pattern)) || node.Type == ScriptableNodeType.EndOfLine)
                {
                    seenSeparator = true;
                    if (i == 0)
                        separatorWasOnStart = true;
                    continue;
                }
                if (i > 0 && !seenSeparator) //Если уже не первая нода а мы все еще не видели сепаратора то вертаем false
                    return false;
                seenSeparator = false;
            }
            if (!separatorWasOnStart && !seenSeparator) //Если сепаратора не было ни в начале ни в конце - вертаем false
                return false;
            return true;
        }

        internal string GetListOfNodesRequired()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _requiredSetOfNodes.Length; i++)
                sb.Append($", {_requiredSetOfNodes[i]}");
            sb.Remove(0, 2);
            return sb.ToString();
        }

        internal List<ScriptableNodeType> GetAvailableNodeTypes()
        {
            var result = new List<ScriptableNodeType>();

            result.Add(ScriptableNodeType.Text);
            result.Add(ScriptableNodeType.EndOfLine);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.Name).Any())
                result.Add(ScriptableNodeType.Name);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.Group).Any())
                result.Add(ScriptableNodeType.Group);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.X).Any())
                result.Add(ScriptableNodeType.X);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.Y).Any())
                result.Add(ScriptableNodeType.Y);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.Width).Any())
                result.Add(ScriptableNodeType.Width);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.Height).Any())
                result.Add(ScriptableNodeType.Height);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.PivotX).Any())
                result.Add(ScriptableNodeType.PivotX);
            if (!ScriptableNodes.Where(n => n.Type == ScriptableNodeType.PivotY).Any())
                result.Add(ScriptableNodeType.PivotY);

            return result;
        }

        public int GetNextGroupId()
        {
            if (ChunkGroups.Count == 0)
                return 1;
            return ChunkGroups.OrderByDescending(g => g.Id).First().Id + 1;
        }

        public int GetNextNodeId()
        {
            if (ScriptableNodes.Count == 0)
                return 1;
            return ScriptableNodes.OrderByDescending(g => g.Id).First().Id + 1;
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

        internal bool HaveChunkGroups() => ChunkGroups.Count > 0 && ChunkGroups.Where(cg => cg.Flavor == SpriteGroupFlavor.Group).Any();
        internal (int index, SpriteGroup group) GetGroupInfoById(int index)
        {
            for (int i = 0; i < ChunkGroups.Count; i++)
                if (ChunkGroups[i].Id == index)
                    return (i, ChunkGroups[i]);
            return (-1, default);
        }
        internal (int index, ScriptableNode node) GetNodeInfoById(int index)
        {
            for (int i = 0; i < ScriptableNodes.Count; i++)
                if (ScriptableNodes[i].Id == index)
                    return (i, ScriptableNodes[i]);
            return (-1, default);
        }
    }
}

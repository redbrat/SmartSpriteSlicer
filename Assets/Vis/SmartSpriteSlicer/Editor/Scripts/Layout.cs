using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public struct Layout : IEnumerable<(int globalIndex, int groupIndex, string name, Rect position, Vector2Int pivotPoint, SpriteGroup group, SpriteChunk chunk)>
    {
        private readonly SlicingSettings _slicingSettings;
        private readonly Rect _position;

        public Layout(SlicingSettings slicingSettings, Rect position)
        {
            _slicingSettings = slicingSettings;
            _position = position;
        }

        public IEnumerator<(int globalIndex, int groupIndex, string name, Rect position, Vector2Int pivotPoint, SpriteGroup group, SpriteChunk chunk)> GetEnumerator()
        {
            var globalAnchor = _slicingSettings.LayoutAnchor;
            var offset = getGlobalAnchorPoint(globalAnchor, _position);
            offset.x += formatX(_slicingSettings.Offset.x, globalAnchor);
            offset.y += formatY(_slicingSettings.Offset.y, globalAnchor);
            var initialX = offset.x;
            var initialY = offset.y;
            var groups = _slicingSettings.ChunkGroups;

            var globalName = string.Empty;
            if (_slicingSettings.UseCustomSpriteName)
                globalName = _slicingSettings.CustomName;

            var globalIndex = 0;
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];

                switch (group.Flavor)
                {
                    case SpriteGroupFlavor.Group:
                        var chunk = _slicingSettings.Chunks.Where(c => c.Id == group.ChunkId).First();
                        var groupName = chunk.GetHumanFriendlyName();
                        if (group.UseCustomName)
                            groupName = group.CustomName;

                        offset.x += formatX(group.Offset.x, globalAnchor);
                        offset.y += formatY(group.Offset.y, globalAnchor);
                        for (int t = 0; t < group.Times; t++)
                        {
                            (int globalIndex, int groupIndex, string name, Rect position, Vector2Int pivotPoint, SpriteGroup group, SpriteChunk chunk) result;
                            offset = drawGroupArea(offset, group, globalAnchor, out result);
                            result.globalIndex = globalIndex++;
                            result.groupIndex = t;
                            result.name = $"{globalName}{_slicingSettings.NamePartsSeparator}{groupName}{_slicingSettings.NamePartsSeparator}{t}";
                            yield return result;
                        }
                        break;
                    case SpriteGroupFlavor.EndOfLine:
                        for (int t = 0; t < group.Times; t++)
                        {
                            offset.y += formatY(group.Offset.y, globalAnchor);
                            offset.x = initialX + formatX(group.Offset.x, globalAnchor);
                        }
                        break;
                    case SpriteGroupFlavor.EmptySpace:
                        for (int t = 0; t < group.Times; t++)
                        {
                            offset.y += formatY(group.Offset.y, globalAnchor);
                            offset.x += formatX(group.Offset.x, globalAnchor);
                        }
                        break;
                }
            }
        }

        private int formatY(int y, LayoutAnchor globalAnchor)
        {
            switch (globalAnchor)
            {
                case LayoutAnchor.TopLeft:
                case LayoutAnchor.TopRight:
                    return y;
                case LayoutAnchor.BottomRight:
                case LayoutAnchor.BottomLeft:
                default:
                    return -y;
            }
        }

        private int formatX(int x, LayoutAnchor globalAnchor)
        {
            switch (globalAnchor)
            {
                case LayoutAnchor.BottomLeft:
                case LayoutAnchor.TopLeft:
                    return x;
                case LayoutAnchor.TopRight:
                case LayoutAnchor.BottomRight:
                default:
                    return -x;
            }
        }

        private Vector2Int getGlobalAnchorPoint(LayoutAnchor globalAnchor, Rect position)
        {
            switch (globalAnchor)
            {
                case LayoutAnchor.TopLeft:
                    return new Vector2Int((int)position.x, (int)position.y);
                case LayoutAnchor.TopRight:
                    return new Vector2Int((int)(position.x + position.width), (int)position.y);
                case LayoutAnchor.BottomRight:
                    return new Vector2Int((int)(position.x + position.width), (int)(position.y + position.height));
                case LayoutAnchor.BottomLeft:
                default:
                    return new Vector2Int((int)position.x, (int)(position.y + position.height));
            }
        }

        private (int globalIndex, int groupIndex, string name, Rect position, Vector2Int pivotPoint, SpriteGroup group, SpriteChunk chunk) getFormattedGroupRect(Vector2Int offset, SpriteChunk chunk, SpriteGroup group, LayoutAnchor globalAnchor)
        {
            var result = (globalIndex: 0, groupIndex: 0, name: string.Empty, position: Rect.zero, pivotPoint: Vector2Int.zero, group, chunk);
            switch (globalAnchor)
            {
                case LayoutAnchor.TopLeft:
                    result.position = new Rect(offset.x + group.IndividualMargin.left,
                        offset.y + group.IndividualMargin.top,
                        chunk.Size.x, chunk.Size.y);
                    break;
                case LayoutAnchor.TopRight:
                    result.position = new Rect(offset.x - group.IndividualMargin.left - group.IndividualMargin.right - chunk.Size.x,
                        offset.y + group.IndividualMargin.top,
                        chunk.Size.x, chunk.Size.y);
                    break;
                case LayoutAnchor.BottomRight:
                    result.position = new Rect(offset.x - group.IndividualMargin.left - group.IndividualMargin.right - chunk.Size.x,
                        offset.y - group.IndividualMargin.top - group.IndividualMargin.bottom - chunk.Size.y,
                        chunk.Size.x, chunk.Size.y);
                    break;
                case LayoutAnchor.BottomLeft:
                default:
                    result.position = new Rect(offset.x + group.IndividualMargin.left,
                        offset.y - group.IndividualMargin.top - group.IndividualMargin.bottom - chunk.Size.y,
                        chunk.Size.x, chunk.Size.y);
                    break;
            }

            PivotPoint pivotPoint;
            if (group.UseGroupPivotPointSettings)
                pivotPoint = group.PivotPoint;
            else
                pivotPoint = _slicingSettings.GlobalPivotPoint;


            Vector2Int pivotPointCoords;
            switch (pivotPoint)
            {
                case PivotPoint.Center:
                    pivotPointCoords = toVector2Int(result.position.position + result.position.size * 0.5f);
                    break;
                case PivotPoint.TopLeft:
                    pivotPointCoords = toVector2Int(result.position.position);
                    break;
                case PivotPoint.TopRight:
                    pivotPointCoords = toVector2Int(result.position.position + result.position.size * Vector2.right);
                    break;
                case PivotPoint.BottomLeft:
                    pivotPointCoords = toVector2Int(result.position.position + result.position.size * Vector2.up);
                    break;
                case PivotPoint.BottomRight:
                    pivotPointCoords = toVector2Int(result.position.position + result.position.size);
                    break;
                case PivotPoint.Absolute:
                default:
                    pivotPointCoords = toVector2Int(result.position.position + result.group.AbsolutePivot);
                    break;
            }
            result.pivotPoint = pivotPointCoords;

            return result;
        }

        private Vector2Int drawGroupArea(Vector2Int offset, SpriteGroup group, LayoutAnchor globalAnchor, out (int globalIndex, int groupIndex, string name, Rect position, Vector2Int pivotPoint, SpriteGroup group, SpriteChunk chunk) result)
        {
            var chunk = _slicingSettings.Chunks.Where(ch => ch.Id == group.ChunkId).First();

            result = getFormattedGroupRect(offset, chunk, group, globalAnchor);
            switch (group.Direction)
            {
                case LayoutDirection.Horizontal:
                    offset.x += formatX(chunk.Size.x + group.IndividualMargin.left + group.IndividualMargin.right, globalAnchor);
                    break;
                case LayoutDirection.Vertical:
                    offset.y += formatY(chunk.Size.y + group.IndividualMargin.top + group.IndividualMargin.bottom, globalAnchor);
                    break;
            }
            return offset;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Vector2Int toVector2Int(Vector2 vector2) => new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }
}

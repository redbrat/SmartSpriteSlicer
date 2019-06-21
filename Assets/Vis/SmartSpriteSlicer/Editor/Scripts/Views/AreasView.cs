using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class AreasView : ViewBase
    {
        public AreasView(SmartSpriteSlicerWindow model) : base(model) { }

        public static List<int> IterableCtrlIds = new List<int>();
        public static List<Rect> IterableAreas = new List<Rect>();
        public static int? PreviewedIndex;

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            var globalAnchor = _model.SlicingSettings.LayoutAnchor;
            var offset = getGlobalAnchorPoint(globalAnchor, position);
            offset.x += formatX(_model.SlicingSettings.Offset.x, globalAnchor);
            offset.y += formatY(_model.SlicingSettings.Offset.y, globalAnchor);
            Debug.Log($"offset = {offset}");
            var groups = _model.SlicingSettings.ChunkGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                offset.x += formatX(group.Offset.x, globalAnchor);
                offset.y += formatY(group.Offset.y, globalAnchor);
                //switch (group.Direction)
                //{
                //    case LayoutDirection.Horizontal:
                //        offset.x += formatX(group.Offset.x, globalAnchor);
                //        break;
                //    case LayoutDirection.Vertical:
                //        offset.y += formatY(group.Offset.y, globalAnchor);
                //        break;
                //}
                for (int j = 0; j < group.Times; j++)
                {
                    switch (group.Flavor)
                    {
                        case SpriteGroupFlavor.Group:
                            offset = drawGroupArea(offset, group, globalAnchor);
                            break;
                    }
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

        private Rect getFormattedGroupRect(Vector2Int offset, SpriteChunk chunk, SpriteGroup group, LayoutAnchor globalAnchor)
        {
            switch (globalAnchor)
            {
                case LayoutAnchor.TopLeft:
                    return new Rect(offset.x + group.IndividualMargin.left,
                        offset.y + group.IndividualMargin.top,
                        chunk.Size.x, chunk.Size.y);
                case LayoutAnchor.TopRight:
                    return new Rect(offset.x - group.IndividualMargin.left - group.IndividualMargin.right - chunk.Size.x,
                        offset.y + group.IndividualMargin.top,
                        chunk.Size.x, chunk.Size.y);
                case LayoutAnchor.BottomRight:
                    return new Rect(offset.x - group.IndividualMargin.left - group.IndividualMargin.right - chunk.Size.x,
                        offset.y - group.IndividualMargin.top - group.IndividualMargin.bottom - chunk.Size.y,
                        chunk.Size.x, chunk.Size.y);
                case LayoutAnchor.BottomLeft:
                default:
                    return new Rect(offset.x + group.IndividualMargin.left,
                        offset.y - group.IndividualMargin.top - group.IndividualMargin.bottom - chunk.Size.y,
                        chunk.Size.x, chunk.Size.y);
            }
        }

        private Vector2Int drawGroupArea(Vector2Int offset, SpriteGroup group, LayoutAnchor globalAnchor)
        {
            var chunk = _model.SlicingSettings.Chunks.Where(ch => ch.Id == group.ChunkId).First();

            //var topLeft = new Vector3(offset.x + group.IndividualMargin.left, offset.y + group.IndividualMargin.top);
            //var topRight = new Vector3(topLeft.x + chunk.Size.x, topLeft.y);
            //var bottomLeft = new Vector3(topLeft.x, topLeft.y + chunk.Size.y);
            //var bottomRight = new Vector3(topRight.x, bottomLeft.y);

            var outlineColor = chunk.Color;
            var faceColor1 = chunk.Color;
            faceColor1.a = 0.5f;
            var faceColor0 = faceColor1;
            faceColor0.a = 0.025f;
            //var rect = new Rect(topLeft.x, topRight.y, chunk.Size.x, chunk.Size.y);
            var rect = getFormattedGroupRect(offset, chunk, group, globalAnchor);
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            IterableCtrlIds.Add(controlId);
            IterableAreas.Add(rect);

            var binaryButtonResult = BinaryButton.Draw(GUIContent.none, rect, outlineColor, faceColor0, faceColor1, _model.PreviewedAreaControlId == controlId);
            if (binaryButtonResult.clicked)
            {
                if (_model.PreviewedAreaControlId == controlId)
                {
                    _model.PreviewedAreaControlId = null;
                    _model.PreviewedArea = null;
                    PreviewedIndex = null;
                }
                else
                    _model.PreviewedAreaControlId = controlId;
            }

            if (_model.PreviewedAreaControlId == controlId)
            {
                if (_model.PreviewedAreaControlId == null)
                {
                    _model.PreviewedArea = null;
                    PreviewedIndex = null;
                }
                else
                {
                    _model.PreviewedArea = rect;
                    PreviewedIndex = IterableCtrlIds.Count - 1;
                }
            }

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
    }
}
﻿using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class AreasView : ViewBase
    {
        public AreasView(SmartSpriteSlicerWindow model) : base(model) { }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            var offset = new Vector2Int((int)position.x, (int)position.y);
            var groups = _model.SlicingSettings.ChunkGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                switch (group.Direction)
                {
                    case LayoutDirection.Horizontal:
                        offset.x += group.Offset.x;
                        break;
                    case LayoutDirection.Vertical:
                        offset.y += group.Offset.y;
                        break;
                }
                for (int j = 0; j < group.Times; j++)
                {
                    switch (group.Flavor)
                    {
                        case SpriteGroupFlavor.Group:
                            offset = drawGroupArea(offset, group);
                            break;
                    }
                }
            }
        }

        private Vector2Int drawGroupArea(Vector2Int offset, SpriteGroup group)
        {
            var chunk = _model.SlicingSettings.Chunks.Where(ch => ch.Id == group.ChunkId).First();
            Handles.BeginGUI();

            var topLeft = new Vector3(offset.x + group.IndividualMargin.left, offset.y + group.IndividualMargin.top);
            var topRight = new Vector3(topLeft.x + chunk.Size.x, topLeft.y);
            var bottomLeft = new Vector3(topLeft.x, topLeft.y + chunk.Size.y);
            var bottomRight = new Vector3(topRight.x, bottomLeft.y);

            //Debug.Log($"topLeft = {topLeft}");
            //Debug.Log($"topRight = {topRight}");
            //Debug.Log($"bottomRight = {bottomRight}");
            //Debug.Log($"bottomLeft = {bottomLeft}");
            var outlineColor = chunk.Color;
            var faceColor1 = chunk.Color;
            faceColor1.a = 0.5f;
            var faceColor0 = faceColor1;
            faceColor0.a = 0.025f;
            var rect = new Rect(topLeft.x, topRight.y, chunk.Size.x, chunk.Size.y);
            var controlId = GUIUtility.GetControlID(FocusType.Passive);
            if (BinaryButton.Draw(GUIContent.none, rect, outlineColor, faceColor0, faceColor1).clicked)
            {
                Debug.Log($"Binary Clicked!");
            }
            //Handles.DrawSolidRectangleWithOutline(rect, faceColor, outlineColor);
            //Handles.DrawLine(topLeft, topRight);
            //Handles.DrawLine(topRight, bottomRight);
            //Handles.DrawLine(bottomRight, bottomLeft);
            //Handles.DrawLine(bottomLeft, topLeft);

            Handles.EndGUI();
            switch (group.Direction)
            {
                case LayoutDirection.Horizontal:
                    offset.x += chunk.Size.x + group.IndividualMargin.left + group.IndividualMargin.right;
                    break;
                case LayoutDirection.Vertical:
                    offset.y += chunk.Size.y + group.IndividualMargin.top + group.IndividualMargin.bottom;
                    break;
            }
            return offset;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEditor;
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

            var layout = new Layout(_model.SlicingSettings, position);

            foreach (var area in layout)
            {
                var outlineColor = area.chunk.Color;
                var faceColor1 = area.chunk.Color;
                faceColor1.a = 0.5f;
                var faceColor0 = faceColor1;
                faceColor0.a = 0.1f;
                var controlId = GUIUtility.GetControlID(FocusType.Passive);
                IterableCtrlIds.Add(controlId);
                IterableAreas.Add(area.position);

                var binaryButtonResult = BinaryButton.Draw(GUIContent.none, area.position, outlineColor, faceColor0, faceColor1, _model.PreviewedAreaControlId == controlId);
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
                        _model.PreviewedArea = area.position;
                        PreviewedIndex = IterableCtrlIds.Count - 1;
                    }
                }

                var pivotPoint = default(Vector2Int);
                switch (area.group.PivotPoint)
                {
                    case PivotPoint.Center:
                        pivotPoint = toVector2Int(area.position.position + area.position.size * 0.5f);
                        break;
                    case PivotPoint.TopLeft:
                        pivotPoint = toVector2Int(area.position.position);
                        break;
                    case PivotPoint.TopRight:
                        pivotPoint = toVector2Int(area.position.position + area.position.size * Vector2.right);
                        break;
                    case PivotPoint.BottomLeft:
                        pivotPoint = toVector2Int(area.position.position + area.position.size * Vector2.up);
                        break;
                    case PivotPoint.BottomRight:
                        pivotPoint = toVector2Int(area.position.position + area.position.size);
                        break;
                    case PivotPoint.Absolute:
                    default:
                        pivotPoint = toVector2Int(area.position.position + area.group.AbsolutePivot);
                        break;
                }

                var worldPos = new Vector3(pivotPoint.x, pivotPoint.y, 0);
                var newWorldPos = DraggableDisc.Draw(new Vector3(pivotPoint.x, pivotPoint.y, 0), Vector3.back, 4f, area.chunk.Color);
                if (newWorldPos != worldPos)
                {
                    if (newWorldPos.x < area.position.x)
                        newWorldPos.x = area.position.x;
                    if (newWorldPos.y < area.position.y)
                        newWorldPos.y = area.position.y;
                    if (newWorldPos.x > area.position.x + area.position.width)
                        newWorldPos.x = area.position.x + area.position.width;
                    if (newWorldPos.y > area.position.y + area.position.height)
                        newWorldPos.y = area.position.y + area.position.height;
                    var absolutePivot = toVector2Int(newWorldPos - new Vector3(area.position.position.x, area.position.position.y, 0));

                    var newPivotPoint = PivotPoint.Absolute;
                    if (absolutePivot.x == area.position.size.x && absolutePivot.y == area.position.size.y)
                        newPivotPoint = PivotPoint.BottomRight;
                    else if (absolutePivot.x == 0 && absolutePivot.y == 0)
                        newPivotPoint = PivotPoint.TopLeft;
                    else if (absolutePivot.x == area.position.size.x && absolutePivot.y == 0)
                        newPivotPoint = PivotPoint.TopRight;
                    else if (absolutePivot.x == 0 && absolutePivot.y == area.position.size.y)
                        newPivotPoint = PivotPoint.BottomLeft;
                    else if (absolutePivot.x == Mathf.FloorToInt(area.position.size.x / 2) && absolutePivot.y == Mathf.FloorToInt(area.position.size.y / 2))
                        newPivotPoint = PivotPoint.Center;

                    Undo.RecordObject(_model.SlicingSettings, "Pivot point of group manually changed");
                    _model.SlicingSettings.ChunkGroups[getIndexOf(_model.SlicingSettings.ChunkGroups, area.group)] = area.group.SetPivotPoint(newPivotPoint).SetAbsolutePivot(absolutePivot);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }

                //Handles.BeginGUI();
                //var ctrlId = GUIUtility.GetControlID(FocusType.Passive);
                //var originalColor = Handles.color;
                //Handles.color = makeOpaque(Color.white - area.chunk.Color);
                //Handles.DrawWireDisc(new Vector3(pivotPoint.x, pivotPoint.y, 0), Vector3.back, 4f);
                //Handles.color = area.chunk.Color;
                //Handles.DrawSolidDisc(new Vector3(pivotPoint.x, pivotPoint.y, 0), Vector3.back, 3f);
                //Handles.color = originalColor;
                //Handles.EndGUI();
            }
        }

        private int getIndexOf(List<SpriteGroup> chunkGroups, SpriteGroup group)
        {
            for (int i = 0; i < chunkGroups.Count; i++)
                if (group.Id == chunkGroups[i].Id)
                    return i;
            return -1;
        }

        private Vector2Int toVector2Int(Vector2 vector2) => new Vector2Int(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }
}
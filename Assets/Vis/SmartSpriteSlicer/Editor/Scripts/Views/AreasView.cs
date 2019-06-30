using System.Collections.Generic;
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

            switch (_model.ControlPanelTab)
            {
                case ControlPanelTabs.ManualSlicing:
                    drawManualSlicingAreas(position);
                    break;
                case ControlPanelTabs.ScriptableSclicing:
                    if (!string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText) && 
                        _model.SlicingSettings.ScriptableNodes.Count > 0 && 
                        _model.SlicingSettings.HasWholeSetOfNodes() && 
                        _model.SlicingSettings.HasAllNodesSeparated() && 
                        _model.SlicingSettings.NodesDeepTestPassed())
                        drawScriptableSlicingAreas(position);
                    break;
            }
        }

        private (int hash, (int globalIndex, string name, Rect position, Rect localPosition, Vector2Int pivotPoint, Vector2Int localPivotPoint)[] areas) _nodesAreasCache;
        private void drawScriptableSlicingAreas(Rect position)
        {
            if (_nodesAreasCache.hash != _model.SlicingSettings.ScriptableSlicingLayoutHash || _nodesAreasCache.areas == null)
            {
                var layout = new ScriptableLayout(_model.SlicingSettings, position);
                var areasList = new List<(int globalIndex, string name, Rect position, Rect localPosition, Vector2Int pivotPoint, Vector2Int localPivotPoint)>();
                foreach (var area in layout)
                    areasList.Add(area);
                _nodesAreasCache = (_model.SlicingSettings.ScriptableSlicingLayoutHash, areasList.ToArray());
            }

            var outlineColor = Color.gray;
            var faceColor1 = outlineColor;
            faceColor1.a = 0.5f;
            var faceColor0 = faceColor1;
            faceColor0.a = 0.1f;

            foreach (var area in _nodesAreasCache.areas)
            {
                var controlId = GUIUtility.GetControlID(FocusType.Passive);
                _model.IterableCtrlIds.Add(controlId);
                _model.IterableAreas.Add(area.position);
                _model.IterablePivotPoints.Add(area.pivotPoint);

                var scaledPart = area.position.position - position.position;
                var scaledPos = new Rect(Vector2.Scale(_model.TextureScale, scaledPart), Vector2.Scale(_model.TextureScale, area.position.size));
                scaledPos.position += position.position;

                var binaryButtonResult = BinaryButton.Draw(GUIContent.none, scaledPos, outlineColor, faceColor0, faceColor1, _model.PreviewedAreaControlId == controlId);
                if (binaryButtonResult.clicked)
                {
                    if (_model.PreviewedAreaControlId == controlId)
                    {
                        _model.PreviewedAreaControlId = null;
                        _model.PreviewedArea = null;
                        _model.PreviewedPivotPoint = null;
                        _model.PreviewedGlobalIndex = null;
                        _model.PreviewName = null;
                        _model.PreviewGroup = null;
                        _model.PreviewChunk = null;
                        _model.EditedChunkId = 0;
                    }
                    else
                    {
                        _model.PreviewedAreaControlId = controlId;
                    }
                }

                if (_model.PreviewedAreaControlId == controlId)
                {
                    if (_model.PreviewedAreaControlId == null)
                    {
                        _model.PreviewedArea = null;
                        _model.PreviewedPivotPoint = null;
                        _model.PreviewedGlobalIndex = null;
                        _model.PreviewName = null;
                        _model.PreviewGroup = null;
                        _model.PreviewChunk = null;
                        _model.EditedChunkId = 0;
                    }
                    else
                    {
                        _model.PreviewedArea = area.position;
                        _model.PreviewedPivotPoint = area.pivotPoint;
                        _model.PreviewedGlobalIndex = _model.IterableCtrlIds.Count - 1;
                        _model.PreviewName = area.name;
                    }
                }

                scaledPart = area.pivotPoint - position.position;
                var scaledPivot = Vector2.Scale(_model.TextureScale, scaledPart);
                scaledPivot += position.position;
                var worldPos = new Vector3(scaledPivot.x, scaledPivot.y, 0);
                var newWorldPos = DraggableDisc.Draw(worldPos, Vector3.back, 4f, outlineColor);
            }
        }

        private void drawManualSlicingAreas(Rect position)
        {
            //var layout = new Layout(_model.SlicingSettings, position);
            var layout = new Layout(_model.SlicingSettings, new Rect(position.position, new Vector2Int(_model.Texture.width, _model.Texture.height)));

            foreach (var area in layout)
            {
                var outlineColor = area.chunk.Color;
                var faceColor1 = outlineColor;
                faceColor1.a = 0.5f;
                var faceColor0 = faceColor1;
                faceColor0.a = 0.1f;
                var controlId = GUIUtility.GetControlID(FocusType.Passive);
                _model.IterableCtrlIds.Add(controlId);
                _model.IterableAreas.Add(area.position);
                _model.IterablePivotPoints.Add(area.pivotPoint);
                if (!_model.IterableCtrlIdsToGroupsIds.ContainsKey(controlId))
                    _model.IterableCtrlIdsToGroupsIds.Add(controlId, area.group.Id);

                var scaledPart = area.position.position - position.position;
                var scaledPos = new Rect(Vector2.Scale(_model.TextureScale, scaledPart), Vector2.Scale(_model.TextureScale, area.position.size));
                scaledPos.position += position.position;

                var binaryButtonResult = BinaryButton.Draw(GUIContent.none, scaledPos, outlineColor, faceColor0, faceColor1, _model.PreviewedAreaControlId == controlId);
                if (binaryButtonResult.clicked)
                {
                    if (_model.PreviewedAreaControlId == controlId)
                    {
                        _model.PreviewedAreaControlId = null;
                        _model.PreviewedArea = null;
                        _model.PreviewedPivotPoint = null;
                        _model.PreviewedGlobalIndex = null;
                        _model.PreviewName = null;
                        _model.PreviewGroup = null;
                        _model.PreviewChunk = null;
                        _model.EditedChunkId = 0;
                    }
                    else
                    {
                        _model.PreviewedAreaControlId = controlId;
                        _model.SelectedGroupIndex = getIndexOf(_model.SlicingSettings.ChunkGroups, area.group);
                        _model.EditedChunkId = area.chunk.Id;
                        _model.EditedGroupId = area.group.Id;
                    }
                }

                if (_model.PreviewedAreaControlId == controlId)
                {
                    if (_model.PreviewedAreaControlId == null)
                    {
                        _model.PreviewedArea = null;
                        _model.PreviewedPivotPoint = null;
                        _model.PreviewedGlobalIndex = null;
                        _model.PreviewName = null;
                        _model.PreviewGroup = null;
                        _model.PreviewChunk = null;
                    }
                    else
                    {
                        _model.PreviewedArea = area.position;
                        _model.PreviewedPivotPoint = area.pivotPoint;
                        _model.PreviewedGlobalIndex = _model.IterableCtrlIds.Count - 1;
                        _model.PreviewName = area.name;
                        _model.PreviewGroup = area.group;
                        _model.PreviewChunk = area.chunk;
                    }
                }

                scaledPart = area.pivotPoint - position.position;
                var scaledPivot = Vector2.Scale(_model.TextureScale, scaledPart);
                scaledPivot += position.position;
                var worldPos = new Vector3(scaledPivot.x, scaledPivot.y, 0);
                var newWorldPos = DraggableDisc.Draw(worldPos, Vector3.back, 4f, area.chunk.Color);
                if (newWorldPos != worldPos)
                {
                    scaledPart = newWorldPos.ToVector2Int() - position.position;
                    var unscaledPivot = scaledPart / _model.TextureScale;
                    var newPivot = (position.position + unscaledPivot).ToVector2Int();

                    if (newPivot.x < area.position.x)
                        newPivot.x = Mathf.RoundToInt(area.position.x);
                    if (newPivot.y < area.position.y)
                        newPivot.y = Mathf.RoundToInt(area.position.y);
                    if (newPivot.x > area.position.x + area.position.width)
                        newPivot.x = Mathf.RoundToInt(area.position.x + area.position.width);
                    if (newPivot.y > area.position.y + area.position.height)
                        newPivot.y = Mathf.RoundToInt(area.position.y + area.position.height);
                    var absolutePivot = (newPivot - new Vector2(area.position.position.x, area.position.position.y)).ToVector2Int();

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
                    _model.SlicingSettings.ChunkGroups[getIndexOf(_model.SlicingSettings.ChunkGroups, area.group)] = area.group.SetUseGroupPivotPointSettings(true).SetPivotPoint(newPivotPoint).SetAbsolutePivot(absolutePivot);
                    EditorUtility.SetDirty(_model.SlicingSettings);
                }
            }
        }

        private int getIndexOf(List<SpriteGroup> chunkGroups, SpriteGroup group)
        {
            for (int i = 0; i < chunkGroups.Count; i++)
                if (group.Id == chunkGroups[i].Id)
                    return i;
            return -1;
        }
    }
}
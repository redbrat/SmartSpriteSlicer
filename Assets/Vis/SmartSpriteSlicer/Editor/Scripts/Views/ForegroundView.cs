using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ForegroundView : ViewBase
    {
        public ForegroundView(SmartSpriteSlicerWindow model) : base(model) { }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            var rect = new Rect(Vector2.zero, position.size);

            if (Event.current.type == EventType.MouseUp && rect.Contains(Event.current.mousePosition))
            {
                _model.PreviewedAreaControlId = null;
                _model.PreviewedArea = null;
                _model.PreviewedPivotPoint = null;
                GUI.changed = true;
                Event.current.Use();
            }

            if (_model.PreviewedGlobalIndex.HasValue && Event.current.type == EventType.KeyDown &&
                (Event.current.keyCode == KeyCode.LeftArrow || Event.current.keyCode == KeyCode.RightArrow))
            {
                if (Event.current.keyCode == KeyCode.LeftArrow)
                {
                    _model.PreviewedGlobalIndex = _model.PreviewedGlobalIndex.Value - 1;
                    if (_model.PreviewedGlobalIndex.Value < 0)
                        _model.PreviewedGlobalIndex = _model.IterableCtrlIds.Count - 1;
                    while (!validIterableItem(_model.PreviewedGlobalIndex.Value))
                    {
                        _model.PreviewedGlobalIndex = _model.PreviewedGlobalIndex.Value - 1;
                        if (_model.PreviewedGlobalIndex.Value < 0)
                            _model.PreviewedGlobalIndex = _model.IterableCtrlIds.Count - 1;
                    }
                }
                else
                {
                    _model.PreviewedGlobalIndex = _model.PreviewedGlobalIndex.Value + 1;
                    if (_model.PreviewedGlobalIndex.Value >= _model.IterableCtrlIds.Count)
                        _model.PreviewedGlobalIndex = 0;
                    while (!validIterableItem(_model.PreviewedGlobalIndex.Value))
                    {
                        _model.PreviewedGlobalIndex = _model.PreviewedGlobalIndex.Value + 1;
                        if (_model.PreviewedGlobalIndex.Value >= _model.IterableCtrlIds.Count)
                            _model.PreviewedGlobalIndex = 0;
                    }
                }

                _model.PreviewedAreaControlId = _model.IterableCtrlIds[_model.PreviewedGlobalIndex.Value];
                _model.PreviewedArea = _model.IterableAreas[_model.PreviewedGlobalIndex.Value];
                _model.PreviewedPivotPoint = _model.IterablePivotPoints[_model.PreviewedGlobalIndex.Value];
                if (_model.ControlPanelTab == ControlPanelTabs.ManualSlicing)
                {
                    _model.EditedGroupId = _model.IterableCtrlIdsToGroupsIds[_model.PreviewedAreaControlId.Value];
                    for (int i = 0; i < _model.SlicingSettings.ChunkGroups.Count; i++)
                        if (_model.SlicingSettings.ChunkGroups[i].Id == _model.EditedGroupId)
                            _model.SelectedGroupIndex = i;
                }

                Event.current.Use();
            }
            _model.IterableCtrlIds.Clear();
            _model.IterableAreas.Clear();
            _model.IterableCtrlIdsToGroupsIds.Clear();
            _model.IterablePivotPoints.Clear();
        }

        private bool validIterableItem(int globalIndex)
        {
            switch (_model.IterationMode)
            {
                case SpriteIterationMode.Group:
                    {
                        var layout = new Layout(_model.SlicingSettings, Rect.zero);
                        foreach (var area in layout)
                            if (area.globalIndex == globalIndex)
                                return _model.PreviewGroup.Value.Id == area.group.Id;
                        return false;
                    }
                case SpriteIterationMode.Chunk:
                    {
                        var layout = new Layout(_model.SlicingSettings, Rect.zero);
                        foreach (var area in layout)
                            if (area.globalIndex == globalIndex)
                                return _model.PreviewChunk.Value.Id == area.chunk.Id;
                        return false;
                    }
                case SpriteIterationMode.Global:
                default:
                    return true;
            }
        }
    }
}
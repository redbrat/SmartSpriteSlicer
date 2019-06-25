using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class PreviewSpriteView : ViewBase
    {
        private const int _resizeAreaWidth = 4;

        private readonly SpritePreviewWindow _subWindow;

        private Rect _leftResizeArea;
        private Rect _rightResizeArea;

        private ExtractedPreviewWindow _windowInstanceCache;

        public PreviewSpriteView(SmartSpriteSlicerWindow model) : base(model)
        {
            _subWindow = new SpritePreviewWindow(model);
        }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            if (_model.PreviewedArea == null)
                return;

            if (!SpritePreviewWindow.Extracted)
            {
                if (_windowInstanceCache != null)
                {
                    _windowInstanceCache.Model = null;
                    _windowInstanceCache.Close();
                }
            }
            else if (_windowInstanceCache == null)
            {
                _windowInstanceCache = (ExtractedPreviewWindow)EditorWindow.GetWindow(typeof(ExtractedPreviewWindow), true, _model.GetPreviewTitle());
                _windowInstanceCache.Model = _model;
            }

            if (SpritePreviewWindow.Extracted)
                return;

            _subWindow.WindowPosition = _model.PreviewWindowRect;
            _model.PreviewWindowRect = GUILayout.Window(1, _model.PreviewWindowRect, _subWindow.WindowContentCallback, new GUIContent(_model.GetPreviewTitle()));
            if (_model.PreviewWindowRect.x < 0)
                _model.PreviewWindowRect.x = 0;
            if (_model.PreviewWindowRect.y < 0)
                _model.PreviewWindowRect.y = 0;
            if (_model.PreviewWindowRect.width != SmartSpriteSlicerWindow.MaxPreviewWindowWidth)
                _model.PreviewWindowRect.width = SmartSpriteSlicerWindow.MaxPreviewWindowWidth;
            if (_model.PreviewWindowRect.x >= _model.position.width - _subWindow.WindowWorkaroundRect.width)
                _model.PreviewWindowRect.x = _model.position.width - _subWindow.WindowWorkaroundRect.width;
            if (_model.PreviewWindowRect.y >= _model.position.height - 18)
                _model.PreviewWindowRect.y = _model.position.height - 18;

            //var leftResizeCtrlId = GUIUtility.GetControlID(FocusType.Passive);
            //var rightResizeCtrlId = GUIUtility.GetControlID(FocusType.Passive);

            //if (Event.current.isMouse)
            //{
            //    //var leftResizeArea = new Rect(position.position + _model.PreviewWindowRect.position, _model.PreviewWindowRect.size);
            //    _leftResizeArea = _model.PreviewWindowRect;
            //    _leftResizeArea.width = _resizeAreaWidth;
            //    _leftResizeArea.x -= _resizeAreaWidth / 2;
            //    //var rightResizeArea = new Rect(position.position + _model.PreviewWindowRect.position, _model.PreviewWindowRect.size);
            //    var _rightResizeArea = _model.PreviewWindowRect;
            //    _rightResizeArea.width = _resizeAreaWidth;
            //    _rightResizeArea.x = _rightResizeArea.x + _rightResizeArea.width - _resizeAreaWidth / 2;

            //    var leftState = (ResizeAreaState)GUIUtility.GetStateObject(typeof(ResizeAreaState), leftResizeCtrlId);
            //    var rightState = (ResizeAreaState)GUIUtility.GetStateObject(typeof(ResizeAreaState), rightResizeCtrlId);
            //    switch (Event.current.type)
            //    {
            //        case EventType.MouseDown:
            //            if (_leftResizeArea.Contains(Event.current.mousePosition) && Event.current.button == 0)
            //            {
            //                leftState.IsDragging = true;
            //                leftState.LastPosition = Event.current.mousePosition;
            //                GUIUtility.hotControl = leftResizeCtrlId;
            //            }
            //            else if (_rightResizeArea.Contains(Event.current.mousePosition) && Event.current.button == 0)
            //            {
            //                rightState.IsDragging = true;
            //                rightState.LastPosition = Event.current.mousePosition;
            //                GUIUtility.hotControl = rightResizeCtrlId;
            //            }
            //            break;
            //    }

            //    if (GUIUtility.hotControl == leftResizeCtrlId)
            //    {
            //        var delta = Event.current.mousePosition.x - leftState.LastPosition.x;
            //        _model.PreviewWindowRect.x += delta;
            //        _model.PreviewWindowRect.width -= delta;

            //    }
            //    else if (GUIUtility.hotControl == rightResizeCtrlId)
            //    {
            //        var delta = Event.current.mousePosition.x - leftState.LastPosition.x;
            //        _model.PreviewWindowRect.width += delta;
            //    }
            //}

            //if (Event.current.type == EventType.Repaint)
            //{
            //    EditorGUIUtility.AddCursorRect(_leftResizeArea, MouseCursor.ResizeHorizontal);
            //    EditorGUIUtility.AddCursorRect(_rightResizeArea, MouseCursor.ResizeHorizontal);
            //}

            var yMax = _model.PreviewWindowRect.height;
            _model.PreviewWindowRect.height = 0;

            var windowRect = _model.PreviewWindowRect;
            windowRect.yMax = yMax;

            if (Event.current.type == EventType.MouseUp && windowRect.Contains(Event.current.mousePosition))
                Event.current.Use();
        }
    }
}
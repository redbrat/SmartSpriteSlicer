using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ReorderableBlobListState<T>
    {
        internal bool IsDragging;
        internal Vector2 DraggingPosition;
        internal GUIStyle Style;
        internal Color Color;
        internal GUIContent Content;
        internal Rect Position;
        internal List<T> TempList;
        internal int DraggedIndex;
        //internal List<GUIStyle> CachedStyleList;
        internal List<bool> CachedSelectedList;

        internal void Purge()
        {
            IsDragging = default;
            DraggingPosition = default;
            Style = default;
            Color = default;
            Content = default;
            Position = default;
            TempList = default;
            DraggedIndex = default;
            //CachedStyleList = default;
            CachedSelectedList = default;
        }
    }
}
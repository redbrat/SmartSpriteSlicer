using System;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public enum PivotPoint
    {
        Center = 0,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        Absolute
    }
}
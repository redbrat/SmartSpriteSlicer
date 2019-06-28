using System;

namespace Vis.SmartSpriteSlicer
{
    [Serializable]
    public enum ScriptableNodeType
    {
        Text = 0,
        EndOfLine,
        Name,
        Group,
        X,
        Y,
        Width,
        Height,
        PivotX,
        PivotY
    }
}

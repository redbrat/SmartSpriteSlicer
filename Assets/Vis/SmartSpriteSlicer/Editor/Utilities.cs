using System;
using UnityEngine;

namespace Vis
{
    public enum FloatConvertionMode
    {
        Cast,
        Round,
        Floor,
        Ceil
    }

    public static class Utilities
    {
        public static Vector2Int ToVector2Int(this Vector2 value, FloatConvertionMode mode = FloatConvertionMode.Round)
        {
            switch (mode)
            {
                case FloatConvertionMode.Cast:
                    return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                case FloatConvertionMode.Round:
                    return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                case FloatConvertionMode.Floor:
                    return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                case FloatConvertionMode.Ceil:
                    return new Vector2Int(Mathf.RoundToInt(value.x), Mathf.RoundToInt(value.y));
                default:
                    throw new ApplicationException($"Vis.Utilities. Unknown mode: {mode}");
            }
        }
    }
}

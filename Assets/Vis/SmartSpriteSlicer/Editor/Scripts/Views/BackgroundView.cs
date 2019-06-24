using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class BackgroundView : ViewBase
    {
        public BackgroundView(SmartSpriteSlicerWindow model) : base(model) { }

        public override void OnGUI(Rect position)
        {
            base.OnGUI(position);

            var x0 = position.x;
            var y0 = position.y;
            var size = _model.BackgroundTileSize;
            var xCount = Mathf.CeilToInt(position.width / size);
            var yCount = Mathf.CeilToInt(position.height / size);

            for (int x = 0; x < xCount; x++)
            {
                for (int y = 0; y < yCount; y++)
                {
                    var rect = new Rect(x * size, y * size, size, size);
                    if (Mathf.Abs((x % 2) * -1 + y % 2) == 0)
                        GUI.DrawTexture(rect, _model.WhiteTexture, ScaleMode.StretchToFill);
                }
            }
        }
    }
}
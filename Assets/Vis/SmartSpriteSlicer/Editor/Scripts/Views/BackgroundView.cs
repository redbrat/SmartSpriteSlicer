using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class BackgroundView : ViewBase
    {
        private Texture2D _tile;

        public BackgroundView(SmartSpriteSlicerWindow model) : base(model)
        {
            _tile = new Texture2D(1, 1);
            _tile.SetPixel(0, 0, Color.white);
            _tile.Apply();
        }

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
                    //Debug.Log($"Drawing Tile ({x}, {y})");
                    var rect = new Rect(x * size, y * size, size, size);
                    //Debug.Log($"Drawing Tile ({x}, {y}) rect = {rect}");
                    if (Mathf.Abs((x % 2) * -1 + y % 2) == 0)
                        GUI.DrawTexture(rect, _tile, ScaleMode.StretchToFill);
                }
            }
            //var texture = getTexture(position);
            //GUI.DrawTexture(position, texture);
        }

        //private Texture2D getTexture(Rect position)
        //{
        //    if (_textureInstance == null)
        //        _textureInstance = createTexture(position);
        //    else if (_textureInstance.width != position.width || _textureInstance.height != position.height)
        //    {
        //        Object.DestroyImmediate(_textureInstance);
        //        _textureInstance = createTexture(position);
        //    }
        //    return _textureInstance;

        //}

        //private Texture2D createTexture(Rect position)
        //{
        //    var width = (int)position.width;
        //    var height = (int)position.height;
        //    var result = new Texture2D(width, height, TextureFormat.RGBA4444, false, false);

        //    for (int x = 0; x < width; x++)
        //    {
        //        for (int y = 0; y < height; y++)
        //        {
        //            result.SetPixel(x, y, Mathf.Abs(Mathf.FloorToInt(x / _model.BackgroundCellSize) % 2 + Mathf.FloorToInt(y / _model.BackgroundCellSize) % 2) == 0 ? _model.BackgroundColor1 : _model.BackgroundColor2);
        //        }
        //    }

        //    return result;
        //}
    }
}
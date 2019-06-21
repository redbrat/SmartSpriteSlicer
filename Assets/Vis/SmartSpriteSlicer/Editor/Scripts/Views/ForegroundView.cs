using UnityEditor;
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
                GUI.changed = true;
                Event.current.Use();
            }

            if (AreasView.PreviewedIndex.HasValue && Event.current.type == EventType.KeyDown &&
                (Event.current.keyCode == KeyCode.LeftArrow || Event.current.keyCode == KeyCode.RightArrow))
            {
                if (Event.current.keyCode == KeyCode.LeftArrow)
                {
                    AreasView.PreviewedIndex = AreasView.PreviewedIndex.Value - 1;
                    if (AreasView.PreviewedIndex.Value < 0)
                        AreasView.PreviewedIndex = AreasView.IterableCtrlIds.Count - 1;
                }
                else
                {
                    AreasView.PreviewedIndex = AreasView.PreviewedIndex.Value + 1;
                    if (AreasView.PreviewedIndex.Value >= AreasView.IterableCtrlIds.Count)
                        AreasView.PreviewedIndex = 0;
                }

                _model.PreviewedAreaControlId = AreasView.IterableCtrlIds[AreasView.PreviewedIndex.Value];
                _model.PreviewedArea = AreasView.IterableAreas[AreasView.PreviewedIndex.Value];

                Event.current.Use();
            }
            AreasView.IterableCtrlIds.Clear();
            AreasView.IterableAreas.Clear();
        }
    }
}
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    public abstract class ViewBase
    {
        protected readonly SmartSpriteSlicerWindow _model;

        public ViewBase(SmartSpriteSlicerWindow model)
        {
            _model = model;
        }

        public virtual void OnGUI(Rect position)
        {

        }
    }
}

using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public abstract class ViewBase
    {
        protected readonly SpriteEditorProWindow _model;

        public ViewBase(SpriteEditorProWindow model)
        {
            _model = model;
        }

        public virtual void OnGUI(Rect position)
        {

        }
    }
}

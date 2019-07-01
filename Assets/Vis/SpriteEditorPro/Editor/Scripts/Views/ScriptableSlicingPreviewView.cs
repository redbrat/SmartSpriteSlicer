using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ScriptableSlicingPreviewView : LayoutViewBase
    {
        private readonly LayoutViewBase _top;
        private readonly LayoutViewBase _center;
        private readonly LayoutViewBase _bottom;

        public ScriptableSlicingPreviewView(SmartSpriteSlicerWindow model) : base(model)
        {
            _top = new ScriptableSlisingTopView(model);
            _center = new ScriptableSlisingCenterView(model);
            _bottom = new ScriptableSlisingBottomView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _top.OnGUILayout();
            _center.OnGUILayout();
            if (!string.IsNullOrEmpty(_model.SlicingSettings.ScriptabeSlicingTestText) && 
                _model.SlicingSettings.HasWholeSetOfNodes() && 
                _model.SlicingSettings.HasAllNodesSeparated())
                _bottom.OnGUILayout();
        }
    }
}
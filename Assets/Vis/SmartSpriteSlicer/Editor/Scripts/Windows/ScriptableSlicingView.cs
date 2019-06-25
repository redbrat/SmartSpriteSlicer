using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class ScriptableSlicingView : LayoutViewBase
    {
        private ScriptableSlicingTopView _topView;
        private ScriptableSlicingBlobsView _blobsView;
        private ScriptableSlicingEditView _editView;

        private bool _unfolded;

        public ScriptableSlicingView(SmartSpriteSlicerWindow model) : base(model)
        {
            _topView = new ScriptableSlicingTopView(model);
            _blobsView = new ScriptableSlicingBlobsView(model);
            _editView = new ScriptableSlicingEditView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _topView.OnGUILayout();
            _blobsView.WindowWidth = WindowWidth;
            _blobsView.OnGUILayout();
            if (_model.SlicingSettings.ScriptableNodes.Count(c => c.Id == _model.EditedNodeId) > 0)
                _editView.OnGUILayout();
        }
    }
}
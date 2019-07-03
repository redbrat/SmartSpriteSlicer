using System.Linq;
using UnityEditor;

namespace Vis.SpriteEditorPro
{
    internal class ScriptableSlicingView : LayoutViewBase
    {
        private readonly ScriptableSlicingTopView _topView;
        private readonly ScriptableSlicingBlobsView _blobsView;
        private readonly ScriptableSlicingEditView _editView;
        private readonly ScriptableSlicingPreviewView _previewView;

        private bool _unfolded;

        public ScriptableSlicingView(SpriteEditorProWindow model) : base(model)
        {
            _topView = new ScriptableSlicingTopView(model);
            _blobsView = new ScriptableSlicingBlobsView(model);
            _editView = new ScriptableSlicingEditView(model);
            _previewView = new ScriptableSlicingPreviewView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            _topView.OnGUILayout();
            _blobsView.WindowWidth = WindowWidth;
            _blobsView.OnGUILayout();
            if (_model.SlicingSettings.ScriptableNodes.Count(c => c.Id == _model.EditedNodeId) > 0)
                _editView.OnGUILayout();
            EditorGUILayout.Space();
            _previewView.OnGUILayout();
        }
    }
}
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsView : LayoutViewBase
    {
        public static int EditedGroupId;

        private readonly LayoutViewBase _groupsTopPanel;
        private readonly LayoutViewBase _groupsMainPanel;
        private readonly LayoutViewBase _groupEditPanel;

        private Rect _mainRect;

        public GroupsView(SmartSpriteSlicerWindow model) : base(model)
        {
            _groupsTopPanel = new GroupsTopPanelView(model);
            _groupsMainPanel = new GroupsMainPanelView(model);
            _groupEditPanel = new GroupEditPanelView(model);
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.LabelField(new GUIContent($"<b>Groups</b>", $"Here you can edit groups of chunks, end-of-lines, spaces etc."), _model.RichTextStyle);

            _groupsTopPanel.OnGUILayout();
            
            if (Event.current.type == EventType.Repaint)
            {
                _mainRect = GUILayoutUtility.GetLastRect();
                _mainRect.y = _mainRect.y + _mainRect.height;
            }
            _groupsMainPanel.OnGUILayout();
            if (Event.current.type == EventType.Repaint)
            {
                var lastRect = GUILayoutUtility.GetLastRect();
                _mainRect.height = lastRect.y + lastRect.height - _mainRect.y;
            }
            DragableButton.AcceptDragArea = _mainRect;

            if (_model.SlicingSettings.ChunkGroups.Count(group => group.Id == EditedGroupId) > 0)
                _groupEditPanel.OnGUILayout();
        }
    }
}
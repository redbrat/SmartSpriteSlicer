using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsView : LayoutViewBase
    {
        private readonly LayoutViewBase _groupsTopPanel;
        private readonly LayoutViewBase _groupsMainPanel;
        private readonly LayoutViewBase _groupEditPanel;

        private readonly GUIStyle _richTextStyle;

        private Rect _mainRect;

        public GroupsView(SmartSpriteSlicerWindow model) : base(model)
        {
            _groupsTopPanel = new GroupsTopPanelView(model);
            _groupsMainPanel = new GroupsMainPanelView(model);
            _groupEditPanel = new GroupEditPanelView(model);

            _richTextStyle = model.Skin.GetStyle("RichText");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.LabelField(new GUIContent($"<b>Groups</b>", $"Here you can edit groups of chunks, eofs, spaces etc."), _richTextStyle);

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

            _groupEditPanel.OnGUILayout();
        }
    }
}
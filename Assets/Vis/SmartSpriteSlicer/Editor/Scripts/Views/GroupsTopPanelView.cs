using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Vis.SmartSpriteSlicer
{
    internal class GroupsTopPanelView : LayoutViewBase
    {
        private readonly GUIStyle _panelStyle;
        private readonly GUIStyle _buttonsStyle;

        public GroupsTopPanelView(SmartSpriteSlicerWindow model) : base(model)
        {
            _panelStyle = model.Skin.GetStyle("GroupsTopPanel");
            _buttonsStyle = model.Skin.GetStyle("GroupsTopPanelButton");
        }

        public override void OnGUILayout()
        {
            base.OnGUILayout();

            EditorGUILayout.BeginHorizontal(_panelStyle);
            if (GUILayout.Button(new GUIContent($"Add end of line"), _buttonsStyle, GUILayout.MaxWidth(100f)))
            {
                var lastGroupOffset = _model.SlicingSettings.GetLastGroupOffset();
                if (lastGroupOffset == default)
                    lastGroupOffset.y = 100;
                lastGroupOffset.x = 0;
                Undo.RecordObject(_model.SlicingSettings, "End of line added");
                _model.SlicingSettings.ChunkGroups.Add(new SpriteGroup(_model.SlicingSettings.GetNextGroupId(), SpriteGroupFlavor.EndOfLine, lastGroupOffset));
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            if (GUILayout.Button(new GUIContent($"Add empty space"), _buttonsStyle, GUILayout.MaxWidth(120f)))
            {
                var lastGroupOffset = _model.SlicingSettings.GetLastGroupOffset();
                if (lastGroupOffset == default)
                    lastGroupOffset = Vector2Int.one * 100;
                Undo.RecordObject(_model.SlicingSettings, "Empty space added");
                _model.SlicingSettings.ChunkGroups.Add(new SpriteGroup(_model.SlicingSettings.GetNextGroupId(), SpriteGroupFlavor.EmptySpace, lastGroupOffset));
                EditorUtility.SetDirty(_model.SlicingSettings);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
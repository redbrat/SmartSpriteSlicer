using UnityEditor;
using UnityEngine;

namespace Vis.SpriteEditorPro
{
    public static class EntryPoints
    {
        private const string _editMenuPath = "Assets/Vis/Sprite Editor Pro";

        [MenuItem(_editMenuPath, false)]
        public static void EditWindow()
        {
            var window = EditorWindow.GetWindow<SpriteEditorProWindow>("Sprite Editor Pro", true, typeof(SceneView));
            window.Initialize(Selection.activeObject as Texture2D, GetTextureImporter(Selection.activeObject));
        }

        public static TextureImporter GetTextureImporter(Object @object)
        {
            var assetPath = AssetDatabase.GetAssetPath(@object);
            var textureImporter = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            return textureImporter;
        }

        [MenuItem(_editMenuPath, true)]
        public static bool EditorWindowValidation()
        {
            if (Selection.activeObject == null)
                return false;
            return ValidateObjectIsEditableSprite(Selection.activeObject);
        }

        public static bool ValidateObjectIsEditableSprite(Object @object)
        {
            if (!(@object is Texture2D))
                return false;
            if (!AssetDatabase.IsMainAsset(@object) && !AssetDatabase.IsSubAsset(@object))
                return false;
            if (GetTextureImporter(@object).textureType != TextureImporterType.Sprite)
                return false;
            return true;
        }
    }
}

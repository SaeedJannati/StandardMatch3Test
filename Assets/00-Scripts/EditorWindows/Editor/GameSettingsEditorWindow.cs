using Match3.General;
using Match3.General.MoveTest;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Match3.EditorWindows
{
    public class GameSettingsEditorWindow : OdinMenuEditorWindow
    {
        private static OdinMenuTree tree;

        [MenuItem("Match3/Settings")]
        static void ShowBuildEditor()
        {
            var window = GetWindow<GameSettingsEditorWindow>("Game Settings");
            window.minSize = new Vector2(300, 400);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            tree = new OdinMenuTree();
            var gridConfig = AssetDatabase.LoadAssetAtPath("Assets/Resources/Models/Grid/GridGeneratorModel.asset",
                typeof(GridGeneratorModel));
            tree.Add("Grid Configuration", gridConfig);
            
            var moveTestConfig = AssetDatabase.LoadAssetAtPath(
                "Assets/Resources/Models/MoveTest/MoveTestControllerModel.asset",
                typeof(MoveTestControllerModel));
            tree.Add("Move Test Config", moveTestConfig);
            return tree;
        }
    }
}
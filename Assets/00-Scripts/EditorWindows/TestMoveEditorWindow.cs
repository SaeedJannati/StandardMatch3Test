using System.Collections;
using System.Collections.Generic;
using Match3.General;
using Match3.General.MoveTest;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Match3.EditorWindows
{
    public class TestMoveEditorWindow : OdinMenuEditorWindow
    {
        private static OdinMenuTree tree;

        [MenuItem("Match3/Move Test Configuration")]
        static void ShowBuildEditor()
        {
            var window = GetWindow<TestMoveEditorWindow>("Move Test Configuration");
            window.minSize = new Vector2(400, 200);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            tree = new OdinMenuTree();
            var tab = AssetDatabase.LoadAssetAtPath("Assets/Resources/Models/MoveTest/MoveTestControllerModel.asset",
                typeof(MoveTestControllerModel));
            tree.Add("Move Test Config", tab);
            return tree;
        }
    }
}

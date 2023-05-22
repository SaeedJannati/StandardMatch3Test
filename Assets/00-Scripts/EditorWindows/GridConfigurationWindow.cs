using System.Collections;
using System.Collections.Generic;
using Match3.General;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Match3.EditorWindows
{
    public class GridConfigurationWindow : OdinMenuEditorWindow
    {
        private static OdinMenuTree tree;

        [MenuItem("Match3/Grid Configuration")]
        static void ShowBuildEditor()
        {
            var window = GetWindow<GridConfigurationWindow>("Grid Configuration");
            window.minSize = new Vector2(400, 200);
            window.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            tree = new OdinMenuTree();
            var tab = AssetDatabase.LoadAssetAtPath("Assets/Resources/Models/Grid/GridGeneratorModel.asset",
                typeof(GridGeneratorModel));
            tree.Add("Grid Configuration", tab);
            return tree;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace Match3.Auxiliary
{
    public static class Match3Extensions 
    {
        #region ColourExtentions

        public static string GetHexadecimal(this Color colour)
        {
            return  $"#{GetIntOfChannel(colour.r):X2}{GetIntOfChannel(colour.g):X2}{GetIntOfChannel(colour.b):X2}";
        }

        static int GetIntOfChannel(float value)
        {
            var output = (int)(value * 255);
            
            return output;
        }

        #endregion
        #region IEnumerables

        public static void Shuffle<T>(this List<T> list)
        {
            var rng = new System.Random();
            var n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        #endregion
        #region Transfrom

        public static void ClearChildren(this Transform transform, int fromIndex = 0)
        {
            var childCount = transform.childCount;
            if (childCount == 0)
                return;
            for (int i = childCount - 1; i >= fromIndex; i--)
            {
                GameObject.Destroy(transform.GetChild(i).gameObject);
            }
        }

        #endregion

        #region GameObject

        public static void ClearChildren(this GameObject go, int fromIndex = 0)
        {
            go.transform.ClearChildren(fromIndex);
        }
        #endregion
    }
}


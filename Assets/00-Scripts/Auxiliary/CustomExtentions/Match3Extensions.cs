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
    }
}


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
    }
}


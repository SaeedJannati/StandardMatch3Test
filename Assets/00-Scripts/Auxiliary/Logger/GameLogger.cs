
using UnityEngine;

namespace Match3.Auxiliary
{
    public  static partial class GameLogger
    {
        public static void Log<T>(T log, int r, int g, int b)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), (float)r / 255, (float)g / 255, (float)b / 255);
        }
    
        public static void Log<T>(T log, float r, float g, float b)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(),new Color(r,g,b));
        }
    
        public static void Log<T>(T log, Vector3 colour)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), colour.x, colour.y, colour.z);
        }
        public static void Log<T>(T log, Vector3Int colour)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(), colour.x, colour.y, colour.z);
        }
            
        public static void Log<T>(T log, Color colour)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Log(log.ToString(),colour.GetHexadecimal());
        }
    
        public static void Log<T>(T log, string hexadecimalColour)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            var outPut = $"<color={hexadecimalColour}>{log.ToString()}</color>";
                
            Log(outPut);
        }
    
        public static void Log<T>(T value)
        {
            if(!Debug.unityLogger.logEnabled)
                return;
            Debug.Log(value.ToString());
        }
    }
}


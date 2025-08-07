using UnityEngine;

namespace Common.Script
{
    /// <summary>
    /// Debug.Logラッパー
    /// デバッグビルド時のみログを出力
    /// </summary>
    public static class DebugLogger
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void Log(string message)
        {
            Debug.Log(message);
        }
    
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogWarning(object message)
        {
            Debug.LogWarning(message);
        }
    
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        [System.Diagnostics.Conditional("DEVELOPMENT_BUILD")]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}

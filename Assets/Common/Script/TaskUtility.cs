using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Fire-and-Forgetでタスクを実行するクラス
/// 例外キャッチとログ出力
/// </summary>
public static class TaskUtility
{
    private static readonly bool IsThreadLog = false;
    private static readonly bool IsThreadErrorLog = true;
    
    public static void FireAndForget(Task task, string taskName)
    {
        if (IsThreadLog)
        {
            DebugLogger.Log($"スレッド タスク名:{taskName} : スレッドID:{Thread.CurrentThread.ManagedThreadId} 開始前");
        }
        
        _ = Task.Run(async () =>
        {
            if (IsThreadLog)
            {
                DebugLogger.Log($"スレッド タスク名:{taskName} : スレッドID:{Thread.CurrentThread.ManagedThreadId} 開始");
            }
            
            try
            {
                await task.ConfigureAwait(false);
                if (IsThreadLog)
                {
                    DebugLogger.Log($"スレッド タスク名:{taskName} : スレッドID:{Thread.CurrentThread.ManagedThreadId} 完了");
                }
            }
            catch (Exception ex)
            {
                if (IsThreadErrorLog)
                {
                    DebugLogger.LogError($"スレッド タスク名:{taskName} : スレッドID:{Thread.CurrentThread.ManagedThreadId} 失敗\n{ex.Message}");
                }
            }
        });
    }
}

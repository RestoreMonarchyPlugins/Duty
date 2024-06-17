using System;
using System.Threading;
using Rocket.Core.Logging;
using Rocket.Core.Utils;
using SDG.Unturned;
using Action = System.Action;

namespace RestoreMonarchy.Duty.Helpers;

public class ThreadHelper
{
    public static void Run(Action action, bool asynchronously, string exceptionMessage = null)
    {
        if (asynchronously)
        {
            RunAsynchronously(action, exceptionMessage);
        } else
        {
            RunSynchronously(action);
        }
    }

    public static void RunAsynchronously(Action action, string exceptionMessage = null)
    {
        ThreadPool.QueueUserWorkItem((_) =>
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                RunSynchronously(() => Logger.LogException(e, exceptionMessage));
            }
        });
    }

    public static void RunSynchronously(Action action, float delaySeconds = 0)
    {
        if (ThreadUtil.IsGameThread(Thread.CurrentThread))
        {
            action.Invoke();
            return;
        }

        TaskDispatcher.QueueOnMainThread(action, delaySeconds);
    }
}
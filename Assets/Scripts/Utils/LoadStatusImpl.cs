using System;
using System.Collections.Generic;
using Unity.Loading;

public class LoadStatusImpl : ILoadStatus
{
    private readonly Dictionary<LoadingStatus, Action> statusActions = new();

    public void RegisterAction(LoadingStatus status, Action action)
    {
        if (statusActions.ContainsKey(status))
        {
            statusActions[status] = action;
        }
        else
        {
            statusActions.Add(status, action);
        }
    }

    public void ExecuteAction(LoadingStatus status)
    {
        if (statusActions.ContainsKey(status))
        {
            statusActions[status].Invoke();
        }
    }
}

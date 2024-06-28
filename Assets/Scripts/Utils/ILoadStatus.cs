using System;
using Unity.Loading;

public interface ILoadStatus 
{
    void RegisterAction(LoadingStatus status, Action action);
    void ExecuteAction(LoadingStatus status);
}

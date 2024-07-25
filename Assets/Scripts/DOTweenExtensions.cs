using DG.Tweening;
using Cysharp.Threading.Tasks;

public static class DOTweenExtensions
{
    public static UniTask ToUniTask(this Tween tween)
    {
        var tcs = new UniTaskCompletionSource();

        tween.OnComplete(() => tcs.TrySetResult())
             .OnKill(() => tcs.TrySetCanceled())
             .OnRewind(() => tcs.TrySetCanceled())
             .OnPause(() => tcs.TrySetCanceled())
             .OnStepComplete(() => tcs.TrySetResult())
             .OnWaypointChange((int index) => tcs.TrySetResult());

        return tcs.Task;
    }
}
public enum MainUIStatus
{
    Empty,
    EditScenario,
    WaitEditingScenario,
    EditAction,
    WaitEditingAction,
    ConfirmAction,
    ResponseResult,
    Disconnect,
}

public class MainViewModel : CanvasManager<MainUIStatus>
{
    
}
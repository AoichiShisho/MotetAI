using System;

[Serializable]
public enum MainUIStatus
{
    Empty,
    SelectScenario,
    EditScenario,
    WaitEditingScenario,
    EditAction,
    WaitEditingAction,
    ConfirmAction,
    ResponseResult,
    Disconnect,
}
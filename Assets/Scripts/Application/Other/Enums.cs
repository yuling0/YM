
public enum E_SoundType
{

    UI,
    Sence,
    Character,
    Story,
    Other
}

/// <summary>
/// 事件参数类型
/// </summary>
public enum E_EventArgsType
{
    E_Null,     //没有参数
    E_String,
    E_Int,
    E_Float,
    E_Bool,
    E_Custom
}


public enum E_TimeScaleType
{
    /// <summary>
    /// 受时间缩放
    /// </summary>
    Normal,     
    /// <summary>
    /// 不受时间缩放
    /// </summary>
    UnScaled
}

public enum E_TweenState
{
    InActive,
    Playing,
    Pause,
    Complete,
    Killed
}

public enum E_UnitType
{
    Player,
    Emeny,
    NPC,
    Other
}

/// <summary>
/// 任务状态
/// </summary>
public enum E_RequestState
{
    InActive,
    Underway,
    Completed
}

/// <summary>
/// 时间段
/// </summary>
public enum E_TimeBucket
{
    AM,
    PM,
    Night
}
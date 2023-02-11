namespace NPBehave
{
    public enum Stops
    {
        NONE,
        SELF,
        LOWER_PRIORITY,     //低优先级
        BOTH,               
        IMMEDIATE_RESTART,      //立即重启
        LOWER_PRIORITY_IMMEDIATE_RESTART    //低优先级立即重启
    }
}
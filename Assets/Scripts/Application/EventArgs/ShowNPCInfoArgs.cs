using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShowNPCInfoArgs : EventArgs
{
    private bool isFacingRight;

    public bool IsFacingRight => isFacingRight;

    public static ShowNPCInfoArgs Create(bool isFacingRight)
    {
        ShowNPCInfoArgs args = ReferencePool.Instance.Acquire<ShowNPCInfoArgs>();
        args.isFacingRight = isFacingRight;
        return args;
    }
    public override void Clear()
    {
        isFacingRight = default;
    }
}

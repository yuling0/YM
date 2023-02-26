using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ShowPlayerInfoArgs : EventArgs
{
    private bool isFacingRight;

    public bool IsFacingRight => isFacingRight;

    public static ShowPlayerInfoArgs Create(bool isFacingRight)
    {
        ShowPlayerInfoArgs args = ReferencePool.Instance.Acquire<ShowPlayerInfoArgs>();
        args.isFacingRight = isFacingRight;
        return args;
    }
    public override void Clear()
    {
        isFacingRight = default;
    }
}

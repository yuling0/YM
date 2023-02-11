using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于黑板，避免装拆箱
/// </summary>
public interface ISharedType
{
    
}

public abstract class SharedTypeBase<T> : ISharedType , IReference
{
    public SharedTypeBase()
    {

    }

    public SharedTypeBase(T val)
    {
        this.val = val;
    }

    public T val;
    public T GetValue()
    {
        return val;
    }
    public void SetValue(T val)
    {
        this.val = val;
    }

    public void SetValue(SharedTypeBase<T> st)
    {
        this.val = st.GetValue();
    }

    public abstract void Clear();
}

public class SharedInt : SharedTypeBase<int>
{
    public SharedInt(int val) : base(val)
    {

    }

    public SharedInt()
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedInt)obj);
    }

    public bool Equals(SharedInt val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }

    public static SharedInt CreateSharedInt()
    {
        return ReferencePool.Instance.Acquire<SharedInt>();
    }

    public override void Clear()
    {
        val = 0;
    }

    public static bool operator ==(SharedInt lhs , SharedInt rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedInt lhs, SharedInt rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator <=(SharedInt lhs, SharedInt rhs)
    {
        return lhs.GetValue() <= rhs.GetValue();
    }

    public static bool operator >=(SharedInt lhs, SharedInt rhs)
    {
        return lhs.GetValue() >= rhs.GetValue();
    }

    public static bool operator <(SharedInt lhs, SharedInt rhs)
    {
        return lhs.GetValue() < rhs.GetValue();
    }

    public static bool operator >(SharedInt lhs, SharedInt rhs)
    {
        return lhs.GetValue() > rhs.GetValue();
    }
}

public class SharedFloat : SharedTypeBase<float>
{
    public SharedFloat()
    {
    }

    public SharedFloat(float val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedFloat)obj);
    }

    public bool Equals(SharedFloat val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }

    public static SharedFloat CreateSharedFloat()
    {
        return ReferencePool.Instance.Acquire<SharedFloat>();
    }
    public override void Clear()
    {
        val = 0f;
    }

    public static bool operator ==(SharedFloat lhs, SharedFloat rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedFloat lhs, SharedFloat rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator <=(SharedFloat lhs, SharedFloat rhs)
    {
        return lhs.GetValue() <= rhs.GetValue();
    }

    public static bool operator >=(SharedFloat lhs, SharedFloat rhs)
    {
        return lhs.GetValue() >= rhs.GetValue();
    }

    public static bool operator <(SharedFloat lhs, SharedFloat rhs)
    {
        return lhs.GetValue() < rhs.GetValue();
    }

    public static bool operator >(SharedFloat lhs, SharedFloat rhs)
    {
        return lhs.GetValue() > rhs.GetValue();
    }
}

public class SharedBool : SharedTypeBase<bool>
{
    public SharedBool()
    {
    }

    public SharedBool(bool val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedBool)obj);
    }

    public bool Equals(SharedBool val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }
    public static SharedBool CreateSharedBool()
    {
        return ReferencePool.Instance.Acquire<SharedBool>();
    }
    public override void Clear()
    {
        val = false;
    }

    public static bool operator ==(SharedBool lhs, SharedBool rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedBool lhs, SharedBool rhs)
    {
        return !(lhs == rhs);
    }
}

public class SharedVector2 : SharedTypeBase<Vector2>
{
    public SharedVector2()
    {
    }

    public SharedVector2(Vector2 val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedVector2)obj);
    }

    public bool Equals(SharedVector2 val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }

    public static SharedVector2 CreateSharedVector2()
    {
        return ReferencePool.Instance.Acquire<SharedVector2>();
    }
    public override void Clear()
    {
        val = Vector2.zero;
    }
    public static bool operator ==(SharedVector2 lhs, SharedVector2 rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedVector2 lhs, SharedVector2 rhs)
    {
        return !(lhs == rhs);
    }

    //TODO:这里没有重载向量的运算
}

public class SharedVector3 : SharedTypeBase<Vector3>
{
    public SharedVector3()
    {
    }

    public SharedVector3(Vector3 val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedVector3)obj);
    }

    public bool Equals(SharedVector3 val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }
    public static SharedVector3 CreateSharedVector3()
    {
        return ReferencePool.Instance.Acquire<SharedVector3>();
    }
    public override void Clear()
    {
        val = Vector3.zero;
    }
    public static bool operator ==(SharedVector3 lhs, SharedVector3 rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedVector3 lhs, SharedVector3 rhs)
    {
        return !(lhs == rhs);
    }
}

public class SharedString : SharedTypeBase<string>
{
    public SharedString()
    {
    }

    public SharedString(string val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedString)obj);
    }

    public bool Equals(SharedString val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }
    public static SharedString CreateSharedString()
    {
        return ReferencePool.Instance.Acquire<SharedString>();
    }
    public override void Clear()
    {
        val = default;
    }

    public static bool operator ==(SharedString lhs, SharedString rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedString lhs, SharedString rhs)
    {
        return !(lhs == rhs);
    }
}

public class SharedObject : SharedTypeBase<object>
{
    public SharedObject()
    {
    }

    public SharedObject(object val) : base(val)
    {
    }

    public override bool Equals(object obj)
    {
        return Equals((SharedObject)obj);
    }

    public bool Equals(SharedObject val)
    {
        if (val is null)
        {
            return false;
        }

        if (Object.ReferenceEquals(this, val))
        {
            return true;
        }

        if (val.GetType() != this.GetType())
        {
            return false;
        }
        return this.val == val.val;
    }

    public override int GetHashCode()
    {
        return val.GetHashCode();
    }
    public static SharedObject CreateSharedObject()
    {
        return ReferencePool.Instance.Acquire<SharedObject>();
    }
    public override void Clear()
    {
        val = default;
    }
    public static bool operator ==(SharedObject lhs, SharedObject rhs)
    {
        if (lhs is null)
        {
            if (rhs is null)
            {
                return true;
            }
            return false;
        }
        return lhs.Equals(rhs);
    }

    public static bool operator !=(SharedObject lhs, SharedObject rhs)
    {
        return !(lhs == rhs);
    }
}
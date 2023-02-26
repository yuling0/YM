using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MovementController : ComponentBase
{
    protected Rigidbody2D rig;
    //检测地面相关
    [SerializeField]
    protected bool isGrounded;                        //是否在地面
    [SerializeField]
    protected bool isOnSlope;                           //是否在斜坡

    public Transform groundCheckTF;                   //检测地面的位置坐标

    public float nextCheckGroundInterval = 0.05f;   //下一次检测地面的时间间隔（防止连跳）

    public float checkGroundTime;                  //检测地面的计时器

    public float checkSloepOffsetX;
    public float checkSloepOffsetY;

    public float maxAngle;

    public LayerMask groundMask;                    //地面的层级
    public LayerMask SlopeMask;                    //斜坡的层级

    //移动相关
    protected Vector2 tempVelocity;                   //用于设置刚体速度的变量
    [SerializeField]
    protected Vector2 slopeDir;
    //转向相关
    [SerializeField]
    protected bool isFacingRight = true;                      //是否面向右边
    protected readonly Vector3 scaleVector = new Vector3(-1,1,1); //转向时乘以的缩放量

    //跳跃相关
    public float fallForce = 1f;
    public float shortJumpFallForce = 3f;

    //属性
    public bool IsGrounded => isGrounded;
    [ShowInInspector]
    public bool IsFacingRight => isFacingRight;

    public Vector3 Velocity => rig.velocity;

    public bool IsOnSlope => isOnSlope;

    public override void Init(Core obj, object userData)
    {
        base.Init(obj, userData);
        rig = GetComponent<Rigidbody2D>();
    }

    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        CheckGround();
        CheckSlope();

    }

    public virtual void Move()
    {

 
    }

    public virtual void MoveTowards(float xOffset ,UnityAction onCompleted)
    {

    }

    public virtual void RunTowards(float xOffset, UnityAction onCompleted)
    {

    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public virtual void Jump()
    {
    }

    /// <summary>
    /// 下落的优化（越下落越快）
    /// </summary>
    public virtual void OptimizeFall()
    {
        if (!isGrounded && rig.velocity.y < 0)
        {
            tempVelocity = rig.velocity;

            tempVelocity.y += fallForce * Physics.gravity.y * Time.deltaTime;

            rig.velocity = tempVelocity;
        }
       
    }

    /// <summary>
    /// 人物的转向，这里使用Scale控制转向
    /// </summary>
    /// <param name="h"></param>
    public virtual void Flip()
    {
        //if( isFacingRight && ih.leftPress || !isFacingRight && ih.rightPress)
        {
            transform.localScale = Vector3.Scale(transform.localScale, scaleVector);
            isFacingRight = !isFacingRight;
        }
    }

    public virtual void Flip(UnityAction onCompleted)
    {

    }
    /// <summary>
    /// 检测地面，当跳跃触发时，有一段不可检测的时间
    /// </summary>
    public virtual void CheckGround()
    {
        DrawCheckGroundArea();
    }

    public void  CheckSlope()
    {

        isOnSlope = false;
        if (!isGrounded) return;

        CheckSlopeVertical();
        CheckSlopeHorizontal();
        

    }

    /// <summary>
    /// 检测水平方向斜坡
    /// </summary>
    public virtual void CheckSlopeHorizontal()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, isFacingRight ? Vector2.left : Vector2.right, checkSloepOffsetX, SlopeMask);
        if (hit)
        {
            isOnSlope = true;

            slopeDir = Vector2.Perpendicular(hit.normal).normalized;

            Debug.DrawLine(transform.position, hit.point, Color.green);
            Debug.DrawLine(hit.point, hit.point + slopeDir, Color.black);
        }

        RaycastHit2D hit1 = Physics2D.Raycast(transform.position, isFacingRight ? Vector2.right : Vector2.left, checkSloepOffsetX, SlopeMask);
        if (hit1)
        {
            isOnSlope = true;

            slopeDir = Vector2.Perpendicular(hit1.normal).normalized;

            Debug.DrawLine(transform.position, hit1.point, Color.green);
            Debug.DrawLine(hit1.point, hit1.point + slopeDir, Color.blue);
        }

    }


    /// <summary>
    /// 检测垂直方向斜坡
    /// </summary>
    public virtual void CheckSlopeVertical()
    {
        Vector2 origin = transform.position + (IsFacingRight ? 1 : -1) * checkSloepOffsetX * transform.right;
        origin += Vector2.up * checkSloepOffsetY;
        RaycastHit2D hit = Physics2D.Raycast(origin, - Vector2.up, 0.3f, SlopeMask);
        if(hit)
        {
            maxAngle = Vector2.Angle(hit.normal, Vector2.up);
            isOnSlope = true;
            slopeDir = Vector2.Perpendicular(hit.normal).normalized;
            Debug.DrawLine(origin, origin - Vector2.up * 0.2f, Color.red);
            Debug.DrawLine(hit.point, hit.point + slopeDir, Color.red);
        }
    }

    /// <summary>
    /// 绘制检测地面区域
    /// </summary>
    protected virtual void DrawCheckGroundArea()
    {

    }

    #region 设置速度相关
    public void SetVelocityZore()
    {
        rig.velocity = Vector2.zero;
    }

    public void SetVelocityY(float v)
    {
        tempVelocity = rig.velocity;
        tempVelocity.y = v;
        rig.velocity = tempVelocity;

        isGrounded = false;//防止连跳

        SetNextCheckGroundTime();
    }

    public void SetNextCheckGroundTime()
    {
        checkGroundTime = Time.time + nextCheckGroundInterval;//设置检测地面的时间
    }

    /// <summary>
    /// 设置x的值
    /// </summary>
    /// <param name="v"></param>
    /// <param name="isWithFlip">是否根据朝向设置速度</param>
    public void SetVelocityX(float v,bool isWithFlip = true)
    {
        if(isGrounded && isOnSlope)
        {

            tempVelocity = (isWithFlip ? (isFacingRight ? v : -v) : v) * -slopeDir;
        }
        else
        {
            tempVelocity = rig.velocity;
            tempVelocity.x = isWithFlip ? (isFacingRight ? v : -v) : v;
        }

        rig.velocity = tempVelocity;
    }

    public void SetVelocityXAndFlip(float v, bool isFlip = false)
    {
        tempVelocity = rig.velocity;
        tempVelocity.x = v;
        rig.velocity = tempVelocity;
        if(isFlip)
        {
            if(v > 0 && !isFacingRight || v < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    public void SetVelocity(Vector2 vel)
    {
        rig.velocity = vel;
    }

    public void SetVelocity(float x, float y)
    {
        rig.velocity = new Vector2(x, y);
    }
    #endregion

}

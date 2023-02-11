using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementController : MovementController
{
    private ArcheInfo info;                          //尔茄的信息
    private InputHandler input;
    public PhysicsMaterial2D friction;
    public PhysicsMaterial2D smooth;
    private LayerMask oneWayPlatformLayerMask;
    public Vector2 boxSize;

    public float SlopeSpeedMultiplier = 1.5f;
    private Collider2D[] groundCheckColliders = new Collider2D[5];
    [SerializeField]
    private float acceleration;         //加速度
    [SerializeField]
    private float decceleration;         //减速度
    [SerializeField]
    private float velPower;             //加速度功率

    [SerializeField]
    private float frictionAmount;       //摩擦力
    [SerializeField]
    private float speedDif;
    [SerializeField]
    private float movement;             //平面运动时的力
    [SerializeField]
    public float radius = 0.1f;                //

    private bool isRightJump;
    public override void Init(Core obj)
    {
        base.Init(obj);
        info = obj.info as ArcheInfo;
        input = _core.GetComponentInCore<InputHandler>();
        oneWayPlatformLayerMask = LayerMask.NameToLayer("OneWayPlatform");
    }
    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();

        if (isOnSlope)
        {
            rig.gravityScale = 0f;
        }
        else
        {
            rig.gravityScale = 1f;
        }

        if (input.isCrossPlatform)
        {
            groundMask &= ~(1<<oneWayPlatformLayerMask);
            gameObject.layer = LayerMask.NameToLayer("PenetrablePlayer");
        }
        else
        {
            groundMask |= (1 << oneWayPlatformLayerMask);
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
    public override void OnFixedUpdateComponent()
    {
        Friction();
    }


    //public void Run()
    //{
    //    float targetSpeed = ih.h * info.runSpeed;
    //    if (isSlope)
    //    {
    //        tempVelocity = rig.velocity;
    //        tempVelocity = ih.h * info.runSpeed * -slopeDir.normalized * SlopeSpeedMultiplier;
    //        tempVelocity.x = ih.h * info.runSpeed;
    //        SetVelocity(tempVelocity);

    //    }
    //    else
    //    {
    //        float speedDif = targetSpeed - Velocity.x;
    //        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : decceleration;
    //        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
    //        rig.AddForce(movement * Vector2.right);
    //    }
    //}
    //public void Run()
    //{
    //    float targetSpeed = input.h * info.runSpeed;
    //    if(isOnSlope && input.h != 0)
    //    {
    //        //if (Velocity.magnitude > info.runSpeed)
    //        //{
    //            tempVelocity = input.h * info.runSpeed * -slopeDir.normalized;
    //            SetVelocity(tempVelocity);
    //        //}
    //        //else
    //        //{
    //        //    float speedDif = targetSpeed - Velocity.magnitude;
    //        //    float movement = Mathf.Min(info.runSpeed, Mathf.Abs(speedDif)) * Mathf.Sign(speedDif) * acceleration;
    //        //    rig.AddForce(movement * -slopeDir.normalized);
    //        //}
    //    }
    //    else
    //    {
    //        //if (Velocity.magnitude > info.runSpeed)
    //        //{
    //        //    tempVelocity.x = input.h * info.runSpeed;
    //        //    SetVelocity(tempVelocity);
    //        //}
    //        //else
    //        //{
    //            float speedDif = targetSpeed - Velocity.x;
    //            float movement = Mathf.Min(info.runSpeed, Mathf.Abs(speedDif)) * Mathf.Sign(speedDif) * acceleration;
    //            rig.AddForce(movement * Vector2.right);
    //        //}
    //    }
    //}

    //public void Walk()
    //{
    //    float targetSpeed = input.h * info.walkSpeed;
    //    if (isOnSlope && input.h != 0)
    //    {
    //        //if (Velocity.magnitude > info.walkSpeed)
    //        //{

    //            tempVelocity = input.h * info.walkSpeed * -slopeDir.normalized;
    //            SetVelocity(tempVelocity);
    //        //}
    //        //else
    //        //{
    //        //    float speedDif = targetSpeed - Velocity.magnitude;
    //        //    float movement = Mathf.Min(info.walkSpeed, Mathf.Abs(speedDif)) * Mathf.Sign(speedDif) * acceleration;
    //        //    rig.AddForce(movement * -slopeDir.normalized);
    //        //}
    //    }
    //    else
    //    {
    //        //if (Velocity.magnitude > info.walkSpeed)
    //        //{
    //        //    tempVelocity.x = input.h * info.walkSpeed;
    //        //    SetVelocity(tempVelocity);
    //        //}
    //        //else
    //        //{
    //            float speedDif = targetSpeed - Velocity.x;
    //            float movement = Mathf.Min(info.walkSpeed, Mathf.Abs(speedDif)) * Mathf.Sign(speedDif) * acceleration;
    //            rig.AddForce(movement * Vector2.right);
    //        //}
    //    }
    //}
    public void Run()
    {
        tempVelocity = rig.velocity;
        if (isOnSlope && input.h != 0)
        {
            //有斜坡的移动，使用AddForce的方式的话，会受重力影响，上坡速度与下坡速度不统一，直接改变刚体Velocity上斜坡时，可以无视重力（Velocity.y的量）
            tempVelocity = input.h * info.runSpeed * -slopeDir.normalized * SlopeSpeedMultiplier;
            //tempVelocity.x = ih.h * info.runSpeed;

        }
        else if (isOnSlope)
        {
            float h = IsFacingRight ? 1 : -1;
            float vel = Velocity.magnitude;
            tempVelocity = h * vel * -slopeDir.normalized;
        }
        else if (input.h != 0)
        {
            tempVelocity.x = input.h * info.runSpeed;
        }
        SetVelocity(tempVelocity);
    }

    public void Walk()
    {
        tempVelocity = rig.velocity;
        if (isOnSlope && input.h != 0)
        {
            tempVelocity = input.h * info.walkSpeed * -slopeDir.normalized * SlopeSpeedMultiplier;
            //tempVelocity.x = ih.h * info.walkSpeed;

        }
        else if (isOnSlope)
        {
            float h = IsFacingRight ? 1 : -1;
            float vel = Velocity.magnitude;
            tempVelocity = h * vel * -slopeDir.normalized;
        }
        else if (input.h != 0)
        {
            tempVelocity.x = input.h * info.walkSpeed;
        }
        SetVelocity(tempVelocity);
    }

    public void OptimizeSlopeMovement()
    {
        if (isOnSlope)
        {
            float h = IsFacingRight ? 1 : -1;
            float vel = Velocity.magnitude;
            tempVelocity = h * vel * -slopeDir.normalized;
            SetVelocity(tempVelocity);
        }

    }
    /// <summary>
    /// 跳跃
    /// </summary>
    public override void Jump()
    {
        if (isFacingRight && input.isLeftWalk || !isFacingRight && input.isRightWalk)
        {
            //transform.localScale = Vector3.Scale(transform.localScale, scaleVector);
            //isFacingRight = !isFacingRight;
            Flip();
        }

        if (isGrounded && input.jumpPress)
        {
            tempVelocity.x = rig.velocity.x;
            tempVelocity.y = 0;
            rig.velocity = tempVelocity;
            rig.AddForce(Vector2.up * info.jumpForce);

            isGrounded = false;//防止连跳
            checkGroundTime = Time.time + nextCheckGroundInterval;//设置检测地面的时间
        }
        else if(IsFacingRight && input.h > 0 || !IsFacingRight && input.h <= 0)
        {
            tempVelocity.x = input.h * info.airSpeed;
            tempVelocity.y = rig.velocity.y;
            SetVelocity(tempVelocity);
        }
        isRightJump = IsFacingRight;
        OptimizeJump();
    }

    public void MovementOfSkillInTheAir()
    {
        if (isRightJump && input.h > 0 || !isRightJump && input.h < 0)
        {
            tempVelocity.x = input.h * info.airSpeed;
            tempVelocity.y = rig.velocity.y;
            SetVelocity(tempVelocity);
        }
        OptimizeJump();
    }
    public void Fall()
    {
        if (isFacingRight && input.isLeftWalk || !isFacingRight && input.isRightWalk)
        {
            //transform.localScale = Vector3.Scale(transform.localScale, scaleVector);
            //isFacingRight = !isFacingRight;
            Flip();
        }
        if(/*input.h != 0*/ IsFacingRight && input.h > 0 || !IsFacingRight && input.h <= 0)
        {
            tempVelocity.x = input.h * info.airSpeed;
            tempVelocity.y = rig.velocity.y;
            SetVelocity(tempVelocity);
        }
        if (isGrounded && input.jumpPress)
        {
            tempVelocity.x = rig.velocity.x;
            tempVelocity.y = 0;
            rig.velocity = tempVelocity;
            rig.AddForce(Vector2.up * info.jumpForce);

            isGrounded = false;//防止连跳

            checkGroundTime = Time.time + nextCheckGroundInterval;//设置检测地面的时间
        }

        OptimizeJump();
    }

    /// <summary>
    /// 跳跃的优化，主要优化长跳与短跳
    /// </summary>
    private void OptimizeJump()
    {
        OptimizeFall();

        if (rig.velocity.y > 0 && !input.jumpStay)
        {
            tempVelocity = rig.velocity;

            tempVelocity.y += shortJumpFallForce * Physics.gravity.y * Time.deltaTime;

            rig.velocity = tempVelocity;
        }
    }
    /// <summary>
    /// 添加摩擦力
    /// </summary>
    private void Friction()
    {
        if(isGrounded /*&& (isFacingRight && !input.isRightPress || !isFacingRight && !input.isLeftPress)*/)
        {
            float amount = Mathf.Min(Mathf.Abs(rig.velocity.x), frictionAmount);
            amount *= Mathf.Sign(rig.velocity.x);
            rig.AddForce(Vector2.right * - amount , ForceMode2D.Impulse);
        }
    }

    public void AddFriction(float friction)
    {
        if (isGrounded)
        {
            float amount = Mathf.Min(Mathf.Abs(rig.velocity.x), friction);
            amount *= Mathf.Sign(rig.velocity.x);
            rig.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    /// <summary>
    /// 检测地面，当跳跃触发时，有一段不可检测的时间
    /// </summary>
    public override void CheckGround()
    {
        isGrounded = false;

        if (Time.time < checkGroundTime) return;//检测时间未到

        //int count = Physics2D.OverlapCircleNonAlloc(groundCheck.position, radius, groundCheckColliders, groundMask);
        int count = Physics2D.OverlapBoxNonAlloc(groundCheckTF.position, boxSize , 0 , groundCheckColliders, groundMask);
        if (count > 0)
        {
            for (int i = 0; i < groundCheckColliders.Length; i++)
            {
                if (groundCheckColliders[i].gameObject != gameObject)
                {
                    isGrounded = true;
                    break;
                }
            }
        }
        //DrawUtility.DrawCircle(groundCheck, radius, Color.green);
        DrawUtility.DrawRectangle(groundCheckTF, boxSize, Color.green);
    }

    public void SetFriction()
    {
        rig.sharedMaterial = friction;
    }

    public void SetSmooth()
    {
        rig.sharedMaterial = smooth;
    }

    public void SetDefaultFriction()
    {
        rig.sharedMaterial = null;
    }
}

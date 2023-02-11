using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterosaurMovementController : MovementController
{
    private PterosaurInfo info;
    private Transform playerTF;
    public float sinMultiplier;
    public float waggleMultiplier;
    public Vector2 boxSize;
    private Collider2D[] groundCheckColliders;
    public float g;
    public float gravityVelocity;
    public override void Init(Core obj)
    {
        base.Init(obj);
        playerTF = GameManager.Instance.PlayerTF;
        info = obj.info as PterosaurInfo;
        groundCheckColliders = new Collider2D[3];
        isFacingRight= false;
    }
    public void HorizontalFollow(Vector2 limitRange)
    {
        if (playerTF == null) return;
        Vector2 distance = playerTF.transform.position - transform.position;
        float absX = Mathf.Abs(distance.x);
        tempVelocity.x = 0f;
        if (absX > limitRange.y)    //�������ķ�Χ
        {
            tempVelocity.x = Mathf.Sign(distance.x) * info.moveSpeed.x;
        }
        else if (absX < limitRange.x)   //С����С�ķ�Χ
        {
            tempVelocity.x = - Mathf.Sign(distance.x) * info.moveSpeed.x;
        }
        rig.velocity = tempVelocity;
        Waggle();
    }

    public void VerticalFollow(Vector2 limitRange)
    {
        if (playerTF == null) return;
        float distanceY = transform.position.y - playerTF.position.y;
        if (distanceY < limitRange.x)//С����С�ķ�Χ
        {
            SetVelocityY(info.moveSpeed.y);
        }
        else if (distanceY > limitRange.y) // �������ķ�Χ
        {
            SetVelocityY(-info.moveSpeed.y);
        }
    }

    public bool IsNeedFlip(Vector2 limitRange)
    {
        if (playerTF == null) return false;
        float distanceX = playerTF.transform.position.x - transform.position.x;
        if (Mathf.Abs(distanceX) < limitRange.x && (distanceX > 0 && isFacingRight || distanceX < 0 && !isFacingRight))
        {
            return true;
        }
        
        else if (Mathf.Abs(distanceX) > limitRange.x && (distanceX > 0 && !isFacingRight || distanceX < 0 && isFacingRight))
        {
            return true;
        }
        return false;
    }

    public bool IsXAxisInRange(Vector2 xLimitRange)
    {
        if (playerTF == null) return false;
        float absX = Mathf.Abs(playerTF.transform.position.x - transform.position.x);
        if (absX >= xLimitRange.x && absX <= xLimitRange.y)
        {
            return true;
        }
        return false;
    }
    public bool IsYAxisInRange(Vector2 yLimitRange)
    {
        if (playerTF == null) return false;
        float absY = Mathf.Abs(playerTF.transform.position.y - transform.position.y);
        if (absY >= yLimitRange.x && absY <= yLimitRange.y)
        {
            return true;
        }
        return false;
    }

    public bool IsInCloseCombatRange()
    {
        if (playerTF == null) return false;
        float absX = Mathf.Abs(playerTF.transform.position.x - transform.position.x);
        if (absX < info.CloseCombatMaxDistanceX)
        {
            return true;
        }
        return false;
    }
    public bool IsFacingTarget()
    {
        if (playerTF == null) return false;
        float distanceX = playerTF.transform.position.x - transform.position.x;
        return distanceX >= 0 && isFacingRight || distanceX < 0 && !isFacingRight;
    }

    /// <summary>
    /// ���»ζ�
    /// </summary>
    public void Waggle()
    {
        SetVelocityY(Mathf.Sin(Time.time * sinMultiplier) * waggleMultiplier);
    }
    public void SimulatedGravity()
    {
        if (IsGrounded) return;
        gravityVelocity += g * Time.deltaTime;
        tempVelocity = rig.velocity;
        tempVelocity.y = gravityVelocity;
        rig.velocity = tempVelocity;
    }
    public override void CheckGround()
    {
        base.CheckGround();
        isGrounded = false;
        if (Time.time < checkGroundTime)
        {
            return;
        }

        if (Physics2D.OverlapBoxNonAlloc(groundCheckTF.transform.position,boxSize,0f,groundCheckColliders,groundMask) > 0)
        {
            isGrounded = true;
            gravityVelocity = 0f;
        }
    }

    protected override void DrawCheckGroundArea()
    {
        DrawUtility.DrawRectangle(groundCheckTF, boxSize, Color.red);
    }
}

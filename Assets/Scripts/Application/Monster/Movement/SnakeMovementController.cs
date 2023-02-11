using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeMovementController : MonsterMovementController
{
    private SnakeInfo snakeInfo;

    public Vector2 boxSize;
    public float xOffset;
    public override void Init(Core obj)
    {
        base.Init(obj);
        snakeInfo = obj.info as SnakeInfo;

    }
    public override bool CheckDetectionRange()
    {
        if (Mathf.Acos(Vector2.Dot(distance.normalized,(Vector2)(isFacingRight ? transform.right : -transform.right))) * Mathf.Rad2Deg <= 30f 
            && distance.magnitude <= snakeInfo.detectionRange)
        {
            return true;
        }

        return false;
    }
    public override void CheckGround()
    {
        base.CheckGround();
        isGrounded = false;

        Collider2D[] cs = Physics2D.OverlapBoxAll(groundCheckTF.position, boxSize, 0f, groundMask);

        for (int i = 0; i < cs.Length; i++)
        {
            if (cs[i].gameObject != gameObject)
                isGrounded = true;
        }

    }

    public bool CheckBiteRange()
    {
        return CheckInRange(snakeInfo.biteRange);
    }

    public bool CheckWiteBiteRange()
    {
        return CheckInRange(snakeInfo.wideBiteRange);
    }
    protected override void DrawCheckGroundArea()
    {
        DrawUtility.DrawRectangle(transform, boxSize, Color.white);
    }

}

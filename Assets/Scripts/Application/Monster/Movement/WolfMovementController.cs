using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfMovementController : MonsterMovementController
{
    public Vector2 boxSize;

    public override void CheckGround()
    {
        base.CheckGround();
        isGrounded = false;
        if (Physics2D.OverlapBoxAll(transform.position, boxSize, 0f, groundMask).Length > 0)
        {
            isGrounded = true;
        }
    }

    protected override void DrawCheckGroundArea()
    {
        DrawUtility.DrawRectangle(transform, boxSize, Color.white);

    }
}

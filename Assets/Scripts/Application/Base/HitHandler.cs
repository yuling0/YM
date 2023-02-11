using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : ComponentBase
{
    public Vector2 testBox;
    public Transform hitBox;
    public bool DetectAttackHit(Collider2D[] collider2D , LayerMask mask)
    {
        //collider2D = Physics2D.OverlapBoxAll(jumpKickCheck.position, testBox, 0f,1<<LayerMask.NameToLayer("Wall") 
        //    | 1<< LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Monster"));

        int count = Physics2D.OverlapBoxNonAlloc(hitBox.position, testBox, 0f, collider2D , mask);

        return count > 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(hitBox.position, testBox);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitHandler : ComponentBase
{
    public Vector2 testBox;
    public Transform hitBox;
    public bool DetectAttackHit(RaycastHit2D[] hits , LayerMask mask , out int hitCount)
    {
        //collider2D = Physics2D.OverlapBoxAll(jumpKickCheck.position, testBox, 0f,1<<LayerMask.NameToLayer("Wall") 
        //    | 1<< LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Monster"));

        hitCount = Physics2D.BoxCastNonAlloc(hitBox.position, testBox, 0f, Vector2.right, hits, 0f, mask);

        return hitCount > 0;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(hitBox.position, testBox);
    }
}

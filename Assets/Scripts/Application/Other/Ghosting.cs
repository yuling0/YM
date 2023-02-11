using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色残影
public class Ghosting : UnitLogic
{
    private SpriteRenderer thisRenderer;
    private SpriteRenderer targetRenderer;
    private Transform targetTF;

    public Color color;
    public float duration;
    private float timer;

    public override void OnInit(Unit unit, object userData)
    {
        base.OnInit(unit, userData);
        thisRenderer = GetComponent<SpriteRenderer>();

    }
    public override void OnShow(object userData)
    {
        SpriteRenderer targetRenderer = userData as SpriteRenderer;
        Transform targetTF = targetRenderer.transform;
        thisRenderer.sprite = targetRenderer.sprite;
        thisRenderer.color = color;
        transform.position = targetTF.position;
        transform.localScale= targetTF.lossyScale;
        Invoke("PushPool", duration);

    }
    //public void Enable(SpriteRenderer target)
    //{
    //    targetRenderer = target;
    //    targetTF = target.transform;
    //    thisRenderer.sprite = targetRenderer.sprite;
    //    thisRenderer.color = color;

    //    transform.position = targetTF.position;

    //    transform.localScale = targetTF.lossyScale;

    //    Invoke("PushPool", duration);
    //}

    private void PushPool()
    {
        //PoolMgr.Instance.PushObj(Consts.P_Ghosting, this.gameObject);
        UnitManager.Instance.HideUnit(this.unit,null);
    }
}

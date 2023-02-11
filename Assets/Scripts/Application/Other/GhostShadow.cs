using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色残影
public class GhostShadow : MonoBehaviour
{
    private SpriteRenderer thisRenderer;
    private SpriteRenderer playerRenderer;
    private Transform playerTF;

    public Color color;
    public float duration;
    private float timer;    

    private void Awake()
    {
        thisRenderer = GetComponent<SpriteRenderer>();
        playerTF = GameObject.FindGameObjectWithTag("Player").transform;
        playerRenderer = playerTF.GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        thisRenderer.sprite = playerRenderer.sprite;
        thisRenderer.color = color;

        transform.position = playerTF.position;
        transform.localScale = playerTF.localScale;

        Invoke("PushPool", duration);
    }

    private void PushPool()
    {
        PoolMgr.Instance.PushObj(Consts.P_Ghosting, this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem ps;
    public string path;
    public float duation;
    private void Awake()
    {
        duation = ps.main.duration;
    }
    private void OnEnable()
    {
        ps?.Play();
        Invoke("PushPool", duation);
    }

    private void PushPool()
    {
        PoolMgr.Instance.PushObj<GameObject>(path, this.gameObject);
    }
}

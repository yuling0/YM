using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTest : MonoBehaviour
{
    public Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            animator.Play("FlipState");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.Play("SmashHitState");
        }
    }
}

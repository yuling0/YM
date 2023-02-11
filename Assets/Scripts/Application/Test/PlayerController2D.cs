using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerController2D : CharacterController2D
{

    private Rigidbody2D rig;
    [InlineEditor]
    public ArcheInfo info;                  //尔茄的信息

    [Title("状态变量")]
    public float h;                         //水平输入量
    public Vector2 vel;                     //刚体的速度
    public bool isGrounded;                 //是否在地面
    public bool isFacingRight;              //是否面向右边
    public LayerMask groundMask;            //地面的层级
    public Transform groundCheck;           //检测地面的坐标
    public float radius = 0.02f;                    //检测地面使用的小球半径

    public float nextCheckGroundInterval = 0.05f;   //下一次检测地面的时间间隔（防止连跳）
    public float checkGroundTimer;          //检测地面的计算器

    public bool isJumpPress;
    public bool isJumpStay;
    public bool isRunning;                  //是否为跑步状态
    public bool doubleKey;                  //双击的持续 是否还在

    public float preArrowKeyTime;           //上一次方向键按下的时间
    public float allowMaxInterval = 0.2f;   //跑步的按键的允许的最大时间间隔

    public float fallForce;                 //下落施加的力
    public float shortJumpFallForce;        //短跳时，施加的下落力
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        Init();
    }

    public override void Init()
    {

    }

    private void Update()
    {
        isRunning = false;

        mc.CheckGround();

        h = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);

        isJumpStay = Input.GetKey(KeyCode.C);

        if (Input.GetKeyDown(KeyCode.C))
        {
            isJumpPress = true;

        }


        if(Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.LeftArrow))          //检测是否双击
        {
            if(Time.time - preArrowKeyTime <= allowMaxInterval)
            {
                doubleKey = true;
            }

            preArrowKeyTime = Time.time;
        }

        if (doubleKey &&( Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) //检测双击的效果是否还在持续
        {
            isRunning = true;
        }
        else//不再持续
        {
            doubleKey = false;
        }


        print(doubleKey);
    }
    void FixedUpdate()
    {
        //mc.Move(h, isRunning, isJumpPress, isJumpStay);

        //isJumpPress = false;
    }






}

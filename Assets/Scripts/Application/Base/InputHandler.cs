using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家输入管理者
/// </summary>
public class InputHandler : ComponentBase
{
    private MovementController _mc;
    private InputMgr inputMgr;
    public float h;
    [SerializeField]
    //输出相关
    public bool isLeftRunning;          //是否跑步
    public bool isRightRunning;          //是否跑步

    public bool isWalking;          //是否行走

    public bool jumpPress;          //是否跳跃触发

    public bool jumpStay;           //跳跃是否持续按下

    public bool isCourch;           //是否在下蹲

    public bool isDefend;           //是否在防御

    public bool isLeftFlip;             //是否在左转向
    public bool isRightFlip;             //是否在右转向
    public bool isCrossPlatform;        //是否穿过平台


    private bool leftDoubleKey;
    private bool rightDoubleKey;

    public bool isLeftPress;
    public bool isRightPress;

    public bool isLeftWalk;
    public bool isRightWalk;

    public bool isDrawSwordPress;   //拔剑按键

    public bool isAttackPress;   //攻击按键



    public bool isDownAttack;  //骑士突击 （下 + 攻击键）
    public bool isUpAttack;      //上 + 攻击

    public bool isLeftThrust;   //左突刺    （左 + 攻击键）

    public bool isRightThrust;  //右突刺    （右 + 攻击键）

    public float axisSensitivity;
    public float AbsH => Mathf.Abs(h);
    public override void Init(Core obj, object userData)
    {
        base.Init(obj,userData);
        _mc = obj.GetComponentInCore<PlayerMovementController>();
        inputMgr = InputMgr.Instance;
    }
    public override void OnDisableComponent()
    {
        isLeftRunning = false;
        isRightRunning = false;
        isWalking = false;
        isCourch = false;
        isDefend = false;
        isLeftFlip = false;
        isRightFlip = false;
        isCrossPlatform = false;
        leftDoubleKey = false;
        rightDoubleKey = false;
        isLeftPress = false;
        isRightPress = false;
        isDrawSwordPress = false;
        isAttackPress = false;
        isDownAttack = false;
        isUpAttack = false;
        isLeftThrust = false;
        isRightThrust = false;
        h = 0;
    }

    public override void OnHideUnit(object userData)
    {
        OnDisableComponent();
    }
    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        isLeftRunning = false;
        isRightRunning = false;
        isLeftFlip = false;
        isRightFlip = false;

        isLeftPress = inputMgr.GetKeyStay(Consts.K_Left);
        isRightPress = inputMgr.GetKeyStay(Consts.K_Right);
        isLeftWalk = isLeftPress || inputMgr.GetKeyUpExtend(Consts.K_Left);
        isRightWalk = isRightPress || inputMgr.GetKeyUpExtend(Consts.K_Right);

        if (isRightWalk || rightDoubleKey)
        {
            isLeftWalk = false;
            leftDoubleKey = false;
        }


        if (inputMgr.GetDoubleKey(Consts.K_Right))
        {
            rightDoubleKey = true;
        }
        else if (!isRightWalk && inputMgr.GetDoubleKey(Consts.K_Left))
        {
            leftDoubleKey = true;
        }

        if (rightDoubleKey && (isRightWalk || inputMgr.GetKeyUpExtend(Consts.K_Right)))
        {
            isRightRunning = true;
        }
        else if ( leftDoubleKey && (isLeftWalk || inputMgr.GetKeyUpExtend(Consts.K_Left)))
        {
            isLeftRunning = true;
        }
        else
        {
            leftDoubleKey = false;
            rightDoubleKey = false;
        }
        
        jumpPress = inputMgr.GetKeyDown(Consts.K_Jump);
        jumpStay = inputMgr.GetKeyStay(Consts.K_Jump);
        isCourch = inputMgr.GetKeyStay(Consts.K_Down);
        isDefend = inputMgr.GetKeyStay(Consts.K_Up);
        isDrawSwordPress = inputMgr.GetKeyDown(Consts.K_DrawSword);
        isAttackPress = inputMgr.GetKeyDown(Consts.K_Attack);

        isDownAttack = (inputMgr.GetKeyDown(Consts.K_Down)
            || inputMgr.GetKeyUpExtend(Consts.K_Down))
            && (inputMgr.GetKeyDown(Consts.K_Attack)
            || inputMgr.GetKeyUpExtend(Consts.K_Attack));

        isUpAttack = (inputMgr.GetKeyDown(Consts.K_Up)
        || inputMgr.GetKeyUpExtend(Consts.K_Up))
        && (inputMgr.GetKeyDown(Consts.K_Attack)
        || inputMgr.GetKeyUpExtend(Consts.K_Attack));

        isLeftThrust = (inputMgr.GetKeyDown(Consts.K_Left)
            || inputMgr.GetKeyUpExtend(Consts.K_Left))
            && (inputMgr.GetKeyDown(Consts.K_Attack)
            || inputMgr.GetKeyUpExtend(Consts.K_Attack));

        isRightThrust = (inputMgr.GetKeyDown(Consts.K_Right)
            || inputMgr.GetKeyUpExtend(Consts.K_Right))
            && (inputMgr.GetKeyDown(Consts.K_Attack)
            || inputMgr.GetKeyUpExtend(Consts.K_Attack));

        h = (isRightWalk || isRightRunning) ? 1 : 0 - ((isLeftWalk || isLeftRunning) ? 1 : 0);
        isLeftFlip = _mc.IsFacingRight && h < 0;
        isRightFlip = !_mc.IsFacingRight && h > 0;

        if (isCrossPlatform)
        {
            if (!(jumpStay || inputMgr.GetKeyUpExtend(Consts.K_Jump)))
            {
                isCrossPlatform = false;
            }
        }
        else if (inputMgr.GetKeyStay(Consts.K_Down) && jumpPress)
        {
            isCrossPlatform = true;
        }
        //if (leftPress)
        //{
        //    if (isLeftRunning && Mathf.Abs(-2f - h) < 0.01f)
        //    {
        //        h = -2f;
        //    }
        //    else if (Mathf.Abs(-1f - h) < 0.01f)
        //    {
        //        h = -1f;
        //    }
        //    else
        //    {
        //        h = Mathf.Lerp(h, isLeftRunning ? -2 : -1, axisSensitivity);
        //    }

        //}
        //else if (rightPress)
        //{
        //    if (isRightRunning && Mathf.Abs(2f - h) < 0.01f)
        //    {
        //        h = 2f;
        //    }
        //    else if (Mathf.Abs(1f - h) < 0.01f)
        //    {
        //        h = 1f;
        //    }
        //    else
        //    {
        //        h = Mathf.Lerp(h, isRightRunning ? 2 : 1, axisSensitivity);
        //    }
        //}
        //else
        //{
        //    if (Mathf.Abs(0f - h) < 0.01f)
        //    {
        //        h = 0f;
        //    }
        //    else
        //    {
        //        h = Mathf.Lerp(h, 0, axisSensitivity);
        //    }

        //}
    }

}

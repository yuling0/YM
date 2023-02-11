using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalKeyInfo : KeyInfo
{
    private bool keyDown;       //按键按下
    private bool keyUp;         //按键抬起
    private bool keyStay;       //按键持续按下
    private bool doubleKey;     //按键双击
    private bool keyDownExtend; //按键按下的延伸
    private bool keyUpExtend;     //按键的延伸（特定情况时使用，如尔茄的跑步，跑步按键抬起后，仍有一段时间的移动，所有需要延伸一下这按键的时间）
    private bool keyDelay;      //按键的延迟


    private bool acceptDoubleKey;
    [LabelText("双击间隔时间")]
    public float keyInterval = 0.2f;      //间隔时间
    [LabelText("按键按下的延伸时间")]
    public float keyDownExtendTime = 0.2f;    //需要延伸的时间
    [LabelText("按键抬起的延伸时间")]
    public float keyUpExtendTime = 0.2f;    //需要延伸的时间
    [LabelText("按键的延迟时间")]
    public float keyDelayTime = 0.2f;     //需要延迟的时间

    private float doubleKeyTimer;         //双击计时器

    private float keyDownExtendTimer;         //按下时延伸计时器

    private float keyUpExtendTimer;         //抬起时延伸计时器

    private float keyDelayTimer;          //延迟计时器
    //float preTime;                      //上一次按下的时间

    public bool KeyDown => keyDown;
    public bool KeyUp => keyUp;
    public bool KeyStay => keyStay;
    public bool DoubleKey => doubleKey;

    public bool KeyUpExtend => keyUpExtend;
    public bool KeyDownExtend => keyDownExtend;

    public bool KeyDelay => keyDelay;

    public NormalKeyInfo(KeyCode kc) : base(kc)
    {

    }
    public override void OnUpdate()
    {
        doubleKey = false;
        keyDownExtend = false;
        keyUpExtend = false;
        keyDelay = false;

        keyDown = Input.GetKeyDown(_keyCode);
        keyUp = Input.GetKeyUp(_keyCode);
        keyStay = Input.GetKey(_keyCode);

        CheckDoubleKey();

        CheckKeyExtend();

        CheckKeyDelay();

    }

    private void CheckDoubleKey()
    {
        //判断双击
        if (acceptDoubleKey)
        {
            doubleKeyTimer += Time.deltaTime;
            if (doubleKeyTimer > keyInterval)
            {
                acceptDoubleKey = false;
                doubleKeyTimer = 0f;
            }
            else if (Input.GetKeyDown(_keyCode))
            {
                doubleKey = true;
                doubleKeyTimer = 0f;
            }
        }
        else
        {
            if (Input.GetKeyDown(_keyCode))
            {
                acceptDoubleKey = true;
                doubleKeyTimer = 0f;
            }
        }
    }

    private void CheckKeyDelay()
    {
        //判断按键延迟
        if (Input.GetKeyDown(_keyCode))
        {
            keyDelayTimer = 0f;
        }

        if (Input.GetKey(_keyCode))
        {
            keyDelayTimer += Time.deltaTime;
            if(keyDelayTimer >= keyDelayTime)
            {
                keyDelay = true;
            }
        }
    }

    private void CheckKeyExtend()
    {
        keyUpExtendTimer += Time.deltaTime;
        keyDownExtendTimer += Time.deltaTime;
        //判断按键延伸
        if (Input.GetKeyUp(_keyCode))
        {
            keyUpExtendTimer = 0f;
        }
        if (keyUpExtendTimer <= keyUpExtendTime)
        {
            keyUpExtend = true;
        }

        if (Input.GetKeyDown(_keyCode))
        {
            keyDownExtendTimer = 0f;
        }
        if (keyDownExtendTimer <= keyDownExtendTime)
        {
            keyDownExtend = true;
        }
    }
}

using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillItem
{
    [HorizontalGroup("SkillItem",LabelWidth = 80),Multiline(3)]
    public string skillName;                //技能名称
    [VerticalGroup("SkillItem/InputsInfo")]
    public string inputs;                   //技能搓招输入列表
    private int curIndex;                   //当前输入索引
    [VerticalGroup("SkillItem/InputsInfo")]
    public float inputInterval;             //输入间隔
    [VerticalGroup("SkillItem/InputsInfo")]
    public float resetTime;                 //技能重置时间

    [VerticalGroup("SkillItem/InputsInfo")]
    public float delay;

    [VerticalGroup("SkillItem/InputsInfo")]
    public int priority;                    //技能优先级

    [VerticalGroup("SkillItem/InputsInfo")]
    public bool ignoreObliqueInput;         //是否忽略斜方向上的输入（例如 1 3 7 9）

    [VerticalGroup("SkillItem/Description"), Multiline(3)]
    public string description;

    private float resetTimer;//重设计时器
    private float delayTimer;

    private bool isDelay;
    private bool ok;//技能是否释放成功
    private float intervalTimer;
    public bool OK => ok;

    public void Disable()
    {
        ok = false;
        intervalTimer = 0;
        delayTimer = 0;
        resetTimer = 0f;
    }
    public int CurrentInputDir()
    {

        if (InputMgr.Instance.GetKeyDownExtend(Consts.K_Up))
        {
            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Left))
            {
                return 7;
            }

            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Right))
            {
                return 9;
            }
            return 8;
        }

        if (InputMgr.Instance.GetKeyDownExtend(Consts.K_Down))
        {
            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Left))
            {
                return 1;
            }

            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Right))
            {
                return 3;
            }
            return 2;
        }

        if (InputMgr.Instance.GetKeyDownExtend(Consts.K_Left))
        {
            if(!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Down))
            {
                return 1;
            }

            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Up))
            {
                return 7;
            }
            return 4;
        }

        if (InputMgr.Instance.GetKeyDownExtend(Consts.K_Right))
        {
            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Down))
            {
                return 3;
            }

            if (!ignoreObliqueInput && InputMgr.Instance.GetKeyDownExtend(Consts.K_Up))
            {
                return 9;
            }
            return 6;
        }

        return 5;
    }

    public int CurrentKeyStay()
    {
        if(InputMgr.Instance.GetKeyDown(Consts.K_Attack))
        {
            return 0;
        }
        return -1;
    }

    public int CharToDir(char c)
    {
        if(char.IsDigit(c))
        {
            return c - '0';
        }
        return -1;
    }

    public int CharToHit(char c)
    {
        if (c == 'x') return 0;
        return -2;
    }

    public void OnUpdate()
    {
        if(ok)
        {
            resetTimer -= Time.deltaTime;
            if(resetTimer <= 0)
            {
                //Debug.Log($"{skillName} : 释放成功");
                ok = false;
                curIndex = 0;
                intervalTimer = 0f;
                delayTimer = 0f;
            }
            return;
        }

        if(isDelay)
        {
            delayTimer += Time.deltaTime;
            if(delayTimer >= delay)
            {
                ok = true;
                isDelay = false;
            }
            return;
        }

        intervalTimer += Time.deltaTime;

        if(curIndex > 0 && intervalTimer > inputInterval)
        {
            curIndex = 0;
            intervalTimer = 0f;
        }
        bool match = false;
        int requireDir = CharToDir(inputs[curIndex]);
        int requireHit = CharToHit(inputs[curIndex]);

        //Debug.Log(CurrentInputDir());
        if (CurrentInputDir() == requireDir)
        {
            match = true;
        }
        else if(CurrentKeyStay() == requireHit)
        {
            match = true;
        }

        if (match)
        {
            curIndex++;
            intervalTimer = 0f;
            if(curIndex == inputs.Length)
            {
                isDelay = true;

                resetTimer = resetTime;
            }
        }
    }
}

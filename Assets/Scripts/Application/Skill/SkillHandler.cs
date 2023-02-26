using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : ComponentBase
{
    public SkillData skillData;

    public override void Init(Core obj, object userData)
    {
        base.Init(obj,userData);
        //后续要动态加载SkillData
        skillData = Resources.Load<SkillData>("ScriptableObjects/SkillData");
    }
    public override void OnDisableComponent()
    {
        skillData.Disable();
    }

    public override void OnHideUnit(object userData)
    {
        skillData.Disable();
    }
    /// <summary>
    /// 根据名称查找技能是否完成
    /// </summary>
    /// <param name="skillName"></param>
    /// <returns></returns>
    public bool CheckSkillIsCompelted(string skillName)
    {
        return skillData.CheckSkillIsCompelted(skillName);
    }

    /// <summary>
    /// 根据优先级返回输入技能
    /// </summary>
    /// <returns></returns>
    public string GetCurrentSkillName()
    {
        return skillData.GetCurrentSkillName();
    }

    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        skillData?.OnUpdate();
    }

}

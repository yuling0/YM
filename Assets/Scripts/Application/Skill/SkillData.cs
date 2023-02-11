using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="YM/SkillData",fileName ="SkillData")]
public class SkillData : ScriptableObject
{
    [TableList(NumberOfItemsPerPage = 7,ShowPaging = true)]
    public List<SkillItem> skillItemList = new List<SkillItem>();

    public bool CheckSkillIsCompelted(string skillName)
    {
        var item = GetSkillItem(skillName);
        if (item != null)
        {
            return item.OK;
        }

        Debug.Log($"未找到技能名称为{skillName}的技能");
        return false;
    }

    /// <summary>
    /// 返回当前输入的优先级最高的技能的名称，没有时，返回空字符
    /// </summary>
    /// <returns></returns>
    public string GetCurrentSkillName()
    {
        string curName = "";
        int curPriority = -1;
        for (int i = 0; i < skillItemList.Count; i++)
        {
            SkillItem skill = skillItemList[i];
            if (skill.OK && skill.priority > curPriority)
            {
                curName = skill.skillName;

                curPriority = skill.priority;
            }
        }

        return curName;
    }

    private SkillItem GetSkillItem(string skillName)
    {
        SkillItem skillItem = null;
        for (int i = 0; i < skillItemList.Count; i++)
        {
            if (skillItemList[i].skillName.Equals(skillName))
            {
                skillItem = skillItemList[i];
            }
        }

        return skillItem;
    }

    public void OnUpdate()
    {
        for (int i = 0; i < skillItemList.Count; i++)
        {
            skillItemList[i].OnUpdate();
        }
    }


}

using System.Collections.Generic;
public class PlayerSkillDataContainer : BaseContainer
{
	public Dictionary<int,PlayerSkillData> dic1 = new Dictionary<int,PlayerSkillData>();
	public PlayerSkillData GetPlayerSkillData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

using System.Collections.Generic;
public class SceneSwitchUnitDataContainer : BaseContainer
{
	public Dictionary<int,SceneSwitchUnitData> dic1 = new Dictionary<int,SceneSwitchUnitData>();
	public SceneSwitchUnitData GetSceneSwitchUnitData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

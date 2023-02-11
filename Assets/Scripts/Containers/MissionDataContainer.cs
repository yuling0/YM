using System.Collections.Generic;
public class MissionDataContainer : BaseContainer
{
	public Dictionary<int,MissionData> dic1 = new Dictionary<int,MissionData>();
	public Dictionary<string,MissionData> dic2 = new Dictionary<string,MissionData>();
	public MissionData GetMissionData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
	public MissionData GetMissionData (string key)
	{
		if(dic2.ContainsKey(key))
		{
			return dic2[key];
		}
		return null;
	}
}

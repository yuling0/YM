using System.Collections.Generic;
public class UnitDataContainer : BaseContainer
{
	public Dictionary<int,UnitData> dic1 = new Dictionary<int,UnitData>();
	public Dictionary<string,UnitData> dic2 = new Dictionary<string,UnitData>();
	public UnitData GetUnitData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
	public UnitData GetUnitData (string key)
	{
		if(dic2.ContainsKey(key))
		{
			return dic2[key];
		}
		return null;
	}
}

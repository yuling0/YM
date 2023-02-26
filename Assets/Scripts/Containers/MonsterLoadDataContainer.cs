using System.Collections.Generic;
public class MonsterLoadDataContainer : BaseContainer
{
	public Dictionary<int,MonsterLoadData> dic1 = new Dictionary<int,MonsterLoadData>();
	public MonsterLoadData GetMonsterLoadData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

using System.Collections.Generic;
public class NPCLoadDataContainer : BaseContainer
{
	public Dictionary<int,NPCLoadData> dic1 = new Dictionary<int,NPCLoadData>();
	public NPCLoadData GetNPCLoadData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

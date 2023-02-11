using System.Collections.Generic;
public class StoryEventStoredDataContainer : BaseContainer
{
	public Dictionary<int,StoryEventStoredData> dic1 = new Dictionary<int,StoryEventStoredData>();
	public StoryEventStoredData GetStoryEventStoredData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

using System.Collections.Generic;
public class StoryEventDataContainer : BaseContainer
{
	public Dictionary<int,StoryEventData> dic1 = new Dictionary<int,StoryEventData>();
	public StoryEventData GetStoryEventData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

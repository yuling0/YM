using System.Collections.Generic;
public class StoryConditionDataContainer : BaseContainer
{
	public Dictionary<int,StoryConditionData> dic1 = new Dictionary<int,StoryConditionData>();
	public Dictionary<string,StoryConditionData> dic2 = new Dictionary<string,StoryConditionData>();
	public StoryConditionData GetStoryConditionData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
	public StoryConditionData GetStoryConditionData (string key)
	{
		if(dic2.ContainsKey(key))
		{
			return dic2[key];
		}
		return null;
	}
}

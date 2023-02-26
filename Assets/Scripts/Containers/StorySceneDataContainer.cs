using System.Collections.Generic;
public class StorySceneDataContainer : BaseContainer
{
	public Dictionary<int,StorySceneData> dic1 = new Dictionary<int,StorySceneData>();
	public StorySceneData GetStorySceneData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

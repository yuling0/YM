using System.Collections.Generic;
public class SceneDataContainer : BaseContainer
{
	public Dictionary<int,SceneData> dic1 = new Dictionary<int,SceneData>();
	public Dictionary<string,SceneData> dic2 = new Dictionary<string,SceneData>();
	public SceneData GetSceneData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
	public SceneData GetSceneData (string key)
	{
		if(dic2.ContainsKey(key))
		{
			return dic2[key];
		}
		return null;
	}
}

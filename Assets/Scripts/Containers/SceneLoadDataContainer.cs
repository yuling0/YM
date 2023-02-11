using System.Collections.Generic;
public class SceneLoadDataContainer : BaseContainer
{
	public Dictionary<int,SceneLoadData> dic1 = new Dictionary<int,SceneLoadData>();
	public SceneLoadData GetSceneLoadData (int key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

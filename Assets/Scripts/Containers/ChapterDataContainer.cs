using System.Collections.Generic;
public class ChapterDataContainer : BaseContainer
{
	public Dictionary<string,ChapterData> dic1 = new Dictionary<string,ChapterData>();
	public ChapterData GetChapterData (string key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

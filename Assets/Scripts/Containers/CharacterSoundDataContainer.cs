using System.Collections.Generic;
public class CharacterSoundDataContainer : BaseContainer
{
	public Dictionary<string,CharacterSoundData> dic1 = new Dictionary<string,CharacterSoundData>();
	public CharacterSoundData GetCharacterSoundData (string key)
	{
		if(dic1.ContainsKey(key))
		{
			return dic1[key];
		}
		return null;
	}
}

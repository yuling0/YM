using System.Collections.Generic;
using UnityEngine;
public class CharacterSoundData
{
	/// <summary>
	///持有音效的对象名
	/// </summary>
	public string ownerName;
	/// <summary>
	///拥有的角色音效切片名（多个音效之间用逗号隔开）
	/// </summary>
	public List<string> characterSoundNames;
	/// <summary>
	///其他音效名（如走路、跑步、打铁等的音效,多个音效之间用逗号隔开）
	/// </summary>
	public List<string> otherSoundNames;
	/// <summary>
	/// AudioSource的pitch（通过改变音效播放速度来产生高低音、主要用于尔茄的斩击音效产生高低音）
	/// </summary>
	public float pitch;
	/// <summary>
	///播放的声音大小
	/// </summary>
	public float volume;
	/// <summary>
	///播放的延迟时间
	/// </summary>
	public float delay;
	/// <summary>
	///播放间隔（一般用于需要持续触发的音效，如脚步音）
	/// </summary>
	public float interval;
	/// <summary>
	///播放次数（-1为循环播放）
	/// </summary>
	public int time;
}

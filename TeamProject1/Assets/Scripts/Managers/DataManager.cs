using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface ILoader<T>
{
	T MakeStruct();
}

public class DataManager
{
	public CharacterRedBlackTree Characters
	{
		get;
		private set;
	}

	public CharacterRedBlackTree Bosses
	{
		get;
		private set;
	}

	public List<UltimateSkill> UltimateSkills
	{
		get;
		private set;
	}

	public List<UniqueAttack> UniqueAttacks
	{
		get;
		private set;
	}

	public List<Attack> Attacks
	{
		get;
		private set;
	}

	public List<Stage> Stages 
	{ 
		get;
		private set; 
	}

	public List<Story> Storys
	{
		get;
		private set;
	}

	public ReadWriteData Data
	{
		get;
		private set;
	}

	public void Init()
	{
		Characters = LoadJson<CharacterData, CharacterRedBlackTree>("CharacterData").MakeStruct();
		Bosses = LoadJson<CharacterData, CharacterRedBlackTree>("BossData").MakeStruct();
		UltimateSkills = LoadJson<UltimateSkillData, List<UltimateSkill>>("UltimateSkillData").MakeStruct();
		UniqueAttacks = LoadJson<UniqueAttackData, List<UniqueAttack>>("UniqueAttackData").MakeStruct();
		Attacks = LoadJson<AttackData, List<Attack>>("AttackData").MakeStruct();
		Stages = LoadJson<StageData, List<Stage>>("StageData").MakeStruct();
		Storys = LoadJson<StoryData, List<Story>>("StoryData").MakeStruct();
		Data = ReadJson("Data");
	}

	Loader LoadJson<Loader, T>(string path) where Loader : ILoader<T>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
		return JsonUtility.FromJson<Loader>(textAsset.text);
	}

	ReadWriteData ReadJson(string path)
	{
		string jsonPath = $"{path}.json";
		if (!System.IO.File.Exists(jsonPath))
		{
			ReadWriteData data = new ReadWriteData();
			data.Init();
			WriteJson(path, data);
		}
		string text = System.IO.File.ReadAllText(jsonPath);
		return JsonUtility.FromJson<ReadWriteData>(text);
	}

	public void WriteJson(string path, ReadWriteData data)
	{
		string jsonPath = $"{path}.json";
		string json = JsonUtility.ToJson(data, true);
		System.IO.File.WriteAllText(jsonPath, json);
	}
}

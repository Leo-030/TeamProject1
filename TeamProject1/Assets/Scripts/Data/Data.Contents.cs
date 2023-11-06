using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Diagnostics;

#region Character/Boss

[Serializable]
public class Character
{
	public int id;
	public string name;
	public string description;
	public string job;
	public int str;
	public int strInc;
	public int hp;
	public int hpInc;
	public int gauge;
	public int gaugeInc;
	public int ultimateSkill;
	public int uniqueAttack;
	public int attack;


	public Character(Character c)
	{
		id = c.id;
		name = c.name;
		description = c.description;
		job = c.job;
		str = c.str;
		strInc = c.strInc;
		hp = c.hp;
		hpInc = c.hpInc;
		gauge = c.gauge;
		gaugeInc = c.gaugeInc;
		ultimateSkill = c.ultimateSkill;
		uniqueAttack = c.uniqueAttack;
		attack = c.attack;
	}
}

[Serializable]
public class CharacterData : ILoader<CharacterRedBlackTree>
{
	public List<Character> characters = new List<Character>();

	public CharacterRedBlackTree MakeStruct()
	{
		CharacterRedBlackTree rbt = new CharacterRedBlackTree();
		foreach (Character c in characters)
		{
			rbt.Insert(c);
		}
		return rbt;
	}
}

public class CharacterRedBlackTree
{
	enum Colors
	{
		Red,
		Black,
	}

	class Node
	{
		public Character Data
		{
			get;
			private set;
		} = null;

		public Node Left
		{
			get;
			set;
		} = null;

		public Node Right
		{
			get;
			set;
		} = null;

		public Colors ColorRB
		{
			get;
			set;
		}

		public Node(Character data)
		{
			Data = data;
			ColorRB = Colors.Red;
		}
	}

	Node _root = null;

	public Character SearchById(int id)
	{
		Node current = _root;
		while (current != null)
		{
			if (id == current.Data.id)
			{
				return current.Data;
			}
			else if (id < current.Data.id)
			{
				current = current.Left;
			}
			else
			{
				current = current.Right;
			}
		}

		return null;
	}

	public bool Search(Character data)
	{
		Node current = _root;
		while (current != null)
		{
			if (data.id == current.Data.id)
			{
				return true;
			}
			else if (data.id < current.Data.id)
			{
				current = current.Left;
			}
			else
			{
				current = current.Right;
			}
		}

		return false;
	}

	public void Insert(Character data)
	{
		if (Search(data))
			return;

		_root = insert(_root, data);
		_root.ColorRB = Colors.Black;
	}

	bool isRed(Node node)
	{
		if (node == null)
			return false;
		return node.ColorRB == Colors.Red;
	}

	Node insert(Node root,Character data)
	{
		if (root == null)
			return new Node(data);

		if (data.id < root.Data.id)
			root.Left = insert(root.Left, data);
		else if (data.id > root.Data.id)
			root.Right = insert(root.Right, data);
		else
			return root;

		if (isRed(root.Right) && !isRed(root.Left))
			root = rotateLeft(root);
		if (isRed(root.Left) && isRed(root.Left.Left))
			root = rotateRight(root);
		if (isRed(root.Left) && isRed(root.Right))
			flipColors(root);

		return root;
	}

	Node rotateLeft(Node node)
	{
		Node x = node.Right;
		node.Right = x.Left;
		x.Left = node;
		x.ColorRB = node.ColorRB;
		node.ColorRB = Colors.Red;
		return x;
	}

	Node rotateRight(Node node)
	{
		Node x = node.Left;
		node.Left = x.Right;
		x.Right = node;
		x.ColorRB = node.ColorRB;
		node.ColorRB = Colors.Red;
		return x;
	}

	void flipColors(Node node)
	{
		if (node.ColorRB == Colors.Red)
			node.ColorRB = Colors.Black;
		else
			node.ColorRB = Colors.Red;

		if (node.Left.ColorRB == Colors.Red)
			node.Left.ColorRB = Colors.Black;
		else
			node.Left.ColorRB = Colors.Red;

		if (node.Right.ColorRB == Colors.Red)
			node.Right.ColorRB = Colors.Black;
		else
			node.Right.ColorRB = Colors.Red;
	}

	public List<Character> Inorder()
	{
		if (_root == null)
		{
			return null;
		}

		return inorder(_root);
	}

	private List<Character> inorder(Node root)
	{
		if (root == null)
		{
			return null;
		}

		List<Character> leftList = inorder(root.Left);
		if (leftList == null)
		{
			leftList = new List<Character>();
		}

		leftList.Add(root.Data);

		List<Character> rightList = inorder(root.Right);
		if (rightList != null)
		{
			leftList.AddRange(rightList);
		}
		return leftList;
	}
}

#endregion

#region UltimateSkill

[Serializable]
public class UltimateSkill
{
	public int id;
	public string name;
	public string description;
}

[Serializable]
public class UltimateSkillData : ILoader<List<UltimateSkill>>
{
	public List<UltimateSkill> ultimateSkills;

	public List<UltimateSkill> MakeStruct()
	{
		return ultimateSkills;
	}
}

#endregion

#region UniqueAttack

[Serializable]
public class UniqueAttack
{
	public int id;
	public string name;
	public string description;
}

[Serializable]
public class UniqueAttackData : ILoader<List<UniqueAttack>>
{
	public List<UniqueAttack> uniqueAttacks;

	public List<UniqueAttack> MakeStruct()
	{
		return uniqueAttacks;
	}
}

#endregion

#region Attack

[Serializable]
public class Attack
{
	public int id;
	public string name;
	public string description;
}

[Serializable]
public class AttackData : ILoader<List<Attack>>
{
	public List<Attack> attacks;

	public List<Attack> MakeStruct()
	{
		return attacks;
	}
}

#endregion

#region Stage

[Serializable]
public class StageCharacter
{
	public int id;
	public int star;
}

[Serializable]
public class Stage
{
	public List<StageCharacter> monsters;
	public List<StageCharacter> bosses;
	public int storyNum;
}

[Serializable]
public class StageData : ILoader<List<Stage>>
{
	public List<Stage> stages;

	public List<Stage> MakeStruct()
	{
		return stages;
	}
}

#endregion

#region Story

[Serializable]
public class Story
{
	public int id;
	public List<string> texts;
	public string bgm;
}

[Serializable]
public class StoryData : ILoader<List<Story>>
{
	public List<Story> storys;

	public List<Story> MakeStruct()
	{
		return storys;
	}
}

#endregion

#region ReadWrite

[Serializable]
public class HaveChar
{
	public Character character;
	public int star;
}

[Serializable]
public class AudioVolume
{
	public float master;
	public List<float> audios;
}

[Serializable]
public class PlayerInfo
{
	public bool tutorial;
	public int money;
	public int gacha;
	public AudioVolume audioVolume;
	public int stage;
	public List<HaveChar> characters;

	public void Init()
	{
		tutorial = false;
		money = 0;
		gacha = 0;
		audioVolume = new AudioVolume();
		audioVolume.master = 1.0f;
		audioVolume.audios = new List<float>();
		for (int i = 0; i < (int)Define.Sound.MaxCount; i++)
		{
			audioVolume.audios.Add(1.0f);
		}
		stage = 0;
		characters = new List<HaveChar>();
	}
}

[Serializable]
public class ReadWriteData
{
	public PlayerInfo player;

	public void Init()
	{
		player = new PlayerInfo();
		player.Init();
	}
}

#endregion
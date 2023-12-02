using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FightManagers : MonoBehaviour
{
	class RingBuffer
	{
		class Node
		{
			public Node next;
			public FightChar data;

			public Node(FightChar data)
			{
				this.data = data;
				next = null;
			}
		}

		int length;
		Node current;

		public RingBuffer(List<FightChar> _chars, List<FightChar> _monsters)
		{
			length = _chars.Count + _monsters.Count;
			current = new Node(_chars[0]);
			Node pre = current;
			int i = 1;
			int j = 0;
			while (i < _chars.Count || j < _monsters.Count)
			{
				if (i < _chars.Count && j < _monsters.Count)
				{
					Node node = new Node(_monsters[j]);
					pre.next = node;
					pre = pre.next;
					node = new Node(_chars[i]);
					pre.next = node;
					pre = pre.next;
					i++;
					j++;
				}
				else if (i < _chars.Count)
				{
					Node node = new Node(_chars[i]);
					pre.next = node;
					pre = pre.next;
					i++;
				}
				else
				{
					Node node = new Node(_monsters[j]);
					pre.next = node;
					pre = pre.next;
					j++;
				}
			}
			pre.next = current;
		}

		public void Next()
		{
			Node c = current.next;
			while (c.data == null)
			{
				c = c.next;
			}
			current = c;
		}

		public FightChar GetCurrent()
		{
			return current.data;
		}

		public void Die(FightChar c)
		{
			Node node = current;
			for (int i = 0; i < length; i++)
			{
				if (node.data == c)
				{
					node.data = null;
				}
				node = node.next;
			}
		}
	}

	static FightManagers s_instance;

	public static FightManagers Instance
	{
		get
		{
			Init();
			return s_instance;
		}
	}

	ActionManager _action = new ActionManager();

	public ActionManager ActionMg
	{
		get
		{
			return s_instance._action;
		}
	}

	int _stage;
	List<FightChar> _chars;
	List<FightChar> _monsters;
	RingBuffer _buffer;


	void Start()
	{
		Init();
	}

	void Update()
	{

	}

	static void Init()
	{
		if (s_instance == null)
		{
			GameObject obj = GameObject.Find("@FightManagers");
			if (obj == null)
			{
				obj = new GameObject { name = "@FightManagers" };
				obj.AddComponent<FightManagers>();
			}

			DontDestroyOnLoad(obj);
			s_instance = obj.GetComponent<FightManagers>();
		}
	}

	public void SetStage(int num)
	{
		_stage = num;
	}

	public void SetParty()
	{
		UI_Set_Party sp = Managers.UI.ShowPopupUI<UI_Set_Party>();
		sp.SetAction = setPartyList;
	}

	void setPartyList(List<HaveChar> clist)
	{
		_chars = new List<FightChar>();
		foreach (HaveChar c in clist)
		{
			FightChar newChar = new FightChar(c, true);
			_chars.Add(newChar);
		}
		_monsters = new List<FightChar>();
		Stage stage = Managers.Data.Stages[_stage - 1];
		foreach (StageCharacter c in stage.bosses)
		{
			HaveChar newhc = new HaveChar();
			newhc.character = new Character(Managers.Data.Bosses.SearchById(c.id));
			newhc.star = c.star;
			FightChar newChar = new FightChar(newhc, false, "Bosses");
			_monsters.Add(newChar);
		}
		foreach (StageCharacter c in stage.monsters)
		{
			HaveChar newhc = new HaveChar();
			newhc.character = new Character(Managers.Data.Characters.SearchById(c.id));
			newhc.star = c.star;
			FightChar newChar = new FightChar(newhc, false);
			_monsters.Add(newChar);
		}
		ReadyFight();
	}

	public void ReadyFight()
	{
		for (int i = 0; i < _monsters.Count; i++)
		{
			_monsters[i].MoveObj(i, _monsters.Count - 1);
			_monsters[i].OnUpdateHp();
			_monsters[i].OnUpdateGauge();
		}

		for (int i = 0; i < _chars.Count; i++)
		{
			_chars[i].MoveObj(i);
			_chars[i].OnUpdateHp();
			_chars[i].OnUpdateGauge();
		}

		_buffer = new RingBuffer(_chars, _monsters);
		StartFight();
	}

	public void StartFight()
	{
		FightChar current = _buffer.GetCurrent();
		current.SetTurn();
		if (current.IsPlayer)
		{
			UI_Player_Turn obj = Managers.UI.ShowPopupUI<UI_Player_Turn>();
			obj.SetInfo(current);
		}
		else
		{
			// 몬스터 매커니즘 발동
			if (ActionMg.CheckUltimateSkill(current.UltimateSkillId, current.Gauge))
			{
				if (ActionMg.CheckSelect(current.UltimateSkillId, "UltimateSkill"))
				{
					List<FightChar> list;
					if (ActionMg.CheckToOurs(current.UltimateSkillId, "UltimateSkill"))
					{
						list = GetEnemy();
					}
					else
					{
						list = GetOurs();	
					}
					float[] floats = new float[list.Count];
					for (int i = 0; i < floats.Length; i++)
					{
						floats[i] = 1.0f;
					}
					FightChar to = list[Util.Choose(floats)];
					ActionMg.Act(current.UltimateSkillId, "UltimateSkill", current, to);
				}
				else
				{
					ActionMg.Act(current.UltimateSkillId, "UltimateSkill", current);
				}
			}
			else
			{
				float[] prob = { 1.0f, 1.0f };
				int random = Util.Choose(prob);
				if (random == 0)
				{
					if (ActionMg.CheckSelect(current.UniqueAttackId, "UniqueAttack"))
					{
						List<FightChar> list;
						if (ActionMg.CheckToOurs(current.UniqueAttackId, "UniqueAttack"))
						{
							list = GetEnemy();
						}
						else
						{
							list = GetOurs();
						}
						float[] floats = new float[list.Count];
						for (int i = 0; i < floats.Length; i++)
						{
							floats[i] = 1.0f;
						}
						FightChar to = list[Util.Choose(floats)];
						ActionMg.Act(current.UniqueAttackId, "UniqueAttack", current, to);
					}
					else
					{
						ActionMg.Act(current.UniqueAttackId, "UniqueAttack", current);
					}
				}
				else
				{
					if (ActionMg.CheckSelect(current.AttackId, "Attack"))
					{
						List<FightChar> list;
						if (ActionMg.CheckToOurs(current.AttackId, "Attack"))
						{
							list = GetEnemy();
						}
						else
						{
							list = GetOurs();
						}
						float[] floats = new float[list.Count];
						for (int i = 0; i < floats.Length; i++)
						{
							floats[i] = 1.0f;
						}
						FightChar to = list[Util.Choose(floats)];
						ActionMg.Act(current.AttackId, "Attack", current, to);
					}
					else
					{
						ActionMg.Act(current.AttackId, "Attack", current);
					}
				}
			}
		}
	}

	public List<FightChar> GetEnemy()
	{
		return _monsters;
	}

	public List<FightChar> GetOurs()
	{
		return _chars;
	}

	public bool CheckDeath(FightChar c, List<FightChar> removelist = null)
	{
		if (c.CheckDeath())
		{
			c.Die();
			if (removelist != null)
			{
				removelist.Add(c);
			}
			else
			{
				if (c.IsPlayer)
				{
					_chars.Remove(c);
				}
				else
				{
					_monsters.Remove(c);
				}
			}
			_buffer.Die(c);
			return true;
		}
		return false;
	}

	public bool EndGame()
	{
		if (_chars.Count == 0)
		{
			Managers.UI.Clear();
			Managers.UI.ShowPopupUI<UI_Defeat>();
			return true;
		}

		if (_monsters.Count == 0)
		{
			bool isStory = false;
			if (Managers.Data.Data.player.stage < _stage)
			{
				Managers.Data.Data.player.stage = _stage;
				isStory = true;
			}
			// 승리시 받는 보상 수정시 여기 수정
			Managers.Data.Data.player.money += 4 * _stage + 10;
			Managers.Data.WriteJson("Data", Managers.Data.Data);
			Managers.UI.Clear();
			if (isStory)
			{
				Managers.UI.ShowPopupUI<UI_Win>().SetInfo(Managers.Data.Stages[_stage - 1].storyNum);
			}
			else
			{
				Managers.UI.ShowPopupUI<UI_Win>().SetInfo(0);
			}
			return true;
		}

		return false;
	}

	public void Damage(int damage, FightChar fightChar, bool isLast = true)
	{
		List<FightChar> list = new List<FightChar>();
		if (fightChar.IsPlayer)
		{
			foreach (FightChar c in _chars)
			{
				if (c.CheckFirstHit())
				{
					list.Add(c);
				}
			}
		}
		else
		{
			foreach (FightChar c in _monsters)
			{
				if (c.CheckFirstHit())
				{
					list.Add(c);
				}
			}
		}
		if (list.Count == 0)
		{
			if (!fightChar.CheckBlockAndDec())
			{
				fightChar.Damage(damage);
				fightChar.HitMotion();
				if (!CheckDeath(fightChar))
				{
					ActionMg.PrintLog($"{fightChar.LogName}이/가 {damage}만큼 피해를 입었습니다.", (isLast == true));
					fightChar.OnUpdateHp();
				}
				else
				{
					ActionMg.PrintLog($"{fightChar.LogName}이/가 죽었습니다.", (isLast == true));
					ActionMg.PrintLog($"{fightChar.LogName}이/가 {damage}만큼 피해를 입었습니다.", false);
				}
			}
			else
			{
				ActionMg.PrintLog($"{fightChar.LogName}이/가 방어하였습니다.", (isLast == true));
			}
		}
		else
		{
			float[] floats = new float[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				floats[i] = 1.0f;
			}
			int id = Util.Choose(floats);
			list[id].DecFirstHit();
			if (!list[id].CheckBlockAndDec())
			{
				list[id].Damage(damage);
				list[id].HitMotion();
				if (!CheckDeath(list[id]))
				{
					ActionMg.PrintLog($"{list[id].LogName}이/가 {damage}만큼 피해를 입었습니다.", true);
					list[id].OnUpdateHp();
				}
				else
				{
					ActionMg.PrintLog($"{list[id].LogName}이/가 죽었습니다.", true);
					ActionMg.PrintLog($"{list[id].LogName}이/가 {damage}만큼 피해를 입었습니다.", false);
				}
			}
			else
			{
				ActionMg.PrintLog($"{list[id].LogName}이/가 방어하였습니다.", true);
			}
			ActionMg.PrintLog($"{list[id].LogName}이/가 도발을 통해 {fightChar.LogName} 대신 공격을 받았습니다.", false);
		}
	}

	public void DamageGroup(int damage, int damage2, FightChar to)
	{
		List<FightChar> list;
		if (to.IsPlayer)
		{
			list = _chars;
		}
		else
		{
			list = _monsters;
		}

		int index = -2;
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == to)
			{
				index = i;
				break;
			}
		}

		if (index - 1 >= 0 && index + 1 < list.Count)
		{
			Damage(damage2, list[index - 1], true);
			Damage(damage2, list[index + 1], false);
		}
		else if (index - 1 >= 0)
		{
			Damage(damage2, list[index - 1], true);
		}
		else if (index + 1 < list.Count)
		{
			Damage(damage2, list[index + 1], true);
		}
		else
		{
			ActionMg.PrintLog($"{to.LogName}의 양옆에 아무도 없어 피해를 주지 못하였습니다.", true);
		}
		Damage(damage, to, false);
	}

	public void DamageAll(int damage, bool isPlayer)
	{
		if (isPlayer)
		{
			List<FightChar> list = new List<FightChar>();
			for (int i = 0; i < _chars.Count; i++)
			{
				FightChar c = _chars[i];
				if (!c.CheckBlockAndDec())
				{
					c.Damage(damage);
					c.HitMotion();
					if (!CheckDeath(c, list))
					{
						ActionMg.PrintLog($"{c.LogName}이/가 {damage}만큼 피해를 입었습니다.", (i == 0));
						c.OnUpdateHp();
					}
					else
					{
						ActionMg.PrintLog($"{c.LogName}이/가 죽었습니다.", (i == 0));
						ActionMg.PrintLog($"{c.LogName}이/가 {damage}만큼 피해를 입었습니다.", false);
					}
				}
				else
				{
					ActionMg.PrintLog($"{c.LogName}이/가 방어하였습니다.", (i == 0));
				}
			}
			foreach (FightChar rm in list)
			{
				_chars.Remove(rm);
			}
		}
		else
		{
			List<FightChar> list = new List<FightChar>();
			for (int i = 0; i < _monsters.Count; i++)
			{
				FightChar c = _monsters[i];
				if (!c.CheckBlockAndDec())
				{
					c.Damage(damage);
					c.HitMotion();
					if (!CheckDeath(c, list))
					{
						ActionMg.PrintLog($"{c.LogName}이/가 {damage}만큼 피해를 입었습니다.", (i == 0));
						c.OnUpdateHp();
					}
					else
					{
						ActionMg.PrintLog($"{c.LogName}이/가 죽었습니다.", (i == 0));
						ActionMg.PrintLog($"{c.LogName}이/가 {damage}만큼 피해를 입었습니다.", false);
					}
				}
				else
				{
					ActionMg.PrintLog($"{c.LogName}이/가 방어하였습니다.", (i == 0));
				}
			}
			foreach (FightChar rm in list)
			{
				_monsters.Remove(rm);
			}
		}
	}

	public void HealPercent(int healPercent, FightChar to, bool isLast = true)
	{
		int amount = to.Heal(to.MaxHp * healPercent / 100);
		ActionMg.PrintLog($"{to.LogName}의 체력이 {amount}만큼 회복되었습니다.", isLast);
		to.OnUpdateHp();
	}

	public void HealPercentAll(int healPercent, bool isPlayer)
	{
		if (isPlayer)
		{
			for (int i = 0; i < _chars.Count; i++)
			{
				FightChar c = _chars[i];
				HealPercent(healPercent, c, (i == 0));
			}
		}
		else
		{
			for (int i = 0; i < _monsters.Count; i++)
			{
				FightChar c = _monsters[i];
				HealPercent(healPercent, c, (i == 0));
			}
		}
	}

	public void IncBufAll(int turn, int strIncPercent, bool isPlayer)
	{
		if (isPlayer)
		{
			for (int i = 0; i < _chars.Count; i++)
			{
				FightChar c = _chars[i];
				c.IncBuf(turn, c.OStr * strIncPercent / 100);
			}
		}
		else
		{
			for (int i = 0; i < _monsters.Count; i++)
			{
				FightChar c = _chars[i];
				c.IncBuf(turn, c.OStr * strIncPercent / 100);
			}
		}
	}

	// 마지막 로그 확인시 호출
	public void EndTurn()
	{
		_buffer.GetCurrent().EndTurn();
		if (EndGame())
		{
			return;
		}
		_buffer.Next();
		StartFight();
	}
}

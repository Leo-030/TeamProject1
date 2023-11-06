using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightChar
{
	class Buf
	{
		public int turn;
		public int strInc;

		public Buf(int turn, int strInc)
		{
			this.turn = turn;
			this.strInc = strInc;
		}
	}

	bool _isPlayer;
	GameObject _obj;
	Vector3 _pos;
	int _id;
	string _name;
	string _logName;
	string _kind;
	int _chp; // 현재 hp
	int _maxhp;
	int _cstr; // 현재 str
	int _ostr; // 기본 str
	int _cgauge; // 현재 게이지
	int _maxgauge; //최대 게이지
	List<Buf> _bufs;
	int _stun;
	int _firstHit;
	int _block;
	int _ultimateSkillId;
	int _uniqueAttackId;
	int _attackId;

	public bool IsPlayer
	{
		get
		{
			return _isPlayer;
		}
	}

	public GameObject Obj
	{
		get
		{
			return _obj;
		}
	}

	public int Id
	{
		get
		{
			return _id;
		}
	}

	public string Name
	{
		get
		{
			return _name;
		}
	}

	public string LogName
	{
		get
		{
			return _logName;
		}
	}

	public string Kind
	{
		get
		{
			return _kind;
		}
	}

	public int CStr
	{
		get
		{
			return _cstr;
		}
	}

	public int OStr
	{
		get
		{
			return _ostr;
		}
	}

	public int MaxHp
	{ 
		get
		{
			return _maxhp;
		}
	}

	public int Gauge
	{
		get
		{
			return _cgauge;
		}
	}

	public int UltimateSkillId
	{
		get
		{
			return _ultimateSkillId;
		}
	}

	public int UniqueAttackId
	{
		get
		{
			return _uniqueAttackId;
		}
	}

	public int AttackId
	{
		get
		{
			return _attackId;
		}
	}

	public FightChar(HaveChar c, bool isPlayer, string kind = "Characters")
	{
		_isPlayer = isPlayer;
		_chp = c.star * c.character.hpInc + c.character.hp;
		_maxhp = c.star * c.character.hpInc + c.character.hp;
		_cstr = c.star * c.character.strInc + c.character.str;
		_ostr = c.star * c.character.strInc + c.character.str;
		_cgauge = 0;
		_maxgauge = c.star * c.character.gaugeInc + c.character.gauge;
		_bufs = new List<Buf>();
		_stun = 0;
		_firstHit = 0;
		_block = 0;
		_ultimateSkillId = c.character.ultimateSkill;
		_uniqueAttackId = c.character.uniqueAttack;
		_attackId = c.character.attack;

		_id = c.character.id;
		_name = $"{c.character.name}({c.star})";
		if (_isPlayer)
		{
			_logName = $"{c.character.name}({c.star})";
		}
		else
		{
			_logName = $"적의 {c.character.name}({c.star})";
		}
		_kind = kind;

		_obj = Managers.Resource.Instantiate("FightCharObject");
		_obj.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Images/{_kind}/{c.character.id}/FightChar");
	}

	public void DamageMotion(FightChar c = null)
	{
		Managers.Sound.Play("Gun Fire Sound");
		if (c != null)
		{
			_obj.GetComponent<Coroutine>().SetDamage(c.Obj.transform.position);
		}
	}

	public void HealOrBufMotion(FightChar c = null)
	{
		Managers.Sound.Play("Correct 1");
		if (c != null)
		{
			_obj.GetComponent<Coroutine>().SetDamage(c.Obj.transform.position);
		}
	}

	public void HitMotion()
	{
		_obj.GetComponent<Coroutine>().SetHit();
	}

	public void MoveObj(int i, int max = 4)
	{
		if (_isPlayer)
		{
			_obj.transform.position = new Vector3(-5.0f + 1.75f * i, 0.5f, 0.0f);
			_pos = _obj.transform.position;
			_obj.GetComponent<Coroutine>().SetInfo(_pos);
		}
		else
		{
			_obj.transform.position = new Vector3(5.0f - 1.75f * (max - i), 2.5f, 0.0f);
			_pos = _obj.transform.position;
			_obj.GetComponent<Coroutine>().SetInfo(_pos);
		}
	}

	public void OnUpdateHp()
	{
		_obj.GetComponent<FightCharObject>().OnUpdateHp(_chp, _maxhp);
	}

	public void OnUpdateGauge()
	{
		_obj.GetComponent<FightCharObject>().OnUpdateGauge(_cgauge, _maxgauge);
	}

	public void SetTurn()
	{
		_obj.GetComponent<FightCharObject>().SetTurn();
	}

	public void EndTurn()
	{
		CheckBufAndDec();
		_obj.GetComponent<FightCharObject>().EndTurn();
	}

	public bool CheckHpDown(int percent)
	{
		if (_chp <= _maxhp * percent / 100)
		{
			return true;
		}
		return false;
	}

	public bool CheckDeath()
	{
		if (_chp <= 0)
		{
			return true;
		}
		return false;
	}

	public void Die()
	{
		Managers.Resource.Destroy(_obj);
		_bufs.Clear();
	}

	public int Heal(int heal)
	{
		if (_chp + heal > _maxhp)
		{
			int amount = _maxhp - _chp;
			_chp = _maxhp;
			return amount;
		}
		_chp += heal;
		return heal;
	}

	public void Damage(int damage)
	{
		_chp -= damage;
    }

	public void IncGauge(int gauge)
	{
		_cgauge += gauge;
		if (_cgauge > _maxgauge)
		{
			_cgauge = _maxgauge;
		}
		OnUpdateGauge();
	}
	
	public void SetGauge(int gauge)
	{
		_cgauge = gauge;
		if (_cgauge > _maxgauge)
		{
			_cgauge = _maxgauge;
		}
		OnUpdateGauge();
	}

	public void DecGauge(int gauge)
	{
		_cgauge -= gauge;
		OnUpdateGauge();
	}

	public void IncStun(int inc)
	{
		_stun += inc;
	}

	public bool CheckStunAndDec()
	{
		if (_stun > 0)
		{
			_stun--;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void IncFirstHit(int inc)
	{
		_firstHit += inc;
	}

	public bool CheckFirstHit()
	{
		if (_firstHit > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void DecFirstHit()
	{
		if (_firstHit > 0)
		{
			_firstHit--;
		}
	}

	public void IncBlock(int inc)
	{
		_block += inc;
	}

	public bool CheckBlockAndDec()
	{
		if (_block > 0)
		{
			_block--;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void IncBuf(int turn, int strInc)
	{
		Buf buf = new Buf(turn, strInc);
		_bufs.Add(buf);
		_cstr += buf.strInc;
	}

	public void CheckBufAndDec()
	{
		List<Buf> removeList = new List<Buf>();
		foreach (Buf buf in _bufs)
		{
			buf.turn--;
			if (buf.turn <= 0)
			{
				removeList.Add(buf);
			}
		}
		for (int i = 0; i < removeList.Count; i++)
		{
			Buf buf = removeList[i];
			_cstr -= buf.strInc;
			_bufs.Remove(buf);
		}
	}
}

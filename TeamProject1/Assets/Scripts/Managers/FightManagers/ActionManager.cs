using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager
{
	// index 0은 안씀
	int[] _ultimateSkillNeed = { 0, 10, 1, 2, 4, 4, 5, 2, 7, 1, 3, 5, 3 };

	public void PrintLog(string text, bool isLast = false)
	{
		UI_Fight_Log log = Managers.UI.ShowPopupUI<UI_Fight_Log>();
		log.SetInfo(text);
		if (isLast)
		{
			log.LogAction -= FightManagers.Instance.EndTurn;
			log.LogAction += FightManagers.Instance.EndTurn;
		}
	}

	public string FindUltimateSkillName(int id)
	{
		string skillName = "";
		foreach (UltimateSkill a in Managers.Data.UltimateSkills)
		{
			if (a.id == id)
			{
				skillName = a.name;
				break;
			}
		}
		return skillName;
	}

	public string FindUniqueAttackName(int id)
	{
		string skillName = "";
		foreach (UniqueAttack a in Managers.Data.UniqueAttacks)
		{
			if (a.id == id)
			{
				skillName = a.name;
				break;
			}
		}
		return skillName;
	}

	public string FindAttackName(int id)
	{
		string skillName = "";
		foreach (Attack a in Managers.Data.Attacks)
		{
			if (a.id == id)
			{
				skillName = a.name;
				break;
			}
		}
		return skillName;
	}

	public bool CheckUltimateSkill(int id, int cgauge)
	{
		if (id > 12 || id <= 0)
		{
			return false;
		}

		if (cgauge < _ultimateSkillNeed[id])
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	public bool CheckSelect(int id, string kind)
	{
		if (kind == "UltimateSkill")
		{
			switch (id)
			{
				case 1:
					return false;
				case 2:
					return true;
				case 3:
					return true;
				case 4:
					return false;
				case 5:
					return false;
				case 6:
					return false;
				case 7:
					return true;
				case 8:
					return true;
				case 9:
					return true;
				case 10:
					return false;
				case 11:
					return true;
				case 12:
					return true;
			}
		}
		else if (kind == "UniqueAttack")
		{
			switch (id)
			{
				case 1:
					return false;
				case 2:
					return false;
				case 3:
					return false;
				case 4:
					return true;
				case 5:
					return false;
				case 6:
					return false;
				case 7:
					return true;
				case 8:
					return false;
				case 9:
					return false;
				case 10:
					return true;
				case 11:
					return false;
				case 12:
					return true;
			}
		}
		else if (kind == "Attack")
		{
			switch (id)
			{
				case 1:
					return true;
				case 2:
					return false;
				case 3:
					return true;
				case 4:
					return true;
			}
		}
		return false;
	}

	public bool CheckToOurs(int id, string kind)
	{
		if (kind == "UltimateSkill")
		{
			switch (id)
			{
				case 1:
					return false;
				case 2:
					return false;
				case 3:
					return false;
				case 4:
					return false;
				case 5:
					return false;
				case 6:
					return false;
				case 7:
					return false;
				case 8:
					return false;
				case 9:
					return false;
				case 10:
					return false;
				case 11:
					return false;
				case 12:
					return true;
			}
		}
		else if (kind == "UniqueAttack")
		{
			switch (id)
			{
				case 1:
					return false;
				case 2:
					return false;
				case 3:
					return false;
				case 4:
					return false;
				case 5:
					return false;
				case 6:
					return false;
				case 7:
					return false;
				case 8:
					return false;
				case 9:
					return false;
				case 10:
					return true;
				case 11:
					return false;
				case 12:
					return true;
			}
		}
		else if (kind == "Attack")
		{
			switch (id)
			{
				case 1:
					return false;
				case 2:
					return false;
				case 3:
					return false;
				case 4:
					return false;
			}
		}
		return false;
	}

	// 해야할 일
	// 스킬은 정해진 상태이니 상대 선택 필수
	// 모션 취하기 하고 스킬 발동
	// 그후 로그 띄우기 필수
	// 마지막 로그시 다음 턴으로

	// 결론:
	// 1. 선택창
	// 2. 모션 -> 선택창에 액션으로 붙이기
	// 3. 스킬 발동 -> 선택창에 액션으로 붙이기
	// 4. 로그 -> 선택창에 액션으로 붙이기
	// 선택창 필요없을시
	// 1. 모션
	// 2. 스킬 발동
	// 3. 로그
	public void Act(int id, string kind, FightChar from, FightChar to = null)
	{
		if (from.CheckStunAndDec())
		{
			// 기절 때문에 행동을 못했다는 로그 띄우기
			PrintLog($"{from.LogName}이/가 기절해있습니다.", true);
			return;
		}

		if (kind == "UltimateSkill")
		{
			switch (id)
			{
				case 1:
					UltimateSkill1(from);
					break;
				case 2:
					UltimateSkill2(from, to);
					break;
				case 3:
					UltimateSkill3(from, to);
					break;
				case 4:
					UltimateSkill4(from);
					break;
				case 5:
					UltimateSkill5(from);
					break;
				case 6:
					UltimateSkill6(from);
					break;
				case 7:
					UltimateSkill7(from, to);
					break;
				case 8:
					UltimateSkill8(from, to);
					break;
				case 9:
					UltimateSkill9(from, to);
					break;
				case 10:
					UltimateSkill10(from);
					break;
				case 11:
					UltimateSkill11(from, to);
					break;
				case 12:
					UltimateSkill12(from, to);
					break;
			}
		}
		else if (kind == "UniqueAttack")
		{
			switch (id)
			{
				case 1:
					UniqueAttack1(from);
					break;
				case 2:
					UniqueAttack2(from);
					break;
				case 3:
					UniqueAttack3(from);
					break;
				case 4:
					UniqueAttack4(from, to);
					break;
				case 5:
					UniqueAttack5(from);
					break;
				case 6:
					UniqueAttack6(from);
					break;
				case 7:
					UniqueAttack7(from, to);
					break;
				case 8:
					UniqueAttack8(from);
					break;
				case 9:
					UniqueAttack9(from);
					break;
				case 10:
					UniqueAttack10(from, to);
					break;
				case 11:
					UniqueAttack11(from);
					break;
				case 12:
					UniqueAttack12(from, to);
					break;
			}
		}
		else if (kind == "Attack")
		{
			switch (id)
			{
				case 1:
					Attack1(from, to);
					break;
				case 2:
					Attack2(from);
					break;
				case 3:
					Attack3(from, to);
					break;
				case 4:
					Attack4(from, to);
					break;
			}
		}
	}

	public void UltimateSkill1(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[1]);
		from.HealOrBufMotion();
		from.IncBlock(1);
		PrintLog($"{from.LogName}이/가 {FindUltimateSkillName(1)}을 사용하였습니다.", true);
	}

	public void UltimateSkill2(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[2]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 180 / 100, to);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(2)}을 사용하였습니다.", false);
	}

	public void UltimateSkill3(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[3]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 300 / 100, to);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(3)}을 사용하였습니다.", false);
	}

	public void UltimateSkill4(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[4]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr, !from.IsPlayer);
		FightManagers.Instance.IncBufAll(1, -10, !from.IsPlayer);
		PrintLog($"{from.LogName}이/가 {FindUltimateSkillName(4)}을 사용하였습니다.", false);
	}

	public void UltimateSkill5(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[5]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr * 120 / 100, !from.IsPlayer);
		PrintLog($"{from.LogName}이/가 {FindUltimateSkillName(5)}을 사용하였습니다.", false);
	}

	public void UltimateSkill6(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[6]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr * 150 / 100, !from.IsPlayer);
		PrintLog($"{from.LogName}이/가 {FindUltimateSkillName(6)}을 사용하였습니다.", false);
	}

	public void UltimateSkill7(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[7]);
		from.DamageMotion(to);
		FightManagers.Instance.DamageGroup(from.CStr * 200 / 100, from.CStr * 50 / 100, to);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(7)}을 사용하였습니다.", false);
	}

	public void UltimateSkill8(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[8]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(to.MaxHp * 35 / 100, to);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(8)}을 사용하였습니다.", false);
	}

	public void UltimateSkill9(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[9]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 200 / 100, to);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(9)}을 사용하였습니다.", false);
	}

	public void UltimateSkill10(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[10]);
		from.HealOrBufMotion();
		FightManagers.Instance.HealPercentAll(10, from.IsPlayer);
		PrintLog($"{from.LogName}이/가 {FindUltimateSkillName(10)}을 사용하였습니다.", false);
	}

	public void UltimateSkill11(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[11]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 150 / 100, to);
		float[] floats = { 1.0f, 1.0f };
		int isStun = Util.Choose(floats);
		to.IncStun(isStun);
		if (isStun != 0)
		{
			PrintLog($"{to.LogName}이/가 기절하였습니다.", false);
		}
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(11)}을 사용하였습니다.", false);
	}

	public void UltimateSkill12(FightChar from, FightChar to)
	{
		if (to == from)
		{
			from.HealOrBufMotion();
		}
		else
		{
			from.HealOrBufMotion(to);
		}
		from.DecGauge(_ultimateSkillNeed[12]);
		if (to == from)
		{
			to.IncBuf(2, to.OStr * 50 / 100);
		}
		else
		{
			to.IncBuf(1, to.OStr * 50 / 100);
		}
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUltimateSkillName(12)}을 사용하였습니다.", true);
	}

	public void UniqueAttack1(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncFirstHit(1);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(1)}을 사용하였습니다.", true);
	}

	public void UniqueAttack2(FightChar from)
	{
		if (from.CheckHpDown(50))
		{
			from.HealOrBufMotion();
			from.IncBuf(2, from.OStr * 30 / 100);
			PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(2)}을 사용하였습니다.", true);
		}
		else
		{
			PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(2)}을 사용하였지만 조건이 충족되지 못해 발동하지 않았습니다.", true);
			return;
		}
	}

	public void UniqueAttack3(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr * 40 / 100);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(3)}을 사용하였습니다.", true);
	}

	public void UniqueAttack4(FightChar from, FightChar to)
	{
		from.HealOrBufMotion(to);
		to.IncBuf(1, to.OStr * -20 / 100);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUniqueAttackName(4)}을 사용하였습니다.", true);
	}

	public void UniqueAttack5(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(5)}을 사용하였습니다.", true);
	}

	public void UniqueAttack6(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr * 40 / 100);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(6)}을 사용하였습니다.", true);
	}

	public void UniqueAttack7(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		int tmp = from.Gauge;
		from.SetGauge(to.Gauge);
		to.SetGauge(tmp);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUniqueAttackName(7)}을 사용하였습니다.", true);
	}

	public void UniqueAttack8(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(8)}을 사용하였습니다.", true);
	}

	public void UniqueAttack9(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(9)}을 사용하였습니다.", true);
	}

	public void UniqueAttack10(FightChar from, FightChar to)
	{
		from.HealOrBufMotion(to);
		FightManagers.Instance.HealPercent(20, to, true);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUniqueAttackName(10)}을 사용하였습니다.", false);
	}

	public void UniqueAttack11(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}이/가 {FindUniqueAttackName(11)}을 사용하였습니다.", true);
	}

	public void UniqueAttack12(FightChar from, FightChar to)
	{
		if (to == from)
		{
			from.HealOrBufMotion();
			to.IncBuf(2, to.OStr * 30 / 100);
		}
		else
		{
			from.HealOrBufMotion(to);
			to.IncBuf(1, to.OStr * 30 / 100);
		}
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindUniqueAttackName(12)}을 사용하였습니다.", true);
	}

	public void Attack1(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindAttackName(1)}을 사용하였습니다.", false);
	}

	public void Attack2(FightChar from)
	{
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr, !from.IsPlayer);
		from.IncGauge(1);
		PrintLog($"{from.LogName}이/가 {FindAttackName(2)}을 사용하였습니다.", false);
	}

	public void Attack3(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindAttackName(3)}을 사용하였습니다.", false);
	}

	public void Attack4(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}이/가 {to.LogName}에게 {FindAttackName(4)}을 사용하였습니다.", false);
	}
}

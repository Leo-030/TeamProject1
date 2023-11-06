using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager
{
	// index 0�� �Ⱦ�
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

	// �ؾ��� ��
	// ��ų�� ������ �����̴� ��� ���� �ʼ�
	// ��� ���ϱ� �ϰ� ��ų �ߵ�
	// ���� �α� ���� �ʼ�
	// ������ �α׽� ���� ������

	// ���:
	// 1. ����â
	// 2. ��� -> ����â�� �׼����� ���̱�
	// 3. ��ų �ߵ� -> ����â�� �׼����� ���̱�
	// 4. �α� -> ����â�� �׼����� ���̱�
	// ����â �ʿ������
	// 1. ���
	// 2. ��ų �ߵ�
	// 3. �α�
	public void Act(int id, string kind, FightChar from, FightChar to = null)
	{
		if (from.CheckStunAndDec())
		{
			// ���� ������ �ൿ�� ���ߴٴ� �α� ����
			PrintLog($"{from.LogName}��/�� �������ֽ��ϴ�.", true);
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
		PrintLog($"{from.LogName}��/�� {FindUltimateSkillName(1)}�� ����Ͽ����ϴ�.", true);
	}

	public void UltimateSkill2(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[2]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 180 / 100, to);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(2)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill3(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[3]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 300 / 100, to);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(3)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill4(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[4]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr, !from.IsPlayer);
		FightManagers.Instance.IncBufAll(1, -10, !from.IsPlayer);
		PrintLog($"{from.LogName}��/�� {FindUltimateSkillName(4)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill5(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[5]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr * 120 / 100, !from.IsPlayer);
		PrintLog($"{from.LogName}��/�� {FindUltimateSkillName(5)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill6(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[6]);
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr * 150 / 100, !from.IsPlayer);
		PrintLog($"{from.LogName}��/�� {FindUltimateSkillName(6)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill7(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[7]);
		from.DamageMotion(to);
		FightManagers.Instance.DamageGroup(from.CStr * 200 / 100, from.CStr * 50 / 100, to);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(7)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill8(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[8]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(to.MaxHp * 35 / 100, to);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(8)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill9(FightChar from, FightChar to)
	{
		from.DecGauge(_ultimateSkillNeed[9]);
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr * 200 / 100, to);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(9)}�� ����Ͽ����ϴ�.", false);
	}

	public void UltimateSkill10(FightChar from)
	{
		from.DecGauge(_ultimateSkillNeed[10]);
		from.HealOrBufMotion();
		FightManagers.Instance.HealPercentAll(10, from.IsPlayer);
		PrintLog($"{from.LogName}��/�� {FindUltimateSkillName(10)}�� ����Ͽ����ϴ�.", false);
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
			PrintLog($"{to.LogName}��/�� �����Ͽ����ϴ�.", false);
		}
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(11)}�� ����Ͽ����ϴ�.", false);
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
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUltimateSkillName(12)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack1(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncFirstHit(1);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(1)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack2(FightChar from)
	{
		if (from.CheckHpDown(50))
		{
			from.HealOrBufMotion();
			from.IncBuf(2, from.OStr * 30 / 100);
			PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(2)}�� ����Ͽ����ϴ�.", true);
		}
		else
		{
			PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(2)}�� ����Ͽ����� ������ �������� ���� �ߵ����� �ʾҽ��ϴ�.", true);
			return;
		}
	}

	public void UniqueAttack3(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr * 40 / 100);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(3)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack4(FightChar from, FightChar to)
	{
		from.HealOrBufMotion(to);
		to.IncBuf(1, to.OStr * -20 / 100);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUniqueAttackName(4)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack5(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(5)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack6(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr * 40 / 100);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(6)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack7(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		int tmp = from.Gauge;
		from.SetGauge(to.Gauge);
		to.SetGauge(tmp);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUniqueAttackName(7)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack8(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(8)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack9(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncBuf(2, from.OStr);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(9)}�� ����Ͽ����ϴ�.", true);
	}

	public void UniqueAttack10(FightChar from, FightChar to)
	{
		from.HealOrBufMotion(to);
		FightManagers.Instance.HealPercent(20, to, true);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUniqueAttackName(10)}�� ����Ͽ����ϴ�.", false);
	}

	public void UniqueAttack11(FightChar from)
	{
		from.HealOrBufMotion();
		from.IncGauge(2);
		PrintLog($"{from.LogName}��/�� {FindUniqueAttackName(11)}�� ����Ͽ����ϴ�.", true);
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
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindUniqueAttackName(12)}�� ����Ͽ����ϴ�.", true);
	}

	public void Attack1(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindAttackName(1)}�� ����Ͽ����ϴ�.", false);
	}

	public void Attack2(FightChar from)
	{
		from.DamageMotion();
		FightManagers.Instance.DamageAll(from.CStr, !from.IsPlayer);
		from.IncGauge(1);
		PrintLog($"{from.LogName}��/�� {FindAttackName(2)}�� ����Ͽ����ϴ�.", false);
	}

	public void Attack3(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindAttackName(3)}�� ����Ͽ����ϴ�.", false);
	}

	public void Attack4(FightChar from, FightChar to)
	{
		from.DamageMotion(to);
		FightManagers.Instance.Damage(from.CStr, to);
		from.IncGauge(1);
		PrintLog($"{from.LogName}��/�� {to.LogName}���� {FindAttackName(4)}�� ����Ͽ����ϴ�.", false);
	}
}

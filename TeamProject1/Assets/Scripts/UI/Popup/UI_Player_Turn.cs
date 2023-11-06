using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Player_Turn : UI_Popup
{
	enum Buttons
	{
		MenuButton,
		UltimateSkillButton,
		UniqueAttackButton,
		AttackButton,
		
	}

	enum Texts
	{
		UltimateSkillText,
		UniqueAttackText,
		AttackText,
	}

	FightChar c;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));

		GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnMenuButtonClicked);
		GetButton((int)Buttons.UltimateSkillButton).gameObject.BindEvent(OnUltimateSkillButtonClicked);
		GetButton((int)Buttons.UniqueAttackButton).gameObject.BindEvent(OnUniqueAttackButtonClicked);
		GetButton((int)Buttons.AttackButton).gameObject.BindEvent(OnAttackButtonClicked);
		string text = "";
		foreach (UltimateSkill a in Managers.Data.UltimateSkills)
		{
			if (a.id == c.UltimateSkillId)
			{
				text = a.description;
				break;
			}	
		}
		Get<TextMeshProUGUI>((int)Texts.UltimateSkillText).text = $"얼티밋 스킬 - {text}";
		text = "";
		foreach (UniqueAttack a in Managers.Data.UniqueAttacks)
		{
			if (a.id == c.UniqueAttackId)
			{
				text = a.description;
				break;
			}
		}
		Get<TextMeshProUGUI>((int)Texts.UniqueAttackText).text = $"고유 능력 - {text}";
		text = "";
		foreach (Attack a in Managers.Data.Attacks)
		{
			if (a.id == c.AttackId)
			{
				text = a.description;
				break;
			}
		}
		Get<TextMeshProUGUI>((int)Texts.AttackText).text = $"공격 - {text}";
	}

	public void SetInfo(FightChar c)
	{
		this.c = c;
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	public void OnUltimateSkillButtonClicked(PointerEventData data)
	{
		bool usable = FightManagers.Instance.ActionMg.CheckUltimateSkill(c.UltimateSkillId ,c.Gauge);
		if (usable)
		{
			// 가능
			ClosePopupUI();
			if (FightManagers.Instance.ActionMg.CheckSelect(c.UltimateSkillId, "UltimateSkill"))
			{
				// 선택창
				Managers.UI.ShowPopupUI<UI_Player_Select>().SetInfo(c, "UltimateSkill", c.UltimateSkillId);
			}
			else
			{
				FightManagers.Instance.ActionMg.Act(c.UltimateSkillId, "UltimateSkill", c);
			}
			return;
		}
		else
		{
			Managers.UI.ShowPopupUI<UI_NoGauge>();
			return;
		}
	}

	public void OnUniqueAttackButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		if (FightManagers.Instance.ActionMg.CheckSelect(c.UniqueAttackId, "UniqueAttack"))
		{
			// 팝업창
			Managers.UI.ShowPopupUI<UI_Player_Select>().SetInfo(c, "UniqueAttack", c.UniqueAttackId);
		}
		else
		{
			FightManagers.Instance.ActionMg.Act(c.UniqueAttackId, "UniqueAttack", c);
		}
	}

	public void OnAttackButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		if (FightManagers.Instance.ActionMg.CheckSelect(c.AttackId, "Attack"))
		{
			// 팝업창
			Managers.UI.ShowPopupUI<UI_Player_Select>().SetInfo(c, "Attack", c.AttackId);
		}
		else
		{
			FightManagers.Instance.ActionMg.Act(c.AttackId, "Attack", c);
		}
	}
}

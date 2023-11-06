using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_CharacterInfo : UI_Popup
{
	enum Images
	{
		Background,
		InfoImage,
	}

	enum Panels
	{
		NamePanel,
		DescriptionPanel,
		ContentPanel,
	}

	enum Texts
	{
		NameText,
		DescriptionText,
		JobText,
		StrText,
		HpText,
		GaugeText,
		UltimateSkillText,
		UniqueAttackText,
		AttackText,		
		ExitText,
	}

	enum Buttons
	{
		ExitButton,
	}

	Character _character;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Image>(typeof(Images));
		Bind<GameObject>(typeof(Panels));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<Button>(typeof(Buttons));

		Get<TextMeshProUGUI>((int)Texts.NameText).text = _character.name;
		Get<TextMeshProUGUI>((int)Texts.DescriptionText).text = _character.description;
		Get<TextMeshProUGUI>((int)Texts.JobText).text = $"직업: {_character.job}";
		Get<TextMeshProUGUI>((int)Texts.StrText).text = $"str: {_character.str}\n강화당 str: {_character.strInc}";
		Get<TextMeshProUGUI>((int)Texts.HpText).text = $"hp: {_character.hp}\n강화당 hp: {_character.hpInc}";
		Get<TextMeshProUGUI>((int)Texts.GaugeText).text = $"얼티밋 스킬 게이지: {_character.gauge}\n강화당 얼티밋 스킬 게이지: {_character.gaugeInc}";
		Get<TextMeshProUGUI>((int)Texts.UltimateSkillText).text = $"얼티밋 스킬:\n";
		foreach (UltimateSkill ultimateSkill in Managers.Data.UltimateSkills)
		{
			if (_character.ultimateSkill == ultimateSkill.id)
			{
				Get<TextMeshProUGUI>((int)Texts.UltimateSkillText).text += $"{ultimateSkill.name}: {ultimateSkill.description}\n";
				break;
			}
		}
		Get<TextMeshProUGUI>((int)Texts.UniqueAttackText).text = $"고유능력:\n";
		foreach (UniqueAttack uniqueAttack in Managers.Data.UniqueAttacks)
		{
			if (_character.uniqueAttack == uniqueAttack.id)
			{
				Get<TextMeshProUGUI>((int)Texts.UniqueAttackText).text += $"{uniqueAttack.name}: {uniqueAttack.description}\n";
				break;
			}
		}
		Get<TextMeshProUGUI>((int)Texts.AttackText).text = $"공격:\n";
		foreach (Attack attack in Managers.Data.Attacks)
		{
			if (_character.attack == attack.id)
			{
				Get<TextMeshProUGUI>((int)Texts.AttackText).text += $"{attack.name}: {attack.description}\n";
				break;
			}
		}

		Get<Image>((int)Images.InfoImage).sprite = Managers.Resource.Load<Sprite>($"Images/Characters/{_character.id}/Info");

		Get<Button>((int)Buttons.ExitButton).gameObject.BindEvent(OnButtonClicked);
	}

	public void SetInfo(Character character)
	{
		_character = character;
	}

	public void OnButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}
}

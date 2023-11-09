using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Party_Character : UI_Base
{
	enum Images
	{
		Image,
	}

	enum Panels
	{
		NamePanel,
		StarPanel,
		TouchPanel
	}

	enum Texts
	{
		NameText,
		StarText,
	}

	HaveChar _havecharacter;

	public HaveChar Havecharacter
	{
		get 
		{
			return _havecharacter;
		}
	}

	void Start()
	{
		Init();
	}

	public override void Init()
	{
		Bind<Image>(typeof(Images));
		Bind<GameObject>(typeof(Panels));
		Bind<TextMeshProUGUI>(typeof(Texts));

		OnUpdate();
	}

	public void SetInfo(HaveChar character)
	{
		_havecharacter = character;
	}

	public void OnUpdate()
	{
		Get<Image>((int)Images.Image).sprite = Managers.Resource.Load<Sprite>($"Images/Characters/{_havecharacter.character.id}/Original");
		Get<TextMeshProUGUI>((int)Texts.NameText).text = _havecharacter.character.name;
		Get<TextMeshProUGUI>((int)Texts.StarText).text = _havecharacter.star.ToString();
	}

	public void OnReset(HaveChar character)
	{
		SetInfo(character);
		OnUpdate();
	}
}

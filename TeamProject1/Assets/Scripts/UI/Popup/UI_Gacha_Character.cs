using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Gacha_Character : UI_Popup
{
	enum Images
	{
		Image,
	}

	enum Panels
	{
		NamePanel,
	}

	enum Texts
	{
		NameText,
	}

	enum Buttons
	{
		ExitButton,
	}

	Character _character;

	void Start()
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

		Get<Image>((int)Images.Image).sprite = Managers.Resource.Load<Sprite>($"Images/Characters/{_character.id}/Original");
		Get<TextMeshProUGUI>((int)Texts.NameText).text = _character.name;

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	public void SetInfo(Character character)
	{
		_character = character;
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}
}

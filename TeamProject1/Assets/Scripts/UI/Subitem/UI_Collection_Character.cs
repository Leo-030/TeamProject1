using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Collection_Character : UI_Base
{
	enum Images
	{
		Image,
	}

	enum Panels
	{
		NamePanel,
		TouchPanel
	}

	enum Texts
	{
		NameText,
	}

	Character _character;

	public Character Charac
	{
		get
		{
			return _character;
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

		Get<GameObject>((int)Panels.TouchPanel).gameObject.BindEvent(OnButtonClicked);
	}

	public void SetInfo(Character character)
	{
		_character = character;
	}

	public void OnUpdate()
	{
		Get<Image>((int)Images.Image).sprite = Managers.Resource.Load<Sprite>($"Images/Characters/{_character.id}/Collection");
		Get<TextMeshProUGUI>((int)Texts.NameText).text = _character.name;
	}

	public void OnReset(Character character)
	{
		SetInfo(character);
		OnUpdate();
	}

	public void OnButtonClicked(PointerEventData data)
	{
		UI_CharacterInfo characterInfo = Managers.UI.ShowPopupUI<UI_CharacterInfo>();
		characterInfo.SetInfo(_character);
	}
}

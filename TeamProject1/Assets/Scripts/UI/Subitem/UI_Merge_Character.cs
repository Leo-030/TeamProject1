using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Merge_Character : UI_Base
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

	public Action<HaveChar, bool> ClickAction;

	bool _isSelected;
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

		Get<GameObject>((int)Panels.TouchPanel).gameObject.BindEvent(OnButtonClicked);
	}

	public void SetInfo(HaveChar character)
	{
		_havecharacter = character;
		_isSelected = false;
		ClickAction = null;
	}

	public void OnReset(HaveChar character)
	{
		SetInfo(character);
		OnUpdate();
	}

	public void OnUpdate()
	{
		Get<Image>((int)Images.Image).sprite = Managers.Resource.Load<Sprite>($"Images/Characters/{_havecharacter.character.id}/Collection");
		Get<TextMeshProUGUI>((int)Texts.NameText).text = _havecharacter.character.name;
		Get<TextMeshProUGUI>((int)Texts.StarText).text = _havecharacter.star.ToString();
		Get<GameObject>((int)Panels.TouchPanel).GetComponent<Image>().color = new Color32(0, 0, 0, 0);
	}

	public void OnButtonClicked(PointerEventData data)
	{
		if (_isSelected)
		{
			_isSelected = false;
			Get<GameObject>((int)Panels.TouchPanel).GetComponent<Image>().color = new Color32(0, 0, 0, 0);
		}
		else
		{
			_isSelected = true;
			Get<GameObject>((int)Panels.TouchPanel).GetComponent<Image>().color = new Color32(0, 0, 0, 225);
		}
		if (ClickAction == null)
			return;
		ClickAction.Invoke(_havecharacter, _isSelected);
	}

}

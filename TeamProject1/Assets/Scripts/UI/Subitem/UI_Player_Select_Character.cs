using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_Player_Select_Character : UI_Base
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

	public Action ClickAction = null;

	FightChar to;

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

	public void SetInfo(FightChar character)
	{
		to = character;
		ClickAction = null;
	}

	public void OnUpdate()
	{
		Get<Image>((int)Images.Image).sprite = Managers.Resource.Load<Sprite>($"Images/{to.Kind}/{to.Id}/Original");
		Get<TextMeshProUGUI>((int)Texts.NameText).text = to.Name;
	}

	public void OnButtonClicked(PointerEventData data)
	{
		if (ClickAction != null)
			ClickAction.Invoke();
	}

}

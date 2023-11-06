using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Fight_Log : UI_Popup
{
	enum Buttons
	{
		MenuButton,
		ExitButton,
	}

	enum Texts
	{
		LogText
	}

	string log;
	public Action LogAction;

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
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
		
		Get<TextMeshProUGUI>((int)Texts.LogText).text = log;
	}

	public void SetInfo(string text)
	{
		log = text;
		LogAction = null;
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		if (LogAction != null)
		{
			LogAction.Invoke();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ChangeAlert : UI_Popup
{
	enum Buttons
	{
		ExitButton,
		OkayButton
	}

	enum Texts
	{
		ExitText,
		Text,
	}

	string _text;
	public Action OkayAction;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));

		Get<TextMeshProUGUI>((int)Texts.Text).text = _text;
		
		GetButton((int)Buttons.OkayButton).gameObject.BindEvent(OnOkayButtonClicked);
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	public void OnOkayButtonClicked(PointerEventData data)
	{
		OkayAction.Invoke();
		ClosePopupUI();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}

	public void SetInfo(string info)
	{
		_text = info;
		OkayAction = null;
	}
}

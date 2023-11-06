using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Credits : UI_Popup
{
	enum Buttons
	{
		ExitButton,
	}

	enum Texts
	{
		TitleText,
		Text1,
		ExitText
	}

	enum GameObjects
	{
		Background,
		TitlePanel,
		Content
	}

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<GameObject>(typeof(GameObjects));

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnButtonClicked);
	}

	public void OnButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}	
}

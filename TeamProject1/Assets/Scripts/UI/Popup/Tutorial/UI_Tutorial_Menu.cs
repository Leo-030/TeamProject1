using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tutorial_Menu : UI_Popup
{
	enum Buttons
	{
		MenuButton
	}

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		
		GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnMenuButtonClicked);
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}
}

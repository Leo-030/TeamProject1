using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Defeat : UI_Popup
{
	enum Buttons
	{
		ExitButton,
	}

	enum Texts
	{
		ExitText,
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

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		Managers.Resource.Destroy(FightManagers.Instance.gameObject);
		Managers.Scene.LoadScene(Define.Scene.Lobby);
	}
}

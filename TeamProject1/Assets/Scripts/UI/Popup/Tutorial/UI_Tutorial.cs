using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tutorial : UI_Popup
{
	enum Buttons
	{
		ExitButton,
	}

	int _count;
	int _max;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	public void SetInfo(int i, int max)
	{
		_count = i;
		_max = max;
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		if (_count >= _max)
		{
			close();
		}
		else
		{
			next();
		}
	}
	
	void next()
	{
		Managers.UI.ShowPopupUI<UI_Tutorial>($"Tutorial/UI_Tutorial_{_count + 1}").SetInfo(_count + 1, _max);
	}

	void close()
	{
		if (Managers.Data.Data.player.tutorial == false)
		{
			Character c = new Character(Managers.Data.Characters.SearchById(3));
			HaveChar hc = new HaveChar();
			hc.character = c;
			hc.star = 0;
			Managers.Data.Data.player.characters.Add(hc);
			Managers.Data.Data.player.tutorial = true;
			Managers.Data.WriteJson("Data", Managers.Data.Data);
		}
		Managers.Scene.LoadScene(Define.Scene.Lobby);
	}
}

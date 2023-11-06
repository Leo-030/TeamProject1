using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Player_Select : UI_Popup
{
	enum Buttons
	{
		MenuButton,
		
	}

	enum GameObjects
	{
		Panel
	}

	string kind;
	int id;
	FightChar from;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<GameObject>(typeof(GameObjects));
		Get<Button>((int)Buttons.MenuButton).gameObject.BindEvent(OnMenuButtonClicked);

		GameObject obj = Get<GameObject>((int)GameObjects.Panel);
		List<FightChar> list;
		if (FightManagers.Instance.ActionMg.CheckToOurs(id, kind))
		{
			list = FightManagers.Instance.GetOurs();
		}
		else
		{
			list = FightManagers.Instance.GetEnemy();
		}
		int count = 0;
		foreach (FightChar to in list)
		{
			UI_Player_Select_Character upsc = Managers.UI.MakeSubItem<UI_Player_Select_Character>(obj.transform);
			upsc.SetInfo(to);
			upsc.ClickAction -= () => { ClosePopupUI(); FightManagers.Instance.ActionMg.Act(id, kind, from, to); };
			upsc.ClickAction += () => { ClosePopupUI(); FightManagers.Instance.ActionMg.Act(id, kind, from, to); };
			upsc.gameObject.transform.localPosition = new Vector3(-400 + 200 * count, 0.0f, 0.0f);
			count++;
		}
	}

	public void SetInfo(FightChar c, string kind, int id)
	{
		from = c;
		this.id = id;
		this.kind = kind;
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	
}

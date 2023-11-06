using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Win : UI_Popup
{
	enum Buttons
	{
		ExitButton,
	}

	enum Texts
	{
		ExitText,
	}

	int storyNum = 0;

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

	public void SetInfo(int storyNum)
	{
		this.storyNum = storyNum;
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
		Managers.Resource.Destroy(FightManagers.Instance.gameObject);
		if (storyNum > 0)
		{
			StoryManager.Instance.SetStoryNum(storyNum);
			Managers.Scene.LoadScene(Define.Scene.Story);
		}
		else
		{
			Managers.Scene.LoadScene(Define.Scene.Lobby);
		}
	}
}

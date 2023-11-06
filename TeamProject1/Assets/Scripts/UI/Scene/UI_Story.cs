using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Story : UI_Scene
{
	enum Buttons
	{
		MenuButton,
		ExitButton,
	}

	enum Texts
	{
		StoryText,
		ExitText,
	}

	List<string> storyText;
	string storyBgm;
	int count = 0;

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
		int storyNum = StoryManager.Instance.StoryNum;
		foreach (Story s in Managers.Data.Storys)
		{
			if (s.id  == storyNum)
			{
				setInfo(s.texts, s.bgm);
				StoryManager.Instance.SetStoryObj(s.id);
				break;
			}
		}

		if (storyText != null && storyText.Count > 0)
		{
			Get<TextMeshProUGUI>((int)Texts.StoryText).text = storyText[count];
			if (storyBgm != null)
			{
				Managers.Sound.Play(storyBgm, Define.Sound.Bgm);
			}
		}
	}

	void setInfo(List<string> text, string bgm)
	{
		storyText = text;
		storyBgm = bgm;
		count = 0;
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		count++;
		if (storyText == null)
		{
			Managers.Scene.LoadScene(Define.Scene.Lobby);
		}
		else if (storyText.Count > count)
		{
			Get<TextMeshProUGUI>((int)Texts.StoryText).text = storyText[count];
		}
		else if (storyText.Count <= count)
		{
			Managers.Scene.LoadScene(Define.Scene.Lobby);
		}
	}
}

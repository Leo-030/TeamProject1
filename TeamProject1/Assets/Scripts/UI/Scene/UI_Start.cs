using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Start : UI_Scene
{
	enum Buttons
	{
		StartButton,
		MenuButton,
		ExitButton,
	}
	
	enum Texts
	{
		TitleText,
		StartText,
		MenuText,
		ExitText,
	}

	enum Panels
	{
		TitlePanel
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
		Bind<GameObject>(typeof(Panels));

		GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnStartButtonClicked);
		GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnMenuButtonClicked);
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	public void OnStartButtonClicked(PointerEventData data)
	{
		if (Managers.Data.Data.player.tutorial == false)
		{
			Managers.Scene.LoadScene(Define.Scene.Tutorial);
		}
		else
		{
			Managers.Scene.LoadScene(Define.Scene.Lobby);
		}
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{

		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}

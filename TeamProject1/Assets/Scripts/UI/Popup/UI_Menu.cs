using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Menu : UI_Popup
{
	enum Buttons
	{
		ExitButton,
		OptionButton,
		CreditsButton,
		GameQuitButton,
	}

	enum Texts
	{
		TitleText,
		OptionText,
		CreditsText,
		ExitText,
	}

	enum Panels
	{
		Background,
		TitlePanel,
		GameQuitPanel,
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

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
		GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OnOptionButtonClicked);
		GetButton((int)Buttons.CreditsButton).gameObject.BindEvent(OnCreditsButtonClicked);
		GetButton((int)Buttons.GameQuitButton).gameObject.BindEvent(OnGameQuitButtonClicked);
		GetButton((int)Buttons.GameQuitButton).gameObject.BindEvent(OnGameQuitButtonDown, Define.UIEvent.PointerDown);
		GetButton((int)Buttons.GameQuitButton).gameObject.BindEvent(OnGameQuitButtonUp, Define.UIEvent.PointerUp);
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}

	public void OnOptionButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Option>();
	}

	public void OnCreditsButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Credits>();
	}

	public void OnGameQuitButtonClicked(PointerEventData data)
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void OnGameQuitButtonDown(PointerEventData data)
	{
		Get<GameObject>((int)Panels.GameQuitPanel).GetComponent<Image>().color = new Color32(190, 23, 4, 255);
	}

	public void OnGameQuitButtonUp(PointerEventData data)
	{

		Get<GameObject>((int)Panels.GameQuitPanel).GetComponent<Image>().color = new Color32(239, 250, 6, 74);
	}
}

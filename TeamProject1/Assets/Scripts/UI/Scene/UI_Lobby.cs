using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby : UI_Scene
{
	enum Buttons
	{
		StageButton,
		TutorialButton,
		MenuButton,
		ShopButton,
		CollectionButton,
		HaveCollectionButton,
		MergeButton,
		ChangeButton,
	}

	enum Texts
	{
		StageText,
		TutorialText,
		MenuText,
		ShopText,
		CollectionText,
		HaveCollectionText,
		MergeText,
		ChangeText,
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

		GetButton((int)Buttons.StageButton).gameObject.BindEvent(OnStageButtonClicked);
		GetButton((int)Buttons.TutorialButton).gameObject.BindEvent(OnTutorialButtonClicked);
		GetButton((int)Buttons.MenuButton).gameObject.BindEvent(OnMenuButtonClicked);
		GetButton((int)Buttons.ShopButton).gameObject.BindEvent(OnShopButtonClicked);
		GetButton((int)Buttons.CollectionButton).gameObject.BindEvent(OnCollectionButtonClicked);
		GetButton((int)Buttons.HaveCollectionButton).gameObject.BindEvent(OnHaveCollectionButtonClicked);
		GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnMergeClicked);
		GetButton((int)Buttons.ChangeButton).gameObject.BindEvent(OnChangeClicked);
	}

	public void OnStageButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Select_Stage>();
	}

	public void OnTutorialButtonClicked(PointerEventData data)
	{
		Managers.Scene.LoadScene(Define.Scene.Tutorial);
	}

	public void OnMenuButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Menu>();
	}

	public void OnShopButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Shop>();
	}

	public void OnCollectionButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Collection>();
	}

	public void OnHaveCollectionButtonClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_HaveCollection>();
	}

	public void OnMergeClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Merge>();
	}

	public void OnChangeClicked(PointerEventData data)
	{
		Managers.UI.ShowPopupUI<UI_Change>();
	}
}

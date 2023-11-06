using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Select_Stage : UI_Popup
{
	enum Buttons
	{
		ExitButton,
		LoopStageButton,
		NextStageButton,
	}

	enum Texts
	{
		TitleText,
		LoopStageText,
		NextStageText,
		ExitText,
	}

	enum Panels
	{
		Background,
		TitlePanel,
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
		GetButton((int)Buttons.LoopStageButton).gameObject.BindEvent(OnLoopStageButtonClicked);
		GetButton((int)Buttons.NextStageButton).gameObject.BindEvent(OnNextStageButtonClicked);
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}

	public void OnLoopStageButtonClicked(PointerEventData data)
	{
		if (Managers.Data.Data.player.stage == 0)
		{
			Managers.UI.ShowPopupUI<UI_CantGo>();
			return;
		}
		FightManagers.Instance.SetStage(Managers.Data.Data.player.stage);
		Managers.Scene.LoadScene(Define.Scene.Fight);
	}

	public void OnNextStageButtonClicked(PointerEventData data)
	{
		if (Managers.Data.Data.player.stage >= Managers.Data.Stages.Count)
		{
			Managers.UI.ShowPopupUI<UI_NoStage>();
			return;
		}
		FightManagers.Instance.SetStage(Managers.Data.Data.player.stage + 1);
		Managers.Scene.LoadScene(Define.Scene.Fight);
	}
}

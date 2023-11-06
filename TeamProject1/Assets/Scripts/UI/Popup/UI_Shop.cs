using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Shop : UI_Popup
{
	enum Buttons
	{
		ExitButton,
		GachaButton,
	}

	enum Texts
	{
		ExitText,
		GachaText,
		MoneyTitleText,
		MoneyText
	}

	enum GameObjects
	{
		Background,
		MoneyPanel,
	}

	int gachaMoney;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<UnityEngine.UI.Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<GameObject>(typeof(GameObjects));

		updateGacha(Managers.Data.Data.player.gacha);
		updateMoney();

		GetButton((int)Buttons.GachaButton).gameObject.BindEvent(OnGachaButtonClicked);
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
	}

	void updateGacha(int gacha)
	{
		// 뽑기시 필요한 "악의 구슬" 개수 변경시 수정 필요
		gachaMoney = 10 * gacha;
		Get<TextMeshProUGUI>((int)Texts.GachaText).text = $"캐릭터 뽑기\n악의 구슬 {gachaMoney} 소모";
	}

	void updateMoney()
	{
		Get<TextMeshProUGUI>((int)Texts.MoneyText).text = $"{Managers.Data.Data.player.money}";
	}

	public void OnGachaButtonClicked(PointerEventData data)
	{
		if (Managers.Data.Data.player.money < gachaMoney)
		{
			Managers.UI.ShowPopupUI<UI_NoMoneyAlert>();
			return;
		}

		Managers.Data.Data.player.money -= gachaMoney;
		Managers.Data.Data.player.gacha += 1;

		// 뽑기 확률 변경시 수정 필요
		// id 순서임.
		// id는 1번부터
		float[] gachaProbability = { 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f };

		int random = Util.Choose(gachaProbability) + 1;
		HaveChar c = new HaveChar();
		c.star = 0;
		c.character = new Character(Managers.Data.Characters.SearchById(random));
		Managers.Data.Data.player.characters.Add(c);

		Managers.Data.WriteJson("Data", Managers.Data.Data);

		UI_Gacha_Character gacha = Managers.UI.ShowPopupUI<UI_Gacha_Character>();
		gacha.SetInfo(c.character);

		updateGacha(Managers.Data.Data.player.gacha);
		updateMoney();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}
}

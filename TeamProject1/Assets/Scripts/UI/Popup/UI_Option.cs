using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Option : UI_Popup
{
	enum Buttons
	{
		ExitButton,
	}

	enum Texts
	{
		TitleText,
		SoundTitleText,
		MasterText,
		BgmText,
		EffectText,
		ExitText
	}

	enum Panels
	{
		Background,
		TitlePanel,
		SoundPanel,
		SoundTitlePanel,
		SoundContentPanel,
		MasterPanel,
		BgmPanel,
		EffectPanel
	}

	enum Sliders
	{
		MasterSlider,
		BgmSlider,
		EffectSlider,
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
		Bind<Slider>(typeof(Sliders));

		Get<Slider>((int)Sliders.MasterSlider).value = Managers.Sound.MasterVolume;
		Get<Slider>((int)Sliders.BgmSlider).value = Managers.Sound.AudioVolumes[(int)Define.Sound.Bgm];
		Get<Slider>((int)Sliders.EffectSlider).value = Managers.Sound.AudioVolumes[(int)Define.Sound.Effect];

		Get<Slider>((int)Sliders.MasterSlider).onValueChanged.AddListener(ChangeMaster);
		Get<Slider>((int)Sliders.BgmSlider).onValueChanged.AddListener(ChangeBgm);
		Get<Slider>((int)Sliders.EffectSlider).onValueChanged.AddListener(ChangeEffect);

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnButtonClicked);
	}

	public void ChangeMaster(float volume)
	{
		Managers.Sound.TurnMaster(volume);
	}

	public void ChangeBgm(float volume)
	{
		Managers.Sound.TurnVolume(Define.Sound.Bgm, volume);
	}

	public void ChangeEffect(float volume)
	{
		Managers.Sound.TurnVolume(Define.Sound.Effect, volume);
	}

	public void OnButtonClicked(PointerEventData data)
	{
		Managers.Data.WriteJson("Data", Managers.Data.Data);
		ClosePopupUI();
	}	
}

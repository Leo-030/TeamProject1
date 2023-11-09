using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Tutorial_Bgm : UI_Popup
{
	public string _bgm;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Managers.Sound.Play(_bgm, Define.Sound.Bgm);
	}
}

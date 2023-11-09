using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Tutorial;

		Managers.UI.ShowPopupUI<UI_Tutorial>($"Tutorial/UI_Tutorial_1").SetInfo(1, 38);
	}

	public override void Clear()
	{
		
	}
}

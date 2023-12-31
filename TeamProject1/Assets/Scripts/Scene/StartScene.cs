using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Start;

		Managers.UI.ShowSceneUI<UI_Start>();
		Managers.Sound.Play("Morning", Define.Sound.Bgm);
	}

	public override void Clear()
	{
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Fight;

		Managers.Sound.Play("Super Power Cool Dude", Define.Sound.Bgm);
		FightManagers.Instance.SetParty();
	}

	public override void Clear()
	{
		
	}
}

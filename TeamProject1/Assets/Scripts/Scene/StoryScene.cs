using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryScene : BaseScene
{
	protected override void Init()
	{
		base.Init();

		SceneType = Define.Scene.Story;

		Managers.UI.ShowSceneUI<UI_Story>();
	}

	public override void Clear()
	{
		
	}
}

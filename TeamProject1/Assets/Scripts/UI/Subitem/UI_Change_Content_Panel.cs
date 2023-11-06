using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Change_Content_Panel : UI_Base
{
	enum Panels
	{
		UI_Change_Content_Panel
	}

	void Start()
	{
		Init();
	}

	public override void Init()
	{
		Bind<GameObject>(typeof(Panels));
	}
}

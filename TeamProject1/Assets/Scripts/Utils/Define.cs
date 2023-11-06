using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
	public enum Sound
	{
		Bgm,
		Effect,
		MaxCount,
	}

	public enum Scene
	{
		Unknown,
		Start,
		Lobby,
		Fight,
		Story,
		Tutorial,
	}

	public enum UIEvent
	{
		Click,
		Drag,
		PointerDown,
		PointerUp,
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_HaveCollection : UI_Popup
{
	enum Buttons
	{
		ExitButton,
		AllButton,
		ClassButton1,
		ClassButton2,
		ClassButton3,
		ClassButton4,
	}

	enum Texts
	{
		ExitText,
	}

	enum GameObjects
	{
		Background,
		Content
	}

	int _numPerOneLine = 5;
	List<UI_HaveCollection_Character> _UI_HaveCollection_CharacterList;
	List<Transform> _panelList;
	UnityEngine.UI.Button _selected = null;

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

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
		GetButton((int)Buttons.AllButton).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.AllButton, "모두"); });
		GetButton((int)Buttons.ClassButton1).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton1, "전사"); });
		GetButton((int)Buttons.ClassButton2).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton2, "마법사"); });
		GetButton((int)Buttons.ClassButton3).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton3, "암살자"); });
		GetButton((int)Buttons.ClassButton4).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton4, "사제"); });

		Transform content = Get<GameObject>((int)GameObjects.Content).transform;
		List<HaveChar> characterList = new List<HaveChar>();
		foreach (HaveChar c in Managers.Data.Data.player.characters)
		{
			characterList.Add(c);
		}
		Util.Sort<HaveChar>(characterList, CompareName);

		_panelList = new List<Transform>();
		for (int i = 0; i < (characterList.Count + _numPerOneLine - 1) / _numPerOneLine; i++)
		{
			_panelList.Add(Managers.UI.MakeSubItem<UI_Collection_Content_Panel>(content).gameObject.transform);
		}

		_UI_HaveCollection_CharacterList = new List<UI_HaveCollection_Character>();
		for (int i = 0; i < characterList.Count; i++)
		{
			UI_HaveCollection_Character character = Managers.UI.MakeSubItem<UI_HaveCollection_Character>(_panelList[i / _numPerOneLine]);
			character.SetInfo(characterList[i]);
			character.gameObject.transform.localPosition = new Vector3(-420 + 220 * (i % _numPerOneLine), 0, 0);
			_UI_HaveCollection_CharacterList.Add(character);
		}

		_selected = GetButton((int)Buttons.AllButton);
		GetButton((int)Buttons.AllButton).enabled = false;
		GetButton((int)Buttons.AllButton).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(190, 23, 4, 255);
	}

	#region CompareFunc

	int CompareName(HaveChar c1, HaveChar c2)
	{
		if (c1.character.name == c2.character.name)
		{
			return 0;
		}
		else if (c1.character.name.CompareTo(c2.character.name) < 0)
		{
			return -1;
		}
		else
		{
			return 1;
		}
	}

	#endregion

	void SortAndPrint(List<HaveChar> characterList)
	{
		Util.Sort<HaveChar>(characterList, CompareName);
		for (int i = 0; i < (characterList.Count + _numPerOneLine - 1) / _numPerOneLine; i++)
		{
			_panelList[i].gameObject.SetActive(true);
		}
		for (int i = 0; i < characterList.Count; i++)
		{
			_UI_HaveCollection_CharacterList[i].OnReset(characterList[i]);
			Transform t = _UI_HaveCollection_CharacterList[i].gameObject.transform;
			t.SetParent(_panelList[i / _numPerOneLine]);
			t.localPosition = new Vector3(-420 + 220 * (i % _numPerOneLine), 0, 0);
			t.gameObject.SetActive(true);
		}
	}

	void Clear()
	{
		for (int i = 0; i < _panelList.Count; i++)
		{
			_panelList[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < _UI_HaveCollection_CharacterList.Count; i++)
		{
			_UI_HaveCollection_CharacterList[i].gameObject.SetActive(false);
		}
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}

	public void OnClassButtonsClicked(int button, string job)
	{
		if (_selected != null)
		{
			_selected.enabled = true;
			_selected.gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(239, 250, 0, 135);
		}
		_selected = GetButton(button);
		GetButton(button).enabled = false;
		GetButton(button).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(190, 23, 4, 255);

		List<HaveChar> list;
		if (job == "모두")
		{
			list = Managers.Data.Data.player.characters;
		}
		else
		{
			list = new List<HaveChar>();
			for (int i = 0; i < Managers.Data.Data.player.characters.Count; i++)
			{
				if (Managers.Data.Data.player.characters[i].character.job == job)
					list.Add(Managers.Data.Data.player.characters[i]);
			}
		}
		Clear();
		SortAndPrint(list);
	}
}

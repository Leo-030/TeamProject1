using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class UI_Collection: UI_Popup
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
		Content,
	}

	int _numPerOneLine = 5;
	List<UI_Collection_Character> collectionList;
	List<Transform> _panelList = new List<Transform>();
	UnityEngine.UI.Button _selected = null;

	private void Start()
	{
		Init();
	}

	public override void Init()
	{
		base.Init();

		Bind<Button>(typeof(Buttons));
		Bind<TextMeshProUGUI>(typeof(Texts));
		Bind<GameObject>(typeof(GameObjects));

		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);
		GetButton((int)Buttons.AllButton).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.AllButton, "모두"); });
		GetButton((int)Buttons.ClassButton1).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton1, "전사"); });
		GetButton((int)Buttons.ClassButton2).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton2, "마법사"); });
		GetButton((int)Buttons.ClassButton3).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton3, "암살자"); });
		GetButton((int)Buttons.ClassButton4).gameObject.BindEvent((PointerEventData data) => { OnClassButtonsClicked((int)Buttons.ClassButton4, "사제"); });

		Transform content = Get<GameObject>((int)GameObjects.Content).transform;
		List<Character> characterList = Managers.Data.Characters.Inorder();
		Util.Sort<Character>(characterList, CompareName);

		_panelList = new List<Transform>();
		for (int i = 0; i < (characterList.Count + _numPerOneLine - 1) / _numPerOneLine; i++)
		{
			_panelList.Add(Managers.UI.MakeSubItem<UI_Collection_Content_Panel>(content).gameObject.transform);
		}

		collectionList = new List<UI_Collection_Character>();
		for (int i = 0; i < characterList.Count; i++)
		{
			UI_Collection_Character character = Managers.UI.MakeSubItem<UI_Collection_Character>(_panelList[i / _numPerOneLine]);
			character.SetInfo(characterList[i]);
			character.gameObject.transform.localPosition = new Vector3(-420 + 220 * (i % _numPerOneLine), 0, 0);
			collectionList.Add(character);
		}

		_selected = GetButton((int)Buttons.AllButton);
		GetButton((int)Buttons.AllButton).enabled = false;
		GetButton((int)Buttons.AllButton).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(190, 23, 4, 255);
	}

	#region CompareFunc

	int CompareName(Character c1, Character c2)
	{
		if (c1.name == c2.name)
		{
			return 0;
		}
		else if (c1.name.CompareTo(c2.name) < 0)
		{
			return -1;
		}
		else
		{
			return 1;
		}
	}

	#endregion

	void SortAndPrint(List<Character> characterList)
	{
		Util.Sort<Character>(characterList, CompareName);
		for (int i = 0; i < (characterList.Count + _numPerOneLine - 1) / _numPerOneLine; i++)
		{
			_panelList[i].gameObject.SetActive(true);
		}
		for (int i = 0; i < characterList.Count; i++)
		{
			collectionList[i].OnReset(characterList[i]);
			Transform t = collectionList[i].gameObject.transform;
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

		for (int i = 0; i < collectionList.Count; i++)
		{
			collectionList[i].gameObject.SetActive(false);
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
			_selected.gameObject.GetComponent<Image>().color = new Color32(239, 250, 0, 135);
		}
		_selected = GetButton(button);
		GetButton(button).enabled = false;
		GetButton(button).gameObject.GetComponent<Image>().color = new Color32(190, 23, 4, 255);

		List<Character> list;
		if (job == "모두")
		{
			list = Managers.Data.Characters.Inorder();
		}
		else
		{
			List<Character> characters = Managers.Data.Characters.Inorder();
			list = new List<Character>();
			for (int i = 0; i < characters.Count; i++)
			{
				if (characters[i].job == job)
					list.Add(characters[i]);
			}
		}
		Clear();
		SortAndPrint(list);
	}
}

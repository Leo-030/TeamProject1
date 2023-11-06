using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UI_Set_Party : UI_Popup
{
	enum Buttons
	{
		OkayButton,
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

	List<HaveChar> _haveCharacterList = new List<HaveChar>();
	List<UI_Party_Character> _partyCharacterList = new List<UI_Party_Character>();
	List<UI_Party_Character_Null> _partyCharaceterNullList = new List<UI_Party_Character_Null>();

	int _numPerOneLine = 9;
	List<Transform> _panelList = null;
	List<UI_Party_Select_Character> UI_Party_Select_CharacterList = null;
	UnityEngine.UI.Button _selected = null;

	public Action<List<HaveChar>> SetAction = null;

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

		GetButton((int)Buttons.OkayButton).gameObject.BindEvent(OnOkayButtonClicked);
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
			_panelList.Add(Managers.UI.MakeSubItem<UI_Merge_Content_Panel>(content).gameObject.transform);
		}

		UI_Party_Select_CharacterList = new List<UI_Party_Select_Character>();
		for (int i = 0; i < characterList.Count; i++)
		{
			UI_Party_Select_Character character = Managers.UI.MakeSubItem<UI_Party_Select_Character>(_panelList[i / _numPerOneLine]);
			character.SetInfo(characterList[i]);
			character.ClickEvent -= OnCharacterClicked;
			character.ClickEvent += OnCharacterClicked;
			character.gameObject.transform.localPosition = new Vector3(-450 + 110 * (i % _numPerOneLine), 0, 0);
			UI_Party_Select_CharacterList.Add(character);
		}

		_selected = GetButton((int)Buttons.AllButton);
		GetButton((int)Buttons.AllButton).enabled = false;
		GetButton((int)Buttons.AllButton).gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(190, 23, 4, 255);

		for (int i = 0; i < 5; i++)
		{
			UI_Party_Character_Null obj = Managers.UI.MakeSubItem<UI_Party_Character_Null>(transform);
			obj.gameObject.transform.localPosition = new Vector3(-450 + 225 * i, 120, 0);
			_partyCharaceterNullList.Add(obj);
		}
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

	int CompareName(UI_Party_Select_Character c1, UI_Party_Select_Character c2)
	{
		if (c1.Havecharacter.character.name == c2.Havecharacter.character.name)
		{
			return 0;
		}
		else if (c1.Havecharacter.character.name.CompareTo(c2.Havecharacter.character.name) < 0)
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
		List<UI_Party_Select_Character> tmpList = new List<UI_Party_Select_Character>();
		for (int i = 0; i < characterList.Count; i++)
		{
			foreach (UI_Party_Select_Character c in UI_Party_Select_CharacterList)
			{
				if (c.Havecharacter == characterList[i])
				{
					tmpList.Add(c);
					break;
				}
			}
		}
		for (int i = 0; i < tmpList.Count; i++)
		{
			Transform t = tmpList[i].gameObject.transform;
			t.SetParent(_panelList[i / _numPerOneLine]);
			t.localPosition = new Vector3(-450 + 110 * (i % _numPerOneLine), 0, 0);
			t.gameObject.SetActive(true);
		}
	}

	void Clear()
	{
		for (int i = 0; i < _panelList.Count; i++)
		{
			_panelList[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < UI_Party_Select_CharacterList.Count; i++)
		{
			UI_Party_Select_CharacterList[i].gameObject.SetActive(false);
		}
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

	public bool OnCharacterClicked(HaveChar character, bool isSelected)
	{
		if (isSelected)
		{
			_haveCharacterList.Add(character);
		}	
		else
		{
			_haveCharacterList.Remove(character);
		}

		if (_haveCharacterList.Count > 5)
		{
			Managers.UI.ShowPopupUI<UI_CantParty>();
			return false;
		}
		foreach(UI_Party_Character c in _partyCharacterList)
		{
			c.gameObject.SetActive(false);
		}
		foreach(UI_Party_Character_Null c in _partyCharaceterNullList)
		{
			c.gameObject.SetActive(false);
		}
		int i = 0;
		for (; i < _haveCharacterList.Count; i++)
		{
			if ( i <_partyCharacterList.Count)
			{
				_partyCharacterList[i].OnReset(_haveCharacterList[i]);
				_partyCharacterList[i].gameObject.SetActive(true);
			}
			else
			{
				UI_Party_Character obj = Managers.UI.MakeSubItem<UI_Party_Character>(transform);
				obj.SetInfo(_haveCharacterList[i]);
				obj.gameObject.transform.localPosition = new Vector3(-450 + 225 * i, 120, 0);
				_partyCharacterList.Add(obj);
			}
		}
		for (; i < 5; i++)
		{
			_partyCharaceterNullList[i].gameObject.SetActive(true);
		}

		return true;
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		Managers.Resource.Destroy(FightManagers.Instance.gameObject);
		Managers.Scene.LoadScene(Define.Scene.Lobby);
	}

	public void OnOkayButtonClicked(PointerEventData data)
	{
		if (_haveCharacterList.Count == 0 || _haveCharacterList.Count > 5)
		{
			Managers.UI.ShowPopupUI<UI_CantParty>();
			return;
		}
		ClosePopupUI();
		if (SetAction == null)
			return;
		SetAction.Invoke(_haveCharacterList);
		
	}
}

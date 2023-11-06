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

public class UI_Merge : UI_Popup
{
	enum Buttons
	{
		MergeButton,
		ExitButton,
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

	int _numPerOneLine = 9;
	List<Transform> _panelList = null;
	List<UI_Merge_Character> UI_Merge_CharacterList = null;

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

		GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnMergeButtonClicked);
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);

		Print();
	}

	#region CompareFunc

	int CompareName(UI_Merge_Character c1, UI_Merge_Character c2)
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

	void Print()
	{
		Transform content = Get<GameObject>((int)GameObjects.Content).transform;
		List<HaveChar> characterList = Managers.Data.Data.player.characters;
		if (_panelList == null)
		{
			_panelList = new List<Transform>();
			for (int i = 0; i < (characterList.Count + _numPerOneLine - 1) / _numPerOneLine; i++)
			{
				_panelList.Add(Managers.UI.MakeSubItem<UI_Merge_Content_Panel>(content).gameObject.transform);
			}
		}
		else
		{
			while (true)
			{
				if ((characterList.Count / _numPerOneLine) + 1 == _panelList.Count)
				{
					break;
				}
				else if ((characterList.Count / _numPerOneLine) + 1 > _panelList.Count)
				{
					_panelList.Add(Managers.UI.MakeSubItem<UI_Merge_Content_Panel>(content).gameObject.transform);
				}
				else
				{
					_panelList.RemoveAt(_panelList.Count - 1);
				}
			}
		}

		if (UI_Merge_CharacterList != null)
		{
			while (true)
			{
				if (UI_Merge_CharacterList.Count == characterList.Count)
				{
					break;
				}
				else if (UI_Merge_CharacterList.Count > characterList.Count)
				{
					Managers.Resource.Destroy(UI_Merge_CharacterList[UI_Merge_CharacterList.Count - 1].gameObject);
					UI_Merge_CharacterList.RemoveAt(UI_Merge_CharacterList.Count - 1);
				}
				else
				{
					UI_Merge_Character character = Managers.UI.MakeSubItem<UI_Merge_Character>();
					UI_Merge_CharacterList.Add(character);
				}
			}

			for (int i = 0; i < characterList.Count; i++)
			{
				UI_Merge_CharacterList[i].OnReset(characterList[i]);
				UI_Merge_CharacterList[i].ClickAction -= OnCharacterClicked;
				UI_Merge_CharacterList[i].ClickAction += OnCharacterClicked;
			}
		}
		else
		{
			UI_Merge_CharacterList = new List<UI_Merge_Character>();
			for (int i = 0; i < characterList.Count; i++)
			{
				UI_Merge_Character character = Managers.UI.MakeSubItem<UI_Merge_Character>();
				character.SetInfo(characterList[i]);
				character.ClickAction -= OnCharacterClicked;
				character.ClickAction += OnCharacterClicked;
				UI_Merge_CharacterList.Add(character);

			}
		}
		Util.Sort<UI_Merge_Character>(UI_Merge_CharacterList, CompareName);
		for (int i = 0; i < UI_Merge_CharacterList.Count; i++)
		{
			Transform t = UI_Merge_CharacterList[i].gameObject.transform;
			t.SetParent(_panelList[i / _numPerOneLine]);
			t.localPosition = new Vector3(-450 + 110 * (i % _numPerOneLine), 0, 0);
		}
	}
	
	public void OnCharacterClicked(HaveChar character, bool isSelected)
	{
		if (isSelected)
			_haveCharacterList.Add(character);
		else
			_haveCharacterList.Remove(character);
	}

	public void OnMergeButtonClicked(PointerEventData data)
	{
		if (_haveCharacterList.Count <= 1)
		{
			Managers.UI.ShowPopupUI<UI_NoCharacter>();
			return;
		}

		bool isEqualAll = true;
		int id = _haveCharacterList[0].character.id;
		foreach (HaveChar c in _haveCharacterList)
		{
			if (c.character.id != id)
			{
				isEqualAll = false;
				break;
			}
		}

		if (!isEqualAll)
		{
			Managers.UI.ShowPopupUI<UI_DiffrentAlert>();
			return;
		}

		HaveChar newChar = new HaveChar();
		newChar.character = new Character(_haveCharacterList[0].character);
		int count = 0;
		foreach (HaveChar c in _haveCharacterList)
		{
			count += c.star + 1;
		}
		newChar.star = count - 1;

		// 100°­ ±îÁö
		if (newChar.star > 100)
		{
			Managers.UI.ShowPopupUI<UI_CantMergeAlert>();
			_haveCharacterList.Clear();
			Print();
			return;
		}

		foreach (HaveChar c in _haveCharacterList)
		{
			Managers.Data.Data.player.characters.Remove(c);
		}
		Managers.Data.Data.player.characters.Add(newChar);
		Managers.Data.WriteJson("Data", Managers.Data.Data);
		_haveCharacterList.Clear();
		Print();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}
}

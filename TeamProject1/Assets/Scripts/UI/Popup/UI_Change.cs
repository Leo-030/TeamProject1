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

public class UI_Change : UI_Popup
{
	enum Buttons
	{
		ChangeButton,
		ExitButton,
	}

	enum Texts
	{
		ExitText,
		MoneyTitleText,
		MoneyText,
	}

	enum GameObjects
	{
		Background,
		Content
	}

	UI_Change_Character _selected;

	int _numPerOneLine = 9;
	List<Transform> _panelList = null;
	List<UI_Change_Character> UI_Change_CharacterList = null;

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

		GetButton((int)Buttons.ChangeButton).gameObject.BindEvent(OnChangeButtonClicked);
		GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButtonClicked);

		updateMoney();

		Print();
	}

	#region CompareFunc

	int CompareName(UI_Change_Character c1, UI_Change_Character c2)
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
				_panelList.Add(Managers.UI.MakeSubItem<UI_Change_Content_Panel>(content).gameObject.transform);
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
					_panelList.Add(Managers.UI.MakeSubItem<UI_Change_Content_Panel>(content).gameObject.transform);
				}
				else
				{
					_panelList.RemoveAt(_panelList.Count - 1);
				}
			}
		}

		if (UI_Change_CharacterList != null)
		{
			while (true)
			{
				if (UI_Change_CharacterList.Count == characterList.Count)
				{
					break;
				}
				else if (UI_Change_CharacterList.Count > characterList.Count)
				{
					Managers.Resource.Destroy(UI_Change_CharacterList[UI_Change_CharacterList.Count - 1].gameObject);
					UI_Change_CharacterList.RemoveAt(UI_Change_CharacterList.Count - 1);
				}
				else
				{
					UI_Change_Character character = Managers.UI.MakeSubItem<UI_Change_Character>();
					UI_Change_CharacterList.Add(character);
				}
			}

			for (int i = 0; i < characterList.Count; i++)
			{
				UI_Change_CharacterList[i].OnReset(characterList[i]);
				UI_Change_CharacterList[i].ClickAction -= OnCharacterClicked;
				UI_Change_CharacterList[i].ClickAction += OnCharacterClicked;
			}
		}
		else
		{
			UI_Change_CharacterList = new List<UI_Change_Character>();
			for (int i = 0; i < characterList.Count; i++)
			{
				UI_Change_Character character = Managers.UI.MakeSubItem<UI_Change_Character>();
				character.SetInfo(characterList[i]);
				character.ClickAction -= OnCharacterClicked;
				character.ClickAction += OnCharacterClicked;
				UI_Change_CharacterList.Add(character);

			}
		}
		Util.Sort<UI_Change_Character>(UI_Change_CharacterList, CompareName);
		for (int i = 0; i < UI_Change_CharacterList.Count; i++)
		{
			Transform t = UI_Change_CharacterList[i].gameObject.transform;
			t.SetParent(_panelList[i / _numPerOneLine]);
			t.localPosition = new Vector3(-450 + 110 * (i % _numPerOneLine), 0, 0);
		}
	}

	public void OnCharacterClicked(UI_Change_Character character, bool isSelected)
	{
		if (isSelected)
		{
			if (_selected != null)
			{
				_selected.OnButtonClicked(null);
			}
			_selected = character;
		}
		else
		{
			_selected = null;
		}	
	}
	void updateMoney()
	{
		Get<TextMeshProUGUI>((int)Texts.MoneyText).text = $"{Managers.Data.Data.player.money}";
	}

	public void OnChangeButtonClicked(PointerEventData data)
	{
		if (_selected == null)
		{
			Managers.UI.ShowPopupUI<UI_No_Change_Character>();
			return;
		}

		// 변환시 필요한 "악의 구슬" 개수 변경시 수정 필요
		int money = 20 * _selected.Havecharacter.star + 20;
		string text = $"{_selected.Havecharacter.character.name} {_selected.Havecharacter.star}강을 다른 캐릭터로 변환하시겠습니까?\n";
		text += $"총 악의 구슬 {money}개가 소모됩니다.\n";
		text += $"(1강당 20씩 증가합니다.)";
		UI_ChangeAlert ui = Managers.UI.ShowPopupUI<UI_ChangeAlert>();
		ui.SetInfo(text);
		ui.OkayAction = () => { Change(money); };

	}

	public void Change(int money)
	{
		if (Managers.Data.Data.player.money < money)
		{
			Managers.UI.ShowPopupUI<UI_NoMoneyAlert>();
			return;
		}

		Managers.Data.Data.player.money -= money;

		// 변환 확률 변경시 수정 필요
		// id 순서임.
		// id는 1번부터
		// 자기는 제외
		float[] probability = { 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f, 0.33f };
		probability[_selected.Havecharacter.character.id - 1] = 0.0f;

		int random = Util.Choose(probability) + 1;

		HaveChar c = new HaveChar();
		c.star = _selected.Havecharacter.star;
		c.character = new Character(Managers.Data.Characters.SearchById(random));
		Managers.Data.Data.player.characters.Remove(_selected.Havecharacter);
		Managers.Data.Data.player.characters.Add(c);

		Managers.Data.WriteJson("Data", Managers.Data.Data);
		_selected = null;
		updateMoney();
		Print();
	}

	public void OnExitButtonClicked(PointerEventData data)
	{
		ClosePopupUI();
	}
}

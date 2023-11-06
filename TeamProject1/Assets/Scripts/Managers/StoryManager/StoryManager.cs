using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
	static StoryManager s_instance;
	public static StoryManager Instance
	{
		get
		{
			Init();
			return s_instance;
		}
	}

	int _storyNum;
	public int StoryNum
	{
		get
		{
			return _storyNum; 
		}
	}

	void Start()
    {
		Init();
	}

	void Update()
	{
		
	}

	static void Init()
	{
		if (s_instance == null)
		{
			GameObject obj = GameObject.Find("@StoryManager");
			if (obj == null)
			{
				obj = new GameObject { name = "@StoryManager" };
				obj.AddComponent<StoryManager>();
			}

			DontDestroyOnLoad(obj);
			s_instance = obj.GetComponent<StoryManager>();
		}
	}

	public void SetStoryNum(int storyNum)
	{
		_storyNum = storyNum;
	}

	public void SetStoryObj(int storyNum)
	{
		// 스토리 추가시 추가 필요
		switch (storyNum)
		{
			case 1:
				setStoryObj1();
				break;
			case 2:
				setStoryObj2();
				break;
			case 3:
				setStoryObj3();
				break;
			case 4:
				setStoryObj4();
				break;
		}
	}

	void setStoryObj1()
	{
		GameObject obj = Managers.Resource.Instantiate("StoryObject");
		obj.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Images/Bosses/1/StoryObj");
		obj.transform.position = new Vector3(0.0f, 1.25f, 0.0f);
	}

	void setStoryObj2()
	{
		GameObject obj = Managers.Resource.Instantiate("StoryObject");
		obj.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Images/Bosses/2/StoryObj");
		obj.transform.position = new Vector3(0.0f, 1.25f, 0.0f);
	}

	void setStoryObj3()
	{
		GameObject obj = Managers.Resource.Instantiate("StoryObject");
		obj.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Images/Bosses/3/StoryObj");
		obj.transform.position = new Vector3(0.0f, 1.25f, 0.0f);
	}

	void setStoryObj4()
	{
		GameObject obj = Managers.Resource.Instantiate("StoryObject");
		obj.GetComponent<SpriteRenderer>().sprite = Managers.Resource.Load<Sprite>($"Images/Bosses/4/StoryObj");
		obj.transform.position = new Vector3(0.0f, 1.25f, 0.0f);
	}
}

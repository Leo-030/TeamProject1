using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
	static Managers s_instance;
	static Managers Instance
	{
		get
		{
			Init();
			return s_instance;
		}
	}

	DataManager _data = new DataManager();
	PoolManager _pool = new PoolManager();
	ResourceManager _resource = new ResourceManager();
	SceneManagerEx _scene = new SceneManagerEx();
	SoundManager _sound = new SoundManager();
	UIManager _ui = new UIManager();

	public static DataManager Data
	{
		get
		{
			return Instance._data;
		}
	}

	public static PoolManager Pool
	{
		get
		{
			return Instance._pool;
		}
	}

	public static ResourceManager Resource
	{
		get
		{
			return Instance._resource;
		}
	}

	public static SceneManagerEx Scene
	{
		get
		{
			return Instance._scene;
		}
	}

	public static SoundManager Sound
	{
		get
		{
			return Instance._sound;
		}
	}

	public static UIManager UI
	{
		get
		{
			return Instance._ui;
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
			GameObject obj = GameObject.Find("@Managers");
			if (obj == null)
			{
				obj = new GameObject { name = "@Managers" };
				obj.AddComponent<Managers>();
			}

			DontDestroyOnLoad(obj);
			s_instance = obj.GetComponent<Managers>();

			//Init
			s_instance._data.Init();
			s_instance._sound.Init();
			s_instance._pool.Init();

			//Set
			s_instance._sound.Set();
		}
	}

	public static void Clear()
	{
		Sound.Clear();
		Scene.Clear();
		UI.Clear();
		Pool.Clear();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
	public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
	{
		T component = go.GetComponent<T>();
		if (component == null)
		{
			component = go.AddComponent<T>();
		}

		return component;
	}

	public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
	{
		Transform transform = FindChild<Transform>(go, name, recursive);
		if (transform == null)
		{
			return null;
		}

		return transform.gameObject;
	}

	public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
	{
		if (go == null)
		{
			return null;
		}

		if (recursive == false)
		{
			for (int i = 0; i < go.transform.childCount; i++)
			{
				Transform transform = go.transform.GetChild(i);
				if (string.IsNullOrEmpty(name) || transform.name == name)
				{
					T component = transform.GetComponent<T>();
					if (component != null)
					{
						return component;
					}
				}
			}
		}
		else
		{
			foreach (T component in go.GetComponentsInChildren<T>())
			{
				if (string.IsNullOrEmpty(name) || component.name == name)
				{
					return component;
				}
			}
		}

		return null;
	}

	#region Sort

	public delegate int CompareFuc<T>(T c1, T c2);

	public static void Sort<T>(List<T> list, CompareFuc<T> compare)
	{
		quickSort<T>(list, compare, 0, list.Count - 1);
	}

	static void quickSort<T>(List<T> list, CompareFuc<T> compare, int first, int last)
	{
		if (first >= last)
			return;

		int p = partition<T>(list, compare, first, last);
		quickSort<T>(list, compare, first, p - 1);
		quickSort<T>(list, compare, p + 1, last);
	}

	static int partition<T>(List<T> list, CompareFuc<T> compare, int first, int last)
	{
		T pivot = list[first];
		int i = first + 1;
		int j = last;
		while (i <= j)
		{
			if (compare(list[i], pivot) <= 0)
			{
				i++;
			}
			else if (compare(list[j], pivot) > 0)
			{
				j--;
			}
			else
			{
				swap(list, i, j);
			}
		}

		swap(list, first, j);

		return j;
	}

	static void swap<T>(List<T> list, int i, int j)
	{
		T tmp = list[i];
		list[i] = list[j];
		list[j] = tmp;
	}

	#endregion

	public static int Choose(float[] probs)
	{

		float total = 0;

		foreach (float elem in probs)
		{
			total += elem;
		}

		float randomPoint = UnityEngine.Random.value * total;

		for (int i = 0; i < probs.Length; i++)
		{
			if (randomPoint < probs[i])
			{
				return i;
			}
			else
			{
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}
}

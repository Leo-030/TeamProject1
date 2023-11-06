using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Coroutine : MonoBehaviour
{
	IEnumerator _coroutine = null;
	bool isDamage = false;
	bool isHit = false;
	Vector3 pos;
	Vector3 to;

	void Start()
	{
	}

	void Update()
	{
		if (isDamage == true || isHit == true)
		{
			StartCoroutine(_coroutine);
			isDamage = false;
			isHit = false;
		}
	}

	public void SetInfo(Vector3 pos)
	{
		this.pos = pos;
	}

	public void SetDamage(Vector3 c)
	{
		to = c;
		if (_coroutine != null)
			StopCoroutine(_coroutine);
		_coroutine = damageMotion();
		isDamage = true;
	}

	public void SetHit()
	{
		if (_coroutine != null)
			StopCoroutine(_coroutine);
		_coroutine = hitMotion();
		isHit = true;
	}

	IEnumerator damageMotion()
	{
		transform.position = pos;
		Vector3 from = transform.position;
		float delta = 0;
		float duration = 0.1f;

		while (delta <= duration)
		{
			float t = delta / duration;
			transform.position = Vector3.Lerp(from, to, t);
			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		delta = 0;
		while (delta <= duration)
		{
			float t = delta / duration;
			transform.position = Vector3.Lerp(to, from, t);
			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		transform.position = from;
	}

	IEnumerator hitMotion()
	{
		transform.position = pos;
		Vector3 from = transform.position;
		Vector3 to1 = from + new Vector3(0.0f, 0.5f, 0.0f);
		Vector3 to2 = from + new Vector3(0.0f, -0.5f, 0.0f);

		float delta = 0;
		float duration = 0.1f;

		while (delta <= duration)
		{
			float t = delta / duration;

			transform.position = Vector3.Lerp(from, to1, t);

			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		delta = 0;
		while (delta <= duration)
		{
			float t = delta / duration;

			transform.position = Vector3.Lerp(to1, from, t);

			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		delta = 0;
		while (delta <= duration)
		{
			float t = delta / duration;

			transform.position = Vector3.Lerp(from, to2, t);

			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		delta = 0;
		while (delta <= duration)
		{
			float t = delta / duration;

			transform.position = Vector3.Lerp(to2, from, t);

			delta += Time.deltaTime;
			yield return new WaitForSeconds(0.01f);
		}
		transform.position = from;
	}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightCharObject : MonoBehaviour
{
	Slider _hpslider;
	TextMeshProUGUI _hptext;
	Slider _gaugeslider;
	TextMeshProUGUI _gaugetext;
	SpriteRenderer _color;

	void Awake()
	{
		_hpslider = Util.FindChild<Slider>(this.gameObject, "HpSlider", true);
		_hptext = Util.FindChild<TextMeshProUGUI>(this.gameObject, "Hp", true);
		_gaugeslider = Util.FindChild<Slider>(this.gameObject, "GaugeSlider", true);
		_gaugetext = Util.FindChild<TextMeshProUGUI>(this.gameObject, "Gauge", true);
		_color = gameObject.GetComponent<SpriteRenderer>();
	}

	public void SetTurn()
    {
		_color.color = new Color32(0, 255, 0, 255);
    }

    public void EndTurn()
    {
		_color.color = new Color32(255, 255, 255, 255);
	}

	public void OnUpdateHp(int chp, int maxhp)
	{
		_hptext.text = $"{chp}/{maxhp}";
		_hpslider.value = (float)chp / maxhp ;
	}

	public void OnUpdateGauge(int cgauge, int maxgauge)
	{
		_gaugetext.text = $"{cgauge}/{maxgauge}";
		_gaugeslider.value = (float)cgauge / maxgauge;
	}
}

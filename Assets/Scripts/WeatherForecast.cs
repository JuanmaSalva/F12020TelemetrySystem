using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeatherForecast : MonoBehaviour
{
    public TextMeshProUGUI time;
    public Image weatherIcon;
    public TextMeshProUGUI airTemp;
    public TextMeshProUGUI trackTemp;

    public void UpdateInfo(Sprite weatherSprite, byte timeoffset, sbyte tTemp, sbyte aTemp)
    {
        weatherIcon.sprite = weatherSprite;
        time.text = "+ " + timeoffset.ToString() + "min";
        airTemp.text = "A: " + aTemp.ToString() + "º";
        trackTemp.text = "T: " + tTemp.ToString() + "º";
    }
}

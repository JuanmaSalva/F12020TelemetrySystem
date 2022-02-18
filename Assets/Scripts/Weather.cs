using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Weather : MonoBehaviour
{
    public Image currentWeatherIcon;
    public TextMeshProUGUI currentAirTemp;
    public TextMeshProUGUI currentTrackTemp;

    public Image trackIcon;
    public Image airIcon;

    public WeatherForecast[] futureWeatherForecasts;

    void Start()
    {
        currentWeatherIcon.color = Manager.instance.colorPalette.PanelInfo;
        currentAirTemp.color = Manager.instance.colorPalette.PanelInfo;
        currentTrackTemp.color = Manager.instance.colorPalette.PanelInfo;
        trackIcon.color = Manager.instance.colorPalette.PanelInfo;
        airIcon.color = Manager.instance.colorPalette.PanelInfo;

        foreach (WeatherForecast wf in futureWeatherForecasts)
        {
            wf.time.color = Manager.instance.colorPalette.PanelInfo;
            wf.weatherIcon.color = Manager.instance.colorPalette.PanelInfo;
            wf.airTemp.color = Manager.instance.colorPalette.PanelInfo;
            wf.trackTemp.color = Manager.instance.colorPalette.PanelInfo;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

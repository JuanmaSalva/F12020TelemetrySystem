using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class Weather : MonoBehaviour
{
    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_weather();
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_trackTemperature();
    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_airTemperature();


    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_numWeatherForecastSamples();

    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_weatherForecastSampleTimeOffset(sbyte sample);

    [DllImport("F12020Telemetry")]
    private static extern byte F1TS_weatherForecastSampleWeather(sbyte sample);

    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_weatherForecastSampleTrackTemperature(sbyte sample);

    [DllImport("F12020Telemetry")]
    private static extern sbyte F1TS_weatherForecastSampleAirTemperature(sbyte sample);



    public Image currentWeatherIcon;
    public TextMeshProUGUI currentAirTemp;
    public TextMeshProUGUI currentTrackTemp;

    public Image trackIcon;
    public Image airIcon;

    public Sprite[] weatherSprites;
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


    void Update()
    {
        currentWeatherIcon.sprite = GetWeatherSprite(F1TS_weather());
        UpdateFutureWeatherForecasts();

        currentTrackTemp.text = "Track: " + F1TS_trackTemperature().ToString() + "º";
        currentAirTemp.text = "Air: " + F1TS_airTemperature().ToString() + "º";
    }

    private Sprite GetWeatherSprite(byte state)
    {
        if (state >= 0 && state < weatherSprites.Length)
            return weatherSprites[state];
        else
            return weatherSprites[0];
    }
    private void UpdateFutureWeatherForecasts()
    {
        sbyte i = 0;
        for(i = 0; i < F1TS_numWeatherForecastSamples() && i < futureWeatherForecasts.Length; i++)
        {
            futureWeatherForecasts[i].UpdateInfo(GetWeatherSprite(F1TS_weatherForecastSampleWeather(i)),
                F1TS_weatherForecastSampleTimeOffset(i), F1TS_weatherForecastSampleTrackTemperature(i),
                F1TS_weatherForecastSampleAirTemperature(i));
        }

        for(sbyte j = i; j < futureWeatherForecasts.Length; j++)
        {
            futureWeatherForecasts[j].UpdateInfo(GetWeatherSprite(F1TS_weather()), (byte)(5 * j),
                F1TS_trackTemperature(), F1TS_airTemperature());
        }
    }
}

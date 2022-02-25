using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine.UI;

public class TyreController : TelemetryListener
{
	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_brakesTemperature(byte carId, byte tyre);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_tyresSurfaceTemperature(byte carId, byte tyre);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_tyresInnerTemperature(byte carId, byte tyre);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_engineTemperature(byte carId);

	public Tyre[] tyres;

	public Image engine;
	public TextMeshProUGUI engineTemp;

	public int optimalInnerTyreTemp;
	public int maxInnerTyreTemp;

	public int optimalOuterTyreTemp;
	public int maxOuterTyreTemp;

	public int optimalBrakeTemp;
	public int maxBrakeTemp;

	public int optimalEngineTemp;
	public int maxEngineTemp;


	private byte _currentPlayerCarId = 0;


	private void Start()
	{
		Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
		EventManager.instance.AddListener(this);
		foreach (Tyre tyre in tyres)
			tyre.ChangeColor();
	}

	void Update()
	{
		for (byte i = 0; i < 4; i++)
			UpdateTyreData(i);


		ushort temp = F1TS_engineTemperature(_currentPlayerCarId);
		if (temp == 0)
			engineTemp.text = "- °C";
		else
		{
			engineTemp.text = temp + "°C";
			if (temp <= optimalEngineTemp)
				engine.color = Manager.instance.colorPalette.GreenStatus;
			else
			{
				engine.color = Color.Lerp(Manager.instance.colorPalette.GreenStatus,
					Manager.instance.colorPalette.RedStatus, 
					(temp - optimalEngineTemp) / (float)(maxEngineTemp - optimalEngineTemp));
			}
		}
	}

	void UpdateTyreData(byte tyre)
	{
		UpdateTyreSurfaceTemp(tyre);
		UpdateTyreInnerTemp(tyre);
		UpdateBrakeTemp(tyre);
	}


	void UpdateTyreSurfaceTemp(byte tyre)
	{
		ushort temp = F1TS_tyresSurfaceTemperature(_currentPlayerCarId, tyre);
		if (temp == 0)
			tyres[tyre].surfaceTemp.text = "Surface temp: - °C";
		else
		{
			tyres[tyre].surfaceTemp.text = "Surface temp: " + temp + "°C";
			if (temp <= optimalOuterTyreTemp)
				tyres[tyre].surface.color = Manager.instance.colorPalette.GreenStatus;
			else
			{
				tyres[tyre].surface.color = Color.Lerp(Manager.instance.colorPalette.GreenStatus,
					Manager.instance.colorPalette.RedStatus, 
					(temp - optimalOuterTyreTemp) / (float)(maxOuterTyreTemp - optimalOuterTyreTemp));
			}
		}
	}

	void UpdateTyreInnerTemp(byte tyre)
	{
		ushort temp = F1TS_tyresInnerTemperature(_currentPlayerCarId, tyre);
		if (temp == 0)
			tyres[tyre].innerTemp.text = "Inner temp: - °C";
		else
		{
			tyres[tyre].innerTemp.text = "Inner temp: " + temp + "°C";
			if (temp <= optimalInnerTyreTemp)
				tyres[tyre].inner.color = Manager.instance.colorPalette.GreenStatus;
			else
			{
				tyres[tyre].inner.color = Color.Lerp(Manager.instance.colorPalette.GreenStatus,
					Manager.instance.colorPalette.RedStatus,
					(temp - optimalInnerTyreTemp) / (float)(maxInnerTyreTemp - optimalInnerTyreTemp));
			}
		}
	}

	void UpdateBrakeTemp(byte tyre)
	{
		ushort temp = F1TS_brakesTemperature(_currentPlayerCarId, tyre);
		if (temp == 0)
			tyres[tyre].brakeTemp.text = "Brake temp: - °C";
		else
		{
			tyres[tyre].brakeTemp.text = "Brake temp: " + temp + "°C";
			if (temp <= optimalBrakeTemp)
				tyres[tyre].brake.color = Manager.instance.colorPalette.GreenStatus;
			else
			{
				tyres[tyre].brake.color = Color.Lerp(Manager.instance.colorPalette.GreenStatus,
					Manager.instance.colorPalette.RedStatus, 
					(temp - optimalBrakeTemp) / (float)(maxBrakeTemp - optimalBrakeTemp));
			}
		}
	}

	// void UpdateTyreLaps(byte tyre)
	// {
	//     tyres[tyre].laps.text = "Laps: " + F1TS_tyresAgeLaps(_currentPlayerCarId);
	// }
	//
	//
	// void UpdateTyrePresure(byte tyre)
	// {
	//     tyres[tyre].presure.text = "Presure: " + F1TS_tyrePressure(_currentPlayerCarId, tyre);
	// }


	// void UpdateTyreCompound(byte tyre)
	// {
	//     ushort curretnCompund = F1TS_actualTyreCompound(_currentPlayerCarId);
	//     tyres[tyre].compound.text = tyres[tyre].GetCompundString(curretnCompund);
	// }
	//
	// void UpdateTyreWear(byte tyre)
	// {
	//     tyres[tyre].wear.text = (100 - F1TS_tyresWear(_currentPlayerCarId, tyre)) + "%";
	// }
	public override void OnPlayerCarIdChanged(byte playerCarId)
	{
		_currentPlayerCarId = playerCarId;
	}
}
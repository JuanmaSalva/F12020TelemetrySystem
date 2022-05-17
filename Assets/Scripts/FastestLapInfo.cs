using System;
using System.Runtime.InteropServices;
using TMPro;

public class FastestLapInfo : TelemetryListener
{
	[DllImport("F12020Telemetry")]
	private static extern float F1TS_bestLapTime(byte id);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestLapSector1TimeInMS(byte carId);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestOverallSector1TimeInMS(byte carId);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestLapSector2TimeInMS(byte carId);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestOverallSector2TimeInMS(byte carId);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestLapSector3TimeInMS(byte carId);

	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_bestOverallSector3TimeInMS(byte carId);


	public TextMeshProUGUI fastestLapText;
	public TextMeshProUGUI sector1Text;
	public TextMeshProUGUI sector2Text;
	public TextMeshProUGUI sector3Text;

	
	private int _overallBestLap = Int32.MaxValue;//taking into account all cars on track
	private int _overallBestS1 = Int32.MaxValue; //taking into account all cars on track
	private int _overallBestS2 = Int32.MaxValue; //taking into account all cars on track
	private int _overallBestS3 = Int32.MaxValue; //taking into account all cars on track

	private int _lapTime = Int32.MaxValue;
	private int _s1Time;
	private int _s2Time;
	private int _s3Time;

	private byte _currentPlayerCarId = 0;


	void Start()
	{
		Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
		EventManager.instance.AddListener(this);

		fastestLapText.color = Manager.instance.colorPalette.NormalTime;
		sector1Text.color = Manager.instance.colorPalette.NormalTime;
		sector2Text.color = Manager.instance.colorPalette.NormalTime;
		sector3Text.color = Manager.instance.colorPalette.NormalTime;

		ResetTimes();
	}

	private void ResetTimes()
	{
		fastestLapText.text = "Lap: 00,00.000";
		sector1Text.text = "Sector 1: 00,00.000";
		sector2Text.text = "Sector 2: 00,00.000";
		sector3Text.text = "Sector 3: 00,00.000";
	}

	private void Update()
	{
		ushort s1Time = F1TS_bestLapSector1TimeInMS(_currentPlayerCarId);
		if (s1Time != 0)
		{
			ChangeTextColor(sector1Text, s1Time, 
				F1TS_bestOverallSector1TimeInMS(_currentPlayerCarId), _overallBestS1);
		}


		ushort s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		if(s2Time != 0){
			ChangeTextColor(sector2Text, s2Time, 
				F1TS_bestOverallSector2TimeInMS(_currentPlayerCarId), _overallBestS2);
		}

		int s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		if(s3Time != 0)
		{
			ChangeTextColor(sector3Text, s3Time, 
				F1TS_bestOverallSector3TimeInMS(_currentPlayerCarId), _overallBestS3);
		}
	}
	
	private void ChangeTextColor(TextMeshProUGUI text, int time, int personalBest, int overallBest)
	{
		if (time <= overallBest)
			text.color = Manager.instance.colorPalette.OverallBestTime;
		else if (time <= personalBest)
			text.color = Manager.instance.colorPalette.PersonalBestTime;
		else
			text.color = Manager.instance.colorPalette.NormalTime;
	}

	
	
	
	public void SetOverallFastestSector(int sector, int time)
	{
		switch (sector)
		{
			case 1:
				_overallBestS1 = time;
				break;
			case 2:
				_overallBestS2 = time;
				break;
			case 3:
				_overallBestS3 = time;
				break;
		}
	}
	
	public void SetOverallFastestLap(int time)
	{
		print("Overall fatest lap: " + time);
		_overallBestLap = time;
		fastestLapText.text = "Lap: " + FromTimeToStringFormat(time);
		ChangeTextColor(fastestLapText, (int)(F1TS_bestLapTime(_currentPlayerCarId) * 1000), 
			(int)(F1TS_bestLapTime(_currentPlayerCarId) * 1000), _overallBestLap);
		OnFastestLap(time);
	}
	
	
	public override void OnFastestLap(float time)
	{
		if (time <= 0)
			return;
		
		//Lap time
		int currentLapMili = (int)(time * 1000);
		if (currentLapMili <= 0)
			return;
		_lapTime = currentLapMili;
		
		//fastestLapText.text = "Lap: " + FromTimeToStringFormat(currentLapMili);

		//TODO esto no haria falta
		// if (time <= _overallBestLap)
		// 	fastestLapText.color = Manager.instance.colorPalette.OverallBestTime;
		// else
		// 	fastestLapText.color = Manager.instance.colorPalette.PersonalBestTime;


		ushort s1Time = F1TS_bestLapSector1TimeInMS(_currentPlayerCarId);
		sector1Text.text = "Sector 1: " + FromTimeToStringFormat(s1Time);


		ushort s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		sector2Text.text = "Sector 2: " + FromTimeToStringFormat(s2Time);


		int s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		sector3Text.text = "Sector 3: " + FromTimeToStringFormat(s3Time);
	}
	
	private String FromTimeToStringFormat(int time)
	{            
		int m = time / 60000;
		int secMill = (time - m * 60000);
		int s = secMill / 1000;
		int ms = secMill % 1000;
		return m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");
	}

	public override void OnPlayerCarIdChanged(byte playerCarId)
	{
		_currentPlayerCarId = playerCarId;
	}
}
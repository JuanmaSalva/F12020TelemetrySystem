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

	public CurrentLapInfo currentLapInfo;

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
			if (s1Time <= F1TS_bestOverallSector1TimeInMS(_currentPlayerCarId))
				sector1Text.color = Manager.instance.colorPalette.OverallBestTime;
			else if (s1Time <= currentLapInfo.SessionBestS1())
				sector1Text.color = Manager.instance.colorPalette.PersonalBestTime;
			else
				sector1Text.color = Manager.instance.colorPalette.NormalTime;
		}


		ushort s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		if(s2Time != 0){
			if (s2Time <= F1TS_bestOverallSector2TimeInMS(_currentPlayerCarId))
				sector2Text.color = Manager.instance.colorPalette.OverallBestTime;
			else if (s2Time <= currentLapInfo.SessionBestS2())
				sector2Text.color = Manager.instance.colorPalette.PersonalBestTime;
			else
				sector2Text.color = Manager.instance.colorPalette.NormalTime;
		}

		int s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		if(s3Time != 0)
		{
			if (s3Time <= F1TS_bestOverallSector3TimeInMS(_currentPlayerCarId))
				sector3Text.color = Manager.instance.colorPalette.OverallBestTime;
			else if (s3Time <= currentLapInfo.SessionBestS3())
				sector3Text.color = Manager.instance.colorPalette.PersonalBestTime;
			else
				sector3Text.color = Manager.instance.colorPalette.NormalTime;
		}
	}

	public override void OnFastestLap(float time)
	{
		int s, ms;

		//Lap time
		int currentLapMili = (int)(time * 1000);
		if (currentLapMili != 0.0)
		{
			int m = currentLapMili / 60000;
			int secMill = (currentLapMili - m * 60000);
			s = secMill / 1000;
			ms = secMill % 1000;
			fastestLapText.text = "Lap: " + m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");

			if (time <= F1TS_bestLapTime(_currentPlayerCarId))
				fastestLapText.color = Manager.instance.colorPalette.OverallBestTime;
			else
				fastestLapText.color = Manager.instance.colorPalette.PersonalBestTime;
		}


		ushort s1Time = F1TS_bestLapSector1TimeInMS(_currentPlayerCarId);
		s = s1Time / 1000;
		ms = s1Time % 1000;
		sector1Text.text = "Sector 1: 00," + s.ToString("00") + "." + ms.ToString("000");


		ushort s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		s = s2Time / 1000;
		ms = s2Time % 1000;
		sector2Text.text = "Sector 2: 00," + s.ToString("00") + "." + ms.ToString("000");


		int s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		s = s3Time / 1000;
		ms = s3Time % 1000;
		sector3Text.text = "Sector 3: 00," + s.ToString("00") + "." + ms.ToString("000");
	}

	public override void OnPlayerCarIdChanged(byte playerCarId)
	{
		_currentPlayerCarId = playerCarId;
	}
}
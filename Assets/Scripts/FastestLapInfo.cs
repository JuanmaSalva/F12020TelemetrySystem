using System;
using System.Runtime.InteropServices;
using TMPro;

public class FastestLapInfo : TelemetryListener, ILapListener
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
	
	private int _lapTime = Int32.MaxValue;
	private int _s1Time;
	private int _s2Time;
	private int _s3Time;

	private byte _currentPlayerCarId = 0;

	public LapManager lapManager;

	void Start()
	{
		Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
		EventManager.instance.AddListener(this);
		lapManager.AddLapListener(this);

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
	

	
	public override void OnFastestLap(float time) //fastest personal lap
	{
		print("FASTEST LAP ");
		if (time <= 0)
			return;
		
		//Lap time
		int currentLapMili = (int)(time * 1000);
		if (currentLapMili <= 0)
			return;
		_lapTime = currentLapMili;

		fastestLapText.text = "Lap: " + FromTimeToStringFormat(_lapTime);

		_s1Time = F1TS_bestLapSector1TimeInMS(_currentPlayerCarId);
		sector1Text.text = "Sector 1: " + FromTimeToStringFormat(_s1Time);
		

		_s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		sector2Text.text = "Sector 2: " + FromTimeToStringFormat(_s2Time);


		_s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		sector3Text.text = "Sector 3: " + FromTimeToStringFormat(_s3Time);
		
		//Change text colors
		ChangeTextColor(fastestLapText, _lapTime, lapManager.GetPersonalFastestLap(),
			lapManager.GetOverallFastestLap());
		ChangeTextColor(sector1Text, _s1Time, lapManager.GetPersonalFastestSector1(),
			lapManager.GetOverallFastestSector1());
		ChangeTextColor(sector2Text, _s2Time, lapManager.GetPersonalFastestSector2(),
			lapManager.GetOverallFastestSector2());
		ChangeTextColor(sector3Text, _s3Time, lapManager.GetPersonalFastestSector3(),
			lapManager.GetOverallFastestSector3());
		
	}
	
	
	private void ChangeTextColor(TextMeshProUGUI text, int time, int personalBest, int overallBest)
	{
		if (time <= personalBest)
		{
			if(time <= overallBest && time == personalBest)
				text.color = Manager.instance.colorPalette.OverallBestTime;
			else 
				text.color = Manager.instance.colorPalette.PersonalBestTime;
		}
		else
			text.color = Manager.instance.colorPalette.NormalTime;
	}

	private void DownGradeColor(TextMeshProUGUI text, int time, int personalBest, int overallBest)
	{
		if (time <= overallBest)
		{
			if(time <= personalBest && time == personalBest)
				text.color = Manager.instance.colorPalette.NormalTime;
			else
				text.color = Manager.instance.colorPalette.PersonalBestTime;
		}
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


	public void OnFastestLap(int time, bool personal)
	{
		if (_lapTime == Int32.MaxValue || personal)
			return;
		
		DownGradeColor(fastestLapText, time, lapManager.GetPersonalFastestLap(),
			lapManager.GetOverallFastestLap());
	}

	public void OnFastestSector(int time, int sector, bool personal)
	{
		if (_lapTime == Int32.MaxValue || personal)
			return;

		if (sector == 0)
		{
			DownGradeColor(sector1Text, time, _s1Time, 
				lapManager.GetOverallFastestSector1());
		}
		else if (sector == 1)
		{
			DownGradeColor(sector2Text, time, _s2Time,
				lapManager.GetOverallFastestSector2());
		}
		else if (sector == 2)
		{
			DownGradeColor(sector3Text, time, _s3Time,
				lapManager.GetOverallFastestSector3());
		}
	}
}
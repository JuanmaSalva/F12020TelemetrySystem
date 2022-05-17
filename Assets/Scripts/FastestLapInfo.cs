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

		ushort s1Time = F1TS_bestLapSector1TimeInMS(_currentPlayerCarId);
		sector1Text.text = "Sector 1: " + FromTimeToStringFormat(s1Time);


		ushort s2Time = F1TS_bestLapSector2TimeInMS(_currentPlayerCarId);
		sector2Text.text = "Sector 2: " + FromTimeToStringFormat(s2Time);


		int s3Time = F1TS_bestLapSector3TimeInMS(_currentPlayerCarId);
		sector3Text.text = "Sector 3: " + FromTimeToStringFormat(s3Time);
		
		//Change text colors
		ChangeTextColor(fastestLapText, _lapTime, lapManager.GetOverallFastestLap(),
			lapManager.GetPersonalFastestLap());
		ChangeTextColor(sector1Text, s1Time, lapManager.GetOverallFastestSector1(),
			lapManager.GetPersonalFastestSector1());
		ChangeTextColor(sector2Text, s2Time, lapManager.GetOverallFastestSector2(),
			lapManager.GetPersonalFastestSector2());
		ChangeTextColor(sector3Text, s3Time, lapManager.GetOverallFastestSector3(),
			lapManager.GetPersonalFastestSector3());
		
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
		if (!personal)
			return;
		
		ChangeTextColor(fastestLapText, _lapTime, lapManager.GetOverallFastestLap(),
			lapManager.GetPersonalFastestLap());
	}

	public void OnFastestSector(int time, int sector, bool personal)
	{
		if (!personal || _lapTime == Int32.MaxValue)
			return;

		if (sector == 0)
		{
			ChangeTextColor(sector1Text, time, lapManager.GetPersonalFastestSector1(), 
				lapManager.GetOverallFastestSector1());
		}
		else if (sector == 1)
		{
			ChangeTextColor(sector2Text, time, lapManager.GetPersonalFastestSector2(),
				lapManager.GetOverallFastestSector2());
		}
		else if (sector == 2)
		{
			ChangeTextColor(sector3Text, time, lapManager.GetPersonalFastestSector3(),
				lapManager.GetOverallFastestSector3());
		}
	}
}
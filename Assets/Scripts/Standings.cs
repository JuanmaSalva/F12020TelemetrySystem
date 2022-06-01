using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using TMPro;

public class Standings : TelemetryListener
{
	
	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_carPosition(byte carId);
	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_tyresAgeLaps(byte carId);
	[DllImport("F12020Telemetry")]
	private static extern ushort F1TS_visualTyreCompound(byte carId);
	[DllImport("F12020Telemetry")]
	private static extern void F1TS_name(byte carId, byte[] n);


	public TextMeshProUGUI position;
	public GameObject driverStandingPrefab;
	public Transform standingParent;
	public List<TextMeshProUGUI> textsToChangeColor;

	private byte _currentPlayerCarId = 0;
	private byte _numActiveCars = 0;

	private List<KeyValuePair<byte, DriverStanding>> drivers;

	private void Awake()
	{
		drivers = new List<KeyValuePair<byte, DriverStanding>>();
	}

	void Start()
	{
		Manager.instance.AddGameObjectDependantFromF1TS(this.gameObject);
		EventManager.instance.AddListener(this);

		position.color = Manager.instance.colorPalette.PanelTitle;
		foreach (TextMeshProUGUI text in textsToChangeColor)
		{
			text.color = Manager.instance.colorPalette.PanelInfo;
		}
		
	}

	void Update()
	{
		position.text = "Pos: " + F1TS_carPosition(_currentPlayerCarId) + "/" + _numActiveCars;

		UpdateStandings();
	}


	private void UpdateStandings()
	{
		foreach (KeyValuePair<byte,DriverStanding> driver in drivers)
		{
			ushort pos = F1TS_carPosition(driver.Key);
			driver.Value.currentPosition = pos;
			driver.Value.position.text = pos.ToString();
			
			driver.Value.compoundAge.text = F1TS_tyresAgeLaps(driver.Key).ToString();
		}
		
		drivers = drivers.OrderBy(x => x.Value.currentPosition).ToList();

		for(int i = 0; i < drivers.Count; i++)
		{
			drivers[i].Value.transform.SetSiblingIndex(i);
			drivers[i].Value.ChangeCompoundIcon(F1TS_visualTyreCompound(drivers[i].Key));
		}

	}

	

	public override void OnPlayerCarIdChanged(byte playerCarId)
	{
		_currentPlayerCarId = playerCarId;
	}


	public override void OnNumActiveCarsChange(byte numActiveCars)
	{
		foreach (KeyValuePair<byte,DriverStanding> aux in drivers)
			Destroy(aux.Value.gameObject);
		drivers.Clear();
		
		_numActiveCars = numActiveCars;
		
		
		for (byte i = 0; i < _numActiveCars; i++)
		{
			DriverStanding aux = Instantiate(driverStandingPrefab, standingParent).GetComponent<DriverStanding>();
			aux.currentPosition = F1TS_carPosition(i);
			
			drivers.Add(new KeyValuePair<byte, DriverStanding>(i, aux));
			
			byte[] s = new byte[48];
			F1TS_name(i, s);
			aux.driverName.text = Encoding.Default.GetString(s);
		}
	}
}
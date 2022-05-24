using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DriverStanding : MonoBehaviour
{
	public TextMeshProUGUI position;
	public TextMeshProUGUI driverName;
	public TextMeshProUGUI delta;
	public Image compoundSprite;
	
	public int currentPosition;

	[TabGroup("Compounds sprites")] public Sprite soft;
	[TabGroup("Compounds sprites")] public Sprite medium;
	[TabGroup("Compounds sprites")] public Sprite hard;
	[TabGroup("Compounds sprites")] public Sprite inter;
	[TabGroup("Compounds sprites")] public Sprite wets;

	/// <summary>
	/// Sets initial info for the driver
	/// </summary>
	/// <param name="p">position</param>
	/// <param name="n">name</param>
	/// <param name="d">delta</param>
	public void SetInfo(string p, string n, string d)
	{
		position.text = p;
		driverName.text = n;
		delta.text = d;
	}

	public void ChangeCompoundIcon(ushort compound)
	{
		switch (compound)
		{
			case 16:
				compoundSprite.sprite = soft;
				break;
			case 17:
				compoundSprite.sprite = medium;
				break;
			case 18:
				compoundSprite.sprite = hard;
				break;
			case 7:
				compoundSprite.sprite = inter;
				break;
			case 8:
				compoundSprite.sprite = wets;
				break;
		}
	}
}
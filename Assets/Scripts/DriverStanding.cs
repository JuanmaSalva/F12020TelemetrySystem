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
	public TextMeshProUGUI compoundAge;
	public Image compoundSprite;
	
	
	public int currentPosition;

	[TabGroup("Compounds sprites")] public Sprite soft;
	[TabGroup("Compounds sprites")] public Sprite medium;
	[TabGroup("Compounds sprites")] public Sprite hard;
	[TabGroup("Compounds sprites")] public Sprite inter;
	[TabGroup("Compounds sprites")] public Sprite wets;



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
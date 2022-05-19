using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DriverStanding : MonoBehaviour
{
    public TextMeshProUGUI position;
    public TextMeshProUGUI driverName;
    public TextMeshProUGUI delta;

    public int currentPosition;
    
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
}

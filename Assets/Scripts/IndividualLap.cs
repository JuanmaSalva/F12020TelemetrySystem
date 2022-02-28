using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class IndividualLap : MonoBehaviour
{
    public TextMeshProUGUI lapTextTitle;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI sector1Text;
    public TextMeshProUGUI sector2Text;
    public TextMeshProUGUI sector3Text;

    public int lapTime, s1Time, s2Time, s3Time;
    
    void Start()
    {
        lapTextTitle.color = Manager.instance.colorPalette.PanelTitle;
        lapText.color = Manager.instance.colorPalette.NormalTime;
        sector1Text.color = Manager.instance.colorPalette.NormalTime;
        sector2Text.color = Manager.instance.colorPalette.NormalTime;
        sector3Text.color = Manager.instance.colorPalette.NormalTime;
    }

    public void SetTime(int l, int s1, int s2, int s3, int lapNum)
    {
        lapTime = l;
        s1Time = s1;
        s2Time = s2;
        s3Time = s3;

        lapTextTitle.text = "Lap " + lapNum.ToString();
        lapText.text = "Lap: " + FromTimeToStringFormat(l);
        sector1Text.text =  "Sector 1: " + FromTimeToStringFormat(s1);
        sector2Text.text =  "Sector 2: " + FromTimeToStringFormat(s2);
        sector3Text.text =  "Sector 3: " + FromTimeToStringFormat(s3);
    }


    public void SetTextColors(Color lapColor, Color s1Color, Color s2Color, Color s3Color)
    {
        lapText.color = lapColor;
        sector1Text.color = s1Color;
        sector2Text.color = s2Color;
        sector3Text.color = s3Color;
    }

    public void SetLapColor(Color color)
    {
        lapText.color = color;
    }

    public void SetSectorColor(int sector, Color color)
    {
        switch (sector)
        {
            case 1:
                sector1Text.color = color;
                break;
            case 2:
                sector2Text.color = color;
                break;
            case 3:
                sector3Text.color = color;
                break;
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
}

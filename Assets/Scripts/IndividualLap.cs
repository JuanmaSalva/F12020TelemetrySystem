using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using TMPro;

public class IndividualLap : MonoBehaviour, ILapListener
{
    public TextMeshProUGUI lapTextTitle;
    public TextMeshProUGUI lapText;
    public TextMeshProUGUI sector1Text;
    public TextMeshProUGUI sector2Text;
    public TextMeshProUGUI sector3Text;

    private int _lapTime, _s1Time, _s2Time, _s3Time;

    private LapManager _lapManager;
    
    void Awake()
    {
        lapTextTitle.color = Manager.instance.colorPalette.PanelTitle;
        lapText.color = Manager.instance.colorPalette.NormalTime;
        sector1Text.color = Manager.instance.colorPalette.NormalTime;
        sector2Text.color = Manager.instance.colorPalette.NormalTime;
        sector3Text.color = Manager.instance.colorPalette.NormalTime;
    }

    public void SetTime(int l, int s1, int s2, int s3, int lapNum)
    {
        _lapTime = l;
        _s1Time = s1;
        _s2Time = s2;
        _s3Time = s3;

        lapTextTitle.text = "Lap " + lapNum.ToString();
        lapText.text = "Lap: " + FromTimeToStringFormat(l);
        sector1Text.text =  "Sector 1: " + FromTimeToStringFormat(s1);
        sector2Text.text =  "Sector 2: " + FromTimeToStringFormat(s2);
        sector3Text.text =  "Sector 3: " + FromTimeToStringFormat(s3);
        
        //Change text colors
        ChangeTextColor(lapText, _lapTime, _lapManager.GetPersonalFastestLap(),
            _lapManager.GetOverallFastestLap());
        ChangeTextColor(sector1Text, _s1Time, _lapManager.GetPersonalFastestSector1(),
            _lapManager.GetOverallFastestSector1());
        ChangeTextColor(sector2Text, _s2Time, _lapManager.GetPersonalFastestSector2(),
            _lapManager.GetOverallFastestSector2());
        ChangeTextColor(sector3Text, _s3Time, _lapManager.GetPersonalFastestSector3(),
            _lapManager.GetOverallFastestSector3());
    }

    public void SetLapManager(LapManager lapManager)
    {
        _lapManager = lapManager;
    }
    
    private String FromTimeToStringFormat(int time)
    {            
        int m = time / 60000;
        int secMill = (time - m * 60000);
        int s = secMill / 1000;
        int ms = secMill % 1000;
        return m.ToString("00") + "," + s.ToString("00") + "." + ms.ToString("000");
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
    
    private void DownGradeColor(TextMeshProUGUI text, int lastTime, int newTime, int personalBest, int overallBest)
    {
        if (newTime <= lastTime - 2) //2 of margin for approximation errors
        {
            if (newTime == personalBest)
                text.color = Manager.instance.colorPalette.NormalTime;
            else// if(newTime <= overallBest)
                text.color = Manager.instance.colorPalette.PersonalBestTime;
        }
    }
    

    public void OnFastestLap(int time, bool personal)
    {
        if (time == _lapTime)
            return;
        
        DownGradeColor(lapText, _lapTime, time, _lapManager.GetPersonalFastestLap(),
            _lapManager.GetOverallFastestLap());
    }

    public void OnFastestSector(int time, int sector, bool personal)
    {
        if (sector == 0)
        {
            DownGradeColor(sector1Text, _s1Time,time, _lapManager.GetPersonalFastestSector1(),
                _lapManager.GetOverallFastestSector1());
        }
        else if (sector == 1)
        {
            DownGradeColor(sector2Text, _s2Time, time, _lapManager.GetPersonalFastestSector2(),
                _lapManager.GetOverallFastestSector2());
        }
        else if (sector == 2)
        {
            // if (time == _s3Time)
            //     return;
            
            DownGradeColor(sector3Text, _s3Time, time, _lapManager.GetPersonalFastestSector3(),
                _lapManager.GetOverallFastestSector3());
        }
    }
}

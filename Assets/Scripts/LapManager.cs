using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapManager : MonoBehaviour
{
    public GameObject IndividualLapPrefab;
    public Transform IndividualLapParent;
    
    private IndividualLap _fastestPersonalLap;
    private IndividualLap _fastestOverallLap;
    
    private IndividualLap _fastestPersonalS1;
    private IndividualLap _fastestPersonalS2;
    private IndividualLap _fastestPersonalS3;
    
    private IndividualLap _fastestOverallS1;
    private IndividualLap _fastestOverallS2;
    private IndividualLap _fastestOverallS3;

    
    private int _fastestPersonalLapTime = Int32.MaxValue;
    private int _fastestOverallLapTime = Int32.MaxValue;
    
    private int _fastestPersonalS1Time = Int32.MaxValue;
    private int _fastestPersonalS2Time = Int32.MaxValue;
    private int _fastestPersonalS3Time = Int32.MaxValue;
    
    private int _fastestOverallS1Time = Int32.MaxValue;
    private int _fastestOverallS2Time = Int32.MaxValue;
    private int _fastestOverallS3Time = Int32.MaxValue;


    private void Start()
    {
        
    }

    public void NewLap(int time, int s1Time, int s2Time, int s3Time, int lapNum)
    {
        if (time == 0)
            return;
        IndividualLap individualLap = Instantiate(IndividualLapPrefab, IndividualLapParent).GetComponent<IndividualLap>();
        individualLap.SetTime(time, s1Time, s2Time, s3Time, lapNum);
        
        
    }
    
    public void FastestPersonalSector(int sector, int time)
    {
        
    }

    public void FastestOverallSector(int sector, int time)
    {
        
    }

}

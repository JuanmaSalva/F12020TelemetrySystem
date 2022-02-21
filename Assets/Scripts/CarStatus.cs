using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarStatus : MonoBehaviour
{
    public Image cockPit;
    public Image frontWingLeft;
    public Image frontWingRight;
    public Image DRS;
    public Image engine;
    public Image gearBox;
    public Image[] cojinetes;
    public Image[] tyres;
    
    void Start()
    {
        cockPit.color = Manager.instance.colorPalette.PanelInfo;
        frontWingLeft.color = Manager.instance.colorPalette.GreenStatus;
        frontWingRight.color = Manager.instance.colorPalette.GreenStatus;
        DRS.color = Manager.instance.colorPalette.GreenStatus;
        engine.color = Manager.instance.colorPalette.GreenStatus;
        gearBox.color = Manager.instance.colorPalette.GreenStatus;
        foreach (Image i in cojinetes)
            i.color = Manager.instance.colorPalette.GreenStatus;
        foreach (Image i in tyres)
            i.color = Manager.instance.colorPalette.GreenStatus;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tyre : MonoBehaviour
{
    public TextMeshProUGUI surfaceTemp;
    public TextMeshProUGUI innerTemp;
    public TextMeshProUGUI brakeTemp;

    public void ChangeColor()
    {
        surfaceTemp.color = Manager.instance.colorPalette.PanelInfo;
        innerTemp.color = Manager.instance.colorPalette.PanelInfo;
        brakeTemp.color = Manager.instance.colorPalette.PanelInfo;
    }


    public string GetCompundString(ushort compoundIndex)
    {
        switch (compoundIndex)
        {
            case 16:
               return "Compund: C5";
            case 17:
                return "Compund: C4";
            case 18:
                return "Compund: C3";
            case 19:
                return "Compund: C2";
            case 20:
                return "Compund: C1";
            case 7:
                return "Compund: Inter";
            case 8:
                return "Compund: Wet";
            case 9:
                return "Compund: Dry";
            case 10:
                return "Compund: Wet";
            case 11:
                return "Compund: Super soft";
            case 12:
                return "Compund: Soft";
            case 13:
                return "Compund: Medium";
            case 14:
                return "Compund: Hard";
            case 15:
                return "Compund: Wet";

        }

        return "Compund: null";
    }

    
}
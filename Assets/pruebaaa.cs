using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

public class pruebaaa : MonoBehaviour
{

    [DllImport("F1TelemetriaDll")]
    private static extern void F1TS_sessionEndedCallBack(Action f);
    

    
    // Start is called before the first frame update
    void Start()
    {
        //pruebaCallback(prueba);
    }

    void prueba()
    {
        print("Método en c# llamado desde c++");
        
    }


}

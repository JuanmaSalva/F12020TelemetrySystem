using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.Threading;
using UnityEditor;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    public static Manager instance = null;

    [DllImport("F12020Telemetry")]
    private static extern void F1TS_startF1TelemetryThread();
    [DllImport("F12020Telemetry")]
    private static extern void F1TS_closeF1Telemetry();
    [DllImport("F12020Telemetry")]
    private static extern bool F1TS_isReady();
    [DllImport("F12020Telemetry")]
    private static extern bool F1TS_isClosed();

    
    [DllImport("F12020Telemetry")]
    private static extern void F1TS_sessionEndedCallBack(Action f);


    public Canvas canvas;
    public ColorPalette colorPalette;

    private List<GameObject> _objectsDependantFromF1Ts;


    private static bool _returnToMainMenu = false;
    
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        F1TS_startF1TelemetryThread();

        while (!F1TS_isReady())
        {
            print("Waiting for socket to open");
        } //esperamos hasta que esté listo

        print("Hemos inicializado el socket");

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += HandleOnPlayModeChanged;
#endif
        _objectsDependantFromF1Ts = new List<GameObject>();
        Application.targetFrameRate = 30;

        
        F1TS_sessionEndedCallBack(SessionEndedCallBack);
        
    }

   


#if UNITY_EDITOR
    void HandleOnPlayModeChanged(PlayModeStateChange state)
    {
        if(state == PlayModeStateChange.ExitingPlayMode)
            CloseTelemetrySystem();
    }
#endif

    void Update()
    {
        if (_returnToMainMenu || Input.GetKeyDown(KeyCode.Escape))
        {
            CloseTelemetrySystem();
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void CloseTelemetrySystem()
    {
        foreach (GameObject obj in _objectsDependantFromF1Ts)
        {
            obj.SetActive(false);
        }
        
        print("Vamos a cerrar el socket");
        try
        {
            F1TS_closeF1Telemetry();
            while (!F1TS_isClosed())
            {
                print("Esperando a que se cierre el socket bien");
            }
            print("Hemos cerraro el socket y la dll");
        }
        catch
        {
            print("Petó al cerrar");
        }
    }

    public void AddGameObjectDependantFromF1TS(GameObject obj)
    {
        _objectsDependantFromF1Ts.Add(obj);
    }



    public static void SessionEndedCallBack()
    {
        _returnToMainMenu = true;
    } 
    
    
    
    
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.Threading;
using UnityEditor;

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



    public Canvas canvas;
    public ColorPalette colorPalette;

    private List<GameObject> objectsDependantFromF1TS;
    public sbyte currentTrack {get; set;}

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
        objectsDependantFromF1TS = new List<GameObject>();
        Application.targetFrameRate = 30;
        currentTrack = -1;
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
        //todo condicion de parado
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if !UNITY_EDITOR
        CloseTelemetrySystem();
        Application.Quit();
#endif
        }
    }

    private void CloseTelemetrySystem()
    {
        foreach (GameObject obj in objectsDependantFromF1TS)
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
        objectsDependantFromF1TS.Add(obj);
    }
}

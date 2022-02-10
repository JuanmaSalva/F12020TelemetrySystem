//using System.ComponentModel.Design;
//using System.Runtime.InteropServices;
//using Graph;
//using Graph.Structs;
//using UnityEngine;

//public class MainPanelGraph : MonoBehaviour
//{
//    [DllImport("F12020Telemetry")]
//    private static extern byte F1Ts_playerCarIndex();
//    [DllImport("F12020Telemetry")]
//    private static extern float F1TS_lapDistance(byte carId);
//    [DllImport("F12020Telemetry")]
//    private static extern byte F1TS_currentLapNum(byte carId);
//    [DllImport("F12020Telemetry")]
//    private static extern short F1TS_trackLength();
//    [DllImport("F12020Telemetry")]
//    private static extern short F1TS_speed(byte carId);

        
    
//    private GraphRenderer _graphRenderer;
//    private byte _playerCarId;
//    private short _trackLength;
//    private byte _currentLap;
    
//    //Speed variables
//    private float _maxDistance;

//    void Start()
//    {
//        _trackLength = 5000;
//        _graphRenderer = GetComponent<GraphRenderer>();
//        _playerCarId = F1Ts_playerCarIndex();
//        _currentLap = byte.MaxValue;

//        InitSpeedGraph();
        
//        Manager.instance.AddGameObjectDependantFromF1TS((this.gameObject));
//    }
    

//    void InitSpeedGraph()
//    {
//        //GraphInfo speedGraphInfo = new GraphInfo(_trackLength, 370, 0.001f, Color.green, 
//        //    0.70f, 0.95f, 0.020f, 0.95f);
        
//        //speedGraphInfo.XAxis = false;
//        //speedGraphInfo.CenteredXAxis = false;

//        //speedGraphInfo.XAxisEdgeTextMargin = 0.005f;
//        //speedGraphInfo.YAxisEdgeTextMargin = 0.005f;
            
//        //speedGraphInfo.AddXTitle("Meters", 0.025f,0.015f, Color.white);
//        //speedGraphInfo.AddYTitle("Speed", 0.015f,0.015f, Color.white);
            
//        //speedGraphInfo.AddXAxisTexts(new []{"0", (_trackLength/4).ToString(),(_trackLength/3).ToString(),
//        //    (_trackLength/2).ToString(), (_trackLength).ToString()}, 0.01f, 0.01f, Color.white);
//        //speedGraphInfo.AddYAxisTexts(new []{"0", "100", "200", "300", "400"}, 0.01f, 0.005f, Color.white);

//        //speedGraphInfo.Expand = false;

//        //_graphRenderer.graphInfo = speedGraphInfo;
//        //_graphRenderer.DrawGraph();
//    }

//    void Update()
//    {
//        AddNewPointDebug();
//        //CheckNewLap();
//        //CheckTrackLength(); //TODO mirar solo cuando se empieza una sesion
//        //UpdateSpeedGraph();
//    }

//    void CheckNewLap()
//    {
//        byte lap = F1TS_currentLapNum(_playerCarId);
//        if (lap != _currentLap)
//        {
//            _currentLap = lap;
//            _graphRenderer.ClearGraph();
//            _graphRenderer.DrawGraph();
//            _maxDistance = 0;
//        }
//    }
    
//    void CheckTrackLength()
//    {
//        if (_trackLength != F1TS_trackLength())
//        {
//            _trackLength = F1TS_trackLength();
//            _graphRenderer.graphInfo.MaxXValue = _trackLength;
//        }
//    }

//    void UpdateSpeedGraph()
//    {
//        float currentLapDistance = F1TS_lapDistance(_playerCarId);
        
//        if (currentLapDistance > 0)
//        {
//            if (_maxDistance < currentLapDistance)
//            {
//                _maxDistance = currentLapDistance;
//                Vector2 aux = new Vector2(currentLapDistance, (float)F1TS_speed(_playerCarId));
//                _graphRenderer.AddPoint(aux);
//            }

//        }
//    }

//    private bool pressed = false;
    
//    public void AddNewPointDebug()
//    {
//          _graphRenderer.AddPoint(new Vector2(Random.Range(0,5000), Random.Range(0,350)));
//    }

//    public void ClearGraphDebug()
//    {
//        _graphRenderer.ClearGraph();
//    }
    
    
//    //TODO revisar si se ha cambiado de id del  jugador (se debería hacer fuera, al igual que el circuito, pero en cada update)
//    //TODO poder guardar vueltas
//}

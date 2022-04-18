using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preferences : MonoBehaviour
{

    public static Preferences _instance;

    private void OnEnable()
    {
        if (_instance == null)
        {
            lock (typeof(Preferences))
            {
                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }

            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private int _NrPlayers;
    [SerializeField] private int _NrRounds;
    [SerializeField] private MoveBorderToCameraEdge.BorderTypeEnum _BorderType;
    [SerializeField] private int _StartMoney = 10000;

    public delegate void PreferencesChangedDelegate(Preferences p);
    public PreferencesChangedDelegate OnPreferencesChanged;

    public int NrPlayers { get => _NrPlayers; set { _NrPlayers = value; OnPreferencesChanged?.Invoke(this); } }
    public int NrRounds { get => _NrRounds; set{ _NrRounds = value; OnPreferencesChanged?.Invoke(this); } }
    public MoveBorderToCameraEdge.BorderTypeEnum BorderType { get => _BorderType; set { _BorderType = value; OnPreferencesChanged?.Invoke(this); } }
    public int StartMoney { get => _StartMoney; set => _StartMoney = value; }
}

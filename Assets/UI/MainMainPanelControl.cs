using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMainPanelControl : MonoBehaviour
{

    [SerializeField] Button StartButton;

    [SerializeField] Button UpNrPlayers;
    [SerializeField] Button DownNrPlayers;
    [SerializeField] Text NrPlayersText;
    
    [SerializeField] Button UpNrRoundsButton;
    [SerializeField] Button DownNrRounds;
    [SerializeField] Text NrRoundsText;

    [SerializeField] Button PlayOptionsButton;
    [SerializeField] Button ExitButton;

    private void OnEnable()
    {
        Preferences._instance.OnPreferencesChanged += OnPreferencesChanged;
        OnPreferencesChanged(Preferences._instance);
    }

    private void OnDisable()
    {
        Preferences._instance.OnPreferencesChanged -= OnPreferencesChanged;
    }

    public void StartButton_Clicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DefinePlayersMenu");
    }

    public void UpNrPlayersButton_Clicked()
    {
        if (Preferences._instance.NrPlayers < 10) Preferences._instance.NrPlayers += 1;
        else Preferences._instance.NrPlayers = 1;
    }

    public void DownNrPlayersButton_Clicked()
    {
        if (Preferences._instance.NrPlayers > 1) Preferences._instance.NrPlayers -= 1;
        else Preferences._instance.NrPlayers = 10;
    }

    public void UpNrRoundsButton_Clicked()
    {
        if (Preferences._instance.NrRounds < 100) Preferences._instance.NrRounds += 1;
        else Preferences._instance.NrRounds = 1;
    }

    public void DownNrRoundsButton_Clicked()
    {
        if (Preferences._instance.NrRounds > 1) Preferences._instance.NrRounds -= 1;
        else Preferences._instance.NrRounds = 100;
    }

    public void ExitButton_Clicked()
    {
        Application.Quit();
    }

    private void OnPreferencesChanged(Preferences p)
    {
        NrPlayersText.text = "<color=\"cyan\">P</color>layers: " + p.NrPlayers.ToString();
        NrRoundsText.text = "<color=\"cyan\">R</color>ounds: " + p.NrRounds.ToString();
    }
}

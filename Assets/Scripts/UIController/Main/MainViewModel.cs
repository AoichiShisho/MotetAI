using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class MainViewModel : CanvasManager<MainUIStatus>
{
    [SerializeField] private PhotonManager _photonManager;
    [SerializeField] private TextMeshProUGUI scenarioText;
    [SerializeField] private TMP_InputField scenarioEditInputField;
    [SerializeField] private TextMeshProUGUI scenarioTextAmount;

    private int currentScenarioIndex = 0;
    private List<string> _scenarioList;
    
    protected override void Start()
    {
        base.Start();
        InitScenarioText();
        InitUI();
        
        scenarioEditInputField.AddTextAmountUpdater(scenarioTextAmount);
    }

    private void InitScenarioText()
    {
        _scenarioList = ScenarioLoader.Load();
        if (_scenarioList.Count <= 0) return;
        
        scenarioText.text = _scenarioList[0];
    }
    
    private void InitUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SetUIState(MainUIStatus.SelectScenario);
        }
        else
        {
            SetUIState(MainUIStatus.WaitEditingScenario);
        }
    }

    public void ChangeScenario()
    {
        if (_scenarioList.Count <= 0) return;
        
        currentScenarioIndex = (currentScenarioIndex + 1) % _scenarioList.Count;
        scenarioText.text = _scenarioList[currentScenarioIndex];
    }

    public void EditScenario()
    {
        SetUIState(MainUIStatus.EditScenario);
        scenarioEditInputField.text = scenarioText.text;
    }

    public void ConfirmScenario()
    {
        _scenarioList.Add(scenarioEditInputField.text);
        scenarioText.text = scenarioEditInputField.text;
        //他のPlayerにシナリオ入力完了を伝えるメソッド。
        
        SetUIState(MainUIStatus.EditAction);
    }
}
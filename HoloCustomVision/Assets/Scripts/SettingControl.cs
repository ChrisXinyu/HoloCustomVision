using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingControl : MonoBehaviour {

    public GameObject projectIdText;
    public GameObject predictionKeyText;
    public GameObject bingSearchKeyText;
    public GameObject modeToggle;
    public GameObject simpleModeToggle;
    public GameObject edittingModeToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 显示配置信息
    /// </summary>
    private void OnEnable()
    {
        string projectId = ConfigurationManager.Instance.GetProjectId();
        string predictionKey = ConfigurationManager.Instance.GetPredictionKey();
        string bingSearchKey = ConfigurationManager.Instance.GetBingSearchKey();
        CurrentMode mode = ConfigurationManager.Instance.GetMode();

        projectIdText.GetComponent<InputField>().text = projectId;
        predictionKeyText.GetComponent<InputField>().text = predictionKey;
        bingSearchKeyText.GetComponent<InputField>().text = bingSearchKey;

        modeToggle.GetComponent<ToggleGroup>().allowSwitchOff = true;
        modeToggle.GetComponent<ToggleGroup>().SetAllTogglesOff();
        if (mode == CurrentMode.SimpleMode)
        {
            simpleModeToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            edittingModeToggle.GetComponent<Toggle>().isOn = true;
        }
        modeToggle.GetComponent<ToggleGroup>().allowSwitchOff = false;
    }
}

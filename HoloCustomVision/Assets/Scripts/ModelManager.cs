using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity;

public class ModelManager : Singleton<ModelManager> {

    public Text tipText = null;
    public GameObject imageObject = null;
    public GameObject cropBox = null;
    public GameObject tool = null;
    public GameObject setting = null;

    public GameObject projectIdText;
    public GameObject predictionKeyText;
    public GameObject bingSearchKeyText;
    public GameObject modeToggle;

    public GameObject resultCollection;
    public GameObject resultCanvas;
    public GameObject waitingCanvas;
    public GameObject waitingSearch;

    public GameObject errorCanvas;
    public GameObject prepareCanvas;

    private Animator animator;

    // Use this for initialization
    void Start () {
        animator = imageObject.transform.parent.gameObject.GetComponent<Animator>();
        imageObject.SetActive(false);
        cropBox.SetActive(false);
        tool.SetActive(false);
        setting.SetActive(false);
        resultCanvas.SetActive(false);
        errorCanvas.SetActive(false);
        waitingCanvas.SetActive(false);
        waitingSearch.SetActive(false);
        prepareCanvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.C))
        {
            setting.SetActive(true);
        }
	}

    /// <summary>
    /// 显示Tip 信息
    /// </summary>
    /// <param name="text">Tip 信息</param>
    public void SetTipText(string text)
    {
        if (tipText != null)
        {
            tipText.text = text;
        }
    }

    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="sprite"></param>
    public void SetPhotoImage(Sprite sprite)
    {
        if (imageObject != null)
        {
            imageObject.SetActive(true);
            imageObject.GetComponent<Image>().sprite = sprite;
        }
    }

    /// <summary>
    /// 设置Image active状态
    /// </summary>
    /// <param name="activeFlag"></param>
    public void SetPhotoImageActive(bool activeFlag)
    {
        if (imageObject != null)
        {
            imageObject.SetActive(activeFlag);
        }
    }

    /// <summary>
    /// 设置裁剪框active状态
    /// </summary>
    /// <param name="activeFlag"></param>
    public void SetCropBoxActive(bool activeFlag)
    {
        if (cropBox != null)
        {
            cropBox.SetActive(activeFlag);
        }
    }

    /// <summary>
    /// 获取裁剪框active状态
    /// </summary>
    /// <returns></returns>
    public bool IsCropBoxActive()
    {
        if (cropBox != null)
        {
            return cropBox.activeSelf;
        }
        return false;
    }

    /// <summary>
    /// 重置裁剪框的大小和位置
    /// </summary>
    public void ResetCropBoxTransform()
    {
        if (cropBox != null)
        {
            cropBox.GetComponent<CropBoxManager>().ResetCropBoxTransform();
        }
    }

    /// <summary>
    /// 显示配置信息
    /// </summary>
    public void ShowSetting()
    {
        if (setting != null && CapturePhotoManager.Instance.GetCurrentStatus() == CurrentStatus.WaitingTakingPhoto)
        {
            CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.Setting);
            setting.SetActive(true);
            setting.transform.parent.position = Camera.main.transform.position + Camera.main.transform.forward;
        }
    }

    /// <summary>
    /// 取消按钮点击事件
    /// </summary>
    public void CancelBtnClicked()
    {
        if (setting != null)
        {
            setting.SetActive(false);
            CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.WaitingTakingPhoto);
        }
    }

    /// <summary>
    /// 确定按钮点击事件，配置新的配置信息
    /// </summary>
    public void ConfirmBtnClicked()
    {
        string projectId = projectIdText.GetComponent<InputField>().text;
        string predictionKey = predictionKeyText.GetComponent<InputField>().text;
        string bingSearchKey = bingSearchKeyText.GetComponent<InputField>().text;
        CurrentMode mode = CurrentMode.SimpleMode;
        ToggleGroup toggleGroup = modeToggle.GetComponent<ToggleGroup>();
        if (toggleGroup.AnyTogglesOn())
        {
            foreach (var item in toggleGroup.ActiveToggles())
            {
                switch (item.name)
                {
                    case "SimpleMode":
                        mode = CurrentMode.SimpleMode;
                        break;
                    case "EdittingMode":
                        mode = CurrentMode.EdittingMode;
                        break;
                    default:
                        break;
                }
                break;
            }
        }

        ConfigurationManager.Instance.UpdateConfiguration(projectId, predictionKey, mode, bingSearchKey);
        if (setting != null)
        {
            setting.SetActive(false);
        }
        CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.WaitingTakingPhoto);
    }

    /// <summary>
    /// 隐藏图像识别结果和必应搜索结果
    /// </summary>
    public void HideResult()
    {
        ImageCollectionManager.Instance.HideAllImages();
        resultCanvas.SetActive(false);
        errorCanvas.SetActive(false);
    }

    /// <summary>
    /// 显示图片识别结果
    /// </summary>
    /// <param name="text">识别结果</param>
    public void SetResult(string text)
    {
        resultCanvas.SetActive(true);
        resultCanvas.transform.FindChild("Text").GetComponent<Text>().text = text;
    }

    /// <summary>
    /// 显示/隐藏 等待提示信息
    /// </summary>
    /// <param name="activeFlag">显示/隐藏 Flag</param>
    public void SetWaitingCanvas(bool activeFlag)
    {
        waitingCanvas.SetActive(activeFlag);
        if (activeFlag)
        {
            waitingCanvas.transform.position = Camera.main.transform.position + 2 * Camera.main.transform.forward;
        }
    }

    /// <summary>
    /// 显示/隐藏 等待提示信息
    /// </summary>
    /// <param name="activeFlag">显示/隐藏 Flag</param>
    public void SetWaitingSearch(bool activeFlag)
    {
        waitingSearch.SetActive(activeFlag);
    }

    /// <summary>
    /// 显示/隐藏 等待提示信息
    /// </summary>
    /// <param name="activeFlag">显示/隐藏 Flag</param>
    public void SetPrepareCanvas(bool activeFlag)
    {
        prepareCanvas.SetActive(activeFlag);
        if (activeFlag)
        {
            prepareCanvas.transform.position = Camera.main.transform.position + 2 * Camera.main.transform.forward;
        }
    }

    /// <summary>
    /// 显示错误信息
    /// </summary>
    public void ShowErrorPage()
    {
        errorCanvas.SetActive(true);
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="stateName"></param>
    public void PlayAnimation(string stateName)
    {
        animator.Play(stateName);
    }
}

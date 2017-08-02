using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalListener : MonoBehaviour,IInputClickHandler, ISourceStateHandler
{
    /// <summary>
    /// 点击事件，开始拍照
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        CapturePhotoManager.Instance.TakingPhoto();
    }

    /// <summary>
    /// Hololens检测到手的事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnSourceDetected(SourceStateEventData eventData)
    {
        if (CapturePhotoManager.Instance.GetCurrentStatus() == CurrentStatus.Ready
            || CapturePhotoManager.Instance.GetCurrentStatus() == CurrentStatus.WaitingTakingPhoto)
        {
            CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.WaitingTakingPhoto);
            ModelManager.Instance.ResetCropBoxTransform();
            ModelManager.Instance.SetPhotoImageActive(false);
            ModelManager.Instance.SetCropBoxActive(false);
            ModelManager.Instance.HideResult();
            ToolManager.Instance.HideMenu();
            ModelManager.Instance.SetTipText("点击进行拍照");
            ModelManager.Instance.PlayAnimation("IdleAnimation");
        }
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
    }

    // Use this for initialization
    void Start () {
        InputManager.Instance.AddGlobalListener(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

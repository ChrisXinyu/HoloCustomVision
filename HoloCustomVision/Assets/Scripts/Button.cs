using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;

public enum ButtonType
{
    Cancel,
    Done,
}

public class Button : MonoBehaviour,IFocusable,IInputClickHandler {
    public ButtonType type;
    public Sprite defaultSprite;    //默认Sprite
    public Sprite highlightSprite;  //高亮Sprite
    public Sprite selectedSprite;   //选中Sprite
    Image buttonImage;

    private void Awake()
    {
        buttonImage = gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 视线凝视时触发的事件
    /// </summary>
    public void OnFocusEnter()
    {
        buttonImage.sprite = highlightSprite;
    }

    /// <summary>
    /// 视线移开时触发的事件
    /// </summary>
    public void OnFocusExit()
    {
        buttonImage.sprite = defaultSprite;
    }

    /// <summary>
    /// 恢复默认材质
    /// </summary>
    public void ResetButton()
    {
        buttonImage.sprite = defaultSprite;
    }

    /// <summary>
    /// Button 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        buttonImage.sprite = selectedSprite;
        switch (type)
        {
            case ButtonType.Cancel:
                ModelManager.Instance.ResetCropBoxTransform();
                ToolManager.Instance.HideMenu();
                ModelManager.Instance.SetPhotoImageActive(false);
                ModelManager.Instance.SetCropBoxActive(false);
                ModelManager.Instance.SetTipText("点击进行拍照");
                ModelManager.Instance.PlayAnimation("IdleAnimation");
                CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.Ready);
                break;
            case ButtonType.Done:
                CapturePhotoManager.Instance.RecognizeImage();
                ToolManager.Instance.HideMenu();
                break;
            default:
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;
using UnityEngine.UI;

public enum ToolType
{
    Move,
    Resize,
}

public class Tool : MonoBehaviour, IFocusable, IInputClickHandler {

    public ToolType type;
    public Sprite defaultSprite;    //默认Sprite
    public Sprite highlightSprite;  //高亮Sprite
    public Sprite selectedSprite;   //选中Sprite
    Image toolImage;

    private bool selected = false;

    private void Awake()
    {
        toolImage = gameObject.GetComponent<Image>();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Tool被选中或取消选中事件
    /// </summary>
    private void ToolAction()
    {
        if (selected)
        {
            UnSelect();
        }
        else
        {
            Select();
        }

        if (ToolManager.Instance.selectedTool != null)
        {
            ModelManager.Instance.SetCropBoxActive(true);
        }
        else
        {
            ModelManager.Instance.SetCropBoxActive(false);
        }
    }

    /// <summary>
    /// 设置Tool为highlightSprite
    /// </summary>
    public void Highlight()
    {
        if (!selected)
        {
            toolImage.sprite = highlightSprite;
        }
    }

    /// <summary>
    /// 去除highlightSprite
    /// </summary>
    public void RemoveHighlight()
    {
        if (selected)
        {
            toolImage.sprite = selectedSprite;
        }
        else
        {
            toolImage.sprite = defaultSprite;
        }
    }

    /// <summary>
    /// 选择Tool
    /// </summary>
    public void Select()
    {
        selected = ToolManager.Instance.SelectTool(this);
        if (selected)
        {
            toolImage.sprite = selectedSprite;
        }
    }

    /// <summary>
    /// 重置当前状态
    /// </summary>
    public void ResetSelected()
    {
        ToolManager.Instance.DeselectTool(this);
        selected = false;
        toolImage.sprite = defaultSprite;
    }

    /// <summary>
    /// 取消选择
    /// </summary>
    public void UnSelect()
    {
        ToolManager.Instance.DeselectTool(this);
        selected = false;
        toolImage.sprite = highlightSprite;
    }

    /// <summary>
    /// 物体被凝视
    /// </summary>
    public void OnFocusEnter()
    {
        Highlight();
    }

    /// <summary>
    /// 凝视射线移开
    /// </summary>
    public void OnFocusExit()
    {
        RemoveHighlight();
    }

    /// <summary>
    /// 重置Sprite
    /// </summary>
    public void ResetTool()
    {
        toolImage.sprite = defaultSprite;
    }

    /// <summary>
    /// Tool被选中，设置Sprite
    /// </summary>
    public void SetSelectedImage()
    {
        toolImage.sprite = selectedSprite;
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnInputClicked(InputClickedEventData eventData)
    {
        ToolAction();
    }
}

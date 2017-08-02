using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;

public class ToolManager : Singleton<ToolManager>
{
    public Tool selectedTool = null;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 取消选择所有Tool
    /// </summary>
    public void UnselectAllTools()
    {
        selectedTool = null;

        Tool[] tools = GetComponentsInChildren<Tool>();
        foreach (Tool tool in tools)
        {
            tool.ResetSelected();
        }
    }

    /// <summary>
    /// 所有Tool取消高亮
    /// </summary>
    public void UnHighlightAllTools()
    {
        Tool[] tools = GetComponentsInChildren<Tool>();
        foreach (Tool tool in tools)
        {
            tool.ResetTool();
        }

        Button[] buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.ResetButton();
        }

        if (selectedTool != null)
        {
            selectedTool.SetSelectedImage();
        }
    }

    /// <summary>
    /// 取消选择
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    public bool DeselectTool(Tool tool)
    {
        if (selectedTool == tool)
        {
            selectedTool = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 选择Tool
    /// </summary>
    /// <param name="tool"></param>
    /// <returns></returns>
    public bool SelectTool(Tool tool)
    {
        UnselectAllTools();
        selectedTool = tool;
        return true;
    }

    /// <summary>
    /// 显示菜单
    /// </summary>
    public void ShowMenu()
    {

        gameObject.SetActive(true);
        if (selectedTool != null)
        {
            UnselectAllTools();
        }

        UnHighlightAllTools();
    }

    /// <summary>
    /// 隐藏菜单
    /// </summary>
    public void HideMenu()
    {
        gameObject.SetActive(false);
    }
}

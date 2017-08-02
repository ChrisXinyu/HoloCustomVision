using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CropBoxManager : MonoBehaviour, INavigationHandler
{
    private Vector3 manipulationPreviousPosition;

    public GameObject imageObject = null;
    private float parentWidth = 0;
    private float parentHeight = 0;

    private Vector3 originalLocalPosition;
    private Vector2 originalSizeDelta;

    public float minWidth = 250;
    public float minHeight = 250;
    void Awake()
    {
        parentWidth = imageObject.GetComponent<RectTransform>().rect.width;
        parentHeight = imageObject.GetComponent<RectTransform>().rect.height;
        originalLocalPosition = gameObject.GetComponent<RectTransform>().localPosition;
        originalSizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta;
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 获取裁剪框的矩形区域
    /// </summary>
    /// <returns></returns>
    public RectTransform GetCropBoxRectTransform()
    {
        return gameObject.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 获取父对象的大小
    /// </summary>
    /// <returns></returns>
    public Vector2 GetParentSize()
    {
        return new Vector2(parentWidth, parentHeight);
    }

    /// <summary>
    /// 重置裁剪框的大小和位置
    /// </summary>
    public void ResetCropBoxTransform()
    {
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(originalLocalPosition.x, originalLocalPosition.y, originalLocalPosition.z) ;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(originalSizeDelta.x, originalSizeDelta.y);
    }

    /// <summary>
    /// Navigation手势取消事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        InputManager.Instance.ClearModalInputStack();
    }

    /// <summary>
    /// Navigation手势完成事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        InputManager.Instance.ClearModalInputStack();
    }

    /// <summary>
    /// Navigation手势开始事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnNavigationStarted(NavigationEventData eventData)
    {
        if (ToolManager.Instance.selectedTool == null
            || ((ToolManager.Instance.selectedTool.type != ToolType.Resize)
            && (ToolManager.Instance.selectedTool.type != ToolType.Move))
            || (CapturePhotoManager.Instance.GetCurrentStatus() != CurrentStatus.EdittingPhoto))
        {
            return;
        }
        InputManager.Instance.PushModalInputHandler(gameObject);
    }

    /// <summary>
    /// Navigation手势更新事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        if (ToolManager.Instance.selectedTool == null
            || ((ToolManager.Instance.selectedTool.type != ToolType.Resize)
            && (ToolManager.Instance.selectedTool.type != ToolType.Move))
            || (CapturePhotoManager.Instance.GetCurrentStatus() != CurrentStatus.EdittingPhoto))
        {
            return;
        }

        switch (ToolManager.Instance.selectedTool.type)
        {
            case ToolType.Move:
                Move(eventData);
                break;
            case ToolType.Resize:
                Resize(eventData);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 平移裁剪框
    /// </summary>
    /// <param name="eventData"></param>
    private void Move(NavigationEventData eventData)
    {
        Vector3 moveVector = Vector3.one;
        float deltaX = 8 * eventData.CumulativeDelta.x;
        float deltaY = 8 * eventData.CumulativeDelta.y;

        if (Math.Abs(eventData.CumulativeDelta.x) >= Math.Abs(eventData.CumulativeDelta.y))
        {
            //当水平移动分量大于垂直分量时，水平平移
            moveVector = new Vector3(deltaX, 0, 0);
        }
        else
        {
            //当垂直移动分量大于水平分量时，垂直平移
            moveVector = new Vector3(0, deltaY, 0);
        }

        //计算边界，重新调整位移
        moveVector = RecalculateMoveVector(moveVector);

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        gameObject.GetComponent<RectTransform>().localPosition = new Vector3(
            rectTransform.localPosition.x + moveVector.x,
            rectTransform.localPosition.y + moveVector.y,
            rectTransform.localPosition.z + moveVector.z);
    }

    /// <summary>
    /// 调整裁剪框的大小
    /// </summary>
    /// <param name="eventData"></param>
    private void Resize(NavigationEventData eventData)
    {
        if (Math.Abs(eventData.CumulativeDelta.x) >= Math.Abs(eventData.CumulativeDelta.y))
        {
            //当水平移动分量大于垂直分量时，修改宽度
            float deltaX = 10 * eventData.CumulativeDelta.x;
            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Rect newRect = new Rect((rect.width + deltaX) / 2, rect.height / 2, rect.width + deltaX, rect.height);
            newRect = RecalculateRectWidth(newRect);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newRect.width, newRect.height);
        }
        else
        {
            //当垂直移动分量大于水平分量时，修改高度
            float deltaY = 10 * eventData.CumulativeDelta.y;
            Rect rect = gameObject.GetComponent<RectTransform>().rect;
            Rect newRect = new Rect(rect.width / 2, (rect.height + deltaY) / 2, rect.width, rect.height + deltaY);
            newRect = RecalculateRectHeight(newRect);
            gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(newRect.width, newRect.height);
        }
    }

    /// <summary>
    /// 重新计算位移
    /// </summary>
    /// <param name="moveVector"></param>
    /// <returns></returns>
    private Vector3 RecalculateMoveVector(Vector3 moveVector)
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Rect rect = rectTransform.rect;
        //裁剪框移动不超出大图范围，需要满足以下条件
        //-parentWidth / 2 + width / 2 <= x <= parentWidth / 2 - width / 2
        //-parentHeight / 2 + height / 2 <= y <= parentHeight / 2 - height / 2

        if (((rectTransform.localPosition.x + moveVector.x) >= (-parentWidth / 2 + rect.width / 2))
            && ((rectTransform.localPosition.x + moveVector.x) <= (parentWidth / 2 - rect.width / 2))
            && ((rectTransform.localPosition.y + moveVector.y) >= (-parentHeight / 2 + rect.height / 2))
            && ((rectTransform.localPosition.y + moveVector.y) <= (parentHeight / 2 - rect.height / 2)))
        {
            return moveVector;
        }

        float newX = moveVector.x;
        float newY = moveVector.y;

        //当超出左边框，重新计算水平移动分量
        if ((rectTransform.localPosition.x + moveVector.x) < (-parentWidth / 2 + rect.width / 2))
        {
            newX = -parentWidth / 2 + rect.width / 2 - rectTransform.localPosition.x;
        }
        //当超出右边框，重新计算水平移动分量
        if ((rectTransform.localPosition.x + moveVector.x) > (parentWidth / 2 - rect.width / 2))
        {
            newX = parentWidth / 2 - rect.width / 2 - rectTransform.localPosition.x;
        }
        //当超出下边框，重新计算垂直移动分量
        if ((rectTransform.localPosition.y + moveVector.y) < (-parentHeight / 2 + rect.height / 2))
        {
            newY = -parentHeight / 2 + rect.height / 2 - rectTransform.localPosition.y;
        }
        //当超出上边框，重新计算垂直移动分量
        if ((rectTransform.localPosition.y + moveVector.y) > (parentHeight / 2 - rect.height / 2))
        {
            newY = parentHeight / 2 - rect.height / 2 - rectTransform.localPosition.y;
        }

        return new Vector3(newX, newY, moveVector.z);
    }

    /// <summary>
    /// 重新计算裁剪框的宽度
    /// </summary>
    /// <param name="rect">矩形区域</param>
    /// <returns>返回矩形区域</returns>
    private Rect RecalculateRectWidth(Rect rect)
    {
        //当裁剪框高度小于最小宽度时，重新调整为最小宽度
        if (rect.width < minWidth)
        {
            return new Rect(minWidth / 2, rect.y, minWidth, rect.height);
        }

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        //在大图范围内进行移动，需满足条件 -parentWidth / 2 + width / 2 <= x <= parentWidth / 2 - width / 2
        if ((rectTransform.localPosition.x >= (-parentWidth / 2 + rect.width / 2))
            && rectTransform.localPosition.x <= (parentWidth / 2 - rect.width / 2))
        {
            return rect;
        }

        //当裁剪框超出左边框时，重新进行调整
        if (rectTransform.localPosition.x < (-parentWidth / 2 + rect.width / 2))
        {
            return new Rect(rectTransform.localPosition.x + parentWidth / 2, rect.y, 2 * rectTransform.localPosition.x + parentWidth, rect.height);
        }

        //当裁剪框超出右边框时，重新进行调整
        if (rectTransform.localPosition.x > (parentWidth / 2 - rect.width / 2))
        {
            return new Rect(parentWidth/2 - rectTransform.localPosition.x, rect.y, parentWidth- 2 * rectTransform.localPosition.x, rect.height);
        }

        return rect;
    }

    /// <summary>
    /// 重新计算裁剪框的高度
    /// </summary>
    /// <param name="rect">矩形区域</param>
    /// <returns>返回矩形区域</returns>
    private Rect RecalculateRectHeight(Rect rect)
    {
        //当裁剪框高度小于最小高度时，重新调整为最小高度
        if (rect.height < minHeight)
        {
            return new Rect(rect.x, minHeight / 2, rect.width, minHeight);
        }

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        //在大图范围内进行移动，需满足条件 -parentHeight / 2 + height / 2 <= y <= parentHeight / 2 - height / 2
        if ((rectTransform.localPosition.y >= (-parentHeight / 2 + rect.height / 2))
            && rectTransform.localPosition.y <= (parentHeight / 2 - rect.height / 2))
        {
            return rect;
        }

        //当裁剪框超出下边框时，重新进行调整
        if (rectTransform.localPosition.y < (-parentHeight / 2 + rect.height / 2))
        {
            return new Rect(rect.x, rectTransform.localPosition.y + parentHeight / 2, rect.width, 2 * rectTransform.localPosition.y + parentHeight);
        }

        //当裁剪框超出上边框时，重新进行调整
        if (rectTransform.localPosition.y > (parentHeight / 2 - rect.height / 2))
        {
            return new Rect(rect.x, parentHeight / 2 - rectTransform.localPosition.y, rect.width, parentHeight - 2 * rectTransform.localPosition.y);
        }

        return rect;
    }
}

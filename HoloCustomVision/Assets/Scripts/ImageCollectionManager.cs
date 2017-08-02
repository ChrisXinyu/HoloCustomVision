using HoloToolkit.Unity;
using HUX.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageCollectionManager : Singleton<ImageCollectionManager>
{
    //必应图片搜索URL
    private string hostUrl = "https://api.cognitive.microsoft.com/bing/v7.0/images/search?q={0}&mkt=en-us HTTP/1.1";

    // Use this for initialization
    void Start () {
        HideAllImages();
    }
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 使用Bing Search API搜索图片
    /// </summary>
    /// <param name="keyWord">搜索内容</param>
    /// <returns></returns>
    public IEnumerator SearchImages(string keyWord)
    {
        UnityWebRequest www = UnityWebRequest.Get(String.Format(hostUrl, keyWord));
        www.SetRequestHeader("Ocp-Apim-Subscription-Key", ConfigurationManager.Instance.GetBingSearchKey());
        yield return www.Send();

        if (www.isDone)
        {
            string result = www.downloadHandler.text;
            try
            {
                JSONObject jsonObj = new JSONObject(result);
                JSONObject imageArray = jsonObj.GetField("value");

                if (imageArray.type == JSONObject.Type.ARRAY)
                {
                    string[] imageUrls = new string[imageArray.list.Count];
                    int index = 0;
                    foreach (JSONObject image in imageArray.list)
                    {
                        //string imageUrl = image.GetField("contentUrl").str.Replace("\\", "");
                        string imageUrl = image.GetField("thumbnailUrl").str.Replace("\\", "");
                        imageUrls[index] = imageUrl;
                        index++;
                    }
                    CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.Ready);
                    SetImages(imageUrls);
                }
            }
            catch
            {
                ModelManager.Instance.SetWaitingSearch(false);
                ModelManager.Instance.ShowErrorPage();
                CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.Ready);
            }
        }
        else
        {
            ModelManager.Instance.SetWaitingSearch(false);
            ModelManager.Instance.ShowErrorPage();
            CapturePhotoManager.Instance.SetCurrentStatus(CurrentStatus.Ready);
        }
    }

    /// <summary>
    /// 显示图片
    /// </summary>
    /// <param name="imageUrls">搜索图片列表</param>
    public void SetImages(string[] imageUrls)
    {
        List<ObjectCollection.CollectionNode> nodeList = gameObject.GetComponent<ObjectCollection>().NodeList;
        int length = imageUrls.Length;
        int index = 0;
        foreach (ObjectCollection.CollectionNode node in nodeList)
        {
            Transform imageTransform = node.transform;
            StartCoroutine(ShowImage(imageUrls[index], imageTransform));
            index++;
            if (index >= length)
            {
                break;
            }
        }
    }

    /// <summary>
    /// 获取网络图片并显示
    /// </summary>
    /// <param name="imageUrl"></param>
    /// <param name="imageTransform"></param>
    /// <returns></returns>
    private IEnumerator ShowImage(string imageUrl, Transform imageTransform)
    {
        WWW www = new WWW(imageUrl);
        if (CapturePhotoManager.Instance.GetCurrentStatus() != CurrentStatus.Ready)
        {
            yield break;
        }
        yield return www;

        if (CapturePhotoManager.Instance.GetCurrentStatus() != CurrentStatus.Ready)
        {
            yield break;
        }
        ModelManager.Instance.SetWaitingSearch(false);

        Texture2D targetTexture = www.texture;
        Sprite sprite = Sprite.Create(targetTexture, new Rect(0, 0, targetTexture.width, targetTexture.height), new Vector2(0.5f, 0.5f));
        
        imageTransform.gameObject.SetActive(true);
        imageTransform.FindChild("Image").GetComponent<Image>().sprite = sprite;
    }

    /// <summary>
    /// 隐藏所有图片
    /// </summary>
    public void HideAllImages()
    {
        List<ObjectCollection.CollectionNode> nodeList = gameObject.GetComponent<ObjectCollection>().NodeList;
        foreach (ObjectCollection.CollectionNode node in nodeList)
        {
            node.transform.gameObject.SetActive(false);
        }
    }
}

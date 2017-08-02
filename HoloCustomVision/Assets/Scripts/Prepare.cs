using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepare : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 开始图片分析时间，倒数动画结束时调用
    /// </summary>
    public void AnalyzeImage()
    {
        CapturePhotoManager.Instance.AnalyzeImage();
    }
}

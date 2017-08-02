using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity;
using System.IO;
using System.Text;

public class ConfigurationManager : Singleton<ConfigurationManager>
{
    private static string configurationFromat = "{{\"ProjectId\":\"{0}\",\"PredictionKey\":\"{1}\",\"Mode\":{2},\"BingSearchKey\":\"{3}\"}}";
    private string configuration;
    string fileName = "configuration.json";
    string filePath = "";

    private string projectId = "84c90be7-1e04-4a3b-86c0-25046ac43308";  //custom vision 测试项目projectId
    private string predictionKey = "e124b9c7248143f988a1bfc88bd3454f";  //custom vision 测试项目predictionKey
    //当前模式
    //SimpleMode 简单模式（拍照完成直接调用接口进行分析） 
    //EdittingMode 编辑模式（拍照完成，对图片简单编辑，截取识别有效区域，然后调用接口进行图片识别）
    private CurrentMode mode = CurrentMode.SimpleMode;
    private string bingSearchKey = "d8ca053d107e4ab5a0fc6096e043149d"; //bing search key 测试key

    // Use this for initialization
     void Start () {
        configuration = string.Format(configurationFromat, projectId, predictionKey, (long)mode, bingSearchKey);
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            //配置文件存在时，读取配置信息
            configuration = ReadConfigurationFile();
        }
        else
        {
            //配置文件不存在时，创建配置文件
            CreateConfigurationFile();
        }
        ParseConfiguration();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// 创建配置文件
    /// </summary>
    public void CreateConfigurationFile()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            fs.Seek(0, SeekOrigin.Begin);
            byte[] data = new UTF8Encoding().GetBytes(configuration);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Dispose();
        }
    }

    /// <summary>
    /// 读取配置文件
    /// </summary>
    /// <returns>返回配置文件内容</returns>
    public string ReadConfigurationFile()
    {
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            fs.Seek(0, SeekOrigin.Begin);
            int fileLength = (int)fs.Length;

            if (fileLength == 0)
            {
                byte[] data = new UTF8Encoding().GetBytes(configuration);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Dispose();
                return configuration;
            }
            else
            {
                byte[] data = new byte[fileLength];
                fs.Read(data, 0, fileLength);
                string jsonStr = Encoding.UTF8.GetString(data);
                return jsonStr;
            }
        } 
    }

    /// <summary>
    /// 更新配置文件
    /// </summary>
    /// <param name="projectId">projectId</param>
    /// <param name="predictionKey">predictionKey</param>
    /// <param name="mode">使用模式</param>
    /// <param name="bingSearchKey">bingSearchKey</param>
    public void UpdateConfiguration(string projectId, string predictionKey, CurrentMode mode, string bingSearchKey)
    {
        this.projectId = projectId;
        this.predictionKey = predictionKey;
        this.bingSearchKey = bingSearchKey;
        this.mode = mode;
        configuration = string.Format(configurationFromat, this.projectId, this.predictionKey, (long)this.mode, this.bingSearchKey);
        using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
        {
            fs.Seek(0, SeekOrigin.Begin);
            int fileLength = (int)fs.Length;

            fs.SetLength(0);
            byte[] data = new UTF8Encoding().GetBytes(configuration);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Dispose();
        }  
    }

    /// <summary>
    /// 解析配置信息
    /// </summary>
    private void ParseConfiguration()
    {
        try
        {
            JSONObject jsonObject = new JSONObject(configuration);
            projectId = jsonObject.GetField("ProjectId").str;
            predictionKey = jsonObject.GetField("PredictionKey").str;
            bingSearchKey = jsonObject.GetField("BingSearchKey").str;
            long modevalue = (long)jsonObject.GetField("Mode").i;
            if (modevalue == 0)
            {
                mode = CurrentMode.SimpleMode;
            }
            else
            {
                mode = CurrentMode.EdittingMode;
            }
        }
        catch
        { }
    }

    /// <summary>
    /// 获取projectId
    /// </summary>
    /// <returns>返回projectId</returns>
    public string GetProjectId()
    {
        return projectId;
    }

    /// <summary>
    /// 获取predictionKey
    /// </summary>
    /// <returns>返回predictionKey</returns>
    public string GetPredictionKey()
    {
        return predictionKey;
    }

    /// <summary>
    /// 获取mode
    /// </summary>
    /// <returns>返回mode</returns>
    public CurrentMode GetMode()
    {
        return mode;
    }

    /// <summary>
    /// 获取bingSearchKey
    /// </summary>
    /// <returns>返回bingSearchKey</returns>
    public string GetBingSearchKey()
    {
        return bingSearchKey;
    }
}

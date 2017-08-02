# CustomVisionClient #
本工程使用Custiom Vision API构建，在PC端实现创建工程，创建Tag，新增图片，训练以及获取Iteration等功能

![](../Screenshots/CustomVisionClient/19.jpg)  

### 使用流程 ###
本客户端的使用以名画识别作为示例

#### 1、进入*[Custom Vision Portal](https://www.customvision.ai)*，使用微软账户登录授权，至少创建一个空白工程，获取Training Key   ####
![](../Screenshots/CustomVisionClient/0.jpg)  

####2、打开应用，填写Training Key，进入应用####
![](../Screenshots/CustomVisionClient/1.jpg)  
![](../Screenshots/CustomVisionClient/2.jpg)  

#### 3、点击New Project新建工程，输入Project Name,Description,选择Domain   ####
![](../Screenshots/CustomVisionClient/3.jpg)  
![](../Screenshots/CustomVisionClient/4.jpg)  

#### 4、点击新创建的工程进入Project   ####
![](../Screenshots/CustomVisionClient/5.jpg)  

#### 5、新建Tags   ####
![](../Screenshots/CustomVisionClient/6.jpg)  
![](../Screenshots/CustomVisionClient/7.jpg)  

#### 6、点击Add Images，添加图片   ####
![](../Screenshots/CustomVisionClient/8.jpg)  

#### 7、点击Browse local files添加本地图片   ####
![](../Screenshots/CustomVisionClient/9.jpg)  
![](../Screenshots/CustomVisionClient/10.jpg)  
![](../Screenshots/CustomVisionClient/11.jpg)  
![](../Screenshots/CustomVisionClient/12.jpg)  

#### 8、点击Browse online image添加网络图片（示例步骤，可选）   ####
![](../Screenshots/CustomVisionClient/13.jpg)  
![](../Screenshots/CustomVisionClient/14.jpg)  

#### 9、上传图片后如下图   ####
![](../Screenshots/CustomVisionClient/15.jpg)  

#### 10、点击Train按钮进行训练   ####
*训练中*  
![](../Screenshots/CustomVisionClient/16.jpg)  
*训练完成后*  
![](../Screenshots/CustomVisionClient/17.jpg) 

#### 11、点击设置按钮，获取对应的预测识别用的Project Id和Prediction Key  ####
![](../Screenshots/CustomVisionClient/18.jpg) 


### 参考 ###
[*CustomVision Quickstarts Overview*](https://docs.microsoft.com/zh-cn/azure/cognitive-services/custom-vision-service/getting-started-build-a-classifier)  
[*Cognitive-CustomVision-Windows*](https://github.com/Microsoft/Cognitive-CustomVision-Windows/)  
*[Custom Vision API](https://docs.microsoft.com/zh-cn/azure/cognitive-services/custom-vision-service/csharp-tutorial)*  
*[Custom Vision Portal](https://www.customvision.ai)*  

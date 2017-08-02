public enum CurrentStatus
{
    Init = 0,             //初始状态
    Ready,                //Ready状态
    WaitingTakingPhoto,   //等待拍照状态
    TakingPhoto,          //拍照状态
    EdittingPhoto,        //图片编辑状态
    AnalyzingPhoto,       //图片分析状态
    Setting,              //配置状态
    Searching,            //搜索状态
}

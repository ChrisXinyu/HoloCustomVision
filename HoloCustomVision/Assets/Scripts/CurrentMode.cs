public enum CurrentMode
{
    SimpleMode = 0,    //简单模式（拍照完成直接调用接口进行分析）
    EdittingMode = 1,  //编辑模式（拍照完成，对图片简单编辑，截取有效区域，然后调用接口进行图片识别）
}

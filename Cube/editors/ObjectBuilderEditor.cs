using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UVCube))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        UVCube myScript = (UVCube)target;
        if (GUILayout.Button("Apply Texture"))
        {
            myScript.ApplyTexture();
        }
    }
}
/*
 * 通过这个脚本，Unity将会自动将这个脚本添加到所有类型为UVCube的对象下。
 * 脚本中的DrawDefaultInpector() 函数会被自动调用，并在UVCube的菜单中创建名为ApplyTexture的按钮。
 * 当那个按钮被按下时，就会调用我们立方体上面的ApplyTextureFunction()函数。 
 * 这时我们就可以按下立方体上面的 ApplyTextrue 按钮来更新立方体的贴图，直接在编辑器中看到贴图的效果，而不用每次都去运行游戏了。 
 * 作者：皮皮关做游戏 https://www.bilibili.com/read/cv14011394/?spm_id_from=333.999.0.0 出处：bilibili
 */
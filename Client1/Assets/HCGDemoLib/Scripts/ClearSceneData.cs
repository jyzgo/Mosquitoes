using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;

public class ClearSceneData : MonoBehaviour
{
    //异步对象
    private AsyncOperation async;

    //下一个场景的名称
    public static bool isLoadByName = false;
    private static string nextSceneName;
    private static int nextSceneIndex;

    void Awake()
    {
        Object[] objAry = Resources.FindObjectsOfTypeAll<Material>();

        for (int i = 0; i < objAry.Length; ++i)
        {
            objAry[i] = null;//解除资源的引用
        }

        Object[] objAry2 = Resources.FindObjectsOfTypeAll<Texture>();

        for (int i = 0; i < objAry2.Length; ++i)
        {
            objAry2[i] = null;
        }

        //卸载没有被引用的资源
        Resources.UnloadUnusedAssets();

        //立即进行垃圾回收
        GC.Collect();
        GC.WaitForPendingFinalizers();//挂起当前线程，直到处理终结器队列的线程清空该队列为止
        GC.Collect();

    }

    void Start()
    {
        if (isLoadByName)
        {
            StartCoroutine(AsyncLoadScene(nextSceneName));
        }else
        {

            StartCoroutine(AsyncLoadSceneByIndex(nextSceneIndex));
        }
    }

    /// <summary>
    /// 静态方法，直接切换到ClearScene，此脚本是挂在ClearScene场景下的，就会实例化，执行资源回收
    /// </summary>
    /// <param name="_nextSceneName"></param>
    /// 
    public static void LoadSceneByName(string _nextSceneName)
    {
        Resources.UnloadUnusedAssets();
        isLoadByName = true;
        nextSceneName = _nextSceneName;
        SceneManager.LoadScene(_nextSceneName);
    }

    /// <summary>
    /// 异步加载下一个场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    IEnumerator AsyncLoadScene(string sceneName)
    {
        async =  SceneManager.LoadSceneAsync("ClearDataScene");
        yield return async;
    }

    void OnDestroy()
    {
        async = null;
    }

    internal static void LoadLevelByIndex(int index)
    {
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(index);
        isLoadByName = false;
        nextSceneIndex = index;
        //SceneManager.LoadScene("ClearDataScene");
    }

    IEnumerator AsyncLoadSceneByIndex(int sceneIndex)
    {
        async = SceneManager.LoadSceneAsync(sceneIndex);
        yield return async;
    }

}

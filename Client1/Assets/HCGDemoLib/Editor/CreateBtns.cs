using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;

public class CreateBtns :Editor
{
   
    const string BTNS = "Btns";

    static PlayableDirector[] _playableDirectors;

    static HashSet<string> _playAbleDirectorsNames; 

    static void updateDirector()
    {
        _playableDirectors = GameObject.FindObjectsOfType<PlayableDirector>();
        _playAbleDirectorsNames = new HashSet<string>();
        foreach (var p  in _playableDirectors)
        {
            _playAbleDirectorsNames.Add(p.name);
        }

    }

    static GameObject btns;

    static void updateBtnsRoot()
    {
        btns = GameObject.Find(BTNS);
        if(btns == null)
        {
            btns = new GameObject();
            btns.name = BTNS;
        }
    }

   

    
    [MenuItem("HCG项目/解锁所有关卡")]
    public static void OpenAllLevels()
    {
        PlayerPrefs.SetInt(InitMgr.CUR_MAX_LEVEL_KEY,InitMgr.MAX_LEVEL_INDEX);
        PlayerPrefs.SetInt(InitMgr.CUR_LEVEL_KEY, InitMgr.MAX_LEVEL_INDEX);
    }

    [MenuItem("HCG项目/增加2000钱")]
    public static void AddCoin()
    {
        int getCoin = PlayerPrefs.GetInt(InitMgr.COIN_KEY, 0);
        PlayerPrefs.SetInt(InitMgr.COIN_KEY, 2000 + getCoin);
    }


    [MenuItem("HCG项目/清零钱")]
    public static void ClearCoin()
    {
        PlayerPrefs.SetInt(InitMgr.COIN_KEY, 0);
    }

    [MenuItem("HCG项目/重置关卡1")]
    public static void SetLevelsTo1()
    {
        PlayerPrefs.SetInt(InitMgr.CUR_MAX_LEVEL_KEY, 1);
        PlayerPrefs.SetInt(InitMgr.CUR_LEVEL_KEY, 1);
    }




    [MenuItem("通用/删除所有用户数据")]
    public static void DeletePlayerPref()
    {
        if (EditorUtility.DisplayDialog("确定删除数据", "确定删除全部用户数据", "确定", "取消"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }

    public const string DEBUG_SEPEED = "DEBUG_SPEED";
    [MenuItem("通用/切换速度")]
    public static void SwitchPlayerPref()
    {
        bool playSpeed = PlayerPrefs.GetInt(DEBUG_SEPEED, 0) != 0;
        if(playSpeed)
        {
            PlayerPrefs.SetInt(DEBUG_SEPEED, 0);
            if (Application.isPlaying)
            {
                Time.timeScale = 1f;
            }
        }
        else
        {
            PlayerPrefs.SetInt(DEBUG_SEPEED, 1);
            if(Application.isPlaying)
            {
                Time.timeScale = 10f;
            }
        }
    }


}

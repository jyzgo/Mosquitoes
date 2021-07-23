using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelHorizontalCell: MonoBehaviour
{
    public Button unlockBtn;
    public Button btnCurrent;
    public GameObject btnLocked;
    public Text levelCount;

    private void Awake()
    {
        unlockBtn.onClick.AddListener(BtnPressed);
        btnCurrent.onClick.AddListener(BtnPressed);
    }

    void BtnPressed()
    {
        print("pressed " + _levelIndex);
        InitMgr.current.LoadLevel(_levelIndex);
    }

    private void OnEnable()
    {
        UpdateCell(_levelIndex);
    }
    int _levelIndex = 0;
    internal void UpdateCell(int i)
    {
        _levelIndex = i;
        
        if(_levelIndex > InitMgr.MAX_LEVEL_INDEX)
        {
            unlockBtn.gameObject.SetActive(false);
            btnCurrent.gameObject.SetActive(false);
            btnLocked.gameObject.SetActive(false);
            levelCount.gameObject.SetActive(false);

        }
        else if(_levelIndex == InitMgr.current.GetCurMaxLevelIndex())
        {
            
            unlockBtn.gameObject.SetActive(false);
            btnCurrent.gameObject.SetActive(true);
            btnLocked.gameObject.SetActive(false);
            levelCount.gameObject.SetActive(true);
        }
        else if (_levelIndex < InitMgr.current.GetCurMaxLevelIndex())
        {
            unlockBtn.gameObject.SetActive(true);
            btnCurrent.gameObject.SetActive(false);
            btnLocked.gameObject.SetActive(false);
            levelCount.gameObject.SetActive(true);
        }
        else
        {
            unlockBtn.gameObject.SetActive(false);
            btnCurrent.gameObject.SetActive(false);
            btnLocked.gameObject.SetActive(true);
            levelCount.gameObject.SetActive(false);
        }
        levelCount.text = i.ToString();
    }
    // Start is called before the first frame update
}

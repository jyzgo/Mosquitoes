using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class coinUI : MonoBehaviour
{
    Text coinText;
    private void Awake()
    {
        coinText = GetComponent<Text>();
    }
    private void Start()
    {
        InitMgr.current.onCoinChanged += updateCoin;
        updateCoin();
    }

    void updateCoin()
    {
        if (gameObject != null && coinText != null)
        {
            coinText.text = InitMgr.current.getCurrentCoinNum().ToString();
        }
    }
    private void OnDestroy()
    {
        if (InitMgr.current != null)
        {
            InitMgr.current.onCoinChanged -= updateCoin;
        }
    }
}

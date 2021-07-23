using MonsterLove.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InitState
{
    Ready,
    Playing,
    Lose,
    Win
}

public class InitMgr : MonoBehaviour
{

    public Button levelRewardWinButton;
    static InitMgr _current = null;

    public static InitMgr current
    {
        get
        {

            if (m_ShuttingDown)
            {
                return null;
            }
            if (_current == null)
            {
                var init = Resources.Load("InitMgr");
                var gb = Instantiate(init) as GameObject;
            }
            return _current;
        }
    }
    public static  bool isInit
    {
        get
        {
            return _current != null;
        }
    }

    public System.Action SkinUpdateEvent { get; internal set; }

    public const string COIN_KEY = "coin_key";
    public int getCurrentCoinNum()
    {
        return PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    public void AddCoin(int num)
    {
        print("add coid " + num);
        var c = getCurrentCoinNum();
        c += num;
        PlayerPrefs.SetInt(COIN_KEY, c);
        UpdateCoinInUI();
    }

    public void ReduceCoin(int num)
    {
        var c = getCurrentCoinNum();
        c -= num;
        PlayerPrefs.SetInt(COIN_KEY, c);
        UpdateCoinInUI();
    }

    public void UpdateCoinInUI()
    {
        onCoinChanged?.Invoke();
    }

    public void InitCall()
    {

    }

    //public Button[] resetBtns;

    StateMachine<InitState> _fsm;
    private void Awake()
    {
        if (_current != null)
        {
            Destroy(gameObject);
            return;
        }
        _current = this;
        DontDestroyOnLoad(gameObject);
        //foreach(var b in resetBtns)
        //{
        //    b.onClick.AddListener(RestartLevel);
        //}
        nextLevelBtn.onClick.AddListener(playNextLevel);
        noThanksBtn.onClick.AddListener(RestartLevel);
        rewardBackBtn.onClick.AddListener(rewardBack);
        retryBtn.onClick.AddListener(rewardBackToLast);
        tapToStart.onClick.AddListener(tapToStartCB);
        levelSelectBtn.onClick.AddListener(OnShowLevelSelect);
        backToLevelSelct.onClick.AddListener(OnHideLevelSelect);
        backToLevelSelct2.onClick.AddListener(BackToBeforePlay);
        levelRewardWinButton.onClick.AddListener(OnRewardWinBtnClicked);
        skinButton.onClick.AddListener(ShowSkinSelectRoot);
        hideSkinButton.onClick.AddListener(HideSkinRoot);
        DisableAllUI();

        curLevelIndex = PlayerPrefs.GetInt(CUR_LEVEL_KEY, 1);
        curPlayerMaxIndex = PlayerPrefs.GetInt(CUR_MAX_LEVEL_KEY, 1);
        PlayerPrefs.SetInt(SKIN_INDEX + "0", 1);
        _skinIndex = PlayerPrefs.GetInt(CUR_SKIN_INDEX, 0);
        _oldPer = PlayerPrefs.GetFloat(OLD_PER, 0f);
        StartCoroutine(DelayShowAds());
    }

    IEnumerator DelayShowAds()
    {
        yield return new WaitForSeconds(3f);
        AdsMgr.current.ShowInter();
        yield return new WaitForSeconds(2f);
        AdsMgr.current.ShowRewardAds(Enm);
    }

    void Enm()
    {

    }

    void BackToBeforePlay()
    {
        DisableAllUI();
        AdsMgr.current.ShowInterEvery30s();
        beforePlayUI.SetActive(true);
    }

    internal void ToPlaying()
    {
        HideBeforePlayUI();
        _fsm.ChangeState(InitState.Playing);
    }

    private void Start()
    {
        UpdateSkin();
        _fsm = StateMachine<InitState>.Initialize(this, InitState.Ready);
    }

    void Ready_Enter()
    {
        ShowBeforePlayUI();
        firstPlayUI.SetActive(true);
    }

    public GameObject firstPlayUI;
    void Playing_Enter()
    {
        firstPlayUI.SetActive(false);
        HideBeforePlayUI();
    }


    private void OnRewardWinBtnClicked()
    {
        AdsMgr.current.ShowRewardAds(OnRewardWinCallBack);
    }

    public const int LEVEL_REWARD_COIN_NUM = 50;
    void OnRewardWinCallBack()
    {
        AddCoin(LEVEL_REWARD_COIN_NUM * 4);
        AnalyzeMgr.current.onAddCoinRewardFinshied();
        //levelRewardWinButton.gameObject.SetActive(false);
        playNextLevel();
    }

    public GameObject beforePlayUI;
    public void ShowBeforePlayUI()
    {
        beforePlayUI.SetActive(true);
    }

    public void HideBeforePlayUI()
    {
        beforePlayUI.SetActive(false);
    }

    public bool isFirstPlayed = true;
    private void tapToStartCB()
    {
        print("tap start");
        TimeLineMgr.current.PlayFisrt();
        //if (isFirstPlayed)
        //{
        //    TimeLineMgr.current.PlayFisrt();
        //}else
        //{
        //    CallPlay();
        //}
        HideBeforePlayUI();

    }


    private void rewardBack()
    {
        DisableAllUI();
        AdsMgr.current.ShowRewardAds(rewardBackToLast);
    }


    public const string CUR_LEVEL_KEY = "CURRENT_LEVEL";
    public const string CUR_MAX_LEVEL_KEY = "MAX_LEVEL";

    void rewardBackToLast()
    {
        DisableAllUI();
        AdsMgr.current.ShowInterEvery30s();
        print("reward Back");
        TimeLineMgr.current.BackToLastTimeLine();
    }

    float playLevelTapTime = 0f;
    public const float INTERVALE_TAP = 1f;

    public void playNextLevel()
    {
        if (playLevelTapTime + INTERVALE_TAP > Time.time)
        {
            return;
        }

        AdsMgr.current.ShowRewardAds(CallPlay);
        DisableAllUI();
        //CallPlay();
    }



    int curPlayerMaxIndex = 1;
    int curLevelIndex = 1;

    float gameStartTime;
    private void CallPlay()
    {
        print("cur index " + curLevelIndex + " max " + curPlayerMaxIndex);

        playLevelTapTime = Time.time;
        if (curLevelIndex >= curPlayerMaxIndex)
        {
            //Debug.Log("play level  " + (curLevelIndex).ToString());
            if (AnalyzeMgr.current != null)
            {
                AnalyzeMgr.current.OnFirstPlayNextLevel(curLevelIndex);
            }
        }
        //HideWinCanvas();
        gameStartTime = Time.time;
        LoadLevel(curLevelIndex);
    }


    public Button levelSelectBtn;
    
    public void OnShowLevelSelect()
    {
        levelSelectRoot.SetActive(true);
    }


    public Button backToLevelSelct;
    public Button backToLevelSelct2;
    public void OnHideLevelSelect()
    {
        levelSelectRoot.SetActive(false);
    }
    public const int MAX_CHAR_NUM = 14;
    public const int MAX_LEVEL_INDEX = 28;
    public const string UNLOCK_PREF = "UNLOCK";
    public void LoadLevel(int index)
    {
        print("load level " + index);
        levelSelectRoot.SetActive(false);
        if (index > MAX_LEVEL_INDEX)
        {
            index = UnityEngine.Random.Range(2, MAX_LEVEL_INDEX - 1);
        }
        //Debug.Log("index load " + index);
        if (!Application.CanStreamedLevelBeLoaded(index))
        {
            index = 1;
            
        }

        print("after level " + index);
        PlayerPrefs.SetInt(UNLOCK_PREF + index.ToString(), 1);
        PlayerPrefs.SetInt(CUR_LEVEL_KEY, index);
        ClearSceneData.LoadLevelByIndex(index);
        //OnPlayCanvas.gameObject.SetActive(true);

        //SceneManager.LoadScene("StayAlive_level_" + index.ToString("000"));
        //AdsMgr.current.RequestInterstitial();
        //AdsMgr.current.CreateAndLoadRewardedAd();
        //UpdateHpStatus();
    }

    static bool m_ShuttingDown = false;
    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }


    public GameObject winRoot;
    public GameObject loseRoot;
    public GameObject levelSelectRoot;
    public GameObject skinRoot;
    public GameObject rewardSkinRoot;

    public Button skinButton;
    public Button hideSkinButton;
    public Image rewardSkinIcon;
    public void ShowSkinSelectRoot()
    {
        AnalyzeMgr.current.OnShopBtnTapped();
        skinRoot.SetActive(true);
        rewardSkinIcon.sprite = skinMgr.current.skinDatas[curGiftSkinID].icon;
    }

    public void RewardSkinBtnPressed()
    {

        DisableAllUI();
        winRoot.SetActive(true);
        AdsMgr.current.ShowRewardAds(RewardSkinBtnCallBack);
    }

    void RewardSkinBtnCallBack()
    {
        AnalyzeMgr.current.onSkinRewardWatched(skinMgr.current.skinDatas[curGiftSkinID].bodyName);
        PlayerPrefs.SetInt(SKIN_INDEX + curGiftSkinID.ToString(), 1);
        PlayerPrefs.SetInt(SKIN_SHOW+ curGiftSkinID.ToString(), 1);
        SkinIndex = curGiftSkinID;
        UpdateSkin();
        //ShowSkinSelectRoot();
        playNextLevel();
    }

    public void HideSkinRoot()
    {
        skinRoot.SetActive(false);
    }

    public void DisableAllUI()
    {
        winRoot.SetActive(false);
        loseRoot.SetActive(false);
        rewardSkinRoot.SetActive(false);
        beforePlayUI.SetActive(false);
    }

    public void ShowRewardSkinRoot()
    {
        rewardSkinRoot.SetActive(true);
        rewardSkinIcon.sprite = skinMgr.current.skinDatas[curGiftSkinID].icon;
    }
    public void ToWin()
    {
        print("tttt win");
        //GiveGift();
        ShowBeforePlayUI();
        //AddCoin(LEVEL_REWARD_COIN_NUM);
        AdsMgr.current.ShowInter();
        AnalyzeMgr.current.OnLevelWon(curLevelIndex, (int)(Time.time-TimeLineMgr.StartGameTime));
        DisableAllUI();
        winRoot.SetActive(true);

        if (curPlayerMaxIndex == curLevelIndex)
        {
            curPlayerMaxIndex++;
            PlayerPrefs.SetInt(CUR_MAX_LEVEL_KEY, curPlayerMaxIndex);
        }
        curLevelIndex++;
        PlayerPrefs.SetInt(CUR_LEVEL_KEY, curLevelIndex);

    }

    public void ToLose()
    {
        AnalyzeMgr.current.OnLevelLose(curLevelIndex, "none");
        DisableAllUI();
        //ShowBeforePlayUI();
        loseRoot.SetActive(true);
        bool isFirst = TimeLineMgr.current.timelineIndex == 1;

        retryBtn.gameObject.SetActive(isFirst);
        noThanksBtn.gameObject.SetActive(!isFirst);
        rewardBackBtn.gameObject.SetActive(!isFirst);
    }

    public void RestartLevel()
    {
        AdsMgr.current.ShowInterEvery30s();
        DisableAllUI();
        print("cur level " + curLevelIndex);
        ClearSceneData.LoadLevelByIndex(curLevelIndex);
    }

    internal int GetCurrentLevelIndex()
    {
        return curLevelIndex;
    }

    public int GetCurMaxLevelIndex()
    {
        return curPlayerMaxIndex;
    }

    public Button nextLevelBtn;
    public Button noThanksBtn;
    public Button rewardBackBtn;
    public Button retryBtn;
    public Button tapToStart;
    internal System.Action onCoinChanged;
    int _skinIndex = 0;

    public int SkinIndex
    {
        get { return _skinIndex; }
        set { _skinIndex = value;
            PlayerPrefs.SetInt(CUR_SKIN_INDEX, _skinIndex);
        }
    }
    public Sprite[] icons;

    public Transform playerInInitMgr;

    public readonly string[] ignores = new string[] { 
        "Root",
        "SM_Chr_Eyebrows_01",
        "SM_Chr_Eyes_Male_01" };
    public void UpdateSkin() 
    {
        return;
        var skinData = skinMgr.current.skinDatas[_skinIndex];
        ignoreList.Clear();
        ignoreList.AddRange(ignores);
        ignoreList.Add(skinData.bodyName);
        UpdateSkinFromRoot(playerInInitMgr);
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            UpdateSkinFromRoot(player.transform);
        }

    }

    List<string> ignoreList = new List<string>();

    public void UpdateSkinFromRoot(Transform root)
    {
        foreach (Transform c in root)
        {
            c.gameObject.SetActive(false);
            foreach (var v in ignoreList)
            {
                if (v.Equals(c.name))
                {
                    c.gameObject.SetActive(true);
                    break;
                }
            }
        }


    }

    public Image gift;
    public Text percentage;
    public const string SKIN_INDEX = "SKIN_INDEX";
    //public const string SKIN_PER = "SKIN_PER";
    public const string SKIN_SHOW = "SKIN_SHOW";
    public const string CUR_SKIN_INDEX = "CUR_SKIN_INDEX";

    int curGiftSkinID;
    int GetCurNoExistSkinID()
    {
        var sk = skinMgr.current.skinDatas;
        for (int i = 0; i < sk.Length; i++)
        {
            var curData = sk[i];
            if (PlayerPrefs.GetInt(SKIN_SHOW + i.ToString(), 0) == 0 && (PlayerPrefs.GetInt(SKIN_INDEX + i.ToString(), 0) == 0))
            {
                return i;
            }
        }
        return -1;
    }
    public const float EACH_INCREASE = 35f;
    public void GiveGift()
    {
        curGiftSkinID = GetCurNoExistSkinID();
        print("cur gift " + curGiftSkinID);
        if (curGiftSkinID > 0)
        {
            oldPer = PlayerPrefs.GetFloat(OLD_PER, 0f);
            print("oldd " + oldPer + " " + " new per " + newPer);
            newPer = oldPer + EACH_INCREASE;
            
            if (newPer >= 100)
            {
                newPer = 100f;
                PlayerPrefs.SetInt(SKIN_SHOW + curGiftSkinID.ToString(), 1);
            }
            else
            {
                PlayerPrefs.SetFloat(OLD_PER, newPer);
            }
            StartCoroutine(IGet());
        }

    }

    float speed = 40f;
    public const string OLD_PER = "OLD_PER";

    float _oldPer = 0f;
    float oldPer { set { _oldPer = value; PlayerPrefs.SetFloat(OLD_PER, _oldPer); } get { return _oldPer; } }
    float newPer = 0f;
    IEnumerator IGet()
    {
        gift.fillAmount = oldPer/ 100f;
        percentage.text = oldPer + "%";
        float cur = oldPer;
        oldPer = newPer;
        print("old " + oldPer + " new  " + newPer);
        while (cur < newPer)
        {
            cur += speed * Time.deltaTime;
            percentage.text =  (int)cur + "%";
            gift.fillAmount = cur / 100f;
            yield return null;
        }
        gift.fillAmount = newPer / 100f;
        percentage.text =  newPer + "%";
        ShowRewardSkinBtn();
    }

    void ShowRewardSkinBtn()
    {
        if(newPer>=100f)
        {
            oldPer = 0f;
            newPer = 0f;
            ShowRewardSkinRoot();
        }

    }

}

using MonsterLove.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


public enum TimelineGameStates
{
    Init,
    Playing,
    WaitForAnim,
    Anim,
    CheckWin,
    Win,
    Lose
}

public enum WinState
{
    None,
    Win,
    Lose,
}

[Serializable]
public class TimeLineData
{
    public string timelineKey;
    public PlayableDirector director;
    public PlayableDirector nextDir;
    public WinState winState;
}

public class TimeLineMgr : MonoBehaviour
{

    public TimeLineData[] _datas;
    StateMachine<TimelineGameStates> fsm;
    public Dictionary<string, TimeLineData> _playDir = new Dictionary<string, TimeLineData>();
    public PlayableDirector fistDir;

    public static TimeLineMgr current;
    private void Awake()
    {
        current = this;
        fsm = StateMachine<TimelineGameStates>.Initialize(this, TimelineGameStates.Init);
        foreach(var v in _datas)
        {
            _playDir.Add(v.timelineKey, v);
        }
        timelineIndex = 0;
        StartGameTime = Time.time;
        if(InitMgr.current.isFirstPlayed)
        {
            if (fistDir != null)
            {
                StartCoroutine(PauseLater());
            }

        }
        else
        {
            InitMgr.current.beforePlayUI.SetActive(false);
        }
        InitMgr.current.isFirstPlayed = false;
        CurTimelineName = "";
        InitMgr.current.UpdateSkin();
    }

    IEnumerator PauseLater()
    {
        yield return new WaitForSeconds(0.1f);
        fistDir.Pause();
    }

    private void Start()
    {
        AnalyzeMgr.current.OnLevelStart(InitMgr.current.GetCurrentLevelIndex());
    }
    public void PlayFisrt()
    {
        print("play first");
        InitMgr.current.ToPlaying();
        timelineIndex = 0;
        if (fistDir != null)
        {
            fistDir.time = 0f;
            fistDir.Stop();
            fistDir.Play();
            double test_delay = fistDir.duration;
            StartCoroutine(TestWin(test_delay));
            
        }

    }

    IEnumerator TestWin(double delay)
    {
        print("before " + delay);
        yield return new WaitForSeconds((float)delay);
        fsm.ChangeState(TimelineGameStates.Win);
        print("test win");
    }

    IEnumerator Init_Enter()
    {
        yield return new WaitForSeconds(0.2f);
        fsm.ChangeState(TimelineGameStates.Playing);
    }


    public int timelineIndex = 0;

    public static string CurTimelineName = "";
    List<TimeLineData> _playTimeLineOrder = new List<TimeLineData>();
    TimeLineData _curPlayData = null;
    public void PlayingTimeLine(string s)
    {
        InitMgr.current.HideBeforePlayUI();
        TimeLineData data;
        if(_playDir.TryGetValue(s,out data))
        {
            AnalyzeMgr.current.onTimeLinePlayed(s);
            _playTimeLineOrder.Add(data);
            CurTimelineName = s;
            _winState = data.winState;
            var dir = data.director;
            dir.Stop();
            dir.time = 0f;
            dir.Play();
            _dirDuration = (float)dir.duration;
            _curPlayData = data;
            timelineIndex++;
            fsm.ChangeState(TimelineGameStates.Anim);

        }
    }


    float _dirDuration = 0;
    WinState _winState;

    IEnumerator Anim_Enter()
    {
        print("anim enter " + _winState);
        yield return new WaitForSeconds(_dirDuration);

        if (_winState != WinState.Lose)
        {
            AdsMgr.current.ShowInterEvery30s();
        }
        if (_winState == WinState.Win)
        {
            fsm.ChangeState(TimelineGameStates.Win);

        }
        else if (_winState == WinState.Lose)
        {
            fsm.ChangeState(TimelineGameStates.Lose);
        }
        else
        {

            if (_curPlayData != null && _curPlayData.nextDir != null)
            {
                _curPlayData.nextDir.Play();
                _dirDuration = (float)_curPlayData.nextDir.duration;
                fsm.ChangeState(TimelineGameStates.Playing);
                _curPlayData = null;
            }
            else
            {
                print("playing");
                fsm.ChangeState(TimelineGameStates.Playing);
            }
        }
    }

    void Playing_Enter()
    {
    }


    void Lose_Enter()
    {
        InitMgr.current.ToLose();
    }

    void Win_Enter()
    {
        InitMgr.current.ToWin();
    }

    public static float StartGameTime = 0f;

    public void BackToLastTimeLine()
    {
        if (_playTimeLineOrder.Count > 1 )
        {
            _curPlayData = _playTimeLineOrder[_playTimeLineOrder.Count - 2];
            _playTimeLineOrder.RemoveAt(_playTimeLineOrder.Count - 1);

            if (_curPlayData != null && _curPlayData.nextDir != null)
            {
                _curPlayData.nextDir.Stop();
                _curPlayData.nextDir.time = 0;
                _curPlayData.nextDir.Play();
                _dirDuration = (float)_curPlayData.nextDir.duration;
                fsm.ChangeState(TimelineGameStates.Playing);
            }
                //PlayingTimeLine(p.timelineKey);
            //PlayTheLastTimeline();
        }
        else
        {
            _playTimeLineOrder.Clear();
            var dir = fistDir;
            //dir.time = dir.duration - 2f;
            dir.Stop();
            dir.time = 0f;
            dir.Play();
            _dirDuration = (float)dir.duration;
            timelineIndex = 0;
            _curPlayData = null;
            _winState = WinState.None;
            fsm.ChangeState(TimelineGameStates.Init);

        }
    }
}

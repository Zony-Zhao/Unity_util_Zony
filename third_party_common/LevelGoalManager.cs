using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using animals.scripts.contentDB;
using common;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class LevelGoal
{
    public int index;
    public string desc;
    public string name;

    public LevelGoalType type;
    public float amount;

    public bool completed;

    //累积的
    public bool save;
    public int reward;
    public void Complete()
    {
        if(completed)return;
        completed = true;
        SaveLoadManager.currentData.finishedAchievement.Add(index);
        SaveLoadManager.currentData.finishedAchievement.Sort((a, b) => a - b);
        SaveLoadManager.currentData.coins += reward;
        SaveLoadManager.instance.Save();
        if(LevelManager.Instance != null)
        {
            LevelManager.Instance.UpdateMission(this);
        }
        LevelManager.Instance.killMission += reward;
    }
}

public enum LevelGoalType
{
    KillStreak,

    //单局杀敌
    KillAmount,

    //累计杀敌
    totalKillAmount,

    //存活时间
    Time,

    //无损时间
    UnhurtTime,

    Score,

    //单局大招次数
    UltTimes,

    //累计大招次数
    totalUltTimes,
    bounceTime,

    //累计3次杀敌
    streak3Times,

    //累计5次杀敌
    streak5Times,
    //累计8次杀敌
    streak8Times,

    //累计10次杀敌
    streak10Times,

    //累计15次杀敌
    streak15Times,

    //单局刀兵,
    DaoKills,
    QiangKills,
    GongKills,
    DunKills,
    RenzheKills,

    //累计刀兵
    totalDaoKills,
    totalQiangKills,
    totalGongKills,
    totalDunKills,
    totalRenzheKills,
    //累计登录
    totalSingIn
}

[Serializable]
public class LevelGoalList
{
    public List<LevelGoal> goals;
}

public class LevelGoalManager : Singleton<LevelGoalManager>
{
    public List<LevelGoal> levelGoals=>LevelGoalDB.instance.levelGoals;

    //现在在第几关的挑战关卡 如levelGoals[1]是第二关的所有目标
    public int currentLevel = 0;
    public int levelGoalPassAmount = 2;

    [NonSerialized, ShowInInspector, ReadOnly, OnInspectorGUI("@Sirenix.Utilities.Editor.GUIHelper.RequestRepaint()")]
    public float time = 0;

    public float unhurtTime = 0;
    public int ultTimes = 0;

    [NonSerialized, ShowInInspector, ReadOnly]
    public int highestKillStreak = 0;

    [NonSerialized, ShowInInspector, ReadOnly]
    public int killAmount = 0;

    [NonSerialized, ShowInInspector, ReadOnly]
    public int score = 0;

    [NonSerialized, ShowInInspector, ReadOnly]
    public int bounceTime = 0;

    public int DaoKillTimes;
    public int QiangKillTimes;
    public int GongKillTimes;
    public int DunKillTimes;
    public int RenzheKillTimes;


    // Start is called before the first frame update
    public bool hasStartedTime;

    void Start()
    {
        // StartTime();
        RefreshLevel();
        //LoadLevelGoalCompleted();
    }

    public void RefreshLevel()
    {
        StartTime();
        time = 0;
        unhurtTime = 0;
        ultTimes = 0;
        highestKillStreak = 0;
        killAmount = 0;
        score = 0;
        bounceTime = 0;
    }
    /// <summary>
    /// 加载存档数据中任务目标的完成状态
    /// </summary>
    public void LoadLevelGoalCompleted()
    {
        //todo 初始化成就列表
        foreach (var levelGoal in levelGoals)
        {
            if (SaveLoadManager.currentData.finishedAchievement.Exists(i => i == levelGoal.index))
            {
                levelGoal.completed = true;
            }
            else
            {
                levelGoal.completed = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //开始界面不调用update
        if(LevelManager.instance==null)
        {
            return;
        }
        if(LevelManager.instance!=null && !LevelManager.instance.isEndless)  //剧情关卡
        {
            if (hasStartedTime)
            {
                time += Time.unscaledDeltaTime;
            }
            return;
        }
        if (hasStartedTime)
        {
            time += Time.unscaledDeltaTime;
            if (CharacterController.instance != null && CharacterController.instance.health == CharacterController.instance.healthMax)
            {
                unhurtTime += Time.unscaledDeltaTime;
            }
        }

        var data = SaveLoadManager.currentData;
        foreach (var levelGoal in levelGoals)
        {
            if (!levelGoal.completed)
            {
                switch (levelGoal.type)
                {
                    case LevelGoalType.KillStreak:
                        if (highestKillStreak >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.KillAmount:
                        if (killAmount >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.Time:
                        if (time >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.Score:
                        if (score >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.bounceTime:
                        if (bounceTime >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;

                    case LevelGoalType.totalKillAmount:
                        if (data.totalEnemyKills >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;

                    case LevelGoalType.UnhurtTime:
                        if (unhurtTime >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;

                    case LevelGoalType.UltTimes:
                        if (ultTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.totalUltTimes:
                        if (data.totalUltTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;

                    case LevelGoalType.streak3Times:
                        if (data.streak3Times >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.streak5Times:
                        if (data.streak5Times >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.streak8Times:
                        if (data.streak8Times >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.streak10Times:
                        if (data.streak10Times >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;       
                    case LevelGoalType.streak15Times:
                        if (data.streak15Times >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.DaoKills:
                        if (DaoKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.QiangKills:
                        if (QiangKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.GongKills:
                        if (GongKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.DunKills:
                        if (DunKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }
                        break;
                    case LevelGoalType.RenzheKills:
                        
                        if (RenzheKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }
                        break;
                    case LevelGoalType.totalDaoKills:
                        
                        if (data.DaoKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.totalQiangKills:
                      if (data.QiangKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.totalGongKills:
                      if (data.GongKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }

                        break;
                    case LevelGoalType.totalDunKills:
                       if (data.DunKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }
                        break;
                    case LevelGoalType.totalRenzheKills:
                       if (data.RenzheKillTimes >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }
                        break; 
                    
                    case LevelGoalType.totalSingIn:
                       if (data.signInDays >= levelGoal.amount)
                        {
                            levelGoal.Complete();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (levelGoals.Where(lg => lg.completed).Count() >= levelGoalPassAmount)
        {
            //Debug.Log("通关");
        }
    }

    public void StartTime()
    {
        hasStartedTime = true;
        unhurtTime = 0;
        time = 0;
    }

    public void KillEnemyCount(int killsInOneDash, List<Enemy> enemies)
    {
        if (LevelManager.instance!=null && !LevelManager.instance.isEndless)  //剧情关卡
        {
            highestKillStreak = Mathf.Max(killsInOneDash, highestKillStreak);
            //杀人赢钱，多杀翻倍
            int coinByOneKillinLevel = 1;
            if (highestKillStreak>=2)
            {
                coinByOneKillinLevel = 2;
                LevelManager.Instance.killDouble += killsInOneDash;  //多杀+1
            }
            SaveLoadManager.currentData.coins += killsInOneDash * coinByOneKillinLevel;
            // LevelManager.Instance.CheckKillOnceMax(highestKillStreak);  //记录单次最高击杀记录，后期扩展可能用到
            return;
        }

        highestKillStreak = Mathf.Max(killsInOneDash, highestKillStreak);
        var data = SaveLoadManager.currentData;
        if (highestKillStreak >= 3)
        {
            data.streak3Times++;
        }

        if (highestKillStreak >= 5)
        {
            data.streak5Times++;
        }

        if (highestKillStreak >= 10)
        {
            data.streak10Times++;
        }

        killAmount += killsInOneDash;
        SaveLoadManager.currentData.totalEnemyKills+= killsInOneDash;
        score += killsInOneDash >= 2 ? killsInOneDash * 2 : killsInOneDash;
        //杀人赢钱，多杀翻倍
        int coinByOneKill = 1;
        if (highestKillStreak>=2)
        {
            coinByOneKill = 2;
            LevelManager.Instance.killDouble += killsInOneDash;  //多杀+1
        }
        SaveLoadManager.currentData.coins += killsInOneDash * coinByOneKill;
        LevelManager.Instance.CheckKillOnceMax(highestKillStreak);
        
        Assert.AreEqual(killsInOneDash, enemies.Count);

        // Assert.AreEqual(killsInOneDash, enemies.Count);
        enemies.ForEach(e =>
        {
            switch (e.enemyType)
            {
                case 1:
                    DaoKillTimes++;
                    data.DaoKillTimes++;
                    break;
                case 2:
                    QiangKillTimes++;
                    data.QiangKillTimes++;
                    break;
                case 3:
                    GongKillTimes++;
                    data.GongKillTimes++;
                    break;
                case 4:
                    DunKillTimes++;
                    data.DunKillTimes++;
                    break;
                case 5:
                    RenzheKillTimes++;
                    data.RenzheKillTimes++;
                    break;
            }
        });
        // SaveLoadManager.instance.Save();
    }


    public void bounce()
    {
        bounceTime++;
    }

    public void UseUlt()
    {
        ultTimes++;
        SaveLoadManager.currentData.totalUltTimes++;
    }
}
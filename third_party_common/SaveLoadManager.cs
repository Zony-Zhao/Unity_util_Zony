using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using common;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class PlayerData
{
    public int coins;

    //默认为0，未解锁
    public int unlockedStoryMode;
    public int unlockedChallengeMode;
    public long saveDataTime;

    public string saveDataTimeDebug;

    //默认为0
    public int currentWeapon;
    public List<int> unlockedWeapons;
    public int currentHat;
    public List<int> unlockedHats;
    public int currentScarf;
    public List<int> unlockedScarves;
    public List<Buff> buffs;

    //默认为普通
    public string capName;
    public string capeName;
    public string swordName;
    private string _nomal = "初始装备";
    
    //设置选项
    public float settingSoundVolumeAll;
    public float settingSoundVolumeBGM;
    public float settingSoundVolumeBreath;
    public float settingSoundVolumeVFX;
    public PlayerData()
    {
        coins = 0;
        unlockedStoryMode = unlockedChallengeMode = 0;
        saveDataTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        saveDataTimeDebug = DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
        currentWeapon = currentHat = currentScarf = 0;
        unlockedWeapons = new List<int>();
        unlockedHats = new List<int>();
        unlockedScarves = new List<int>();
        finishedAchievement = new List<int>();
        streak3Times = 0;
        streak5Times = 0;
        streak10Times = 0;
        DaoKillTimes = 0;
        QiangKillTimes = 0;
        GongKillTimes = 0;
        DunKillTimes = 0;
        RenzheKillTimes = 0;
        totalEnemyKills = 0;
        totalUltTimes = 0;
        signInDays = 1;
        capName=_nomal;
        capeName=_nomal;
        swordName= _nomal;
        //设置界面
        settingSoundVolumeAll = 1;  //设置总音量
        settingSoundVolumeBGM = 1;
        settingSoundVolumeBreath = 1;
        settingSoundVolumeVFX = 1;
    }

    public List<int> finishedAchievement;

    //成就系统
    //3连杀次数
    public int streak3Times;
    public int streak5Times;
    public int streak8Times;

    public int streak10Times;
    public int streak15Times;

    //刀兵击杀次数
    public int DaoKillTimes;
    public int QiangKillTimes;
    public int GongKillTimes;
    public int DunKillTimes;
    public int RenzheKillTimes;

    //总计杀敌
    public int totalEnemyKills;

    //蓄力大招次数
    public int totalUltTimes;
    public int signInDays;
}

[Serializable]
public class Buff
{
    public BuffType type;
    public int amount;
}

public enum BuffType
{
    manaMax,
    manaRegen
}

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public static PlayerData currentData;

    // Start is called before the first frame update
    public string fileName = "savedata.json";

    public void Start()
    {
        Load();
    }

    public  void Save()
    {
        currentData.saveDataTime = DateTimeOffset.Now.ToUnixTimeSeconds();
        ;
        currentData.saveDataTimeDebug = DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString();
        ;
        var path = Application.persistentDataPath + fileName;
        File.WriteAllText(path, JsonUtility.ToJson(currentData, true));
        Debug.Log("json保存\n" + JsonUtility.ToJson(currentData, true));
    }

    public  void Load()
    {
        var path = Application.persistentDataPath + fileName;
        if (File.Exists(path))
        {
            currentData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(path));
            DateTime lastSaveDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            lastSaveDateTime = lastSaveDateTime.AddSeconds( currentData.saveDataTime ).ToLocalTime();
            if (lastSaveDateTime.Date != DateTime.Now.Date)
            {
                currentData.signInDays++;
            }
            Debug.Log("json加载\n" + JsonUtility.ToJson(currentData, true));
        }
        else
        {
            currentData = new PlayerData();
            Save();
            Debug.Log("json加载\n" + "生成新数据");
        }
    }

    public void ClearAllData()
    {
        currentData = new PlayerData();
        Save();
        Load();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
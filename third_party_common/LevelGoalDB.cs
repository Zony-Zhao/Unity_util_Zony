using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace animals.scripts.contentDB
{
    [CreateAssetMenu(fileName = "LevelGoalDB", menuName = "LevelGoalDB", order = 0)]
    public class LevelGoalDB : ScriptableObject
    {
        [TableList]
        public List<LevelGoal> levelGoals;
        private static LevelGoalDB _instance;
        public TextAsset csv;
        public static LevelGoalDB instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                else
                {
                    return _instance= Resources.Load<LevelGoalDB>("Achievement/LevelGoalDB");

                }

            }
        }
        // public static List<Card> GloryCommanderCardsPoolInstance=>instance.gloryCommanderCardsPool;
        //
        // [FormerlySerializedAs("cardsPool")] public List<Card> gloryCommanderCardsPool;
        private Dictionary<string, LevelGoalType> map = new Dictionary<string, LevelGoalType>()
        {
            {"单局连杀",LevelGoalType.KillStreak},
            {"单局杀敌数",LevelGoalType.KillAmount},
            {"累计杀敌数",LevelGoalType.totalKillAmount},
            {"单局存活时间",LevelGoalType.Time},
            {"单局无损时间",LevelGoalType.UnhurtTime},
            {"单局大招次数",LevelGoalType.UltTimes},
            {"累计大招次数",LevelGoalType.totalUltTimes},
            {"单局弹墙次数",LevelGoalType.bounceTime},
            {"累计3连杀",LevelGoalType.streak3Times},
            {"累计5连杀",LevelGoalType.streak5Times},
            {"累计8连杀",LevelGoalType.streak8Times},
            {"累计10连杀",LevelGoalType.streak10Times},
            {"累计15连杀",LevelGoalType.streak15Times},
            {"单局刀兵",LevelGoalType.DaoKills},
            {"单局枪兵",LevelGoalType.QiangKills},
            {"单局弓兵",LevelGoalType.GongKills},
            {"单局盾兵",LevelGoalType.DunKills},
            {"单局忍者",LevelGoalType.RenzheKills},
            {"累计刀兵",LevelGoalType.totalDaoKills},
            {"累计枪兵",LevelGoalType.totalQiangKills},
            {"累计弓兵",LevelGoalType.totalQiangKills},
            {"累计盾兵",LevelGoalType.totalDunKills},
            {"累计忍者",LevelGoalType.totalRenzheKills},
            {"累计登录",LevelGoalType.totalSingIn}
        };
        [Button]
        public void Read()
        {
   
            var lines = csv.text.Split('\n');
 
            var lists = new List<List<string>>();
            var columns = 0;
            for(int i = 0; i < lines.Length; i++) {
                var data = lines[i].Split(',');
                var list = new List<string>(data); // turn this into a list
                lists.Add(list); // add this list into a big list
                columns = Mathf.Max(columns, list.Count); // this way we can tell what's the max number of columns in data
            }
            levelGoals.Clear();
            for(int row = 1; row < lists.Count; row++)
            {
                var dataRow = lists[row];
                var lg = new LevelGoal();
                lg.name = dataRow[0];
                lg.desc = dataRow[1];
                try
                {
                    lg.reward = int.Parse(dataRow[5]);
                    lg.amount = float.Parse(dataRow[6]);
                    lg.type = map[dataRow[2]];
                }
                catch (Exception e)
                {
                    lg.amount = 0;
                    Debug.LogError(e);
                }
                lg.index = row - 1;
                levelGoals.Add(lg);
                for(int col = 0; col < columns; col++) {
                    try {
                        // Debug.Log(lists[col][row]);
                    } catch { // with try/catch it won't explode if this particular column/row is out of range
                        // Debug.Log("*");
                    }
                }
            }
        }
    }
 
}
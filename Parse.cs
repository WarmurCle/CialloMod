using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace CialloMod
{
    public static class Parser
    {
        public static Dictionary<string, GameEntry> Apps = new Dictionary<string, GameEntry>()
            {
                { "SerenBanka", new GameEntry("1144400",
                        "ed1_yosh", "ed2_mako", "ed3_mura",
                        "ed4_lena", "ed5_koha", "ed6_roka") }, //千恋万花
                { "SanobaWitch", new GameEntry("2458530",
                "clear_nene", "clear_meguru", "clear_tsumugi", "clear_touko",
                "clear_wakana" ) }, //魔宴
                { "CafeStella", new GameEntry("1829980",
                "clear_kan", "clear_nat", "clear_noz", "clear_mei", "clear_suz") }, //馆死
                { "RiddleJoker", new GameEntry("1277930",
                "clear_aya", "clear_nan", "clear_may", "clear_haz", "clear_chi")},  //RJ
                { "DracuRiot", new GameEntry("1340140",
                "clear_miu", "clear_azu", "clear_rio", "clear_eri", "clear_nic") }   //DR
            };
        public static void Parse()
        {
            var path_ = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Valve\\Steam", "InstallPath", "C:\\Program Files (x86)\\Steam");

            string SteamPath = path_ == null ? string.Empty : (string)path_;
            if (OperatingSystem.IsWindows() && SteamPath != string.Empty)
            {
                string DataPath = Path.Combine(SteamPath, "userdata");
                string[] folders = Directory.GetDirectories(DataPath);
                #region APPID
                
                #endregion
                for (int i = 0; i < folders.Length; i++)
                {
                    foreach(var pair in Apps)
                    {
                        string SavePath = Path.Combine(
                            folders[i], $"{pair.Value.Name}/remote/datasu.ksd")
                            .Replace("\\", "/");
                        if (!Path.Exists(SavePath))
                            continue;
                        string content = Descrambler.Descramble(SavePath);
                        pair.Value.CheckAchievements(content);
                    }
                }
            }
        }
        public class GameEntry
        {
            public string Name;
            public List<string> Achievements;
            public List<bool> AchievementsStatus;
            public GameEntry(string name, params string[] achievements)
            {
                Name = name;
                Achievements = achievements.ToList();
                AchievementsStatus = new();
                for (int i = 0; i < Achievements.Count; i++)
                {
                    AchievementsStatus.Add(false);
                }
            }
            public void CheckAchievements(string save)
            {
                int i = 0;
                foreach (string acName in Achievements)
                {
                    bool flag = AchievementsStatus[i];
                    if (save.Contains($"\"{acName}\" => 1"))
                    {
                        flag = true;
                    }
                    
                    if(flag)
                        AchievementsStatus[i] = flag;
                    i++;
                }
            }
        }
    }
}
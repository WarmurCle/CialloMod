using BaseLib.Audio;
using CialloMod.src.Core.Models.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.CardPools;
using System;
using static Godot.OpenXRCompositionLayer;

namespace CialloMod
{
    [ModInitializer(nameof(Initialize))]
    public static class ModInitializer
    {
        public static void Initialize()
        {

            Parser.Parse();
            int sum = 0;
            foreach (var pair in Parser.Apps)
            {
                foreach (bool value in pair.Value.AchievementsStatus)
                {
                    if (value)
                        sum++;
                }
            }
            CialloStrike.HitCount = sum;
            var harmony = new Harmony("CialloMod.Polaris");

            var prefixMethod = new HarmonyMethod(Prefix);
            harmony.Patch(typeof(SfxCmd).GetMethod("Play", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public, null, new Type[] { typeof(string), typeof(float) }, null), prefixMethod);
            harmony.PatchAll();
            ModHelper.AddModelToPool(typeof(ColorlessCardPool), typeof(CialloStrike));

        }

        [HarmonyPrefix]
        public static bool Prefix(string sfx, float volume)
        {
            try
            {
                if (sfx == "RANDOMCIALLOSND")
                {
                    sfx = $"res://CialloMod/assets/audio/cia{new Random().Next(10).ToString()}.mp3";
                }
                if (sfx.StartsWith("res://CialloMod"))
                {
                    var ss = new AutoModAudio("res://").PlaySfx(sfx.Replace("res://CialloMod/", ""), 0, volume, 0, 1);
                    if (ss != null)
                        ss.PitchScale = 1;
                    return false;
                }
            }
            catch { }
            return true;
        }
    }
}
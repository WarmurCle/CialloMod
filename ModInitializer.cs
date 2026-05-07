using CialloMod.src.Core.Models.Cards;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace CialloMod
{
    [ModInitializer(nameof(Initialize))]
    public static class ModInitializer
    {
        public static void Initialize()
        {
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
                harmony.PatchAll();

                ModHelper.AddModelToPool(typeof(ColorlessCardPool), typeof(CialloStrike));
            }
        }
    }
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CialloMod.src.Core.Models.Cards
{
    public sealed class CialloStrike : CardModel
    {
        public static int HitCount = 0;
        protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag>{(CardTag.Strike)};

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(4m, ValueProp.Move),
            new CalculationBaseVar(0m),
            new CalculationExtraVar(1m),
            new CalculatedVar("CalculatedHits").WithMultiplier((CardModel card, Creature? _) => HitCount)
        };

        public CialloStrike() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy, true)
        {
        }
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(HitCount).FromCard(this).Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}
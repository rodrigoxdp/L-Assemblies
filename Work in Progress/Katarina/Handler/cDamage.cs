using Katarina.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace Katarina.Handler
{
    public static class cDamage
    {

        private static Obj_AI_Hero Player
        {
            get { return Program.Player; }
        }

        private static Spell Q
        {
            get { return cSpell.Q; }
        }

        private static Spell W
        {
            get { return cSpell.W; }
        }

        private static Spell E
        {
            get { return cSpell.E; }
        }

        private static Spell R
        {
            get { return cSpell.R; }
        }

        public static float GetMainComboDamage(Obj_AI_Base target)
        {
            double damage = 0;

            if (Q.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.Q);

            if (W.IsReady())
                damage += (target.IsMarked() ? Player.GetSpellDamage(target, SpellSlot.W) + Player.GetSpellDamage(target, SpellSlot.Q, 1) : Player.GetSpellDamage(target, SpellSlot.W));

            if (E.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                damage += Player.GetSpellDamage(target, SpellSlot.R) * 8;

            if (cUtility.HasIgnite)
                damage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            return (float)damage;
        }

        public static bool IsMainComboKillable(this Obj_AI_Base target)
        {
            return GetMainComboDamage(target) > target.Health;
        }
    }
}

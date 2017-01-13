using System;
using Katarina.Handler;
using Katarina.Utility;
using LeagueSharp;
using LeagueSharp.Common;

namespace Katarina.Manager
{
    public static class cCombo
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

        public static void Combo()
        {
            //Get target
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);

            //Check for target
            if (target == null)
                return;

            //Check available Spells
            var useQ = cMenu.Config.Item("comboUseQ").GetValue<bool>();
            var useW = cMenu.Config.Item("comboUseW").GetValue<bool>();
            var useE = cMenu.Config.Item("comboUseE").GetValue<bool>();
            var useR = cMenu.Config.Item("comboUseR").GetValue<bool>();

            //Check if target is killable
            var isKillable = target.IsMainComboKillable();

            //Auto Ignite
            if (isKillable && cUtility.HasIgnite)
                Player.Spellbook.CastSpell(Player.GetSpellSlot("SummonersDot"), target);

            //Spells
            if (useE && E.IsReady())
            {
                if (isKillable && E.CanCast(target) || E.IsKillable(target))
                {
                    E.Cast(target);
                }
            }

            if (useQ && Q.IsReady())
            {
                if (isKillable && Q.CanCast(target) || Q.IsKillable(target) || (!useW && !useE))
                {
                    Q.Cast(target);
                }
            }

            if (useW && W.IsReady())
            {
                if (isKillable && W.CanCast(target) || W.IsKillable(target) || Q.IsReady(100) ||
                    target.ServerPosition.Distance(Player.Position, true) > Math.Pow(W.Range + 100, 2))
                {
                    W.Cast(target);
                }
            }

            if (useR && R.IsReady())
            {
                if (isKillable && R.CanCast(target) ||
                    R.IsKillable(target) && !Q.IsReady() && !E.IsReady() && !R.IsReady())
                {
                    R.Cast();
                }
            }
        }
    }
}

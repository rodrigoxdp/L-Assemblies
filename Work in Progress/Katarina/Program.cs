using System;
using Katarina.Handler;
using Katarina.Manager;
using LeagueSharp;
using LeagueSharp.Common;

namespace Katarina
{
    internal static class Program
    {
        // Initialize Champion
        private const string ChampionName = "Katarina";
        public static Obj_AI_Hero Player { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            //Listen to OnGameLoad Event
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            //Validate Champion
            if (Player.ChampionName != ChampionName)
                return;

            //Listen to Handlers
            cMenu.Initialize();
            cSpell.Initialize();
           
            //Listen to additional events
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (cMenu.Config.Item("comboActive").GetValue<KeyBind>().Active)
            {
                cCombo.Combo();
            }

            if (cMenu.Config.Item("Wardjump").GetValue<KeyBind>().Active)
            {
                cWardjumper.Wardjump();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (cMenu.Config.Item("QRange").GetValue<Circle>().Active && cSpell.Q.IsReady())
            {
                Render.Circle.DrawCircle(Player.Position, cSpell.Q.Range, cMenu.Config.Item("QRange").GetValue<Circle>().Color);
            }

            if (cMenu.Config.Item("WRange").GetValue<Circle>().Active && cSpell.W.IsReady())
            {
                Render.Circle.DrawCircle(Player.Position, cSpell.W.Range, cMenu.Config.Item("Wange").GetValue<Circle>().Color);
            }

            if (cMenu.Config.Item("ERange").GetValue<Circle>().Active && cSpell.E.IsReady())
            {
                Render.Circle.DrawCircle(Player.Position, cSpell.E.Range, cMenu.Config.Item("ERange").GetValue<Circle>().Color);
            }

            if (cMenu.Config.Item("RRange").GetValue<Circle>().Active && cSpell.R.IsReady())
            {
                Render.Circle.DrawCircle(Player.Position, cSpell.R.Range, cMenu.Config.Item("RRange").GetValue<Circle>().Color);
            }
        }
    }
}

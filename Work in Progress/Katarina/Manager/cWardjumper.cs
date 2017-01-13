using System;
using System.Linq;
using Katarina.Handler;
using Katarina.Utility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Katarina.Manager
{
    public static class cWardjumper
    {
        private static Obj_AI_Hero Player
        {
            get { return Program.Player; }
        }

        private static Spell E
        {
            get { return cSpell.E; }
        }

        private static int _lastPlaced;
        private static Vector3 _lastWardPos;

        public static void Wardjump()
        {
            Game.OnUpdate += Game_OnUpdate;
            GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            if (!E.IsReady() || !(sender is Obj_AI_Minion) || Environment.TickCount >= _lastPlaced + 300)
                return;

            if (Environment.TickCount >= _lastPlaced + 300) return;
            var ward = (Obj_AI_Minion) sender;

            if (ward.Name.ToLower().Contains("ward") && ward.Distance(_lastWardPos) < 500)
            {
                E.Cast(ward);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (!cMenu.Config.Item("enableJumper").GetValue<bool>() || !E.IsReady())
                return;

            //Wards
            foreach (var ward in ObjectManager.Get<Obj_AI_Base>()
                .Where(ward => ward.Name.ToLower().Contains("ward") && ward.Distance(Game.CursorPos) < 250)
                .Where(ward => cMenu.Config.Item("Wardjump").GetValue<KeyBind>().Active))
            {
                E.Cast(ward);
            }

            //Heroes
            foreach (var hero in HeroManager.AllHeroes.Where(
                hero => hero.Distance(Game.CursorPos) < 250 && hero.IsValidTarget(E.Range, false))
                .Where(hero => cMenu.Config.Item("Wardjump").GetValue<KeyBind>().Active))
            {
                E.Cast(hero);
            }

            //Minions
            foreach (var minion in MinionManager.GetMinions(E.Range, MinionTypes.All, MinionTeam.All)
                .Where(minion => minion.Distance(Game.CursorPos) < 250)
                .Where(minion => cMenu.Config.Item("Wardjump").GetValue<KeyBind>().Active))
            {
                E.Cast(minion);
            }

            if (Environment.TickCount <= _lastPlaced + 300)
                return;

            var cursorPos = Game.CursorPos;
            var myPos = Player.ServerPosition;

            var delta = cursorPos - myPos;
            delta.Normalize();

            var wardPosition = myPos + delta*(600 - 5);

            var invSlot = cUtility.FindBestWardItem();
            if (invSlot == null) return;

            Items.UseItem((int) invSlot.Id, wardPosition);
            _lastWardPos = wardPosition;
            _lastPlaced = Environment.TickCount;
        }
    }
}

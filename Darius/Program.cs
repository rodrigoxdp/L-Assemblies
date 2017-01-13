﻿using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;

namespace Darius
{
    internal class Program
    {
        //Player
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private const string ChampionName = "Darius";

        private static void Main(string[] args)
        {
            //Load "faked" OnGameLoad
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            //Return if Player is not playing Darius..
            if (Player.ChampionName != ChampionName)
                return;

            //Initizalize 
            SpellHandler.Initialize();
            ConfigHandler.Initialize();

            //Subscribe to events
            Orbwalking.AfterAttack += ComboHandler.ExecuteAfterAttack;
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            //Combo
            if (ConfigHandler.KeyLinks["comboActive"].Value.Active)
                ComboHandler.ExecuteCombo();

            //Harass
            if (ConfigHandler.KeyLinks["harassActive"].Value.Active)
                ComboHandler.ExecuteHarass();

            //Additionals (Killsteal)
            ComboHandler.ExecuteAdditionals();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            // Draw our circles
            foreach (var circle in ConfigHandler.CircleLinks.Values.Select(link => link.Value))
            {
                if (circle.Active)
                    Utility.DrawCircle(Player.Position, circle.Radius, circle.Color);
            }
        }
    }
}

using System;
using LeagueSharp;
using LeagueSharp.Common;

namespace Skinhack
{
    internal class Program
    {
        //Menu
        private static Menu _config;

        //Player
        private static Obj_AI_Hero _player;

        private static void Main(string[] args)
        {
            //Events
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Game.OnInput += Game_OnInput;
        }

        static void Game_OnInput(GameInputEventArgs args)
        {
            var input = args.Input;

            //Skin
            if (input.Contains("skin"))
            {
                args.Process = false;
                var split = input.Split(' ');

                if (input.Length > 0)
                {
                    _player.SetSkin(_player.BaseSkinName, int.Parse(split[1]));
                }
            }

            //Model
            if (input.Contains("model"))
            {
                args.Process = false;
                var split = input.Split(' ');

                if (input.Length > 0)
                {
                    _player.SetSkin(split[1], 0);
                }
            }
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            _player = ObjectManager.Player;

            //Root
            _config = new Menu("Skinchanger", "Skinchanger", true);

            //Items
            _config.AddItem(new MenuItem("skinID", "Change Skin", true).SetValue(new Slider(_player.BaseSkinId, 0, 15))).ValueChanged += OnValueChange;

            //Add to Menu
            _config.AddToMainMenu();
        }

        private static void OnValueChange(object sender, OnValueChangeEventArgs args)
        {
            _player.SetSkin(_player.BaseSkinName, args.GetNewValue<Slider>().Value);
        }
    }
}
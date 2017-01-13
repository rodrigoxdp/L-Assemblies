using System.Drawing;
using LeagueSharp.Common;

namespace Katarina.Handler
{
    public static class cMenu
    {
        //Menu
        public static Menu Config;

        //Orbwalker
        private static Orbwalking.Orbwalker Orbwalker;

        /// <summary>
        ///    Initializes/builds the Menu
        /// </summary>
        public static void Initialize()
        {
            //Create a new Menu instance
            Config = new Menu("Katarina", "Katarina", true);

            //Orbwalker
            Orbwalker = new Orbwalking.Orbwalker(Config.SubMenu("Orbwalking"));

            //Combo
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("comboUseQ", "Use Q").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("comboUseW", "Use W").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("comboUseE", "Use E").SetValue(true));
            Config.SubMenu("Combo")
                .AddItem(new MenuItem("comboUseR", "Use R").SetValue(true));
            Config.AddItem(
                    new MenuItem("comboActive", "Combo!").SetValue(
                        new KeyBind(Config.Item("Orbwalk").GetValue<KeyBind>().Key, KeyBindType.Press)));

            //Harass
            Config.SubMenu("Harass")
                .AddItem(new MenuItem("harassUseQ", "Use Q").SetValue(true));
            Config.SubMenu("Harass")
                .AddItem(new MenuItem("harassUseW", "Use W").SetValue(true));
            Config.SubMenu("Harass")
                .AddItem(new MenuItem("harassUseE", "Use E").SetValue(true));
            Config.SubMenu("Harass")
                .AddItem(new MenuItem("harassMode", "Harass Mode").SetValue(new StringList(new[] {"Q", "EQ", "EQW"})));

            //Wardjumper
            Config.SubMenu("Wardjumper")
                .AddItem(new MenuItem("enableJumper", "enableJumper").SetValue(true));
            Config.SubMenu("Wardjumper")
                .AddItem(new MenuItem("Wardjump", "Wardjump").SetValue(new KeyBind(71, KeyBindType.Press)));

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after combo").SetValue(true);
            LeagueSharp.Common.Utility.HpBarDamageIndicator.DamageToUnit = cDamage.GetMainComboDamage;
            LeagueSharp.Common.Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                LeagueSharp.Common.Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };

            //Drawings
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("QRange", "Q range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("WRange", "W range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("ERange", "E range").SetValue(new Circle(false, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(new MenuItem("RRange", "R range").SetValue(new Circle(true, Color.FromArgb(100, 255, 0, 255))));
            Config.SubMenu("Drawings")
                .AddItem(dmgAfterComboItem);

            //Add to Menu
            Config.AddToMainMenu();
        }
    }
}

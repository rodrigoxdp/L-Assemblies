using LeagueSharp;
using LeagueSharp.Common;

namespace Katarina.Utility
{
    public static class cUtility
    {
        private static Obj_AI_Hero Player
        {
            get { return Program.Player; }
        }

        public static InventorySlot FindBestWardItem()
        {
            var slot = Items.GetWardSlot();
            return slot == default(InventorySlot) ? null : slot;
        }

        public static bool IsMarked(this Obj_AI_Base target)
        {
            return target.HasBuff("katarinamark", true);
        }

        public static bool HasIgnite
        {
            get
            {
                var ignite = Player.Spellbook.GetSpell(Player.GetSpellSlot("SummonerDot"));
                return ignite != null && ignite.Slot != SpellSlot.Unknown &&
                       Player.Spellbook.CanUseSpell(ignite.Slot) == SpellState.Ready;
            }
        }
    }
}

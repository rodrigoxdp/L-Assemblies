using LeagueSharp;
using LeagueSharp.Common;

namespace Katarina.Handler
{
    public static class cSpell
    {
        public static Spell Q { get; private set; }
        public static Spell W { get; private set; }
        public static Spell E { get; private set; }
        public static Spell R { get; private set; }

        public static void Initialize()
        {
            //Initialize Spells
            Q = new Spell(SpellSlot.Q, 675);
            W = new Spell(SpellSlot.W, 375);
            E = new Spell(SpellSlot.E, 700);
            R = new Spell(SpellSlot.R, 550);

            //Finetune Spells
            Q.SetTargetted(400, 1400);
        }
    }
}

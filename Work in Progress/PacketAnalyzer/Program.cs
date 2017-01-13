#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace PacketAnalyzer
{
    internal static class Program
    {
        private static readonly PAForm PacketForm = new PAForm();

        public static List<byte> BlockedRecvPackets = new List<byte> {0x0D, 0x29, 0x7B, 0x11, 0x48, };
        public static List<GamePacket> SendPackets = new List<GamePacket>();
        public static List<GamePacket> RecvPackets = new List<GamePacket>();
        public static List<byte> BlockedSendPackets = new List<byte>
        {
            Packet.C2S.LeaveGame.Header,
            Packet.C2S.Zoom.Header,
            Packet.C2S.Camera.Header,
            Packet.C2S.SetTarget.Header, 
            Packet.C2S.Move.Header,
            0x4C, 0x97, 0x36,
        };
        public static Thread T;
        public static float TickCount;

        private static void Main(string[] args)
        {
          //  CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            Game.OnSendPacket += Game_OnGameSendPacket;
            Game.OnProcessPacket += Game_OnGameProcessPacket;
            AppDomain.CurrentDomain.DomainUnload += CurrentDomain_DomainUnload;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_DomainUnload;
            //     AppDomain.CurrentDomain.UnhandledException += CurrentDomain_DomainUnload;
            Console.Clear();
            TickCount = Environment.TickCount;
            T = new Thread(Work) { IsBackground = true };
            Thread.Sleep(100);
            T.Start();

        }

        static void Game_OnGameLoad(EventArgs args)
        {
            for(var i = 0; i < 63; i++)
            {
                var spell = ObjectManager.Player.Spellbook.GetSpell((SpellSlot) i);
                if (spell == null || spell.Name == null || spell.Name == "BaseSpell" || spell.Name == "nospelldata")
                {
                    return;
                }

                Console.WriteLine(spell.Slot + " " + spell.Name);
                Console.WriteLine(spell.Level);
                Console.WriteLine(spell.IsSealed);
                Console.WriteLine(spell.State.ToString());
                Console.WriteLine(spell.ToggleState);
                Console.WriteLine(spell.Ammo);
                Console.WriteLine(spell.AmmoRechargeStart);
                Console.WriteLine(spell.SData.CastRange[0]);
            }
        }

        private static void CurrentDomain_DomainUnload(object sender, EventArgs e)
        {
            if (T.IsAlive)
            {
                T.Abort();
            }

            PacketForm.Close();
        }

        private static void Game_OnGameSendPacket(GamePacketEventArgs args)
        {
            if (!PacketForm.chkSend.Checked || BlockedSendPackets.Contains(args.PacketData[0]))
            {
                return;
            }

            var p = new GamePacket(args);
            PacketForm.PGridSend.AddTo(p);
            SendPackets.Add(p);
        }

        private static void Game_OnGameProcessPacket(GamePacketEventArgs args)
        {
            if (!PacketForm.chkRecv.Checked || BlockedRecvPackets.Contains(args.PacketData[0]))
            {
                return;
            }

            var p = new GamePacket(args);
            PacketForm.PGridRecv.AddTo(p);
            RecvPackets.Add(p);
        }

        public static void Work()
        {
            Application.Run(PacketForm);
        }

        private static void AddTo(this DataGridView view, GamePacket p)
        {
            view.Rows.Add(
                new object[] { p.Header.ToHexString(), p.Size().ToString(), p.Channel.ToString(), p.Flags.ToString(), (Environment.TickCount - TickCount)/1000 });
        }
    }
}
namespace SRO_BlackRougeLauncher.Networking.Packets.Gateway
{
    using System.Collections.Generic;
    using System.Windows.Forms;
    using SilkroadLauncher;
    using SilkroadLauncher.Networking;
    using SilkroadLauncher.SSA;
    class NoticeHandler
    {
        public static void SERVER_GATEWAY_NOTICE_RESPONSE(Packet packet, Session client)
        {
            byte NoticeCount = packet.ReadUInt8();
            for (int i = 0; i < NoticeCount; i++)
            {
                string Subject = packet.ReadAscii();
                string Article = packet.ReadAscii();
                ushort Year = packet.ReadUInt16();
                ushort Month = packet.ReadUInt16();
                ushort Day = packet.ReadUInt16();
                ushort Hour = packet.ReadUInt16();
                ushort Minute = packet.ReadUInt16();
                ushort Second = packet.ReadUInt16();
                uint Microsecond = packet.ReadUInt32();

                Global.NoticeDicitonary.Add(new KeyValuePair<string, string>(Subject, Article));
            }

            Global.MainForm.BeginInvoke((MethodInvoker)delegate ()
            {
                if (Global.NoticeDicitonary.Count > 0)
                {
                    foreach (var item in Global.NoticeDicitonary)
                    {
                        Global.MainForm.NoticeList.Items.Add(item.Key);
                    }
                    Global.MainForm.NoticeList.SetSelected(0, true);
                }

                Global.MainForm.startBtn.Visible = true;
            });
        }
    }
}

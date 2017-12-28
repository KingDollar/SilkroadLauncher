namespace SilkroadLauncher
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading;
    using System.Windows.Forms;
    using SilkroadLauncher.Framework;
    using SilkroadLauncher.Networking;
    using SilkroadLauncher.Networking.Handler;
    using SRO_BlackRougeLauncher.Networking.Packets;
    using SRO_BlackRougeLauncher.Networking.Packets.Gateway;

    public partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void LoadHandler()
        {

            PacketManager.AddPacket(0x2001, S2C.SERVER_GLOBAL_MODULE_IDENTIFICATION);
            PacketManager.AddPacket(0xA100, AutoUpdateHandler.SERVER_GATEWAY_PATCH_RESPONSE);
            PacketManager.AddPacket(0xA104, NoticeHandler.SERVER_GATEWAY_NOTICE_RESPONSE);
        }

        private void LoadSignature()
        {
            Global._ClientIP = ClientHelper.GetMediaIP(); 
            Global._ClientPort = ClientHelper.GetMediaPort(); 
            Global._ClientVer = ClientHelper.GetMediaVersion();

            lblIP.Text = Global._ClientIP;
            lblPort.Text = Global._ClientPort.ToString();
            lblVersion.Text = Global._ClientVer.ToString();
        }
        private void StartSession() => new Session().StartSession();
        private void Launcher_Load(object sender, EventArgs e)
        {
            Global.MainForm = this;
            LoadHandler();
            LoadSignature();
            StartSession();

        }
        private void startBtn_Click(object sender, EventArgs e)
        {
            var process = new Process
            {
                StartInfo =
              {
                  FileName = "sro_client.exe",
                  Arguments = "0 /22 0 0"
              }
            };
            process.Start();
            Thread.Sleep(1000);
            Environment.Exit(0);
        }
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void NoticeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = NoticeList.SelectedItem.ToString();
            var value = Global.NoticeDicitonary.Where(i => i.Key == key).Select(i => i.Value).First();

            Notice.Text = value.ToString();
        }
    }
}

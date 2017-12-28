namespace SRO_BlackRougeLauncher.Networking.Packets
{
    using SilkroadLauncher;
    using SilkroadLauncher.Networking;
    using SilkroadLauncher.SSA;
    static class S2C
    {

        public static void SERVER_GLOBAL_MODULE_IDENTIFICATION(Packet packet, Session client)
        {
            if (packet.ReadAscii() == "GatewayServer")
            {
                Packet response = new Packet(0x6100,true,false);
                response.WriteUInt8(22);
                response.WriteAscii("SR_Client");
                response.WriteUInt32(Global._ClientVer);
                client.SendPacket(response);
            }
        }
    }
}

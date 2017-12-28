namespace SRO_BlackRougeLauncher.Networking.Packets.Gateway
{
    using SilkroadLauncher.Networking;
    using SilkroadLauncher.SSA;
    /*
     * Current version does not support auto-update
     * it just send patch response and check it
     * will add it next version
    */
    class AutoUpdateHandler
    {
        public static void SERVER_GATEWAY_PATCH_RESPONSE(Packet packet, Session client)
        {
            byte result = packet.ReadUInt8();
            if (result == 2)
            {
                byte errorCode = packet.ReadUInt8();
                if (errorCode == 2)
                {
                    string tmpIp = packet.ReadAscii();
                    ushort tmpPort = packet.ReadUInt16();
                    uint version = packet.ReadUInt32();
                    byte fileFlag = packet.ReadUInt8(); // [0 = Done, 1 = NextFile]
                    while (fileFlag == 0x01)
                    {
                        uint fileId = packet.ReadUInt32();
                        string fileName = packet.ReadAscii();
                        string filePath = packet.ReadAscii();
                        uint fileLength = packet.ReadUInt32();
                        byte doPack = packet.ReadUInt8();
                        fileFlag = packet.ReadUInt8();
                    }
                    return;
                }

            }
            client.SendPacket(new Packet(0x6101, true));
            client.SendPacket(new Packet(0x6106, true));
            Packet response3 = new Packet(0x6104);  // notice request
            response3.WriteUInt8(22);
            client.SendPacket(response3);
        }
    }
}

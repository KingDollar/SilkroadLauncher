namespace SilkroadLauncher.Networking.Handler
{
    public class PacketStruct
    {
        public ushort Opcode { get; set; }
        public PacketHandler Handler { get; set; }
    }
}

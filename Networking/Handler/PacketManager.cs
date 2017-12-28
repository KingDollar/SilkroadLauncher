namespace SilkroadLauncher.Networking.Handler
{
    using System.Collections.Generic;
    using System.Linq;
    using SilkroadLauncher.SSA;

    public class PacketManager
    {
        private static List<PacketStruct> _handlers = new List<PacketStruct>();

        public static void AddPacket(ushort opcode, PacketHandler handler)
        {
            if (!_handlers.Any(i => i.Opcode == opcode && i.Handler == handler))
                _handlers.Add(new PacketStruct()
                {
                    Opcode = opcode,
                    Handler = handler
                });
        }

        public static void ExecutePacket(List<Packet> packets, Session client)
        {
            packets.ForEach(p =>
            {
                if (_handlers.Any(i => i.Opcode == p.Opcode))
                {
                    List<PacketStruct> handlers = _handlers.Where(i => i.Opcode == p.Opcode).ToList();
                    handlers.ForEach(h =>
                    {
                        h.Handler(p, client);
                    });
                }
            });
        }
    }
}

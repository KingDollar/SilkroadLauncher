﻿namespace SilkroadLauncher.Framework
{
    using System;
    using System.Windows.Forms;
    using System.IO;
    using System.Text;
    using SilkroadLauncher.Client;
    using SilkroadLauncher.SSA;

    static class ClientHelper
    {
        static Reader pk2read = new Reader($@"{Global.path}\Media.pk2", Global.MediaPassword);
        public static string GetMediaIP()
        {
            string ip = null;
            try
            {
                using (Stream fileStream = pk2read.GetFileStream("DIVISIONINFO.txt"))
                {
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        int length = (int)reader.BaseStream.Length;
                        reader.BaseStream.Seek(0, SeekOrigin.Begin);
                        byte ContentID = reader.ReadByte();
                        byte divisionCount = reader.ReadByte();
                        for (int i = 0; i < divisionCount; i++)
                        {
                            uint DivisionNameLength = reader.ReadUInt32();
                            byte[] bytes = reader.ReadBytes(Convert.ToInt32(DivisionNameLength));
                            string DivisionName = Encoding.GetEncoding(1252).GetString(bytes);
                            byte nullTerminator = reader.ReadByte();
                            byte gatewayCount = reader.ReadByte();
                            for (int i2 = 0; i2 < gatewayCount; i2++)
                            {
                                uint IPLength = reader.ReadUInt32();
                                byte[] IPbytes = reader.ReadBytes(Convert.ToInt32(IPLength));
                                string IP = Encoding.GetEncoding(1252).GetString(IPbytes);
                                return IP;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            return ip;
        }

        public static int GetMediaPort()
        {
            int port = 0;
            using (Stream fileStream = pk2read.GetFileStream("GATEPORT.txt"))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    int length = (int)reader.BaseStream.Length;
                    int endpos = 5;
                    int count = 0;
                    int i = 0;
                    byte[] newByteArray = new byte[length - 1];
                    while (count < endpos)
                    {

                        byte currentByte = reader.ReadByte();
                        newByteArray[i++] = currentByte;
                        count++;
                    }

                    string result = System.Text.Encoding.UTF8.GetString(newByteArray);
                    port = Convert.ToInt32(result);
                }
            }
            return port;
        }
        static int GetVersionFromSvt(byte[] svtContents)
        {
            byte[] bfKey = ASCIIEncoding.ASCII.GetBytes("SILKROAD");
            Blowfish bf = new Blowfish();
            bf.Initialize(bfKey);

            byte[] toDecode = new byte[8];
            Buffer.BlockCopy(svtContents, 4, toDecode, 0, 8);

            byte[] decoded = bf.Decode(toDecode);
            string verStr = ASCIIEncoding.ASCII.GetString(decoded);

            int firstZeroTermAt = verStr.IndexOf('\0');
            verStr = verStr.Remove(firstZeroTermAt, verStr.Length - firstZeroTermAt);
            byte[] verStrBytes = ASCIIEncoding.ASCII.GetBytes(verStr);
            return int.Parse(verStr);
        }
        public static int GetMediaVersion()
        {
            int version = 0;
            using (Stream fileStream = pk2read.GetFileStream("SV.T"))
            {
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    int length = (int)reader.BaseStream.Length;

                    byte[] buffer = reader.ReadBytes(length);

                    version = GetVersionFromSvt(buffer);
                }
            }

            return version;
        }
    }
}

using Server.Ygy.Game.Pb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Assets.Script.InterNet
{
    public static class CodingTool
    {
        //解决tcp运输过程的粘包拆包问题，通过发送消息头+消息尾的方法来解决， 消息头==消息尾的长度；
        /// <summary>
        /// 将字节数据进行封装成一个数据包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] EncodingPacket(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(data.Length);
                    bw.Write(data);
                    byte[] packet = new byte[ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, packet, 0, (int)ms.Length);
                    return packet;
                }
            }
        }
        /// <summary>
        /// 解包
        /// </summary>
        /// <param name="dataCache"></param>
        /// <returns></returns>
        public static byte[] DecodingPacket(List<byte> dataCache)
        {
            if (dataCache.Count == 0)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream(dataCache.ToArray()))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int length = br.ReadInt32();
                    if (length > (ms.Length - ms.Position))
                    {
                        return null;
                    }
                    byte[] data = br.ReadBytes(length);
                    dataCache.Clear();
                    dataCache.AddRange(br.ReadBytes((int)(ms.Length - ms.Position)));
                    return data;
                }
            }
        }
        //将消息体序列化成字节数组
        public static byte[] EncodingMessage(SocketMessage msg)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    bw.Write(msg.opCode);
                    bw.Write(msg.subCode);
                    if (msg.value != null)
                    {
                        //使用binaryFormatter
                        //BinaryFormatter bf = new BinaryFormatter();
                        //bf.Serialize(ms, msg.value);

                        // 使用PBSerial
                        PBCodingTool.PBSerialize((MSGID)msg.opCode, ms, msg.value);

                        //使用json
                        //string jsonStr = JsonConvert.SerializeObject(msg.value);
                        //bw.Write(jsonStr);
                    }
                    byte[] data = new byte[ms.Length];
                    Buffer.BlockCopy(ms.GetBuffer(), 0, data, 0, data.Length);
                    return data;
                }
            }
        }
        //将字节数组反序列化为消息
        public static SocketMessage DecodingMessage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                using (BinaryReader br = new BinaryReader(ms))
                {
                    int opcode = br.ReadInt32();
                    int subcode = br.ReadInt32();
                    object value = null;
                    if (ms.Length > ms.Position)
                    {
                        //使用binaryFormatter
                        //BinaryFormatter bf = new BinaryFormatter();
                        //value = bf.Deserialize(ms);

                        // 使用pbserial
                        value = PBCodingTool.PBDeSerialize((MSGID)opcode, ms);

                        //使用json
                        //string jsonString = br.ReadString();
                        //value = JsonConvert.DeserializeObject(jsonString);
                    }
                    SocketMessage msg = new SocketMessage(opcode, subcode, value);
                    return msg;
                }
            }
        }
    }
}

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JangadaWinClient.Network
{
    public class NetworkMessage
    {
        #region Static Methods

        public static NetworkMessage Concat(NetworkMessage message1, NetworkMessage message2)
        {
            NetworkMessage concat = new NetworkMessage();
            Array.Copy(message1.buffer, 6, concat.buffer, 0, message1.length - 6);
            Array.Copy(message2.buffer, 6, concat.buffer, message1.length, message2.length - 6);
            concat.length = message1.length + message2.length - 6;
            return concat;
        }

        #endregion

        #region Instance Variables

        private byte[] buffer;
        private int position, length, bufferSize = 1105920;

        #endregion

        #region Properties

        public int Length
        {
            get { return length; }
            set { length = value; }
        }


        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public int BufferSize
        {
            get { return bufferSize; }
        }

        public byte[] GetPacket()
        {
            byte[] t = new byte[length - 8];
            Array.Copy(buffer, 8, t, 0, length - 8);
            return t;
        }

        #endregion

        #region Constructors
        public NetworkMessage(byte[] bytesToRead)
        {
            buffer = bytesToRead;
        }


        public NetworkMessage()
        {
            Reset();
        }

        public NetworkMessage(int startingIndex)
        {
            Reset(startingIndex);
        }

        public void Reset(int startingIndex)
        {
            buffer = new byte[bufferSize];
            length = startingIndex;
            position = startingIndex;
        }

        public void Reset()
        {
            Reset(4);
        }

        #endregion

        #region Get

        public byte GetByte()
        {
            if (position + 1 > length)
                throw new IndexOutOfRangeException("NetworkMessage GetByte() out of range.");

            return buffer[position++];
        }

        public byte[] GetBytes(int count)
        {
            if (position + count > length)
                throw new IndexOutOfRangeException("NetworkMessage GetBytes() out of range.");

            byte[] t = new byte[count];
            Array.Copy(buffer, position, t, 0, count);
            position += count;
            return t;
        }

        public string GetString()
        {
            int len = (int)GetUInt16();
            string t = System.Text.ASCIIEncoding.Default.GetString(buffer, position, len);
            position += len;
            return t;
        }

        public ushort GetUInt16()
        {
            return BitConverter.ToUInt16(GetBytes(2), 0);
        }

        public uint GetUInt32()
        {
            return BitConverter.ToUInt32(GetBytes(4), 0);
        }

        public Guid GetGuid()
        {
            return new Guid(GetBytes(16));
        }

        private ushort GetPacketHeader()
        {
            return BitConverter.ToUInt16(buffer, 0);
        }

        public float GetDouble()
        {
            return (GetUInt32() / 1000);
        }

        public Vector3 GetPosition()
        {
            return new Vector3(GetDouble(), GetDouble(), GetDouble());
        }

        public Quaternion GetQuaternion()
        {
            return new Quaternion(GetDouble(), GetDouble(), GetDouble(), GetDouble());
        }

        #endregion

        #region Add

        public void AddByte(byte value)
        {
            if (1 + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");

            AddBytes(new byte[] { value });
        }

        public void AddBytes(byte[] value)
        {
            if (value.Length + length > bufferSize)
                throw new Exception("NetworkMessage buffer is full.");

            Array.Copy(value, 0, buffer, position, value.Length);
            position += value.Length;

            if (position > length)
                length = position;
        }

        public void AddString(string value)
        {
            AddUInt16((ushort)value.Length);
            AddBytes(System.Text.ASCIIEncoding.Default.GetBytes(value));
        }

        public void AddUInt16(ushort value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }

        public void AddUInt32(uint value)
        {
            AddBytes(BitConverter.GetBytes(value));
        }

        public void AddGuid(Guid value)
        {
            AddBytes(value.ToByteArray());
        }

        public void AddPaddingBytes(int count)
        {
            position += count;

            if (position > length)
                length = position;
        }


        public void AddDouble(double value)
        {
            uint iValue = (uint)(value * 1000);
            AddUInt32(iValue);
        }

        public void AddPosition(Vector3 pos)
        {
            AddDouble(pos.X);
            AddDouble(pos.Y);
            AddDouble(pos.Z);
        }

        public void AddQuaternion(Quaternion rot)
        {
            AddDouble(rot.X);
            AddDouble(rot.Y);
            AddDouble(rot.Z);
            AddDouble(rot.W);
        }

        #endregion

        #region Peek

        public byte PeekByte()
        {
            return buffer[position];
        }

        public byte[] PeekBytes(int count)
        {
            byte[] t = new byte[count];
            Array.Copy(buffer, position, t, 0, count);
            return t;
        }

        public ushort PeekUInt16()
        {
            return BitConverter.ToUInt16(PeekBytes(2), 0);
        }

        public uint PeekUInt32()
        {
            return BitConverter.ToUInt32(PeekBytes(4), 0);
        }

        public string PeekString()
        {
            int len = (int)PeekUInt16();
            return System.Text.ASCIIEncoding.ASCII.GetString(PeekBytes(len + 2), 2, len);
        }

        #endregion

        #region Replace

        public void ReplaceBytes(int index, byte[] value)
        {
            if (length - index >= value.Length)
                Array.Copy(value, 0, buffer, index, value.Length);
        }

        #endregion

        #region Skip

        public void SkipBytes(int count)
        {
            if (position + count > length)
                throw new IndexOutOfRangeException("NetworkMessage SkipBytes() out of range.");
            position += count;
        }

        #endregion

        #region Prepare

        private void InsertIdentifier()
        {
            //byte[] id = { 0x01, 0x02 };
            //Array.Copy(id, 0, buffer, 0, 2);
        }

        private void InsertTotalLength()
        {
            try
            {
                byte[] len = new byte[4];
                len = BitConverter.GetBytes((int)(length - 4));
                Array.Copy(len, 0, buffer, 0, 4);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool PrepareToSendWithoutEncryption()
        {
            InsertIdentifier();
            InsertTotalLength();

            return true;
        }

        public bool PrepareToSend()
        {
            InsertIdentifier();
            InsertTotalLength();

            return true;
        }

        public bool PrepareToRead()
        {
            position = 4;
            return true;
        }

        #endregion
    }
}

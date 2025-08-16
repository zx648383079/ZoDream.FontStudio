﻿using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace ZoDream.Shared.IO
{
    public class EndianReader : BinaryReader
    {
        internal const int BufferSize = 4096;

        private bool isBigEndian;

        public EndianType EndianType {
            get {
                if (!isBigEndian)
                {
                    return EndianType.LittleEndian;
                }

                return EndianType.BigEndian;
            }
            set {
                isBigEndian = value == EndianType.BigEndian;
            }
        }


        public long Position 
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public long Length => BaseStream.Length;
        /// <summary>
        /// 剩余字节长度
        /// </summary>
        public long RemainingLength => checked(BaseStream.Length - BaseStream.Position);


        public EndianReader(Stream stream, EndianType endian, bool leaveOpen = true)
            : base(stream, Encoding.UTF8, leaveOpen: leaveOpen)
        {
            EndianType = endian;
        }

        ~EndianReader()
        {
            Dispose(disposing: false);
        }

        public override char ReadChar()
        {
            return (char)ReadUInt16();
        }

        public override short ReadInt16()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadInt16BigEndian(base.ReadBytes(2));
            }

            return base.ReadInt16();
        }

        public override ushort ReadUInt16()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadUInt16BigEndian(base.ReadBytes(2));
            }

            return base.ReadUInt16();
        }

        public override int ReadInt32()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadInt32BigEndian(base.ReadBytes(4));
            }

            return base.ReadInt32();
        }

        public override uint ReadUInt32()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadUInt32BigEndian(base.ReadBytes(4));
            }

            return base.ReadUInt32();
        }

        public override long ReadInt64()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadInt64BigEndian(base.ReadBytes(8));
            }

            return base.ReadInt64();
        }

        public override ulong ReadUInt64()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadUInt64BigEndian(base.ReadBytes(8));
            }

            return base.ReadUInt64();
        }

        public override Half ReadHalf()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadHalfBigEndian(base.ReadBytes(2));
            }

            return base.ReadHalf();
        }

        public override float ReadSingle()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadSingleBigEndian(base.ReadBytes(4));
            }

            return base.ReadSingle();
        }

        public override double ReadDouble()
        {
            if (isBigEndian)
            {
                return BinaryPrimitives.ReadDoubleBigEndian(base.ReadBytes(8));
            }

            return base.ReadDouble();
        }

        public override decimal ReadDecimal()
        {
            if (isBigEndian)
            {
                throw new NotSupportedException("");
            }

            return base.ReadDecimal();
        }

        //
        // 摘要:
        //     Read a UTF8 string
        //
        // 言论：
        //     First, a 32-bit integer length is read. It signifies the length of a byte buffer,
        //     which is read next. A string is then parsed from the buffer with System.Text.Encoding.UTF8.
        public override string ReadString()
        {
            int length = ReadInt32();
            return ReadString(length);
        }

        public string Read7BitEncodedString()
        {
            return base.ReadString();
        }
        /// <summary>
        /// >= 0x80 则 有多个字节
        /// </summary>
        /// <returns></returns>
        public uint Read7BitEncodedUInt()
        {
            var val = (uint)ReadByte();
            if (val < 0x80)
            {
                return val;
            }
            var bitShift = 0;
            val &= 0x7f;
            while (true)
            {
                var b = ReadByte();
                bitShift += 7;
                val |= (uint)(b & 0x7f) << bitShift;
                if (b < 0x80)
                {
                    break;
                }
            }
            return val;
        }
        /// <summary>
        /// >= 0x80 则 有多个字节
        /// </summary>
        /// <returns></returns>
        public ulong Read7BitEncodedUInt64()
        {
            var val = (ulong)ReadByte();
            if (val < 0x80)
            {
                return val;
            }
            var bitShift = 0;
            val &= 0x7f;
            while (true)
            {
                var b = ReadByte();
                bitShift += 7;
                val |= (ulong)(b & 0x7f) << bitShift;
                if (b < 0x80)
                {
                    break;
                }
            }
            return val;
        }

        public string ReadString(int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "Length cannot be negative.");
            }

            if (length == 0)
            {
                return "";
            }

            if (length > RemainingLength)
            {
                throw new EndOfStreamException($"Can't read {length}-byte string because there are only {RemainingLength} bytes left in the stream");
            }

            byte[]? array;
            Span<byte> span;
            if (length > BufferSize)
            {
                array = ArrayPool<byte>.Shared.Rent(length);
                span = array.AsSpan(0, length);
            }
            else
            {
                array = null;
                span = new Span<byte>(new byte[length]);
            }

            try
            {
                ReadExactly(span);
                return Encoding.UTF8.GetString(span);
            }
            finally
            {
                if (array != null)
                {
                    ArrayPool<byte>.Shared.Return(array);
                }
            }
        }

        public void ReadExactly(Span<byte> buffer)
        {
            Span<byte> buffer2 = buffer;
            do
            {
                int num = Read(buffer2);
                if (num == 0)
                {
                    throw new EndOfStreamException($"End of stream. Expected to read {buffer.Length} bytes, but only read {checked(buffer.Length - buffer2.Length)} bytes.");
                }

                buffer2 = buffer2[num..];
            }
            while (buffer2.Length > 0);
        }

        //
        // 摘要:
        //     Read C like UTF8 format zero terminated string
        //
        // 返回结果:
        //     Read string
        public string ReadStringZeroTerm()
        {
            if (TryReadStringZeroTerm(BufferSize, out var result))
            {
                return result;
            }

            throw new Exception("Can't find end of string");
        }

        public string ReadStringZeroTerm(int maxLength)
        {
            if (TryReadStringZeroTerm(maxLength, out var result))
            {
                return result;
            }

            throw new Exception("Can't find end of string");
        }

        //
        // 摘要:
        //     Read C like UTF8 format zero terminated string
        //
        // 参数:
        //   maxLength:
        //     Max allowed character count to read
        //
        //   result:
        //     Read string if found
        //
        // 返回结果:
        //     Whether zero term has been found
        public bool TryReadStringZeroTerm(int maxLength, [NotNullWhen(true)] out string? result)
        {
            Span<byte> span = stackalloc byte[maxLength];
            for (int i = 0; i < maxLength; i = checked(i + 1))
            {
                var b = BaseStream.ReadByte();
                if (b <= 0)
                {
                    result = Encoding.UTF8.GetString(span[..i]);
                    return true;
                }
                span[i] = (byte)b;
            }
            result = null;
            return false;
        }


        public void AlignStream(int alignment)
        {
            var pos = Position;
            var mod = pos % alignment;
            if (mod != 0)
            {
                BaseStream.Seek(alignment - mod, SeekOrigin.Current);
            }
        }

        public T[] ReadArray<T>(Func<T> cb)
        {
            return ReadArray(ReadInt32(), cb);
        }

        public T[] ReadArray<T>(int count, Func<T> cb)
        {
            if (count <= 0)
            {
                return [];
            }
            var items = new T[count];
            for (int i = 0; i < count; i++)
            {
                items[i] = cb.Invoke();
            }
            return items;
        }
    }
}

﻿using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace ZoDream.Shared.IO
{
    public class EndianWriter : BinaryWriter
    {
        public EndianType EndianType { get; }

        private bool IsLittleEndian => EndianType == EndianType.LittleEndian;

        public bool IsAlignArray { get; }

        public EndianWriter(Stream stream, EndianType endian)
            : this(stream, endian, alignArray: false)
        {
        }

        protected EndianWriter(Stream stream, EndianType endian, bool alignArray)
            : base(stream, Encoding.UTF8, leaveOpen: true)
        {
            EndianType = endian;
            IsAlignArray = alignArray;
        }

        ~EndianWriter()
        {
            Dispose(disposing: false);
        }

        public sealed override void Write(byte value)
        {
            base.Write(value);
        }

        public sealed override void Write(byte[] buffer)
        {
            base.Write(buffer);
        }

        public sealed override void Write(byte[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
        }

        public sealed override void Write(ReadOnlySpan<byte> buffer)
        {
            OutStream.Write(buffer);
        }

        public sealed override void Write(bool value)
        {
            base.Write(value);
        }

        public override void Write(short value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(ushort value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(int value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(uint value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(long value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(ulong value)
        {
            base.Write(IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value));
        }

        public override void Write(float value)
        {
            Write(BitConverter.SingleToUInt32Bits(value));
        }

        public override void Write(double value)
        {
            Write(BitConverter.DoubleToUInt64Bits(value));
        }

        public override void Write(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            Write(bytes.Length);
            Write(bytes);
        }


        public void WriteStringZeroTerm(string value)
        {
            Write(Encoding.UTF8.GetBytes(value));
            Write((byte)0);
        }

        public void Write7BitEncodedString(string value)
        {
            base.Write(value);
        }

        public void Write7BitEncodedUInt(uint value)
        {
            for (; value > 127; value >>= 7)
            {
                Write((byte)(value | 0xFFFFFF80u));
            }
            Write((byte)value);
        }

        public void Write7BitEncodedUInt64(ulong value)
        {
            for (; value > 127; value >>= 7)
            {
                Write((byte)((int)value | -128));
            }
            Write((byte)value);
        }
    }
}

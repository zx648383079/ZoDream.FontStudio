using System;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class DigitalSignatureConverter : TypefaceConverter<DigitalSignatureTable>
    {
        public override DigitalSignatureTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var beginAt = reader.Position;
            var res = new DigitalSignatureTable();
            var version = reader.ReadUInt32();
            var numSignatures = reader.ReadUInt16();
            var flags = reader.ReadUInt16();
            var signatureRecords = reader.ReadArray(numSignatures, 
                () => new SignatureRecord(reader.ReadUInt32(), reader.ReadUInt32(), reader.ReadUInt32()));

            for (int i = 0; i < signatureRecords.Length; i++)
            {
                var item = signatureRecords[i];
                if (item.Format == 1)
                {
                    reader.ReadUInt16();
                    reader.ReadUInt16();
                    var length = reader.ReadUInt32();
                    // reader.ReadBytes(length);
                }
            }
            return res;
        }

        public override void Write(EndianWriter writer, DigitalSignatureTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

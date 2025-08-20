using ZoDream.Shared.Font;

namespace ZoDream.Shared.OpenType.Tables
{
    public class DigitalSignatureTable : ITypefaceTable
    {
        public const string TableName = "DSIG";
        public string Name => TableName;
    }

    public readonly struct SignatureRecord(uint format, uint length, uint offset)
    {
        public uint Format => Format;

        public uint Length => length;

        public uint SignatureBlockOffset => offset;
    }
}

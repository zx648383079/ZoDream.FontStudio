using System.IO;
using ZoDream.Shared.IO;

namespace ZoDream.Shared.OpenType
{
    public class OTFReader(EndianReader reader) : TTFReader(reader)
    {
        public OTFReader(Stream input) : this(new EndianReader(input, EndianType.BigEndian, false))
        {

        }
    }
}

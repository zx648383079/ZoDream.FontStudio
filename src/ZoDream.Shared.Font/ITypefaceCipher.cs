using System.IO;

namespace ZoDream.Shared.Font
{
    public interface ITypefaceCipher
    {

        public Stream Encrypt(Stream input);
        public Stream Decrypt(Stream input, ITypefaceTableEntry entry);
    }
}
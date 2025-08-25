using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using ZoDream.Shared.Font;
using ZoDream.Shared.ImageEditor;
using ZoDream.Shared.OpenType;
using ZoDream.Shared.WebType;

namespace ZoDream.FontStudio.ViewModels
{
    public partial class WorkspaceViewModel
    {
        private void TapImportFolder()
        {
            
        }

        private void TapImportFile()
        {
        }

        private void TapImport()
        {
        }

        public async Task DragFileAsync(IEnumerable<IStorageItem> items)
        {
            foreach (var item in items)
            {
                if (item is not IStorageFile file)
                {
                    continue;
                }
                using var reader = await OpenRead(file);
                if (reader is null)
                {
                    continue;
                }
                var data = reader.Read();
            }
        }


        public static bool IsSupport(string extension)
        {
            return extension is ".woff" or ".woff2" or ".ttf" or ".otf" or ".ttc";
        }

        public async Task<ITypefaceReader?> OpenRead(IStorageFile file)
        {
            if (!IsSupport(file.FileType))
            {
                return null;
            }
            var input = await file.OpenStreamForReadAsync();
            var buffer = new byte[4];
            input.ReadExactly(buffer);
            input.Seek(0, SeekOrigin.Begin);
            if (buffer.SequenceEqual(WOFF2Reader.Signature))
            {
                return new WOFF2Reader(input);
            }
            if (buffer.SequenceEqual(WOFFReader.Signature))
            {
                return new WOFFReader(input);
            }
            if (buffer.SequenceEqual(TTCReader.Signature))
            {
                return new TTCReader(input);
            }
            if (file.FileType is ".ttf")
            {
                return new TTFReader(input);
            }
            return new OTFReader(input);
        }
    }
}

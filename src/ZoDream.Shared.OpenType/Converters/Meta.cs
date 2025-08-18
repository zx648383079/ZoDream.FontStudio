using System;
using System.Diagnostics;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public class MetaConverter : TypefaceConverter<MetaTable>
    {
        public override MetaTable? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = new MetaTable();

            long tableStartsAt = reader.BaseStream.Position;//***

            uint version = reader.ReadUInt32();
            uint flags = reader.ReadUInt32();
            uint reserved = reader.ReadUInt32();
            uint dataMapsCount = reader.ReadUInt32();
#if DEBUG
            if (version != 1 || flags != 0)
            {
                throw new NotSupportedException();
            }
#endif 

            var dataMaps = new DataMapRecord[dataMapsCount];
            for (int i = 0; i < dataMaps.Length; ++i)
            {
                dataMaps[i] = new DataMapRecord(reader.ReadString(4),
                    reader.ReadUInt32(),
                    reader.ReadUInt32());
            }
            for (int i = 0; i < dataMaps.Length; ++i)
            {
                DataMapRecord record = dataMaps[i];

                switch (record.Tag)
                {
#if DEBUG
                    default:
                        Debug.WriteLine("openfont-meta: unknown tag:" + record.Tag);
                        break;
#endif
                    case "apple": //Reserved — used by Apple.
                    case "bild"://Reserved — used by Apple.
                        break;
                    case "dlng":
                        {
                            //The values for 'dlng' and 'slng' are comprised of a series of comma-separated ScriptLangTags,
                            //which are described in detail below.
                            //Spaces may follow the comma delimiters and are ignored.
                            //Each ScriptLangTag identifies a language or script. 

                            //A list of tags is interpreted to imply that all of the languages or scripts are included.

                            //dlng 	Design languages Text, 
                            //using only Basic Latin (ASCII) characters. 
                            //Indicates languages and/or scripts for the user audiences that the font was primarily designed for.

                            //Only one instance is used.

                            //dlng 	Design languages Text, 
                            //The 'dlng' value is used to indicate the languages or scripts of the primary user audiences for which the font was designed.

                            //This value may be useful for selecting default font formatting based on content language,                             
                            //for presenting filtered font options based on user language preferences, 
                            //or similar applications involving the language or script of content or user settings.

                            if (res.DesignLanguageTags == null)
                            {
                                //Only one instance is used.
                                reader.BaseStream.Position = tableStartsAt + record.DataOffset;
                                res.DesignLanguageTags = ReadCommaSepData(reader.ReadString((int)record.DataLength));
                            }
                        }
                        break;
                    case "slng":
                        {
                            //slng Supported languages Text, using only Basic Latin(ASCII) characters.
                            //Indicates languages and / or scripts that the font is declared to be capable of supporting.

                            //Only one instance is used. 

                            //slng Supported languages Text
                            //The 'slng' value is used to declare languages or scripts that the font is capable of supported. 

                            //This value may be useful for font fallback mechanisms or other applications involving the language or 
                            //script of content or user settings.
                            //Note: Implementations that use 'slng' values in a font may choose to ignore Unicode - range bits set in the OS/ 2 table.

                            if (res.SupportedLanguageTags == null)
                            {   //Only one instance is used. 
                                reader.BaseStream.Position = tableStartsAt + record.DataOffset;
                                res.SupportedLanguageTags = ReadCommaSepData(reader.ReadString((int)record.DataLength));
                            }
                        }
                        break;
                }
            }
            return res;
        }

        static string[] ReadCommaSepData(string text)
        {
            string[] tags = text.Split(',');
            for (int i = 0; i < tags.Length; ++i)
            {
                tags[i] = tags[i].Trim();

#if DEBUG
                if (tags[i].Contains("-"))
                {

                }
#endif

            }
            return tags;
        }

        public override void Write(EndianWriter writer, MetaTable data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}

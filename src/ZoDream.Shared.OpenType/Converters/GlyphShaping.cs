using System;
using System.IO;
using ZoDream.Shared.Font;
using ZoDream.Shared.IO;
using ZoDream.Shared.OpenType.Tables;

namespace ZoDream.Shared.OpenType.Converters
{
    public abstract class GlyphShapingConverter<T> : TypefaceConverter<T>
        where T : GlyphShapingTable
    {
        public override T? Read(EndianReader reader, Type objectType, ITypefaceSerializer serializer)
        {
            var res = (T)Activator.CreateInstance(objectType);
            long tableStartAt = reader.BaseStream.Position;

            res.MajorVersion = reader.ReadUInt16();
            res.MinorVersion = reader.ReadUInt16();

            ushort scriptListOffset = reader.ReadUInt16(); // from beginning of table
            ushort featureListOffset = reader.ReadUInt16(); // from beginning of table
            ushort lookupListOffset = reader.ReadUInt16(); // from beginning of table
            uint featureVariations = (res.MinorVersion == 1) ? reader.ReadUInt32() : 0; // from beginning of table

            //-----------------------
            //1. scriptlist
            res.ScriptList = ReadScriptList(reader, tableStartAt + scriptListOffset);

            if (res.OnlyScriptList)
            {
                return res; //for preview script-list and feature list only
            }

            //-----------------------
            //2. feature list

            res.FeatureList = ReadFeatureList(reader, tableStartAt + featureListOffset);

            //3. lookup list
            var lookupListBeginAt = tableStartAt + lookupListOffset;
            reader.BaseStream.Seek(lookupListBeginAt, SeekOrigin.Begin);
            ushort lookupCount = reader.ReadUInt16();
            ushort[] lookupTableOffsets = reader.ReadArray(lookupCount, reader.ReadUInt16);

            //----------------------------------------------
            //load each sub table

            foreach (ushort lookupTableOffset in lookupTableOffsets)
            {
                long lookupTablePos = lookupListBeginAt + lookupTableOffset;
                reader.BaseStream.Seek(lookupTablePos, SeekOrigin.Begin);

                ushort lookupType = reader.ReadUInt16(); //Each Lookup table may contain only one type of information (LookupType)
                ushort lookupFlags = reader.ReadUInt16();
                ushort subTableCount = reader.ReadUInt16();

                //Each LookupType is defined with one or more subtables, and each subtable definition provides a different representation format
                ushort[] subTableOffsets = reader.ReadArray(subTableCount, reader.ReadUInt16);

                ushort markFilteringSet =
                    ((lookupFlags & 0x0010) == 0x0010) ? reader.ReadUInt16() : (ushort)0;

                ReadLookup(reader,
                    res,
                        lookupTablePos,
                        lookupType,
                        lookupFlags,
                        subTableOffsets, //Array of offsets to SubTables-from beginning of Lookup table
                        markFilteringSet);
            }

            //-----------------------
            //4. feature variations
            if (featureVariations > 0)
            {
                ReadFeatureVariations(reader, res, tableStartAt + featureVariations);
            }
            return res;
        }

        private FeatureList ReadFeatureList(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //
            FeatureList featureList = new FeatureList();
            ushort featureCount = reader.ReadUInt16();
            var entities = reader.ReadRecord(featureCount);
            //read each feature table
            featureList.FeatureTables = new FeatureTable[featureCount];
            for (int i = 0; i < featureCount; ++i)
            {
                var frecord = entities[i];
                featureList.FeatureTables[i] = ReadFeature(reader, beginAt + frecord.Offset);
                featureList.FeatureTables[i].Tag = frecord.Tag;
            }
            return featureList;
        }

        private FeatureTable ReadFeature(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            ushort featureParams = reader.ReadUInt16();
            ushort lookupCount = reader.ReadUInt16();

            var featureTable = new FeatureTable();
            featureTable.LookupListIndices = reader.ReadArray(lookupCount, reader.ReadUInt16);
            return featureTable;
        }

        private ScriptList ReadScriptList(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);

            ushort scriptCount = reader.ReadUInt16();
            var scriptList = new ScriptList();

            // Read records (tags and table offsets)
            var entries = reader.ReadRecord(scriptCount);

            // Read each table and add it to the dictionary
            for (int i = 0; i < scriptCount; ++i)
            {
                var scriptTable = ReadScript(reader, beginAt + entries[i].Offset);
                scriptTable.Name = entries[i].Tag;
                scriptList.Add(entries[i].Tag, scriptTable);
            }

            return scriptList;
        }

        private ScriptTable ReadScript(EndianReader reader, long beginAt)
        {
            reader.BaseStream.Seek(beginAt, SeekOrigin.Begin);
            //---------------
            //Script table
            //Type 	        Name 	                      Description
            //Offset16 	    defaultLangSys 	              Offset to DefaultLangSys table-from beginning of Script table-may be NULL
            //uint16 	    langSysCount 	              Number of LangSysRecords for this script-excluding the DefaultLangSys
            //LangSysRecord langSysRecords[langSysCount]  Array of LangSysRecords-listed alphabetically by LangSysTag

            //---------------
            var scriptTable = new ScriptTable();
            ushort defaultLangSysOffset = reader.ReadUInt16();
            ushort langSysCount = reader.ReadUInt16();
            scriptTable.langSysTables = new LangSysTable[langSysCount];
            var entries = reader.ReadRecord(langSysCount);

            //-----------
            if (defaultLangSysOffset > 0)
            {
                scriptTable.defaultLang = new LangSysTable(string.Empty, defaultLangSysOffset);
                reader.BaseStream.Seek(beginAt + defaultLangSysOffset, SeekOrigin.Begin);
                ushort lookupOrder = reader.ReadUInt16();//reserve
                scriptTable.defaultLang.RequiredFeatureIndex = reader.ReadUInt16();
                ushort featureIndexCount = reader.ReadUInt16();
                scriptTable.defaultLang.featureIndexList = reader.ReadArray(featureIndexCount, reader.ReadUInt16);
            }


            //-----------
            //read actual content of each table
            for (int i = 0; i < langSysCount; ++i)
            {
                var langSysTable = new LangSysTable(entries[i].Tag, entries[i].Offset);
                reader.BaseStream.Seek(beginAt + entries[i].Offset, SeekOrigin.Begin);
                ushort lookupOrder = reader.ReadUInt16();//reserve
                langSysTable.RequiredFeatureIndex = reader.ReadUInt16();
                ushort featureIndexCount = reader.ReadUInt16();
                langSysTable.featureIndexList = reader.ReadArray(featureIndexCount, reader.ReadUInt16);
                scriptTable.langSysTables[i] = langSysTable;
            }

            return scriptTable;
        }

        public override void Write(EndianWriter writer, T data, Type objectType, ITypefaceSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected abstract void ReadLookup(EndianReader reader, T instance, long lookupTablePos,
                                                ushort lookupType, ushort lookupFlags,
                                                ushort[] subTableOffsets, ushort markFilteringSet);

        protected abstract void ReadFeatureVariations(EndianReader reader, T instance, long featureVariationsBeginAt);


    }
}

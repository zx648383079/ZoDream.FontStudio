using System;
using System.Collections.Generic;

namespace ZoDream.Shared.Font
{
    public static class Extension
    {
        public static ITypefaceTableCollection ToCollection(this IEnumerable<ITypefaceTable> items)
        {
            if (items is ITypefaceTableCollection c)
            {
                return c;
            }
            var res = new TypefaceTableCollection();
            foreach (var item in items)
            {
                res.Add(item);
            }
            return res;
        }

        public static ITypefaceTableCollection ToCollection(this IEnumerable<ITypefaceTableEntry> items)
        {
            var res = new TypefaceTableCollection();
            foreach (var item in items)
            {
                res.Add(new UnreadTypefaceTable(item));
            }
            return res;
        }

        public static string FormatEnum<T>(T value) where T : Enum
        {
            var res = Enum.GetName(typeof(T), value);
            if (res is null)
            {
                return string.Empty;
            }
            for (int i = res.Length - 1; i > 0; i--)
            {
                if (res[i] is >= 'A' and <= 'Z')
                {
                    res = res.Insert(i, " ");
                }
            }
            return res;
        }

        public static T EnumParse<T>(string value) where T : Enum
        {
            value = value.Replace(" ", string.Empty);
            return Enum.TryParse(typeof(T), value, out var res) ? (T)res : default;
        }
    }
}

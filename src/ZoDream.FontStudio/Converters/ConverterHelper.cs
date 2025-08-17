using Microsoft.UI.Xaml;
using System;

namespace ZoDream.FontStudio.Converters
{
    public static class ConverterHelper
    {
        public static string Format(DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return "-";
            }
            return date.ToString("yyyy-MM-dd HH:mm");
        }


        public static string FormatSize(long size)
        {
            var len = size.ToString().Length;
            if (len < 4)
            {
                return $"{size}B";
            }
            if (len < 7)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d), 2) + "KB";
            }
            if (len < 10)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024), 2) + "MB";
            }
            if (len < 13)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024), 2) + "GB";
            }
            if (len < 16)
            {
                return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024), 2) + "TB";
            }
            return Math.Round(System.Convert.ToDouble(size / 1024d / 1024 / 1024 / 1024 / 1024), 2) + "PB";
        }


        public static string FormatHour(int value)
        {
            if (value <= 0)
            {
                return "00:00";
            }
            var m = value / 60;
            if (m >= 60)
            {
                return (m / 60).ToString("00") + ":"
                    + (m % 60).ToString("00") + ":" + (value % 60).ToString("00");
            }
            return m.ToString("00") + ":" + (value % 60).ToString("00");
        }

        public static Visibility VisibleIf(bool val)
        {
            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public static Visibility VisibleIf(string val)
        {
            return VisibleIf(!string.IsNullOrWhiteSpace(val));
        }

        public static Visibility VisibleIf(int val)
        {
            return VisibleIf(val > 0);
        }
        public static Visibility CollapsedIf(bool val)
        {
            return VisibleIf(!val);
        }

    }
}

﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Linq;

namespace ZoDream.FontStudio.Converters
{
    public class ToggleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var res = IsVisible(value, parameter);
            if (targetType == typeof(Visibility))
            {
                return res ? Visibility.Visible : Visibility.Collapsed;
            }
            if (targetType == typeof(ListViewSelectionMode))
            {
                return res ? ListViewSelectionMode.Extended : ListViewSelectionMode.Single;
            }
            if (targetType == typeof(SelectionMode))
            {
                return res ? SelectionMode.Extended : SelectionMode.Single;
            }
            return res;
        }

        private static bool IsVisible(object value, object parameter)
        {
            if (value is null)
            {
                return false;
            }
            if (parameter is null)
            {
                if (value is bool o)
                {
                    return o;
                }
                if (value is int i)
                {
                    return i > 0;
                }
                if (value.GetType().IsEnum)
                {
                    return System.Convert.ToInt32(value) > 0;
                }
                return string.IsNullOrWhiteSpace(value.ToString());
            }
            if (parameter is bool b)
            {
                return (bool)value == b;
            }
            var pStr = parameter.ToString();
            var vStr = value.GetType().IsEnum ? System.Convert.ToInt32(value).ToString() : value.ToString();
            if (pStr == vStr)
            {
                return true;
            }
            if (vStr is null || pStr is null)
            {
                return false;
            }
            var isRevert = false;
            if (pStr.StartsWith('^'))
            {
                isRevert = true;
                pStr = pStr[1..];
            }
            return pStr.Split('|').Contains(vStr) == !isRevert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}

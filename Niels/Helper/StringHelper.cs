﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Helper
{
    /// <summary>
    /// 文字列に対する処理をサポートします。
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// int型への変換ディクショナリ
        /// </summary>
        private static Dictionary<string, int> m_IntoConvertDictionary;

        /// <summary>
        /// double型への変換ディクショナリ
        /// </summary>
        private static Dictionary<string, double> m_DoubleConvertDictionary;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static StringHelper()
        {
            m_IntoConvertDictionary = new Dictionary<string, int>();
            m_DoubleConvertDictionary = new Dictionary<string, double>();
        }

        /// <summary>
        /// <para>文字列を指定した文字数で分割して返します。</para>
        /// </summary>
        /// <param name="cnt"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToListSplitCount(string str, int cnt)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(str) || cnt < 1 || str.Length <= cnt) { return new List<string>() { str }; }

            for (int i = 0; i <= str.Length - cnt; i += cnt)
            {
                list.Add(str.Substring(i, cnt));
            }
            return list;
        }

        /// <summary>
        /// <para>0埋めした文字列を取得する</para>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPadLeftZero(string str, int width)
        {
            return str.PadLeft(width, '0');
        }

        /// <summary>
        /// int型へ変換する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(string str)
        {
            if (m_IntoConvertDictionary.ContainsKey(str))
            {
                return m_IntoConvertDictionary[str];
            }
            else
            {
                int value = int.Parse(str);
                m_IntoConvertDictionary.Add(str, value);
                return value;
            }
        }

        /// <summary>
        /// double型へ変換する
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static double ToDouble(string str)
        {
            if (m_DoubleConvertDictionary.ContainsKey(str))
            {
                return m_DoubleConvertDictionary[str];
            }
            else
            {
                double value = double.Parse(str);
                m_DoubleConvertDictionary.Add(str, value);
                return value;
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Collections.Math
{
    public class SparseVector<T> : IEnumerable<T>
    {
        #region "定数"
        #endregion

        #region "メンバ変数"
        /// <summary>
        /// 疎ベクターの大半の成分を構成する要素
        /// </summary>
        private T m_SparseItem;

        /// <summary>
        /// 要素を構成するディクショナリ
        /// </summary>
        private Dictionary<int, T> m_ItemDictionary;

        /// <summary>
        /// 要素数
        /// </summary>
        private int m_Count;
        #endregion

        #region "コンストラクタ"
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"> 疎ベクターの大半の成分を構成する要素</param>
        public SparseVector(T sparseItem)
            : this(sparseItem, 0, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"></param>
        /// <param name="count"></param>
        public SparseVector(T sparseItem, int count)
            : this(sparseItem, count, null)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="sparseItem"></param>
        /// <param name="count"></param>
        /// <param name="itemDictionary"></param>
        public SparseVector(T sparseItem, int count, Dictionary<int, T> itemDictionary)
        {
            this.m_SparseItem = sparseItem;
            if (itemDictionary == null)
            {
                this.m_ItemDictionary = new Dictionary<int, T>();
            }
            else
            {
                this.m_ItemDictionary = itemDictionary;
            }
            this.m_Count = count;
        }
        #endregion

        #region "メソッド"
        #region "公開メソッド"
        /// <summary>
        /// シーケンス内の要素数を返します。
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return this.m_Count;
        }

        /// <summary>
        /// 末尾にオブジェクトを追加します。
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            if (!item.Equals(this.m_SparseItem))
            {
                this.m_ItemDictionary.Add(this.m_Count, item);
            }
            this.m_Count++;
        }

        /// <summary>
        /// インデックスと要素のペアをCSV化して返します
        /// </summary>
        /// <returns></returns>
        public string ToCsv()
        {
            string csv = string.Empty;
            foreach (KeyValuePair<int, T> keyValue in this.m_ItemDictionary)
            {
                if (csv != string.Empty) { csv += ","; }
                csv += keyValue.Key + "," + keyValue.Value;
            }
            return csv;
        }

        /// <summary>
        /// System.Collections.Generic.IEnumerable<T> から System.Collections.Generic.List<T>を作成します。
        /// </summary>
        /// <returns></returns>
        public List<T> ToList()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < this.Count(); i++)
            {
                if (this.m_ItemDictionary.ContainsKey(i))
                {
                    list.Add(this.m_ItemDictionary[i]);
                }
                else
                {
                    list.Add(this.m_SparseItem);
                }
            }
            return list;
        }

        /// <summary>
        /// 自分自身をコピーして新しいインスタンスを作成します。
        /// </summary>
        /// <returns></returns>
        public SparseVector<T> Clone()
        {
            return new SparseVector<T>(this.m_SparseItem, this.m_Count, new Dictionary<int, T>(this.m_ItemDictionary));
        }

        /// <summary>
        /// インデクサ
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public T this[int i]
        {
            set
            {
                if (this.m_Count <= i)
                {
                    throw new ApplicationException("存在しないインデックスです。");
                }
                else if (value.Equals(this.m_SparseItem) && this.m_ItemDictionary.ContainsKey(i))
                {
                    this.m_ItemDictionary.Remove(i);
                }
                this.m_ItemDictionary[i] = value;
            }
            get
            {
                if (this.m_Count <= i)
                {
                    throw new ApplicationException("存在しないインデックスです。");
                }
                return (this.m_ItemDictionary.ContainsKey(i) ? this.m_ItemDictionary[i] : this.m_SparseItem);
            }
        }

        /// <summary>
        /// 列挙子を取得します。
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < this.m_Count; i++)
            {
                yield return (this.m_ItemDictionary.ContainsKey(i) ? this.m_ItemDictionary[i] : this.m_SparseItem);
            }
        }

        /// <summary>
        /// 列挙子を取得します。
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return GetEnumerator(); }

        /// <summary>
        /// 疎な部分を除いた列挙子。
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, T> NoSparseKeyValues
        {
            get
            {
                return this.m_ItemDictionary;
            }
        }
        #endregion

        #region "内部メソッド"
        #endregion
        #endregion
    }
}

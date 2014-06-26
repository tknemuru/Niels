using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niels.Usi
{
    /// <summary>
    /// USIプロトコルで使用するコマンド
    /// </summary>
    public class UsiCommand
    {
        /// <summary>
        /// コマンド名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// パラメータ
        /// </summary>
        public string Parameters { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public UsiCommand(string name)
        {
            this.Name = name;
            this.Parameters = string.Empty;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        public UsiCommand(string name, string parameters)
        {
            this.Name = name;
            this.Parameters = parameters;
        }

        /// <summary>
        /// USIコマンドを解析します。
        /// </summary>
        public static UsiCommand Parse(string line)
        {
            if (line.Contains(Environment.NewLine))
            {
                throw new ArgumentException("改行が含まれた文字列は解析出来ません", "line");
            }

            int sp = line.IndexOf(' ');
            if (sp < 0)
            {
                return new UsiCommand(line);
            }
            else
            {
                return new UsiCommand(line.Substring(0, sp), line.Substring(sp + 1));
            }
        }

        /// <summary>
        /// 分割したパラメータを取得します。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSplitedParameters()
        {
            return this.Parameters.Split(' ');
        }
    }
}

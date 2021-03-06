﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;
using Niels.Helper;
using Niels.Boards;
using Niels.Players;
using Niels.Notation;

namespace Niels.Usi
{
    /// <summary>
    /// USIプロトコルに従ったコマンドの送信機能を提供します。
    /// </summary>
    public class UsiCommandSender
    {
        /// <summary>
        /// 送信ログ
        /// </summary>
        public List<string> SendLog { get; private set; }

        /// <summary>
        /// Sender
        /// </summary>
        private TextWriter Sender { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UsiCommandSender()
        {
            this.Sender = Console.Out;
            this.SendLog = new List<string>();
        }

        /// <summary>
        /// 送信ログをクリアします。
        /// </summary>
        public void ClearSendLog()
        {
            this.SendLog = new List<string>();
        }

        /// <summary>
        /// id nameとid authorを送信します。
        /// </summary>
        public void SendNameAuthor()
        {
            this.Pool("id name Niels");
            this.Pool("id author tknemuru");
            this.Flush();
        }

        /// <summary>
        /// usiokを送信します。
        /// </summary>
        public void SendUsiOk()
        {
            this.Flush("usiok");
        }

        /// <summary>
        /// usiokを送信します。
        /// </summary>
        public void SendReadyOk()
        {
            this.Flush("readyok");
        }

        /// <summary>
        /// bestmoveを送信します。
        /// </summary>
        /// <param name="context"></param>
        public void SendBestMove(BoardContext context)
        {
            CpuPlayer player = new CpuPlayer();

            // 探索情報の送信を開始
            Task.Run(() => this.SendSearchInfo(player));

            // 探索開始
            uint move = player.Put(context);
            string sfenMove = SfenNotation.ConvertToSfenMove(move);
            this.Flush(player.Searcher.SearchInfo.ToString());
            this.Flush("bestmove " + sfenMove);
        }

        /// <summary>
        /// infoを送信します。
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        private async Task SendSearchInfo(Player player)
        {
            while (!player.Searcher.SearchInfo.IsSearchEnd)
            {
                this.Flush(player.Searcher.SearchInfo.ToString());
                await Task.Delay(500);
            }            
        }

        /// <summary>
        /// バッファに情報を書き込みます
        /// </summary>
        private void Pool(string info)
        {
            // ログ
            Debug.WriteLine("UsiSender > " + info);
            FileHelper.WriteLine("UsiSender > " + info);
            this.SendLog.Add(info);

            // 出力
             this.Sender.Write(info + "\n"); // USIの改行コードはLF。
        }

        /// <summary>
        /// バッファの情報を送信します。
        /// </summary>
        private void Flush()
        {
            this.Sender.Flush();
        }

        /// <summary>
        /// バッファの情報を即時送信します。
        /// </summary>
        /// <param name="command">コマンド</param>
        private void Flush(string command)
        {
            this.Pool(command);
            this.Flush();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Diagnostics;
using Niels.Helper;
using Niels.Boards;
using Niels.Collections;
using Niels.Notation;

namespace Niels.Usi
{
    /// <summary>
    /// USIコマンドを受信します。
    /// </summary>
    public class UsiCommandReceiver
    {
        /// <summary>
        /// レシーバ
        /// </summary>
        private TextReader Receiver { get; set; }

        /// <summary>
        /// Sender
        /// </summary>
        private UsiCommandSender Sender { get; set; }

        /// <summary>
        /// 盤状態
        /// </summary>
        private BoardContext BoardContext { get; set; }

        /// <summary>
        /// quitが実行された場合はtrue
        /// </summary>
        private bool IsQuit { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public UsiCommandReceiver()
        {
            this.Receiver = Console.In;
            this.Sender = new UsiCommandSender();
            this.IsQuit = false;
            this.BoardContext = new BoardContext(Turn.Black);
        }

        /// <summary>
        /// 受信を開始します。
        /// </summary>
        public void Run()
        {
            while (!this.IsQuit)
            {
                UsiCommand command = Receive();
                if (command == null) { continue; }

                switch (command.Name)
                {
                    case "usi" :
                        this.Sender.SendNameAuthor();
                        this.Sender.SendUsiOk();
                        break;
                    case "isready" :
                        this.Sender.SendReadyOk();
                        break;
                    case "setoption" :
                        break;
                    case "usinewgame" :
                        break;
                    case "position" :
                        this.BoardContext = this.ReceivePosition(command);
                        break;
                    case "go" :
                        this.Sender.SendBestMove(this.BoardContext);
                        break;
                    case "stop" :
                        break;
                    case "ponderhit" :
                        break;
                    case "quit" :
                        this.IsQuit = true;
                        break;
                    case "gameover" :
                        break;
                    default :
                        throw new ArgumentException("不正なコマンドです。");
                }
            }
        }

        /// <summary>
        /// 受信します。
        /// </summary>
        private UsiCommand Receive()
        {
            string line = this.Receiver.ReadLine();

            if (string.IsNullOrEmpty(line)) { return null; }

            Debug.WriteLine("UsiReceiver < " + line);
            FileHelper.WriteLine("UsiReceiver < " + line);

            return UsiCommand.Parse(line);
        }

        /// <summary>
        /// positionコマンドを受信します。
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected BoardContext ReceivePosition(UsiCommand command)
        {
            Debug.Assert(command.Name == "position", "コマンドがposition以外です。");
            string[] parameters = command.GetSplitedParameters().ToArray();

            // TODO:ターンの概念がないのか・・・まいったね。
            BoardContext context = new BoardContext(Turn.Black);

            if (parameters[0] == "startpos")
            {
                context.SetDefaultStartPosition();

                if (parameters.Length > 2)
                {
                    Debug.Assert(parameters[1] == "moves", "パラメータがmoves以外です。");
                    for (int i = 2; i < parameters.Length; i++)
                    {
                        context.PutPiece(SfenNotation.ConvertToNielsMove(parameters[i], context));
                        context.ChangeTurn();
                    }
                }
            }
            else
            {
                throw new NotSupportedException("サポート外の初期配置です。");
            }

            return context;
        }
    }
}

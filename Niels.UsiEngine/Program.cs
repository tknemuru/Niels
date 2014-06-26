using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Niels.Usi;

namespace Niels.UsiEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            new UsiCommandReceiver().Run();
        }
    }
}

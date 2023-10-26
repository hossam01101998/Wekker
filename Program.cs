using System;
using System.Runtime.Intrinsics.X86;
using System.Threading;
using Gebruik_van_delegates;


class Program
{
    static void Main()
    {
        Wekker wekker = new Wekker();
        wekker.Start();
    }
}

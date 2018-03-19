#define TEST

using Dll;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace tmp
{
    class Program
    {
        private static readonly TestJIT test = new TestJIT(4096);
        static void Main(string[] args)
        {
            Run1();
            Run2();
            Run4();
            Run5();

            Run3();
            Run31();
            Run6();
            Run61();

#if TEST
            SimpleTime(test.X1, nameof(test.X1));
            SimpleTime(test.X2, nameof(test.X2));
            SimpleTime(Run1, nameof(Run1));
            SimpleTime(Run2, nameof(Run2));
            SimpleTime(Run4, nameof(Run4));
            SimpleTime(Run5, nameof(Run5));
            SimpleTime(Run3, nameof(Run3));
            SimpleTime(Run31, nameof(Run31));
            SimpleTime(Run6, nameof(Run6));
            SimpleTime(Run61, nameof(Run61));
#endif
        }

        static void SimpleTime(Action a, string name)
        {
            for (var i = 0; i < 1000; ++i)
            {
                a();
            }
            Stopwatch sw = Stopwatch.StartNew();
            for (var i = 0; i < 1000_000_00; ++i)
            {
                a();
            }
            Console.WriteLine($"{name}: {sw.ElapsedMilliseconds}ms used");
        }

        static void Run1()
        {
            test.RunThrowWithCall(1, 1);
        }

        static void Run2()
        {
            test.CheckIndex0ThrowWithCall(1, 1);
        }

        static void Run3()
        {
            test.CheckIndexThrowWithCall(1);
        }

        static void Run31()
        {
            test.CheckIndexThrowWithCall2(1);
        }

        static void Run4()
        {
            test.RunThrowDirect(1, 1);
        }

        static void Run5()
        {
            test.CheckIndex0ThrowDirect(1, 1);
        }

        static void Run6()
        {
            test.CheckIndexThrowDirect(1);
        }

        static void Run61()
        {
            test.CheckIndexThrowDirect2(1);
        }
    }
}

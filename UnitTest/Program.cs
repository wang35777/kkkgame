using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    class Program
    {

        static void Main(string[] args)
        {
            char a = 'a';

            string abc = "abc";

            abc += a;

            Console.WriteLine(abc);

            int b = 98;
            abc += (char)b;
            Console.WriteLine(abc);

            Console.ReadLine();

        }
    }
}

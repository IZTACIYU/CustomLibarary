using Nexu.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CustomLibrary
{
    public class Test
    {
        public int id;
        public string name;

        public Test(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
    internal class Program
    {
        static Dictionary<int, string> dictionary = new Dictionary<int, string>()
        {
            {1, "AAA" },
            {2, "BBB" },
            {3, "CCC" },
            {4, "DDD" },
        };
        static LinkedList<int> linkedList = new LinkedList<int>();
        static Stack<string> stack = new Stack<string>();
        static Queue<string> queue = new Queue<string>();
        static Test[] tests;// ={ new Test(1, "AAA"), new Test(2, "BBB"),new Test(3, "CCC"), new Test(4, "DDD")};

        static int[] arry = { 1, 2, 3, 4, 5, 99 };

        static string filePath = "C:\\Users\\admin\\Desktop\\Git\\CustomLibarary\\CustomLibarary\\test.json";
        static string copyPath = "C:\\Users\\admin\\Desktop\\Git\\CustomLibarary\\CustomLibarary\\copy.json";
        
        static void Main(string[] args)
        {
            dictionary.SaveJSON(filePath);
            "123".toAscii()[1].print();

            (1, 5).multiple().print();
            (1.0, 5.4).multiple().print();
            ("Hello ", "World").add().print();
        }
    }
}

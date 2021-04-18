using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadJson.Test
{
    public class Program
    {
        public static string myjson = System.IO.File.ReadAllText("test.json");

        public static void Main(string[] args)
        {
            MyBenchmark();

            Console.ReadKey();
        }

        static void MyBenchmark()
        {
            var counter = int.Parse(Console.ReadLine());

            JsonBenchmark jsonBenchmark = new JsonBenchmark(counter);

            var timer = System.Diagnostics.Stopwatch.StartNew();
            jsonBenchmark.MyJsontest();
            timer.Stop();
            Console.WriteLine($"BadJson: {timer.Elapsed}");

            #if DEBUG
            foreach (var i in JsonBenchmark.junkk[0])
            {
                if (i.Value is string[] a) { Console.WriteLine($"{i.Key} | {string.Join(",", a)}"); }
                else { Console.WriteLine($"{i.Key} | {i.Value}"); }
            }
            #endif

            timer.Restart();
            jsonBenchmark.MyJsontestWithStruct();
            timer.Stop();
            Console.WriteLine($"BadJson with struct: {timer.Elapsed}");

            #if DEBUG
            var readed = BadJsonReader.Deserialize<MyJsonStruct>(myjson);
            Console.WriteLine($"{readed.Num}\n{readed.Nums}\n{readed.Text}\n{string.Join(",", readed.TextArray)}");
            #endif

            timer.Restart();
            jsonBenchmark.Newtonsofttest();
            timer.Stop();
            Console.WriteLine($"Newtonsoft: {timer.Elapsed}");

            timer.Restart();
            jsonBenchmark.SystemTextJson();
            timer.Stop();
            Console.WriteLine($"SystemTextJson: {timer.Elapsed}");
        }

        public class JsonBenchmark
        {
            public JsonBenchmark(int counter) => _counter = counter;
            private int _counter;

            public void SystemTextJson()
            {
                for (int i = 0; i < _counter; i++)
                    System.Text.Json.JsonSerializer.Deserialize<MyJsonStruct>(Program.myjson);
            }

            public void Newtonsofttest()
            {
                for (int i = 0; i < _counter; i++)
                    Newtonsoft.Json.JsonConvert.DeserializeObject<MyJsonStruct>(Program.myjson);
            }

            public void MyJsontest()
            {
                for (int i = 0; i < _counter; i++)
                    BadJsonReader.Deserialize(Program.myjson);
            }

            public void MyJsontestWithStruct()
            {
                for (int i = 0; i < _counter; i++)
                    BadJsonReader.Deserialize<MyJsonStruct>(Program.myjson);
            }
        }

        public struct MyJsonStruct
        {
            public string Text { get; set; }

            public string[] TextArray { get; set; }

            public int Num { get; set; }

            public int[] Nums { get; set; }
        }
    }
}

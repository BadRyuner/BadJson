using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace BadJson.Test
{
    public class Program
    {
        public static string myjson = System.IO.File.ReadAllText("test.json");

        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<JsonBenchmark>();

            MyBenchmark();

            Console.ReadKey();
        }

        static void MyBenchmark()
        {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            var a = new BadJsonReader(myjson);
            timer.Stop();
            Console.WriteLine($"BadJson\n{timer.ElapsedMilliseconds}ms\n{a.Result["String"]}\n{string.Join(",", a.Result["String_array"])}\n{a.Result["Int"]}\n{string.Join(",", a.Result["Int_array"])}\n");

            timer.Restart();
            var b = Utf8Json.JsonSerializer.Deserialize<MyJsonStruct>(myjson);
            timer.Stop();
            Console.WriteLine($"utf8json\n{timer.ElapsedMilliseconds}ms\n{b.String}\n{b.String_array}\n{b.Int}\n{b.Int_Array}\n");
        }

        public class JsonBenchmark
        {

            [Benchmark]
            public void Utf8Jsontest()
            {
                Utf8Json.JsonSerializer.Deserialize<MyJsonStruct>(Program.myjson);
            }

            [Benchmark]
            public void MyJsontest()
            {
                new BadJsonReader(Program.myjson);
            }
        }

        public struct MyJsonStruct
        {
            public string String { get; set; }

            public string[] String_array { get; set; }

            public int Int { get; set; }

            public string[] Int_Array { get; set; }
        }
    }
}

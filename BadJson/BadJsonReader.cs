using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadJson
{
    public class BadJsonReader
    {
        public Dictionary<string, object> Result = new Dictionary<string, object>();

        private string targetjson;

        public BadJsonReader(string json)
        {
            targetjson = json;
            StartJob().Wait();
        }

        private async Task StartJob()
        {
            List<Task> tasks = new List<Task>();

            string[] json_in_lines = targetjson.Split('\n');

            foreach(string json_line in json_in_lines)
            {
                if(json_line.Length > 2) tasks.Add(LineWorker(json_line));
            }

            foreach (Task task in tasks) await task; 
        }

        private async Task LineWorker(string line)
        {
            string[] TwoSide = line.Split(':');

            string LeftSide = TwoSide.First();
            string RightSide = TwoSide.Last().Replace(',', ' ');

            var a = RemoveSmth(LeftSide);
            var b = GetRightSide(RightSide); await a; await b;

            LeftSide = a.Result; var rightside = b.Result;

            Result.Add(LeftSide, rightside);

            await Task.CompletedTask;
        }

        private async Task<string> RemoveSmth(string side)
        {
            var localstring = string.Empty;
            bool quoutefound = false;
            foreach(char c in side)
            {
                if (!quoutefound && c == ' ') continue;
                if (!quoutefound && c == '"') { quoutefound = true; continue; }
                if (quoutefound && c != '"') { localstring += c; continue; }
                if (quoutefound && c == '"') { quoutefound = false; continue; }
            }

            await Task.CompletedTask;

            return localstring;
        }

        private async Task<object> GetRightSide(string rightside)
        {
            await Task.CompletedTask;
            bool quoutefound = false;
            #region Array Object
            if (rightside.Contains('['))
            {
                var obj = new List<string>();

                bool arraystarted = false;
                StringBuilder Element_From_Array = new StringBuilder();
                foreach (var _char in rightside)
                {
                    if (_char == '[') { arraystarted = true; continue; }
                    if (_char == ']') { arraystarted = false; continue; }

                    if (_char == '"' && !quoutefound) { quoutefound = true; continue; }
                    if (_char == '"' && quoutefound) { quoutefound = false; obj.Add(Element_From_Array.ToString()); Element_From_Array.Clear(); continue; }

                    if (quoutefound) Element_From_Array.Append(_char);
                }

                return obj.ToArray() as object;
            }
            #endregion
            #region Non-Array Object
            else
            {
                StringBuilder Object = new StringBuilder();
                #region With Quotes
                if(rightside.Contains('"'))
                {
                    foreach (var _char in rightside)
                    {
                        if (_char == '"' && !quoutefound) { quoutefound = true; continue; }
                        if (_char == '"' && quoutefound) { quoutefound = false; continue; }

                        if (quoutefound) Object.Append(_char);
                    }
                }
                #endregion
                #region w/o
                else
                {
                    foreach (var _char in rightside)
                    {
                        if (_char != ' ' && _char != ',') Object.Append(_char);
                    }
                }
                #endregion
                return Object.ToString() as object;
            }
            #endregion
        }
    }
}

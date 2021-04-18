using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BadJson
{
    public class BadJsonReader
    {
        private Dictionary<string, object> Result = new Dictionary<string, object>();

        private string targetjson;

        public static Dictionary<string, object> Deserialize(string json)
        {
            BadJsonReader jsonReader = new BadJsonReader();

            jsonReader.targetjson = json;
            jsonReader.StartJob().Wait();

            return jsonReader.Result;
        }

        public static T Deserialize<T>(string json) where T : struct
        {
            BadJsonReader jsonReader = new BadJsonReader();

            jsonReader.targetjson = json;
            jsonReader.StartJob().Wait();
            //T structure = new T();
            T structure = (T)Activator.CreateInstance<T>();

            foreach (PropertyInfo i in structure.GetType().GetProperties())
            {
                #if DEBUG
                try
                {
                #endif
                    switch (i.PropertyType.Name)
                    {
                        case "String":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, jsonReader.Result[i.Name]);
                            break;
                        case "Int16":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Int16.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Int32":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Int32.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Int64":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Int64.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "UInt16":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, UInt16.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "UInt32":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, UInt32.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "UInt64":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, UInt64.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Byte":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Byte.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "SByte":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, SByte.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Decimal":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Decimal.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Double":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Double.Parse(jsonReader.Result[i.Name] as string));
                            break;
                        case "Single":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, Single.Parse(jsonReader.Result[i.Name] as string));
                            break;

                        // Arrays

                        case "String[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, jsonReader.Result[i.Name]);
                            break;
                        case "Int16[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayInt16(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Int32[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayInt32(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Int64[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayInt64(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "UInt16[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayUInt16(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "UInt32[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayUInt32(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "UInt64[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayUInt64(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Byte[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayByte(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "SByte[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArraySByte(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Decimal[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayDecimal(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Double[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArrayDouble(jsonReader.Result[i.Name] as string[]));
                            break;
                        case "Single[]":
                            ((SetHandler<T>)GetDelegate<T>(i))(ref structure, ToArraySingle(jsonReader.Result[i.Name] as string[]));
                            break;
                    }
                #if DEBUG
                }
                catch (Exception e) { Console.WriteLine($"Error\n{e}"); }
                #endif
            }

            return structure;
        }

        private static Double[] ToArrayDouble(string[] str)
        {
            List<Double> array = new List<Double>();
            foreach(string _string in str)
            {
                array.Add(Double.Parse(_string));
            }
            return array.ToArray();
        }

        private static Int16[] ToArrayInt16(string[] str)
        {
            List<Int16> array = new List<Int16>();
            foreach (string _string in str)
            {
                array.Add(Int16.Parse(_string));
            }
            return array.ToArray();
        }

        private static Int32[] ToArrayInt32(string[] str)
        {
            List<Int32> array = new List<Int32>();
            foreach (string _string in str)
            {
                array.Add(Int32.Parse(_string));
            }
            return array.ToArray();
        }

        private static Int64[] ToArrayInt64(string[] str)
        {
            List<Int64> array = new List<Int64>();
            foreach (string _string in str)
            {
                array.Add(Int64.Parse(_string));
            }
            return array.ToArray();
        }

        private static UInt16[] ToArrayUInt16(string[] str)
        {
            List<UInt16> array = new List<UInt16>();
            foreach (string _string in str)
            {
                array.ToList().Add(UInt16.Parse(_string));
            }
            return array.ToArray();
        }

        private static UInt32[] ToArrayUInt32(string[] str)
        {
            List<UInt32> array = new List<UInt32>();
            foreach (string _string in str)
            {
                array.ToList().Add(UInt32.Parse(_string));
            }
            return array.ToArray();
        }

        private static UInt64[] ToArrayUInt64(string[] str)
        {
            List<UInt64> array = new List<UInt64>();
            foreach (string _string in str)
            {
                array.Add(UInt64.Parse(_string));
            }
            return array.ToArray();
        }

        private static Decimal[] ToArrayDecimal(string[] str)
        {
            List < Decimal> array = new List<Decimal>();
            foreach (string _string in str)
            {
                array.Add(Decimal.Parse(_string));
            }
            return array.ToArray();
        }

        private static Single[] ToArraySingle(string[] str)
        {
            List < Single> array = new List<Single>();
            foreach (string _string in str)
            {
                array.Add(Single.Parse(_string));
            }
            return array.ToArray();
        }

        private static SByte[] ToArraySByte(string[] str)
        {
            List<SByte> array = new List<SByte>();
            foreach (string _string in str)
            {
                array.Add(SByte.Parse(_string));
            }
            return array.ToArray();
        }

        private static Byte[] ToArrayByte(string[] str)
        {
            List<Byte> array = new List<Byte>();
            foreach (string _string in str)
            {
                array.Add(Byte.Parse(_string));
            }
            return array.ToArray();
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
            string RightSide = TwoSide.Last();

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

                if (rightside.Contains('"'))
                {
                    foreach (var _char in rightside)
                    {
                        if (_char == '[') { arraystarted = true; continue; }
                        if (_char == ']') { arraystarted = false; continue; }

                        if (_char == '"' && !quoutefound) { quoutefound = true; continue; }
                        if (_char == '"' && quoutefound) { quoutefound = false; obj.Add(Element_From_Array.ToString()); Element_From_Array.Clear(); continue; }

                        if (quoutefound) Element_From_Array.Append(_char);
                    }
                }
                else // stupid idea for objects without " 
                {
                    foreach (var _char in rightside)
                    {
                        if (_char == '[') { arraystarted = true; continue; }
                        if (_char == ']') { arraystarted = false; obj.Add(Element_From_Array.ToString()); continue; }

                        if (_char == '0' || _char == '1' || _char == '2' || _char == '3' || _char == '4' || _char == '5' || _char == '6' || _char == '7' || _char == '8' || _char == '9') { Element_From_Array.Append(_char); continue; }
                        if (_char == ',') { quoutefound = false; obj.Add(Element_From_Array.ToString()); Element_From_Array.Clear(); continue; }
                    }
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

        private delegate void SetHandler<T>(ref T source, object value);

        private static SetHandler<T> GetDelegate<T>(PropertyInfo propertyInfo)
        {
            return (ref T s, object val) =>
            {
                object obj = s;
                propertyInfo.SetValue(obj, val);
                s = (T)obj;
            };
        }
    }
}

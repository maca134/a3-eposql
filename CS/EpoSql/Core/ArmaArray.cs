#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace EpoSql.Core
{
    public class ArmaArray : List<object>
    {
        public static ArmaArray Unserialize(string strarray)
        {
            if (strarray.ElementAt(0) != '[')
            {
                throw new ArmaArrayException("String does not start with a [");
            }
            strarray = strarray.Substring(1);

            var array = new ArmaArray();
            var nums = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.', '-', 'e' };
            for (var i = 0; i < strarray.Length; i++)
            {
                switch (strarray[i])
                {
                    case '[':
                        {
                            var str = new StringBuilder();
                            var inArr = 0;
                            while (true)
                            {
                                if (strarray[i] == '[')
                                {
                                    inArr++;
                                }
                                if (strarray[i] == ']')
                                {
                                    inArr--;
                                }
                                str.Append(strarray[i]);
                                i++;
                                if (inArr == 0)
                                {
                                    break;
                                }
                            }
                            List<object> innerArray = Unserialize(str.ToString());
                            array.Add(innerArray);
                        }
                        break;
                    case '"':
                        {
                            var str = new StringBuilder();
                            var isEnd = false;
                            i++;
                            while (true)
                            {
                                try
                                {
                                    if (strarray[i] == '"')
                                    {
                                        isEnd = !isEnd;
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                                if (isEnd && (strarray[i] == ',' || strarray[i] == ']'))
                                {
                                    break;
                                }
                                str.Append(strarray[i]);
                                i++;
                            }
                            array.Add(str.ToString().TrimEnd('"'));
                        }
                        break;
                    default:
                        if (nums.Contains(strarray[i]))
                        {
                            var str = new StringBuilder();
                            var isFloat = false;
                            while (nums.Contains(strarray[i]))
                            {
                                if (strarray[i] == '.')
                                    isFloat = true;
                                str.Append(strarray[i]);
                                i++;
                            }
                            if (isFloat)
                            {
                                var num = Convert.ToDouble(str.ToString());
                                array.Add(num);
                            }
                            else
                            {
                                var num = Convert.ToInt32(str.ToString());
                                array.Add(num);
                            }
                        }
                        else if (Substring(strarray, i, 4).ToLower() == "true")
                        {
                            array.Add(true);
                            i = i + 4;
                        }
                        else if (Substring(strarray, i, 5).ToLower() == "false")
                        {
                            array.Add(false);
                            i = i + 5;
                        }
                        break;
                }
            }
            return array;
        }

        public static string Serialize(ArmaArray array)
        {
            var data = new StringBuilder();
            data.Append("[");
            if (array == null)
            {
                data.Append("]");
                return data.ToString();
            }
            foreach (var d in array)
            {
                if (d is string)
                {
                    data.Append("\"");
                    var s = d as string;
                    data.Append(s);
                    data.Append("\"");
                }
                else if (d is int || d is double || d is bool)
                {
                    data.Append(d);
                }
                else if (d is ArmaArray)
                {
                    var a = d as ArmaArray;
                    data.Append(Serialize(a));
                }
                data.Append(",");
            }
            if (data[data.Length - 1] == ',')
            {
                data.Length--;
            }
            data.Append("]");
            return data.ToString();
        }

        private static string Substring(string input, int start, int length)
        {
            var inputLength = input.Length;
            return start + length >= inputLength ? input.Substring(start) : input.Substring(start, length);
        }

        public int AsInt(int index)
        {
            try
            {
                return Convert.ToInt32(this[index]);
            }
            catch
            {
                return -1;
            }
        }

        public float AsFloat(int index)
        {
            try
            {
                return Convert.ToSingle(this[index]);
            }
            catch
            {
                return -1f;
            }
        }

        public ArmaArray AsArray(int index)
        {
            return (this[index] as ArmaArray);
        }

        public string AsString(int index)
        {
            return (this[index] as string);
        }

        public bool AsBool(int index)
        {
            try
            {
                return Convert.ToBoolean(this[index]);
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return Serialize(this);
        }

        public long AsLong(int index)
        {
            try
            {
                return Convert.ToInt64(this[index]);
            }
            catch
            {
                return -1;
            }
        }
    }
}
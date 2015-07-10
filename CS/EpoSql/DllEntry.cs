#region usings

using System.Runtime.InteropServices;
using System.Text;
using EpoSql.Core;
using RGiesecke.DllExport;

#endregion

namespace EpoSql
{
    public class DllEntry
    {
        [DllExport("_RVExtension@12", CallingConvention = CallingConvention.Winapi)]
        public static void RvExtension(StringBuilder output, int outputSize, string function)
        {
            function = function.Replace("\\n", "\n");
            outputSize--;
            var response = Manager.Invoke(function, outputSize);
            output.Append(response);
        }
    }
}
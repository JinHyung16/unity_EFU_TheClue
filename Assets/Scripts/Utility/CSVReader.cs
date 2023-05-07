using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace HughUtility
{
    public class CSVReader : MonoBehaviour
    {
        static string splite_read = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string splite_read_line = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<Dictionary<string, string>> ReadFile(string file)
        {
            var list = new List<Dictionary<string, string>>();
            TextAsset data = Resources.Load(file) as TextAsset;

            var lines = Regex.Split(data.text, splite_read_line);

            if (lines.Length <= 1)
            {
                return list;
            }

            var header_colum = Regex.Split(lines[0], splite_read);

            for (var i = 1; i < lines.Length; i++)
            {
                var values = Regex.Split(lines[i], splite_read);
                if (values.Length == 0 || values[0] == "")
                {
                    continue;
                }

                var body_colum = new Dictionary<string, string>();

                for (var j = 0; j < header_colum.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    string finalvalue = value;
                    int n;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n.ToString();
                    }
                    body_colum[header_colum[j]] = finalvalue;
                }
                list.Add(body_colum);
            }
            return list;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace NoiThat.Library
{
    public class XString
    {
        public static string StrSlug(string text)
        {
            {
                for (int i = 32; i < 48; i++)
                {
                    text = text.Replace(((char)i).ToString(), " ");
                }
                text = text.Replace(".", "-");
                text = text.Replace(" ", "-");
                text = text.Replace(",", "-");
                text = text.Replace(";", "-");

                text = text.Replace(":", "-");
                Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

                string strFormD = text.Normalize(System.Text.NormalizationForm.FormD);

                return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');

            }
        }

        public static string StrLimit(string str, int? lenght)
        {
            int leng = (lenght ?? 20);
            if (str.Length > lenght)
            {
                str = str.Substring(0, leng) + "...";
            }
            return str;
        }
        static bool chkKyTuDacBiet(string pass)
        {
            foreach (var c in pass)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        static bool chkSo(string pass)
        {
            foreach (var c in pass)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }

        static bool chkKyTuHoa(string pass)
        {
            foreach (var c in pass)
            {
                if (!char.IsUpper(c))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool chkMK(string pass)
        {
            if (chkKyTuDacBiet(pass) &&
            chkKyTuHoa(pass) &&
            chkSo(pass) && pass.Length >= 6
            )
            {
                return true;
            }
            return false;
        }
    }
}
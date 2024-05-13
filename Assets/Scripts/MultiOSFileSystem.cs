using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace MultiOSFileSystem 
{
    public class AndroidFileSystem
    {
        public static string Result = "";
        public static string AppDataDirectory
        { 
            get 
            {
                return Directory.GetCurrentDirectory() + "/com.mc_copy_beta";
            } 
        }

    }
    public class FileSystems
    {
        /// <summary>
        /// use char sep to separate two directories
        /// </summary>
        /// <param name="filepath">string file </param>
        /// <param name="sep"> separator char</param>
        /// <returns></returns>
        public static string Combine(string filepath,char sep)
        {
            string[] seps_ = split(filepath,sep);
            return Path.Combine(seps_);
        }

        public static string[] split(string _string, char _separater, bool _canHaveSeparator = true, bool _canHaveQuotes = true)
        {
            List<string> _r = new List<string>();
            string _e = "";
            bool _incSeparaterChar = false;
            for (int i = 0; i < _string.Length; i++)
            {
                if ((_string[i] == '\"' || _string[i] == '\'') && _canHaveSeparator)
                {
                    _incSeparaterChar = toggle(_incSeparaterChar);
                    if (_canHaveQuotes)
                    {
                        _e += $"{_string[i]}";
                    }
                }
                else if (_string[i] != _separater || _incSeparaterChar)
                {
                    _e += _string[i];
                }
                else
                {
                    _r.Add(_e);
                    _e = "";
                }
            }
            _r.Add(_e);
            for (int i = 0; i < _r.Count; i++)
            {
                if (_r[i] == "" || _r[i] == " ")
                {
                    _r.RemoveAt(i);
                }
            }
            if (_r.Count == 0)
            {
                _r.Add("");
            }
            return _r.ToArray();
        }

        public static bool toggle(bool b)
        {
            if (b)
                return false;
            else
                return true;
        }
    }
}

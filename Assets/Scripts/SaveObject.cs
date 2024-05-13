using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Threading.Tasks;
using Vector3 = UnityEngine.Vector3;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    public class SaveObject
    {
        public static string ToSaveDataString(GenerationSettings obj_)
        {
            string str_ = "";
            str_ += "type=GenerationSettings;" + Environment.NewLine;
            str_ += $"ChunkPos={{{obj_.ChunkPos.x},{obj_.ChunkPos.y},{obj_.ChunkPos.z}}};" + Environment.NewLine;
            str_ += $"ChunksLength={obj_.ChunksLength};" + Environment.NewLine;
            str_ += $"SandLevel={obj_.SandLevel};" + Environment.NewLine;

            string c_ = "";
            for(int i = 0; i < obj_.Seed.Length; i++)
            {
                c_ += obj_.Seed[i];
                if (i < obj_.Seed.Length - 1)
                {
                    c_ += ",";
                }
            }

            str_ += $"Seed=[{c_}];" + Environment.NewLine;
            str_ += "treeSettings={" + Environment.NewLine;
            str_ += $"addTrees={obj_.treeSettings.addTrees}," + Environment.NewLine;
            str_ += $"spawnProbabilty={obj_.treeSettings.spawnProbabilty}," + Environment.NewLine;
            str_ += $"treeLayerMax={obj_.treeSettings.treeLayerMax}" + Environment.NewLine;
            str_ += "};" + Environment.NewLine;
            str_ += $"YOffset={obj_.YOffset};" + Environment.NewLine;
            str_ += $"YScale={obj_.YScale};";

            return str_;
        }
        public static GenerationSettings ToGenerationSettings(string s_)
        {
            GenerationSettings obj_ = new GenerationSettings();
            obj_.treeSettings = new TreeSettings();
            try
            {
                string[] buffer_ = split(s_, ';');
                for (int i = 0; i < buffer_.Length; i++)
                {
                    buffer_[i] = buffer_[i].Replace(Environment.NewLine,"");
                    Debug.Log(buffer_[i]);

                    if (buffer_[i].StartsWith("type"))
                    {
                        string[] par_ = split(buffer_[i], '=');
                        if (!par_[1].StartsWith("GenerationSettings"))
                        {
                            throw new Exception("Type is not set to \"GenerationSettings\"");
                        }
                        else
                        {
                            UnityEngine.Debug.Log("type is GenerationSettings");
                        }
                    }
                    if (buffer_[i].StartsWith("ChunkPos"))
                    {
                        string enc_ = GetEncapsulatedData(buffer_[i], '{', '}');
                        string[] spl_ = split(enc_, ',');
                        int x = int.Parse(spl_[0]), y = int.Parse(spl_[1]), z = int.Parse(spl_[2]);
                        Vector3 v_ = new Vector3(x, y, z);
                        obj_.ChunkPos = v_;
                    }
                    if (buffer_[i].StartsWith("ChunksLength"))
                    {
                        string[] e_ = split(buffer_[i], '=');
                        string c_ = e_[1];
                        int clen_ = int.Parse(c_);
                        Debug.Log("ChunksLength" + clen_);
                        obj_.ChunksLength = clen_;
                    }
                    if (buffer_[i].StartsWith("SandLevel"))
                    {
                        string[] e_ = split(buffer_[i], '=');
                        string c_ = e_[1].Replace(";", "");
                        int clen_ = int.Parse(c_);
                        Debug.Log("SandLevel" + clen_);
                        obj_.SandLevel = clen_;
                    }
                    if (buffer_[i].StartsWith("Seed"))
                    {
                        string enc_ = GetEncapsulatedData(buffer_[i], '[', ']');
                        string[] spl_ = split(enc_, ',');
                        List<int> ints_ = new List<int>();
                        for (int k_ = 0; k_ < spl_.Length; k_++)
                        {
                            ints_.Add(int.Parse(spl_[k_]));
                        }
                        Debug.Log($"Seed:[{ints_.ToArray()[0]},{ints_.ToArray()[1]},{ints_.ToArray()[2]}]");
                        obj_.Seed = ints_.ToArray();
                    }
                    if (buffer_[i].StartsWith("treeSettings"))
                    {
                        string enc_ = GetEncapsulatedData(buffer_[i], '{', '}');
                        int readOff_ = 0;
                        string[] pars_ = split(enc_, ',');
                        foreach(string e_ in pars_)
                        {
                            Debug.Log(e_+",");
                        }
                        Debug.Log("pars_ length:"+pars_.Length);
                        if (pars_[0 + readOff_].StartsWith("addTrees"))
                        {
                            string[] p_ = split(pars_[0+ readOff_], '=');
                            if (p_[1].Contains("True"))
                            {
                                obj_.treeSettings.addTrees = true;
                            }
                            else
                            {
                                obj_.treeSettings.addTrees = false;
                            }
                            Debug.Log(obj_.treeSettings.addTrees);
                        }
                        if (pars_[1 + readOff_].StartsWith("spawnProbabilty"))
                        {
                            string[] p_ = split(pars_[1 + readOff_], '=');
                            p_[1] = p_[1].Replace(",", "");
                            p_[1] = p_[1].Replace("=", "");
                            Debug.Log($"p_[1]:\"{p_[1]}\"");
                            obj_.treeSettings.spawnProbabilty = float.Parse(p_[1]);
                        }
                        if (pars_[2 + readOff_].StartsWith("treeLayerMax"))
                        {
                            string[] p_ = split(pars_[2 + readOff_], '=');
                            obj_.treeSettings.treeLayerMax = int.Parse(p_[1]);
                        }
                    }
                    if (buffer_[i].StartsWith("YOffset"))
                    {
                        string[] e_ = split(buffer_[i], '=');
                        string c_ = e_[1].Replace(";", "");
                        int clen_ = int.Parse(c_);
                        Debug.Log("YOffset" + clen_);
                        obj_.YOffset = clen_;
                    }
                    if (buffer_[i].StartsWith("YScale"))
                    {
                        string[] e_ = split(buffer_[i], '=');
                        string c_ = e_[1].Replace(";", "");
                        int clen_ = int.Parse(c_);
                        Debug.Log("YScale" + clen_);
                        obj_.YScale = clen_;
                    }
                }
                Debug.Log(JsonUtility.ToJson(obj_));
            }
            catch(Exception e)
            {
                Debug.LogError(e.ToString());
            }
            
            return obj_;
        }
        private static string GetEncapsulatedData(string s, char a, char b)
        {
            string c = "";
            bool _incSeparaterChar = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == a)
                {
                    _incSeparaterChar = true;
                    for (int j = i + 1; j < s.Length && (s[j] != b || !_incSeparaterChar); j++)
                    {
                        if (s[j] == a)
                        {
                            _incSeparaterChar = false;
                        }
                        if (s[j] == b)
                        {
                            _incSeparaterChar = true;
                        }
                        c += s[j];
                    }
                }
            }
            return c;
        }
        private static bool toggle(bool b)
        {
            if (b)
                return false;
            else
                return true;
        }
        private static string[] split(string _string, char _separater, bool _canHaveSeparator = true, bool _canHaveQuotes = true)
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Language
{
    private Dictionary<string, string> strings = new Dictionary<string, string>();

    public Language(string path)
    {
        Load(path);
    }

    public void Load(string path)
    {

        List<string> contents = new List<string>();
        if (File.Exists(path)) contents.AddRange(File.ReadAllLines(path));

        for (int i = 0; i < contents.Count; ++i)
        {
            try
            {
                string line = contents[i].Replace("\\n", "\n");
                string[] strs = line.Split('=');
                if (strs.Length < 2)
                    SaveError(contents[i]);

                if (!strings.ContainsKey(strs[0]))
                    strings.Add(strs[0], strs[1]);
            }
            catch (Exception ex)
            {
                File.AppendAllText(@".\Error.txt",
                           string.Format("[{0}] {1}{2}", DateTime.Now, ex, Environment.NewLine));
            }
        }
    }

    public string Translate(string src)
    {
        if (strings.ContainsKey(src))
            return strings[src];

        SaveError("translate failed:" + src);
        return src;
    }

    public void SaveError(string ex)
    {
        try
        {
            File.AppendAllText(@".\LanguageError.txt",
                               string.Format("[{0}] {1}{2}", DateTime.Now, ex, Environment.NewLine));
        }
        catch
        {
        }
    }

    public string[] GetNames(Type obj)
    {
        Array arr = Enum.GetValues(obj);
        string[] strings = new string[arr.Length];
        int i = 0;
        foreach (var e in arr)
        {
            strings[i++] = Translate(e.ToString());
        }
        return strings;
    }
}
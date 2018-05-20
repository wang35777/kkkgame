using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Language
{
    private Dictionary<string, string> strings = new Dictionary<string, string>();

    public Language()
    {
        Load();
    }

    public void Load()
    {
        try
        {
            List<string> contents = new List<string>();
            if (File.Exists(@".\Configs\language.txt"))
                contents.AddRange(File.ReadAllLines(@".\Configs\language.txt"));

            for (int i = 0; i < contents.Count; ++i)
            {
                string line = contents[i].Replace("\\n", "\n");
                string[] strs = line.Split('=');
                if (strs.Length < 2)
                    SaveError(contents[i]);

                strings.Add(strs[0], strs[1]);
            }
        }
        catch
        {
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
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastLookup
{
    class Program
    {
        static void Main(string[] args)
        {
            string lookupFile = @"c:\temp\lookupdic.txt";
            string exportFile = @"c:\temp\output.txt";
            LookupArray la = new LookupArray(5, 10, "abcdefghijklmnopqrstuvwxyz".ToArray(), true);

            using (StreamReader sr = new StreamReader(lookupFile))
            {
                while (sr.Peek() >= 0) { 
                    string[] ss = sr.ReadLine().ToLower().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    la.AddLookupName(int.Parse(ss[0].Trim()), ss[1].Trim());
                }
            }

            using (StreamWriter sw = new StreamWriter(exportFile))
            {
                foreach (string s in la.Export())
                    sw.WriteLine(s);
            }
        }
    }
}

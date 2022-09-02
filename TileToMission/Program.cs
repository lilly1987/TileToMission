using System.Text;
using System.Text.RegularExpressions;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        //static string[] tiles;
        static IEnumerable<string?> dirs;
        static string[] missions;
        static Dictionary<string, StringBuilder> dird = new Dictionary<string, StringBuilder>();
        static Dictionary<string, string> reps = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("need folder\nany key exit");
                Console.ReadLine();
                return;
            }

            

            //Console.WriteLine(args[0]);
            FileAttributes chkAtt = File.GetAttributes(args[0]);
            if ((chkAtt & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // 디렉토리일 경우
                //Console.WriteLine("folder");
                dirs = Directory.GetDirectories(args[0] + "\\biomes").Select(x => Path.GetFileName(x));
                missions = Directory.GetFiles(args[0] + "\\missions", "*.mission.tmp", SearchOption.AllDirectories);
            }

            string tTxt;
            dird["all"] = new StringBuilder();
            foreach (var item in dirs)
            {
                //Console.WriteLine(item);
                dird[item] = new StringBuilder();
                foreach (var tile in Directory.GetFiles(args[0] + "\\biomes\\"+ item, "*.tile", SearchOption.AllDirectories))
                {
                    if (Regex.IsMatch(File.ReadAllText(tile), "is_hidden\\s+\"1\""))
                    {
                        continue;
                    }
                    //Console.WriteLine(tile);
                    tTxt = tile.Replace(args[0] + "\\", String.Empty).Replace('\\', '/') + "\n";
                    dird["all"].Append(tTxt);
                    dird[item].Append(tTxt);
                }
            }

            //File.ReadAllText(args[0] + "\\missions\\#key#value.txt")
            string[] t;
            foreach (var item in File.ReadAllLines(args[0] + "\\missions\\#key#value.txt"))
            {
                t = item.Split("\t", 2);
                //Console.WriteLine($"{item} , {t.Length}");
                //Console.WriteLine($"{t[0]}");
                if (t.Length==2)
                {
                    //Console.WriteLine($"{t[0]} , {t[1]}");
                    reps[t[0]] = t[1];
                }
            } 
            foreach (var item in dird.Keys)
            {
                reps["//#tile-" + item + "\r?\n"] = dird[item].ToString();
            }

            string mTxt;
            foreach (var mission in missions)
            {
                Console.WriteLine(mission);
                mTxt = File.ReadAllText(mission);

                foreach (var item in reps.Keys)
                {
                    mTxt = Regex.Replace(mTxt, item, reps[item]);
                }
                File.WriteAllText(Path.ChangeExtension(mission, null), mTxt);
            }

            //Console.ReadLine();
        }

    }

}
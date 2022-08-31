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
        static Dictionary<string, string> txtd = new Dictionary<string, string>();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("need folder");
            }
            Console.WriteLine(args[0]);
            FileAttributes chkAtt = File.GetAttributes(args[0]);
            if ((chkAtt & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // 디렉토리일 경우
                //Console.WriteLine("folder");
                //tiles = Directory.GetFiles(args[0] + "\\biomes", "*.tile", SearchOption.AllDirectories);
                dirs = Directory.GetDirectories(args[0] + "\\biomes").Select(x => Path.GetFileName(x));
                missions = Directory.GetFiles(args[0] + "\\missions", "*.mission.tmp", SearchOption.AllDirectories);
            }

            string tTxt;
            dird["all"] = new StringBuilder();
            foreach (var item in dirs)
            {
                Console.WriteLine(item);
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

            foreach (var item in dird.Keys)
            {
                txtd[item] = dird[item].ToString();
            }

            string mTxt;
            foreach (var mission in missions)
            {
                Console.WriteLine(mission);
                mTxt = File.ReadAllText(mission);

                foreach (var item in txtd.Keys)
                {
                    mTxt = Regex.Replace(mTxt, "//#tile-" + item + "\r?\n", txtd[item]);
                }
                File.WriteAllText(Path.ChangeExtension(mission, null), mTxt);
            }

            //Console.ReadLine();
        }

    }

}
using System.Text;
using System.Text.RegularExpressions;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static string[] tiles;
        //static List<string> tiles2=new List<string>();
        static string[] missions;

        static void Main(string[] args)
        {
            if (args.Length==0)
            {
                Console.WriteLine("need folder");
            }
            Console.WriteLine(args[0]);
            FileAttributes chkAtt = File.GetAttributes(args[0]);
            if ((chkAtt & FileAttributes.Directory) == FileAttributes.Directory)
            {
                // 디렉토리일 경우
                //Console.WriteLine("folder");
                tiles=Directory.GetFiles(args[0] + "\\biomes", "*.tile", SearchOption.AllDirectories);
                missions=Directory.GetFiles(args[0]+ "\\missions", "*.mission.tmp", SearchOption.AllDirectories);
            }

            //tiles2= tiles.Select(s => s.Replace(args[0]+"\\", String.Empty).Replace('\\','/')).ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var tile in tiles)
            {
                if (Regex.IsMatch(File.ReadAllText(tile), "is_hidden\\s+\"1\""))
                {
                    continue;
                }
                Console.WriteLine(tile);
                //tiles2.Add(tile.Replace(args[0] + "\\", String.Empty).Replace('\\', '/'));
                sb.Append(tile.Replace(args[0] + "\\", String.Empty).Replace('\\', '/')+"\n");
            }
            var tileTxt= sb.ToString();
            string mTxt;
            foreach (var mission in missions)
            {
                Console.WriteLine(mission);
                mTxt = File.ReadAllText(mission);
                File.WriteAllText(Path.ChangeExtension(mission,null), Regex.Replace(mTxt, "//#tile-all\r?\n", tileTxt));
            }

            Console.ReadLine();
        }

    }

}
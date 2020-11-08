using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace winfontloader
{
    class Program
    {
        [DllImport("Gdi32.dll")] static extern int AddFontResourceA(string file);

        static void Main(string[] args)
        {
            // https://docs.microsoft.com/pt-br/windows/win32/api/wingdi/nf-wingdi-addfontresourcea

            try
            {
                string targetFolder = string.Empty;

                while(!Directory.Exists(targetFolder)) 
                {
                    Console.WriteLine("Type a valid directory path to load from or type 'exit' to quit.");
                    string typed = Console.ReadLine();

                    if(typed == "exit") throw new Exception();
                    targetFolder = typed;
                }

                LoadFonts(targetFolder);
                Console.ReadKey();
            }
            catch
            {
                Console.WriteLine("Exiting!");
            }

        }

        static void LoadFonts(string targetFolder) 
        {
            Console.WriteLine($"Loading fonts from {targetFolder} into system!");

            // All possible font types that can be loaded.
            Func<string, bool> predicate = f => 
                   f.EndsWith(".fon")
                || f.EndsWith(".fnt")
                || f.EndsWith(".ttf")
                || f.EndsWith(".ttc")
                || f.EndsWith(".fot")
                || f.EndsWith(".otf")
                || f.EndsWith(".mmm")
                || f.EndsWith(".pfb")
                || f.EndsWith(".pfm");

            // Iterate and load all files from the folder.
            foreach (var fontfile in Directory.GetFiles(targetFolder)
                .Where(predicate))
            {
                try
                {
                    if(AddFontResourceA(fontfile) == 0) throw new Exception();
                    Console.WriteLine($"Fontfile {fontfile} loaded.");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"An error ({e.Message}) occurred while loading the fontfile {fontfile}.");
                }
            }

        }

    }
}

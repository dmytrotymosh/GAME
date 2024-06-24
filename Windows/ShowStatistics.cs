using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class ShowStatistics(string nickname) : WindowBase
    {
        public Dictionary<string, (string, string)> GetStatistics(string nickname)
        {
            Dictionary<string, (string, string)> data = new Dictionary<string, (string, string)>();
            string playerStaticticsPath = $"GAME\\Players\\{nickname}\\Statistics";
            string[] files = Directory.GetFiles(playerStaticticsPath);
            if (files.Length == 0)
            {
                data[""] = ("You have not any record here!", "");
                return data;
            }
            foreach (string file in files)
            {
                string dataTitle = Path.GetFileNameWithoutExtension(file);
                string[] dataInfo = File.ReadAllLines(file);
                data[dataTitle] = (dataInfo[0], dataInfo[1]);
            }
            return data;
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("Y O U R  S T A T I S T I C S");
            Dictionary<string, (string, string)> data = GetStatistics(nickname);
            foreach (string key in data.Keys)
            {
                string value = data[key].Item1;
                string difficult = data[key].Item2;
                CenteredColoredWrite((windowWidth / 3), key, "inline", ConsoleColor.DarkYellow);
                CenteredColoredWrite((windowWidth / 3), value, "inline", ConsoleColor.DarkYellow);
                if (difficult == "easy")
                {
                    CenteredColoredWrite((windowWidth / 4), difficult, "inline", ConsoleColor.Green);
                }
                else if (difficult == "normal")
                {
                    CenteredColoredWrite((windowWidth / 4), difficult, "inline", ConsoleColor.DarkBlue);
                }
                else if (difficult == "hard")
                {
                    CenteredColoredWrite((windowWidth / 4), difficult, "inline", ConsoleColor.DarkRed);
                }
                Console.WriteLine();
                if (key == data.Last().Key)
                {
                    WriteSeparator("double");
                }
                else
                {
                    WriteSeparator("single");
                }
            }
            int startY = Console.CursorTop;
            WriteOptionsList(options, activeIndex);
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                if (option.Key == ConsoleKey.UpArrow)
                {
                    WriteOption(startY + activeIndex * 2, options[activeIndex], false);
                    activeIndex = (activeIndex > 0) ? activeIndex - 1 : options.Length - 1;
                    WriteOption(startY + activeIndex * 2, options[activeIndex], true);
                }
                else if (option.Key == ConsoleKey.DownArrow)
                {
                    WriteOption(startY + activeIndex * 2, options[activeIndex], false);
                    activeIndex = (activeIndex + 1) % options.Length;
                    WriteOption(startY + activeIndex * 2, options[activeIndex], true);
                }
                else if (option.Key == ConsoleKey.Enter)
                {
                    switch (activeIndex)
                    {
                        case 0:
                            chosen = "RETURN";
                            break;
                    }
                    if (chosen == "RETURN")
                    {
                        Console.Clear();
                        // return chosen;
                        Continue(chosen);
                    }
                }
            }
        }
        public void Continue(string next)
        {
            if (next == "RETURN")
            {
                AccountMenu accountMenu = new AccountMenu(nickname);
                accountMenu.Print();
            }
        }
    }
}

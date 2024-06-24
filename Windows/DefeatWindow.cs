using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    class DefeatWindow : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Return" };
            int activeIndex = 0;

            string[] defeatLogo = new string[]
            {
                " ██████  ███████ ███████ ███████  █████  ████████ ",
                " ██   ██ ██      ██      ██      ██   ██    ██    ",
                " ██   ██ █████   █████   █████   ███████    ██    ",
                " ██   ██ ██      ██      ██      ██   ██    ██    ",
                " ██████  ███████ ██      ███████ ██   ██    ██    ",
            };
            Console.WriteLine();
            foreach (string line in defeatLogo)
            {
                CenteredColoredWrite(windowWidth, line, "wrap", ConsoleColor.DarkYellow);
            }
            WriteWindowTitle("O O P S . . . T R Y  A G A I N!");
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
                    if (!string.IsNullOrEmpty(chosen))
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
                MainMenu mainMenu = new MainMenu();
                mainMenu.Print();
            }
        }
    }
}

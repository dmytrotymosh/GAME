using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    class MainMenu : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Play with account", "Play without account", "Exit" };
            int activeIndex = 0;

            WriteLogo();
            WriteWindowTitle("W E L C O M E");
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
                            chosen = "WITH_ACCOUNT";
                            break;
                        case 1:
                            chosen = "WITHOUT_ACCOUNT";
                            break;
                        case 2:
                            chosen = "EXIT";
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
            if (next == "WITH_ACCOUNT")
            {
                HaveAccountMenu haveAccountMenu = new HaveAccountMenu();
                haveAccountMenu.Print();
            }
            else if (next == "WITHOUT_ACCOUNT")
            {
                LevelMenu levelMenu = new LevelMenu(null);
                levelMenu.Print();
            }
            else if (next == "EXIT")
            {
                return;
            }
        }
    }
}

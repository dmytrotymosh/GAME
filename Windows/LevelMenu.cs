using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{ 
    class LevelMenu(string? nickname) : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Easy", "Normal", "Hard", "Emulate", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("C H O S E  D I F F I C U L T Y  L E V E L");
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
                            chosen = "EASY";
                            break;
                        case 1:
                            chosen = "NORMAL";
                            break;
                        case 2:
                            chosen = "HARD";
                            break;
                        case 3:
                            chosen = "EMULATE";
                            break;
                        case 4:
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
            else
            {
                InstructionMenu instructionMenu = new InstructionMenu(next, nickname);
                instructionMenu.Print();
            }
        }
    }
}

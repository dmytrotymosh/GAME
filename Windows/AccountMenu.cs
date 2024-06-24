using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class AccountMenu(string nickname) : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Play", "Show statistics", "Change the password", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle($"W E L C O M E, {nickname}");
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
                            chosen = "PLAY";
                            break;
                        case 1:
                            chosen = "SHOW_STATISTICS";
                            break;
                        case 2:
                            chosen = "CHANGE_THE_PASSWORD";
                            break;
                        case 3:
                            chosen = "RETURN";
                            break;
                    }
                    if (!string.IsNullOrEmpty(chosen))
                    {
                        Console.Clear();
                        // return chosen;
                        if (chosen == "RETURN")
                        {
                            Continue(chosen, null);
                        }
                        else
                        {
                            Continue(chosen, nickname);
                        }
                    }
                }
            }
        }
        public void Continue(string next, string? nickname)
        {
            if (next == "RETURN")
            {
                AutorizationMenu autorizationMenu = new AutorizationMenu();
                autorizationMenu.Print();
            }
            else if (next == "CHANGE_THE_PASSWORD")
            {
                ChangePassword changePassword = new ChangePassword(nickname);
                changePassword.Print();
            }
            else if (next == "SHOW_STATISTICS")
            {
                ShowStatistics showStatistics = new ShowStatistics(nickname);
                showStatistics.Print();
            }
            else if (next == "PLAY")
            {
                LevelMenu levelMenu = new LevelMenu(nickname);
                levelMenu.Print();
            }
        }
    }
}

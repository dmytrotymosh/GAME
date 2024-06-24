using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class RegisterMenu : WindowBase
    {
        public bool Registration(string nickname, string password)
        {
            nickname = nickname.ToLower().Trim();
            password = password.Trim();
            string playerPath = $"GAME\\Players\\{nickname}";
            if (!Directory.Exists(playerPath))
            {
                string passwordPath = $"GAME\\Players\\{nickname}\\password.txt";
                string statisticsPath = $"GAME\\Players\\{nickname}\\Statistics";
                Directory.CreateDirectory(playerPath);
                Directory.CreateDirectory(statisticsPath);
                File.WriteAllText(passwordPath, password);
                return true;
            }
            else
            {
                return false;
            }
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Continue", "Change the nickname", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("C R E A T E I N G  A C C O U N T");
            ColoredWrite(" Enter you nickaname here: ", "inline", ConsoleColor.DarkYellow);
            string nickname = Console.ReadLine();
            WriteSeparator("single");
            ColoredWrite(" Enter you password here: ", "inline", ConsoleColor.DarkYellow);
            string password = Console.ReadLine();
            WriteSeparator("double");
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
                            chosen = "CONTINUE";
                            break;
                        case 1:
                            chosen = "CHANGE_THE_NICKNAME";
                            break;
                        case 2:
                            chosen = "RETURN";
                            break;
                    }
                    if (chosen == "RETURN" || chosen == "CHANGE_THE_NICKNAME")
                    {
                        Console.Clear();
                        // return chosen;
                        Continue(chosen, null);
                    }
                    if (nickname != "" && password != "")
                    {
                        bool wasRegistred = Registration(nickname, password);
                        if (wasRegistred)
                        {
                            chosen = "TRUE";
                            Console.Clear();
                            Continue(chosen, nickname);
                        }
                        else
                        {
                            chosen = "WRONG_DATA";
                            Console.Clear();
                            Continue(chosen, null);
                        }
                    }
                    else
                    {
                        chosen = "EMPTY_DATA";
                        Console.Clear();
                        Continue(chosen, null);
                    }
                }
            }
        }
        public void Continue(string next, string? nickname)
        {
            if (next == "RETURN")
            {
                HaveAccountMenu haveAccountMenu = new HaveAccountMenu();
                haveAccountMenu.Print();
            }
            else if (next == "CHANGE_THE_NICKNAME")
            {
                RegisterMenu registerMenu = new RegisterMenu();
                registerMenu.Print();
            }
            else if (next == "TRUE")
            {
                AutorizationMenu autorizationMenu = new AutorizationMenu();
                autorizationMenu.Print();
            }
            else if (next == "WRONG_DATA")
            {
                InvalidData invalidData = new InvalidData("Wrong nickname or password!", "registration", null);
                invalidData.Print();
            }
            else if (next == "EMPTY_DATA")
            {
                InvalidData invalidData = new InvalidData("Nickname or password can not be empty!", "registration", null);
                invalidData.Print();
            }
        }
    }
}

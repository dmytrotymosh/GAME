using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class AutorizationMenu : WindowBase
    {
        public bool Autorization(string nickname, string password)
        {
            nickname = nickname.ToLower().Trim();
            password = password.Trim();
            string playerPath = "GAME\\Players";
            string[] players = Directory.GetDirectories(playerPath);
            foreach (string player in players)
            {
                string playerName = Path.GetFileName(player);
                if (playerName == nickname)
                {
                    string passwordPath = Path.Combine(player, "password.txt");
                    string playerPassword = File.ReadAllText(passwordPath);
                    if (playerPassword == password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Continue", "Change the nickname", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("A U T O R I Z A T I O N");
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
                        bool wasAutorized = Autorization(nickname, password);
                        if (wasAutorized)
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
                AutorizationMenu autorizationMenu = new AutorizationMenu();
                autorizationMenu.Print();
            }
            else if (next == "TRUE")
            {
                AccountMenu accountMenu = new AccountMenu(nickname);
                accountMenu.Print();
            }
            else if (next == "WRONG_DATA")
            {
                InvalidData invalidData = new InvalidData("Wrong nickname or password!", "autorization", null);
                invalidData.Print();
            }
            else if (next == "EMPTY_DATA")
            {
                InvalidData invalidData = new InvalidData("Nickname or password can not be empty!", "autorization", null);
                invalidData.Print();
            }
        }
    }
}

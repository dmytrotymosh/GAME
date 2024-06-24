using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class ChangePassword(string nickname) : WindowBase
    {
        public void PasswordChange(string nickname, string password)
        {
            password = password.Trim();
            string playerPasswordPath = $"GAME\\Players\\{nickname}\\password.txt";
            File.WriteAllText(playerPasswordPath, password);
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Continue", "Enter another password", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("C H A N G E  T H E  P A S S W O R D");
            ColoredWrite(" Enter you new password here: ", "inline", ConsoleColor.DarkYellow);
            string newPassword = Console.ReadLine();
            newPassword = newPassword.Trim();
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
                            chosen = "ENTER_ANOTHER";
                            break;
                        case 2:
                            chosen = "RETURN";
                            break;
                    }
                    if (chosen == "RETURN" || chosen == "ENTER_ANOTHER")
                    {
                        Console.Clear();
                        // return chosen;
                        Continue(chosen);
                    }
                    else if (chosen == "CONTINUE")
                    {
                        if (newPassword != "")
                        {
                            PasswordChange(nickname, newPassword);
                            chosen = "RETURN";
                            Console.Clear();
                            Continue(chosen);
                        }
                        else
                        {
                            chosen = "EMPTY_DATA";
                            Console.Clear();
                            Continue(chosen);
                        }
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
            if (next == "ENTER_ANOTHER")
            {
                ChangePassword changePassword = new ChangePassword(nickname);
                changePassword.Print();
            }
            if (next == "EMPTY_DATA")
            {
                InvalidData invalidData = new InvalidData("You can not have empty password!", "change_password", nickname);
                invalidData.Print();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GAME.Windows
{
    public class InvalidData(string text, string from, string? nickname) : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("I N V A L I D  D A T A");
            CenteredColoredWrite(windowWidth, $"{text}", "wrap", ConsoleColor.DarkRed);
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
                if (from == "autorization")
                {
                    AutorizationMenu autorizationMenu = new AutorizationMenu();
                    autorizationMenu.Print();
                }
                else if (from == "registration")
                {
                    RegisterMenu registerMenu = new RegisterMenu();
                    registerMenu.Print();
                }
                else if (from == "change_password")
                {
                    ChangePassword changePassword = new ChangePassword(nickname);
                    changePassword.Print();
                }
            }
        }
    }
}

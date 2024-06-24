using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class WindowBase
    {
        static string[] logo = new string[]
        {
            " ███    ██ ███████ ██     ██ ██████   █████  ██      ██       ██████   █████  ███    ███ ███████ ",
            " ████   ██ ██      ██     ██ ██   ██ ██   ██ ██      ██      ██       ██   ██ ████  ████ ██      ",
            " ██ ██  ██ █████   ██  █  ██ ██████  ███████ ██      ██      ██   ███ ███████ ██ ████ ██ █████   ",
            " ██  ██ ██ ██      ██ ███ ██ ██   ██ ██   ██ ██      ██      ██    ██ ██   ██ ██  ██  ██ ██      ",
            " ██   ████ ███████  ███ ███  ██████  ██   ██ ███████ ███████  ██████  ██   ██ ██      ██ ███████ ",
        };

        int windowWidth = logo[0].Length;
        public void ColoredWrite(string text, string mode, ConsoleColor textColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            switch (mode)
            {
                case "inline":
                    Console.Write(text);
                    break;
                case "wrap":
                    Console.WriteLine(text);
                    break;
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void CenteredColoredWrite(int lineWidth, string text, string mode, ConsoleColor textColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            string paddedText = text.PadLeft((lineWidth + text.Length) / 2).PadRight(lineWidth);
            ColoredWrite(paddedText, mode, textColor, backgroundColor);
        }

        public void WriteLogo()
        {
            Console.WriteLine();
            foreach (string line in logo)
            {
                ColoredWrite(line, "wrap", ConsoleColor.DarkYellow);
            }
        }

        public void WriteSeparator(string type)
        {
            if (type == "double")
            {
                ColoredWrite(new string('═', windowWidth), "wrap", ConsoleColor.DarkYellow);
            }
            else if (type == "single")
            {
                ColoredWrite(new string('—', windowWidth), "wrap", ConsoleColor.DarkYellow);
            }
        }

        public void WriteWindowTitle(string title)
        {
            WriteSeparator("double");
            CenteredColoredWrite(windowWidth, title, "wrap", ConsoleColor.DarkYellow);
            WriteSeparator("double");
        }

        public void WriteOption(int yPos, string text, bool isActive)
        {
            Console.SetCursorPosition(0, yPos);
            ConsoleColor backgroundColor = isActive ? ConsoleColor.DarkRed : ConsoleColor.Black;
            CenteredColoredWrite(windowWidth, text, "wrap", ConsoleColor.DarkYellow, backgroundColor);
        }

        public void WriteOptionsList(string[] options, int activeIndex)
        {
            int startY = Console.CursorTop;
            for (int i = 0; i < options.Length; i++)
            {
                WriteOption(startY + i * 2, options[i], i == activeIndex);
                if (i < options.Length - 1)
                {
                    WriteSeparator("single");
                    Console.SetCursorPosition(0, startY + i * 2 + 1);
                }
            }
            WriteSeparator("double");
            WriteOptionsListExplanation();
        }

        public void WriteOptionsListExplanation()
        {
            CenteredColoredWrite(windowWidth, "Select one of the options by pressing the appropriate button on the keyboard", "wrap", ConsoleColor.White);
            WriteSeparator("double");
        }

        public void ClearString(int YPosition)
        {
            Console.SetCursorPosition(0, YPosition);
            Console.WriteLine(new string(' ', windowWidth));
            Console.SetCursorPosition(0, YPosition);
        }

        public int GetWindowWidth()
        {
            return windowWidth;
        }
    }
}

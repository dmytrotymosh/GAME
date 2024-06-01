using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace GAME
{
    class WindowBase()
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
        public void ColoredWrite(string text, string mode, string textColor, string backgroundColor = "black")
        {
            switch (textColor)
            {
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "darkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            switch (backgroundColor)
            {
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
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
        public void CenteredColoredWrite(int lineWidth, string text, string mode, string textColor, string backgroundColor = "black")
        {
            ColoredWrite(new string(' ', (lineWidth - text.Length) / 2) + text, mode, textColor, backgroundColor);
        }
        public void WriteLogo()
        {
            Console.WriteLine();
            foreach (string line in logo)
            {
                ColoredWrite(line, "wrap", "darkyellow");
            }
        }
        public void WriteSeparator(string type)
        {
            if (type == "double")
            {
                ColoredWrite(new string('═', windowWidth), "wrap", "darkyellow");
            }
            else if (type == "single")
            {
                ColoredWrite(new string('—', windowWidth), "wrap", "darkyellow");
            }
        }
        public void WriteWindowTitle(string title)
        {
            WriteSeparator("double");
            CenteredColoredWrite(windowWidth, title, "wrap", "darkyellow");
            WriteSeparator("double");
        }
        public void WriteOption(int number, string text, bool isLast)
        {
            ColoredWrite($" {number}", "inline", "darkyellow");
            CenteredColoredWrite(windowWidth - 4, text, "inline", "darkyellow");
            Console.WriteLine();
            if (!isLast)
            {
                WriteSeparator("single");
            }
            else
            {
                WriteSeparator("double");
            }
        }
        public void WriteOptionsList(string[] options)
        {
            for (int i = 0, j = 1; i < options.Length; i += 1, j += 1)
            {
                if (i != options.Length - 1)
                {
                    WriteOption(j, options[i], false);
                }
                else
                {
                    WriteOption(j, options[i], true);
                }
            }
        }
        public void WriteOptionsListExplanation()
        {
            CenteredColoredWrite(windowWidth, "Select one of the options by pressing the appropriate button on the keyboard", "wrap", "");
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
    class MainMenu : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("W E L C O M E");
            string[] options = new string[] { "Play with account", "Play without account", "Exit" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "WITH_ACCOUNT";
                    CenteredColoredWrite(windowWidth, "Chosen play with the account, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "WITHOUT_ACCOUNT";
                    CenteredColoredWrite(windowWidth, "Chosen continue without account, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D3)
                {
                    chosen = "EXIT";
                    CenteredColoredWrite(windowWidth, "Chosen exit, press Enter to confirm", "wrap", "darkblue");
                }
                if (chosen != "" && option.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return chosen;
                }
            }
        }
    }
    class LevelMenu : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("C H O S E  D I F F I C U L T Y  L E V E L");
            string[] options = new string[] { "Easy", "Normal", "Hard", "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "EASY";
                    CenteredColoredWrite(windowWidth, "Chosen easy, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "NORMAL";
                    CenteredColoredWrite(windowWidth, "Chosen normal, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D3)
                {
                    chosen = "HARD";
                    CenteredColoredWrite(windowWidth, "Chosen hard, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D4)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (chosen != "" && option.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return chosen;
                }
            }
        }
    }
    class HaveAccountMenu() : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("D O  Y O U  H A V E  A N  A C C O U N T ?");
            string[] options = new string[] { "Yes", "No", "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "YES";
                    CenteredColoredWrite(windowWidth, "Chosen Yes, press Enter to autorization", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "NO";
                    CenteredColoredWrite(windowWidth, "Chosen No, press Enter to register", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D3)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (chosen != "" && option.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return chosen;
                }
            }
        }
    }
    class RegisterMenu() : WindowBase
    {
        public bool Registration(string nickname, string password)
        {
            nickname = nickname.ToLower().Trim();
            password = password.Trim();
            string playerPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}";
            if (!Directory.Exists(playerPath))
            {
                string passwordPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}\\password.txt";
                string statisticsPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}\\Statistics";
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
            WriteLogo();
            WriteWindowTitle("C R E A T E I N G  A C C O U N T");
            ColoredWrite(" Enter you nickaname here: ", "inline", "darkyellow");
            string nickname = Console.ReadLine();
            WriteSeparator("single");
            ColoredWrite(" Enter you password here: ", "inline", "darkyellow");
            string password = Console.ReadLine();
            WriteSeparator("double");
            string[] options = new string[] { "Return", "Change the nickname" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            CenteredColoredWrite(windowWidth, "Account with name " + nickname.Trim() + " will be created, press Enter to confirm", "wrap", "darkblue");
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 12 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "CHANGE_THE_NICKNAME";
                    CenteredColoredWrite(windowWidth, "Chosen change the nickname, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen != "")
                    {
                        Console.Clear();
                        return chosen;
                    }
                    if (nickname != "" && password != "")
                    {
                        bool wasRegistred = Registration(nickname, password);
                        if (wasRegistred)
                        {
                            chosen = "TRUE";
                            Console.Clear();
                            return chosen;
                        }
                        else
                        {
                            chosen = "WRONG_DATA";
                            Console.Clear();
                            return chosen;
                        }
                    }
                    else
                    {
                        chosen = "EMPTY_DATA";
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    internal class AccountIsExist() : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("C R E A T E I N G  A C C O U N T");
            CenteredColoredWrite(windowWidth, "Account with this name is existing!", "wrap", "darkred", "darkgreen");
            WriteSeparator("double");
            string[] options = new string[] { "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 10 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (chosen != "" && option.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return chosen;
                }
            }
        }
    }
    internal class InvalidData(string text) : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("I N V A L I D  D A T A");
            CenteredColoredWrite(windowWidth, $"{text}", "wrap", "darkred");
            WriteSeparator("double");
            string[] options = new string[] { "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 10 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (chosen != "" && option.Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    return chosen;
                }
            }
        }
    }
    class AutorizationMenu() : WindowBase
    {
        public bool Autorization(string nickname, string password)
        {
            nickname = nickname.ToLower().Trim();
            password = password.Trim();
            string playerPath = "C:\\Users\\ADMIN\\source\\repos\\GAME\\Players";
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
        public (string, string?) Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("A U T O R I Z A T I O N");
            ColoredWrite(" Enter you nickaname here: ", "inline", "darkyellow");
            string nickname = Console.ReadLine();
            WriteSeparator("single");
            ColoredWrite(" Enter you password here: ", "inline", "darkyellow");
            string password = Console.ReadLine();
            WriteSeparator("double");
            string[] options = new string[] { "Return", "Change the nickname" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            CenteredColoredWrite(windowWidth, "Account with name " + nickname.Trim() + " will be autorizated, press Enter to confirm", "wrap", "darkblue");
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 12 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "CHANGE_THE_NICKNAME";
                    CenteredColoredWrite(windowWidth, "Chosen change the nickname, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "RETURN" || chosen == "CHANGE_THE_NICKNAME")
                    {
                        Console.Clear();
                        return (chosen, null);
                    }
                    if (nickname != "" && password != "")
                    {
                        bool wasAutorized = Autorization(nickname, password);
                        if (wasAutorized)
                        {
                            chosen = "TRUE";
                            Console.Clear();
                            return (chosen, nickname);
                        }
                        else
                        {
                            chosen = "WRONG_DATA";
                            Console.Clear();
                            return (chosen, null);
                        }
                    }
                    else
                    {
                        chosen = "EMPTY_DATA";
                        Console.Clear();
                        return (chosen, null);
                    }
                }
            }
        }
    }
    class AccountMenu(string nickname) : WindowBase
    {
        public void AddToStatistic(Dictionary<string, int> time)
        {
            string data = $"{time["minutes"]} minutes {time["seconds"]} seconds {time["milliseconds"]} milliseconds";
            DateTime currentTime = DateTime.Now;
            string fileName = currentTime.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
            nickname = nickname.ToLower().Trim();
            string playerStaticticsPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}\\Statistics";
            string fullPath = Path.Combine(playerStaticticsPath, fileName);
            File.WriteAllText(fullPath, data);
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle($"W E L C O M E, {nickname}");
            string[] options = new string[] { "Play", "Show statistic", "Change password", "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while(true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1) 
                {
                    chosen = "PLAY";
                    CenteredColoredWrite(windowWidth, "Chosen play, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "SHOW_STATISTIC";
                    CenteredColoredWrite(windowWidth, "Chosen show statistic, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D3)
                {
                    chosen = "CHANGE_THE_PASSWORD";
                    CenteredColoredWrite(windowWidth, "Chosen change the password, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D4)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen != "")
                    {
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    class ShowStatistics() : WindowBase
    {
        public Dictionary<string, string> GetStatistics(string nickname)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            string playerStaticticsPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}\\Statistics";
            string[] files = Directory.GetFiles(playerStaticticsPath);
            if (files.Length == 0)
            {
                data[""] = "You have not any record here!";
                return data;
            }
            foreach (string file in files)
            {
                string dataTitle = Path.GetFileNameWithoutExtension(file);
                string dataInfo = File.ReadAllText(file);
                data[dataTitle] = dataInfo;
            }
            return data;
        }
        public string Print(string nickname)
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("Y O U R  S T A T I S T I C S");
            Dictionary<string, string> data = GetStatistics(nickname);
            foreach (string key in data.Keys)
            {
                string value = data[key];
                CenteredColoredWrite((windowWidth / 2), key, "inline", "darkyellow");
                CenteredColoredWrite((windowWidth / 2), value, "inline", "darkyellow");
                Console.WriteLine();
                if (key == data.Last().Key) {
                    WriteSeparator("double");
                } else
                {
                    WriteSeparator("single");
                }
            }
            string[] options = new string[] { "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (data.Count * 2) + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "RETURN")
                    {
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    class ChangePassword() : WindowBase
    {
        public void PasswordChange(string nickname, string password)
        {
            password = password.Trim();
            string playerPasswordPath = $"C:\\Users\\ADMIN\\source\\repos\\GAME\\Players\\{nickname}\\password.txt";
            File.WriteAllText(playerPasswordPath, password);
        }
        public string Print(string nickname)
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("C H A N G E  T H E  P A S S W O R D");
            ColoredWrite(" Enter you new password here: ", "inline", "darkyellow");
            string newPassword = Console.ReadLine();
            newPassword = newPassword.Trim();
            WriteSeparator("double");
            string[] options = new string[] { "Return", "Enter another password" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            CenteredColoredWrite(windowWidth, "Password of " + nickname.Trim() + " will be changed to " + newPassword + ", press Enter to confirm", "wrap", "darkblue");
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 10 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "ENTER_ANOTHER";
                    CenteredColoredWrite(windowWidth, "Chosen enter another password, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "RETURN" || chosen == "ENTER_ANOTHER")
                    {
                        Console.Clear();
                        return chosen;
                    }
                    else if (newPassword != "")
                    {
                        PasswordChange(nickname, newPassword);
                        chosen = "RETURN";
                        Console.Clear();
                        return (chosen);
                    }
                    else
                    {
                        chosen = "EMPTY_DATA";
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    class InstructionMenu() : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            WriteLogo();
            WriteWindowTitle("I N S T R U C T I O N");
            ColoredWrite(" Ціллю гри є зібрати усі енергетичні кулі. Збирати кулі може лише м'яч, але безпосереднього\n" +
                " контролю над його рухом ви не маєте. Натисканням клавіш стрілок ви можете переміщувати гравця\n" +
                " по ігровому полю, натисканням квавіш F1 та F2 ви можете розставляти щити, які змінюють рух м'яча\n" +
                " на 90 градусів. Стіни змінюють траєкторію руху м'яча на 180 градусів. Стіни є непрохідними\n" +
                " для жодного елементу гри", "wrap", "darkyellow");
            WriteSeparator("double");
            string[] options = new string[] { "OK", "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 14 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "OK";
                    CenteredColoredWrite(windowWidth, "Chosen OK, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.D2)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "OK" || chosen == "RETURN")
                    {
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    class DefeatWidow() : WindowBase
    {
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
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
                CenteredColoredWrite(windowWidth, line, "wrap", "darkyellow");
            }
            WriteWindowTitle("O O P S . . . T R Y  A G A I N!");
            string[] options = new string[] { "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 8 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "RETURN")
                    {
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    class VictoryWidow() : WindowBase
    {
        public string Print(Dictionary<string, int> time)
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] defeatLogo = new string[]
            {
                " ██    ██ ██  ██████ ████████  ██████  ██████  ██    ██ ",
                " ██    ██ ██ ██         ██    ██    ██ ██   ██  ██  ██  ",
                " ██    ██ ██ ██         ██    ██    ██ ██████    ████   ",
                "  ██  ██  ██ ██         ██    ██    ██ ██   ██    ██    ",
                "   ████   ██  ██████    ██     ██████  ██   ██    ██    ",
            };
            Console.WriteLine();
            foreach (string line in defeatLogo)
            {
                CenteredColoredWrite(windowWidth, line, "wrap", "darkyellow");
            }
            WriteWindowTitle("G O A A A A A A A A A A A L!");
            CenteredColoredWrite(windowWidth, $"You have did it in {time["minutes"]} minutes {time["seconds"]} seconds {time["milliseconds"]} milliseconds", "wrap", "darkyellow");
            WriteSeparator("double");
            string[] options = new string[] { "Return" };
            WriteOptionsList(options);
            WriteOptionsListExplanation();
            string chosen = "";
            while (true)
            {
                ConsoleKeyInfo option = Console.ReadKey(intercept: true);
                int YPosition = 10 + (options.Length + 1) * 2;
                ClearString(YPosition + 1);
                Console.SetCursorPosition(0, YPosition + 1);
                if (option.Key == ConsoleKey.D1)
                {
                    chosen = "RETURN";
                    CenteredColoredWrite(windowWidth, "Chosen return, press Enter to confirm", "wrap", "darkred");
                }
                if (option.Key == ConsoleKey.Enter)
                {
                    if (chosen == "RETURN")
                    {
                        Console.Clear();
                        return chosen;
                    }
                }
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            string WITH_ACCOUNT;
            string LEVEL;
            MainMenu: MainMenu mainMenu = new MainMenu();
            string mainMenuChosen = mainMenu.Print();
            WITH_ACCOUNT = mainMenuChosen;
            if (mainMenuChosen == "WITH_ACCOUNT")
            {
                HaveAcocunt: HaveAccountMenu haveAccountMenu = new HaveAccountMenu();
                string haveAccountMenuChosen = haveAccountMenu.Print();
                if (haveAccountMenuChosen == "YES")
                {
                    Autorization: AutorizationMenu autorazationMenu = new AutorizationMenu();
                    (string, string?) autorizationMenuChosen = autorazationMenu.Print();
                    if (autorizationMenuChosen.Item1 == "RETURN")
                    {
                        goto HaveAcocunt;
                    }
                    if (autorizationMenuChosen.Item1 == "CHANGE_THE_NICKNAME")
                    {
                        goto Autorization;
                    }
                    if (autorizationMenuChosen.Item1 == "TRUE")
                    {
                        string? nickname = autorizationMenuChosen.Item2;
                        AccountMenu: AccountMenu accountMenu = new AccountMenu(nickname);
                        string accountMenuChosen = accountMenu.Print();
                        if (accountMenuChosen == "PLAY")
                        {
                            LevelMenu: LevelMenu levelMenu = new LevelMenu();
                            string levelMenuChosen = levelMenu.Print();
                            LEVEL = levelMenuChosen;
                            if (levelMenuChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                            else
                            {
                                    InstructionMenu instructionMenu = new InstructionMenu();
                                    string instructionMenuChosen = instructionMenu.Print();
                                    if (instructionMenuChosen == "OK")
                                    {
                                        Game game = new Game();
                                        (string, Dictionary<string, int>?) result = game.StartGame(LEVEL);
                                        if (result.Item1 == "DEFEAT")
                                        {
                                            DefeatWidow defeatWidow = new DefeatWidow();
                                            string defeatWindowChosen = defeatWidow.Print();
                                            if (defeatWindowChosen == "RETURN")
                                            {
                                                goto MainMenu;
                                            }
                                        }
                                        else if (result.Item1 == "WIN")
                                        {
                                        VictoryWidow victoryWidow = new VictoryWidow();
                                        string victoryWindowChosen = victoryWidow.Print(result.Item2);
                                        accountMenu.AddToStatistic(result.Item2);
                                        if (victoryWindowChosen == "RETURN")
                                        {
                                            goto MainMenu;
                                        }
                                    }
                                    }
                                    if (instructionMenuChosen == "RETURN")
                                    {
                                        goto LevelMenu;
                                    }
                            }
                        }
                        if (accountMenuChosen == "SHOW_STATISTIC")
                        {
                            ShowStatistics showStatistics = new ShowStatistics();
                            string showStatisticsChosen = showStatistics.Print(nickname);
                            if (showStatisticsChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                        }
                        if (accountMenuChosen == "CHANGE_THE_PASSWORD")
                        {
                            ChangePassword: ChangePassword changePassword = new ChangePassword();
                            string changePasswordChosen = changePassword.Print(nickname);
                            if (changePasswordChosen == "RETURN")
                            {
                                goto AccountMenu;
                            }
                            if (changePasswordChosen == "ENTER_ANOTHER")
                            {
                                goto ChangePassword;
                            }
                            if (changePasswordChosen == "EMPTY_DATA")
                            {
                                InvalidData invalidData = new InvalidData("You can not have empty password!");
                                string invalidDataChosen = invalidData.Print();
                                if (invalidDataChosen == "RETURN")
                                {
                                    goto ChangePassword;
                                }
                            }
                        }
                        if (accountMenuChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                    if (autorizationMenuChosen.Item1 == "WRONG_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Wrong nickname or password!");
                        string invalidDataChosen = invalidData.Print();
                        if (invalidDataChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                    if (autorizationMenuChosen.Item1 == "EMPTY_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Nickname or password can not be empty!");
                        string invalidDataChosen = invalidData.Print();
                        if (invalidDataChosen == "RETURN")
                        {
                            goto Autorization;
                        }
                    }
                }
                if (haveAccountMenuChosen == "NO")
                {
                    RegisterMenu: RegisterMenu registerMenu = new RegisterMenu();
                    string registerMenuChosen = registerMenu.Print();
                    if (registerMenuChosen == "RETURN")
                    {
                        goto HaveAcocunt;
                    }
                    if (registerMenuChosen == "CHANGE_THE_NICKNAME")
                    {
                        goto RegisterMenu;
                    }
                    if (registerMenuChosen == "TRUE")
                    {
                        goto HaveAcocunt;
                    }
                    if (registerMenuChosen == "WRONG_DATA")
                    {
                        AccountIsExist accountIsExist = new AccountIsExist();
                        string accountIsExistChosen = accountIsExist.Print();
                        if (accountIsExistChosen == "RETURN")
                        {
                            goto RegisterMenu;
                        }
                    }
                    if (registerMenuChosen == "EMPTY_DATA")
                    {
                        InvalidData invalidData = new InvalidData("Nickname or password can not be empty!");
                        string invalidRegistrationDataChosen = invalidData.Print();
                        if (invalidRegistrationDataChosen == "RETURN")
                        {
                            goto RegisterMenu;
                        }
                    }
                }
                if (haveAccountMenuChosen == "RETURN")
                {
                    goto MainMenu;
                }
            }
            else if (mainMenuChosen == "WITHOUT_ACCOUNT")
            {
                LevelMenu: LevelMenu levelMenu = new LevelMenu();
                string levelMenuChosen = levelMenu.Print();
                LEVEL = levelMenuChosen;
                if (levelMenuChosen == "RETURN")
                {
                    goto MainMenu;
                } 
                else
                {
                        InstructionMenu instructionMenu = new InstructionMenu();
                        string instructionMenuChosen = instructionMenu.Print();
                        if (instructionMenuChosen == "OK")
                        {
                            Game game = new Game();
                            (string, Dictionary<string, int>?) result = game.StartGame(LEVEL);
                            if (result.Item1 == "DEFEAT")
                            {
                                DefeatWidow defeatWidow = new DefeatWidow();
                                string defeatWindowChosen = defeatWidow.Print();
                                if (defeatWindowChosen == "RETURN")
                                {
                                    goto MainMenu;
                                }
                            }
                            else if (result.Item1 == "WIN")
                            {
                                VictoryWidow victoryWidow = new VictoryWidow();
                                string victoryWindowChosen = victoryWidow.Print(result.Item2);
                                if (victoryWindowChosen == "RETURN")
                                {
                                    goto MainMenu;
                                }
                            }
                        }
                        if (instructionMenuChosen == "RETURN")
                        {
                            goto LevelMenu;
                        }
                }
            }
            else if (mainMenuChosen == "EXIT")
            {
                return;
            }
        }
    }
}

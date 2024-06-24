using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Windows
{
    public class InstructionMenu(string level, string? nickname) : WindowBase
    {
        public void AddToStatistic(Dictionary<string, int> time, string level, string nickname)
        {
            string data = $"{time["minutes"]} minutes {time["seconds"]} seconds {time["milliseconds"]} milliseconds\n" + level.ToLower();
            DateTime currentTime = DateTime.Now;
            string fileName = currentTime.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";
            nickname = nickname.ToLower().Trim();
            string playerStaticticsPath = $"GAME\\Players\\{nickname}\\Statistics";
            string fullPath = Path.Combine(playerStaticticsPath, fileName);
            File.WriteAllText(fullPath, data);
        }
        public string Print()
        {
            int windowWidth = GetWindowWidth();
            Console.CursorVisible = false;
            string[] options = new string[] { "OK", "Return" };
            int activeIndex = 0;
            WriteLogo();
            WriteWindowTitle("I N S T R U C T I O N");
            ColoredWrite(" Ціллю гри є зібрати усі енергетичні кулі. Збирати кулі може лише м'яч, але безпосереднього\n" +
                " контролю над його рухом ви не маєте. Натисканням клавіш стрілок ви можете переміщувати гравця\n" +
                " по ігровому полю, натисканням квавіш F1 та F2 ви можете розставляти щити, які змінюють рух м'яча\n" +
                " на 90 градусів. Стіни змінюють траєкторію руху м'яча на 180 градусів. Стіни є непрохідними для \n" +
                " жодного елементу гри.", "wrap", ConsoleColor.DarkYellow);
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
                            chosen = "OK";
                            break;
                        case 1:
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
                LevelMenu levelMenu = new LevelMenu(null);
                levelMenu.Print();
            }
            else if (next == "OK")
            {
                Game game = new Game();
                (string, Dictionary<string, int>?) result = game.StartGame(level);
                if (result.Item1 == "DEFEAT")
                {
                    DefeatWindow defeatWidow = new DefeatWindow();
                    defeatWidow.Print();
                }
                else if (result.Item1 == "WIN")
                {
                    if (nickname != null)
                    {
                        AddToStatistic(result.Item2, level, nickname);
                    }
                    VictoryWindow victoryWidow = new VictoryWindow();
                    victoryWidow.Print(result.Item2);
                }
            }
        }
    }
}

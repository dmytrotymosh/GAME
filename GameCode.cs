using System;
using System.Threading;
using System.Transactions;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;
using GAME.Elements;
using System.Diagnostics.Tracing;
using GAME.Levels;

namespace GAME
{
    public enum GameStatus
    {
        InProgress,
        Over,
        Winner,
        Closed
    }
    public class Field
    {
        public int Height;
        public int Width;
        public BaseElement[,] GameField;
        public Ball Ball = new Ball('B', 1, 1, ConsoleColor.White);
        public int EnegryBulletsAmount = 10;
        public int SpikesAmount = 5;
        public GameStatus Status { get; set; }
        private readonly object lockObject = new object();
        public Player Player = new Player('P', 1, 1, ConsoleColor.White);
        public int HorizontalSpeed = 100;
        public int VerticalSpeed = 200;
        public List<(int, int)> Points = new List<(int, int)>();

        public Field(int height, int width)
        {
            Height = height;
            Width = width;
            Status = GameStatus.InProgress;
        }

        public void CreateField(string level)
        {
            GameField = new BaseElement[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (i == 0 || i == Height - 1 || j == 0 || j == Width - 1)
                    {
                        if ((i == 0 && j == 0) || (i == Height - 1 && j == 0) || (i == 0 && j == Width - 1) || (i == Height - 1 && j == Width - 1))
                        {
                            GameField[i, j] = new Wall('+', j, i, ConsoleColor.DarkGreen);
                        }
                        else if (i == 0 || i == Height - 1)
                        {
                            GameField[i, j] = new Wall('-', j, i, ConsoleColor.DarkGreen);
                        }
                        else if (j == 0 || j == Width - 1)
                        {
                            GameField[i, j] = new Wall('|', j, i, ConsoleColor.DarkGreen);
                        }
                    }
                    else if (level == "HARD" && j == (Width / 2))
                    {
                        GameField[i, j] = new Wall('|', j, i, ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        GameField[i, j] = new EmptyCell(' ', i, j, ConsoleColor.Black);
                    }
                }
            }
        }
        public void AddEnergyBullets()
        {
            Random random = new Random();
            for (int i = 0; i < EnegryBulletsAmount; i++)
            {
                int x, y;
                do
                {
                    y = random.Next(1, Height - 2);
                    x = random.Next(1, Width - 2);
                }
                while (GameField[y, x].GetType() != typeof(EmptyCell));
                Points.Add((y, x));
                GameField[y, x] = new EnergyBullet('@', y, x, ConsoleColor.DarkYellow);
            }
        }
        public void AddSpikes()
        {
            Random random = new Random();
            for (int i = 0; i < SpikesAmount; i++)
            {
                int y = random.Next(1, Height - 2);
                int x = random.Next(1, Width - 2);
                GameField[0, x] = new Spike('#', 0, x, ConsoleColor.DarkBlue);
                GameField[Height - 1, x] = new Spike('#', Height - 1, x, ConsoleColor.DarkBlue);
                GameField[y, 0] = new Spike('#', y, 0, ConsoleColor.DarkBlue);
                GameField[y, Width - 1] = new Spike('#', y, Width - 1, ConsoleColor.DarkBlue);
            }
        }
        public void AddBoosters()
        {
            Random random = new Random();
            for (int i = 0; i < 2; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(1, Width - 2);
                    y = random.Next(1, Height - 2);
                }
                while (GameField[y, x].GetType() != typeof(EmptyCell));
                GameField[y, x] = new Booster('?', y, x, ConsoleColor.White, ConsoleColor.Magenta);
            }
        }
        public void AddTeleports()
        {
            Random random = new Random();
            int halfX = Width / 2, halfY = Height / 2;
            Teleport fisrt = Teleport.CreateTeleport(1, halfX, 1, halfY, this);
            Teleport second = Teleport.CreateTeleport(halfX, Width, halfY, Height, this);
            fisrt.Connect(second);
        }
        public void ColoredWrite(string text, string mode, ConsoleColor textColor, ConsoleColor backgroundColor)
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

        public void DrawField()
        {
            lock (lockObject)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        ColoredWrite(GameField[i, j].Skin.ToString(), "inline", GameField[i, j].Color, GameField[i, j].BgColor);
                    }
                    Console.WriteLine();
                }
            }
        }
        public void CenteredColoredWrite(int lineWidth, string text, string mode, ConsoleColor textColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            ColoredWrite(new string(' ', (lineWidth - text.Length) / 2) + text, mode, textColor, backgroundColor);
        }
        public void DeleteShieldFromField()
        {
            lock (lockObject)
            {
                if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Shield))
                {
                    GameField[Player.PositionY, Player.PositionX] = new EmptyCell(' ', Player.PositionY, Player.PositionX, ConsoleColor.Black);
                }
            }
        }
        public bool AddShieldToField(string type)
        {
            lock (lockObject)
            {
                if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(EmptyCell))
                {
                    GameField[Player.PositionY, Player.PositionX] = new Shield(type, Player.PositionY, Player.PositionX);
                    return true;
                }
                return false;
            }
        }
        public void DeleteBallFromField()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                if (Ball.PositionX == Player.PositionX && Ball.PositionY == Player.PositionY)
                {
                    ColoredWrite(Player.Skin.ToString(), "inline", Player.Color, Player.BgColor);

                }
                else
                {
                    ColoredWrite(GameField[Ball.PositionY, Ball.PositionX].Skin.ToString(), "inline", GameField[Ball.PositionY, Ball.PositionX].Color, GameField[Ball.PositionY, Ball.PositionX].BgColor);
                }
            }
        }
        public void UpdateBallOnField()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Wall) || GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Teleport))
                {
                    ColoredWrite(GameField[Ball.PositionY, Ball.PositionX].Skin.ToString(), "inline", GameField[Ball.PositionY, Ball.PositionX].Color, GameField[Ball.PositionY, Ball.PositionX].BgColor);
                }
                else
                {
                    Console.Write(Ball.Skin);
                }
            }
        }
        public void StartBallMovement()
        {
            while (Status != GameStatus.Over && Status != GameStatus.Winner)
            {
                DeleteBallFromField();
                Ball.Move(Ball.Direction);
                UpdateBallOnField();
                if (Ball.Direction == MoveDirection.Left || Ball.Direction == MoveDirection.Right)
                {
                    Thread.Sleep(HorizontalSpeed);
                }
                else
                {
                    Thread.Sleep(VerticalSpeed);
                }
                GameField[Ball.PositionY, Ball.PositionX].BallInteraction(this, Ball);
            }
            DeleteBallFromField();
        }
        public void DeletePlayerFromField()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
            }
        }
        public void UpdatePlayerOnField()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                Console.Write(Player.Skin);
            }
        }
        public void PlayerController()
        {
            while (Status != GameStatus.Over && Status != GameStatus.Winner)
            {
                if (Status == GameStatus.Over && Status == GameStatus.Winner)
                {
                    break;
                }
                ConsoleKeyInfo playerDirection = Console.ReadKey(intercept: true);
                void MovePlayer(string direction, Func<bool> canMove)
                {
                    lock (lockObject)
                    {
                        DeletePlayerFromField();
                        if (canMove())
                        {
                            Player.Move(direction);
                        }
                        UpdatePlayerOnField();
                    }
                }
                switch (playerDirection.Key)
                {
                    case ConsoleKey.RightArrow:
                        MovePlayer("right", () => Player.PositionX < Width - 2);
                        break;
                    case ConsoleKey.LeftArrow:
                        MovePlayer("left", () => Player.PositionX > 1);
                        break;
                    case ConsoleKey.UpArrow:
                        MovePlayer("up", () => Player.PositionY > 1);
                        break;
                    case ConsoleKey.DownArrow:
                        MovePlayer("down", () => Player.PositionY < Height - 2);
                        break;
                    case ConsoleKey.F1:
                    case ConsoleKey.F2:
                        if (AddShieldToField(playerDirection.Key == ConsoleKey.F1 ? "left" : "right"))
                        {
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                        break;
                    case ConsoleKey.F3:
                        DeleteShieldFromField();
                        lock (lockObject)
                        {
                            Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                            Console.Write(Player.Skin);
                        }
                        break;
                    case ConsoleKey.Escape:
                        lock (lockObject)
                        {
                            Console.SetCursorPosition(0, Height);
                            CenteredColoredWrite(Width, "Game W I L L  B E  C L O S E D, press Enter to confirm", "wrap", ConsoleColor.DarkRed);
                            Status = GameStatus.Closed;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (Status == GameStatus.Closed)
                        {
                            Status = GameStatus.Over;
                        }
                        break;
                }
            }
            DeletePlayerFromField();
            lock (lockObject)
            {
                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                Console.Write(GameField[Player.PositionY, Player.PositionX].Skin);
            }
        }
        private (int, int) FindNearBullet(int playerX, int playerY, List<(int, int)> points)
        {
            var nearest = points.OrderBy(p => Math.Abs(p.Item1 - playerY) + Math.Abs(p.Item2 - playerX)).FirstOrDefault();
            return nearest;
        }
        private void MovePlayerTo(int y, int x)
        {
            while (Player.PositionY != y || Player.PositionX != x)
            {
                DeletePlayerFromField();
                if (Player.PositionY < y)
                {
                    Player.PositionY++;
                }
                else if (Player.PositionY > y)
                {
                    Player.PositionY--;
                }

                if (Player.PositionX < x)
                {
                    Player.PositionX++;
                }
                else if (Player.PositionX > x)
                {
                    Player.PositionX--;
                }
                UpdatePlayerOnField();
                Thread.Sleep(50);
            }
        }
        public void Emulate()
        {
            while (EnegryBulletsAmount > 0 && Status == GameStatus.InProgress)
            {
                if (EnegryBulletsAmount == 0)
                {
                    Console.Clear();
                    Status = GameStatus.Winner;
                    break;
                }
                (int nearY, int nearX) = FindNearBullet(Player.PositionX, Player.PositionY, Points);
                if (Ball.Direction == MoveDirection.Top || Ball.Direction == MoveDirection.Bottom)
                {
                    MovePlayerTo(nearY, Ball.PositionX);
                    if (Ball.PositionX < nearX)
                    {
                        if (Ball.PositionY > Player.PositionY)
                        {
                            AddShieldToField("right");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }

                        }
                        else
                        {
                            AddShieldToField("left");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                    }
                    else
                    {
                        if (Ball.PositionY > Player.PositionY)
                        {
                            AddShieldToField("right");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                        else
                        {
                            AddShieldToField("left");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                    }
                }
                else if (Ball.Direction == MoveDirection.Left || Ball.Direction == MoveDirection.Right)
                {
                    MovePlayerTo(Ball.PositionY, nearX);
                    if (Ball.PositionY < nearY)
                    {
                        if (Ball.PositionX < Player.PositionX)
                        {
                            AddShieldToField("left");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                        else
                        {
                            AddShieldToField("right");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                    }
                    else
                    {
                        if (Ball.PositionX < Player.PositionX)
                        {
                            AddShieldToField("right");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }

                        }
                        else
                        {
                            AddShieldToField("right");
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", GameField[Player.PositionY, Player.PositionX].Color, GameField[Player.PositionY, Player.PositionX].BgColor);
                            }
                        }
                    }
                }
                int timeout = 1000;
                Stopwatch sw = Stopwatch.StartNew();
                while (sw.ElapsedMilliseconds < timeout)
                {
                    Thread.Sleep(100);
                    lock (lockObject)
                    {
                        if ((Ball.PositionX != nearX || Ball.PositionY != nearY) && !Points.Contains((Ball.PositionY, Ball.PositionX)))
                        {
                            break;
                        }
                    }
                }
                DeleteShieldFromField();
            }
            return;
        }

    }
    internal class Game
    {
        private Dictionary<string, BaseLevel> levels = new Dictionary<string, BaseLevel>
        {
            { "EASY", new EasyLevel() },
            { "NORMAL", new NormalLevel() },
            { "HARD", new HardLevel() },
            { "EMULATE", new EmulateLevel() }
        };
        public (string, Dictionary<string, int>?) StartGame(string level)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            if (!levels.TryGetValue(level, out BaseLevel? gameLevel))
            {
                throw new ArgumentException("Invalid game level", nameof(level));
            }
            Field field = new Field(gameLevel.Height, gameLevel.Width);
            field.CreateField(level);
            gameLevel.SetupField(field);
            field.DrawField();
            Thread BallThread = new Thread(field.StartBallMovement);
            BallThread.Start();
            gameLevel.Run(field);
            Thread.Sleep(1000);
            Console.Clear();
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Dictionary<string, int> time = new Dictionary<string, int>
            {
                { "minutes", elapsed.Minutes },
                { "seconds", elapsed.Seconds },
                { "milliseconds", elapsed.Milliseconds }
            };
            if (field.Status != GameStatus.Winner)
            {
                return ("DEFEAT", null);
            }
            else if (field.Status == GameStatus.Winner)
            {
                return ("WIN", time);
            }
            else
            {
                return ("", null);
            }
        }
    }

}
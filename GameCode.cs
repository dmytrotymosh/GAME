using System;
using System.Threading;
using System.Transactions;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.ComponentModel;
using System.Collections.Generic;

namespace GAME
{
    internal class Field
    {
        public int Height;
        public int Width;
        public BaseElement[,] GameField;
        public Ball Ball = new Ball('B', 1, 1);
        public int EnegryBulletsAmount = 10;
        public int SpikesAmount = 5;
        public bool IsOver = false;
        public bool IsWinner = false;
        public bool IsClosed = false;
        private readonly object lockObject = new object();
        public Player Player = new Player('P', 1, 1);

        public Field(int height, int width)
        {
            Height = height;
            Width = width;
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
                            GameField[i, j] = new Wall('+', j, i);
                        }
                        else if (i == 0 || i == Height - 1)
                        {
                            GameField[i, j] = new Wall('-', j, i);
                        }
                        else if (j == 0 || j == Width - 1)
                        {
                            GameField[i, j] = new Wall('|', j, i);
                        }
                    }
                    else if (level == "HARD" && j == (Width / 2))
                    {
                        GameField[i, j] = new Wall('|', j, i);
                    }
                    else if (i == Player.PositionY && j == Player.PositionX)
                    {
                        GameField[i, j] = new Player('P', i, j);
                    }
                    else if (i == Ball.PositionY && j == Ball.PositionX)
                    {
                        GameField[i, j] = new Ball('B', i, j);
                    }
                    else
                    {
                        GameField[i, j] = new EmptyCell(' ', i, j);
                    }
                }
            }
        }

        public void AddEnergyBullets()
        {
            Random random = new Random();
            for (int i = 0; i < EnegryBulletsAmount; i++)
            {
                int randomPositionY = random.Next(1, Height - 2);
                int randomPositionX = random.Next(1, Width - 2);
                if (GameField[randomPositionY, randomPositionX].GetType() == typeof(EmptyCell))
                {
                    GameField[randomPositionY, randomPositionX] = new EnergyBullet('@', randomPositionY, randomPositionX);
                }
                else
                {
                    while (true)
                    {
                        randomPositionY = random.Next(1, Height - 2);
                        randomPositionX = random.Next(1, Width - 2);
                        if (GameField[randomPositionY, randomPositionX].GetType() == typeof(EmptyCell))
                        {
                            GameField[randomPositionY, randomPositionX] = new EnergyBullet('@', randomPositionY, randomPositionX);
                            break;
                        }
                    }
                }
            }
        }

        public void AddSpikes()
        {
            Random random = new Random();
            for (int i = 0; i < SpikesAmount; i++)
            {
                int randomPositionY = random.Next(1, Height - 2);
                int randomPositionX = random.Next(1, Width - 2);
                if (GameField[0, randomPositionX].GetType() == typeof(Wall))
                {
                    GameField[0, randomPositionX] = new Spike('#', 0, randomPositionX);
                }
                if (GameField[Height - 1, randomPositionX].GetType() == typeof(Wall))
                {
                    GameField[Height - 1, randomPositionX] = new Spike('#', Height - 1, randomPositionX);
                }
                if (GameField[randomPositionY, 0].GetType() == typeof(Wall))
                {
                    GameField[randomPositionY, 0] = new Spike('#', randomPositionY, 0);
                }
                if (GameField[randomPositionY, Width - 1].GetType() == typeof(Wall))
                {
                    GameField[randomPositionY, Width - 1] = new Spike('#', randomPositionY, Width - 1);
                }
            }
        }
        public void AddBoosters()
        {
            Random random = new Random();
            for (int i = 0; i < 2; i++)
            {
                int randomPositionY = random.Next(1, Height - 2);
                int randomPositionX = random.Next(1, Width - 2);
                if (GameField[randomPositionY, randomPositionX].GetType() == typeof(EmptyCell))
                {
                    GameField[randomPositionY, randomPositionX] = new Booster('?', randomPositionY, randomPositionX);
                }
                else
                {
                    while (true)
                    {
                        randomPositionY = random.Next(1, Height - 2);
                        randomPositionX = random.Next(1, Width - 2);
                        if (GameField[randomPositionY, randomPositionX].GetType() == typeof(EmptyCell))
                        {
                            GameField[randomPositionY, randomPositionX] = new Booster('?', randomPositionY, randomPositionX);
                            break;
                        }
                    }
                }
            }
        }
        public void addTeleports()
        {
            Random random = new Random();
            Teleport firstTeleport = null, secondTeleport = null;
            int randomPositionY1 = random.Next(1, (Height / 2));
            int randomPositionX1 = random.Next(1, (Width / 2));
            if (GameField[randomPositionY1, randomPositionX1].GetType() == typeof(EmptyCell))
            {
                firstTeleport = new Teleport(':', randomPositionY1, randomPositionX1, 0, 0);
            }
            else
            {
                while (true)
                {
                    randomPositionY1 = random.Next(1, (Height / 2));
                    randomPositionX1 = random.Next(1, (Width / 2));
                    if (GameField[randomPositionY1, randomPositionX1].GetType() == typeof(EmptyCell))
                    {
                        GameField[randomPositionY1, randomPositionX1] = new Teleport(':', randomPositionY1, randomPositionX1, 0, 0);
                        break;
                    }
                }
            }
            int randomPositionY2 = random.Next((Height / 2), Height - 2);
            int randomPositionX2 = random.Next((Width / 2), Width - 2);
            if (GameField[randomPositionY2, randomPositionX2].GetType() == typeof(EmptyCell))
            {
                secondTeleport = new Teleport(':', randomPositionY2, randomPositionX2, randomPositionY1, randomPositionX1);
            }
            else
            {
                while (true)
                {
                    randomPositionY2 = random.Next((Height / 2), Height - 2);
                    randomPositionX2 = random.Next((Width / 2), Width - 2);
                    if (GameField[randomPositionY2, randomPositionX2].GetType() == typeof(EmptyCell))
                    {
                        GameField[randomPositionY2, randomPositionX2] = new Teleport(':', randomPositionY2, randomPositionX2, randomPositionY1, randomPositionX1);
                        break;
                    }
                }
            }
            firstTeleport.NextY = secondTeleport.PositionY;
            firstTeleport.NextX = secondTeleport.PositionX;
            GameField[randomPositionY1, randomPositionX1] = firstTeleport;
            GameField[randomPositionY2, randomPositionX2] = secondTeleport;
        }
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
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
            switch (backgroundColor)
            {
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "darkblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case "darkred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
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

        public void DrawField()
        {
            lock (lockObject)
            {
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (GameField[i, j].GetType() == typeof(EnergyBullet))
                        {
                            ColoredWrite(GameField[i, j].Skin.ToString(), "inline", "darkyellow");
                        }
                        else if (GameField[i, j].GetType() == typeof(Spike))
                        {
                            ColoredWrite(GameField[i, j].Skin.ToString(), "inline", "darkblue");
                        }
                        else if (GameField[i, j].GetType() == typeof(Wall))
                        {
                            ColoredWrite(GameField[i, j].Skin.ToString(), "inline", "green");
                        }
                        else if (GameField[i, j].GetType() == typeof(Booster))
                        {
                            ColoredWrite(GameField[i, j].Skin.ToString(), "inline", "white", "magenta");
                        }
                        else if (GameField[i, j].GetType() == typeof(Teleport))
                        {
                            ColoredWrite(GameField[i, j].Skin.ToString(), "inline", "green", "darkblue");
                        }
                        else
                        {
                            Console.Write(GameField[i, j].Skin);
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
        public void CenteredColoredWrite(int lineWidth, string text, string mode, string textColor, string backgroundColor = "black")
        {
            ColoredWrite(new string(' ', (lineWidth - text.Length) / 2) + text, mode, textColor, backgroundColor);
        }

        public void DeletePlayerFromField()
        {
            lock (lockObject)
            {
                if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Player))
                {
                    GameField[Player.PositionY, Player.PositionX] = new EmptyCell(' ', Player.PositionY, Player.PositionX);
                }
            }
        }

        public void DeleteBallFromField()
        {
            lock (lockObject)
            {
                if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Ball))
                {
                    GameField[Ball.PositionY, Ball.PositionX] = new EmptyCell(' ', Ball.PositionY, Ball.PositionX);
                }
            }
        }

        public void DeleteShieldFromField()
        {
            lock (lockObject)
            {
                if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Shield))
                {
                    GameField[Player.PositionY, Player.PositionX] = Player;
                }
            }
        }

        public bool AddShiledToField(string type)
        {
            lock (lockObject)
            {
                if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Player))
                {
                    switch (type)
                    {
                        case "left":
                            GameField[Player.PositionY, Player.PositionX] = new Shield('\\', Player.PositionY, Player.PositionX);
                            break;
                        case "right":
                            GameField[Player.PositionY, Player.PositionX] = new Shield('/', Player.PositionY, Player.PositionX);
                            break;
                    }
                    return true;
                }
                return false;
            }
        }

        public void UpdatePlayerOnField()
        {
            lock (lockObject)
            {
                GameField[Player.PositionY, Player.PositionX] = Player;
            }
        }

        public void UpdateBallOnField()
        {
            lock (lockObject)
            {
                if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(EnergyBullet))
                {
                    EnegryBulletsAmount -= 1;
                }
                if (EnegryBulletsAmount == 0)
                {
                    IsWinner = true;
                    IsOver = true;
                }
                GameField[Ball.PositionY, Ball.PositionX] = Ball;
            }
        }

        public void DirecterMovementOfBall()
        {
            lock (lockObject)
            {
                DeleteBallFromField();
                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Shield))
                {
                    ColoredWrite(GameField[Ball.PositionY, Ball.PositionX].Skin.ToString(), "inline", "darkred");
                }
                else if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Booster))
                {
                    ColoredWrite(GameField[Ball.PositionY, Ball.PositionX].Skin.ToString(), "inline", "white", "magenta");
                }
                else if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Teleport))
                {
                    ColoredWrite(GameField[Ball.PositionY, Ball.PositionX].Skin.ToString(), "inline", "green", "darkblue");
                }
                else
                {
                    Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                }
                Ball.Move(Ball.Direction);
                if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(EmptyCell) ||
                    GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(EnergyBullet))
                {
                    UpdateBallOnField();
                    Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                    Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                }
                else
                {
                    Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                    Console.Write(Ball.Skin);
                }
            }
        }

        public void DrawPlayer()
        {
            if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(EmptyCell))
            {
                lock (lockObject)
                {
                    UpdatePlayerOnField();
                    Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                    Console.Write(GameField[Player.PositionY, Player.PositionX].Skin);
                }
            }
            else
            {
                lock (lockObject)
                {
                    Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                    Console.Write(Player.Skin);
                }
            }
        }
        public void StartMovementOfBall()
        {
            int HorizontalSpeed = 100;
            int VerticalSpeed = 200;
        Cycle:
            for (; !IsOver || !IsWinner;)
            {
                if (Ball.Direction == "right")
                {
                    while (GameField[Ball.PositionY, Ball.PositionX + 1].GetType() != typeof(Wall))
                    {
                        if (IsOver)
                        {
                            break;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX + 1].GetType() == typeof(Spike))
                        {
                            IsOver = true;
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            break;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX + 1].GetType() == typeof(Teleport))
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            Teleport teleport = (Teleport)GameField[Ball.PositionY, Ball.PositionX + 1];
                            Ball.PositionY = teleport.NextY;
                            Ball.PositionX = teleport.NextX;
                            continue;
                        }
                        DirecterMovementOfBall();
                        if (IsWinner == true)
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            goto Cycle;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Booster))
                        {
                            int[] speedValues = new int[] { 25, 100 };
                            Random random = new Random();
                            int randomSpeed = random.Next(0, 2);
                            HorizontalSpeed = speedValues[randomSpeed];
                        }
                        Thread.Sleep(HorizontalSpeed);
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Shield))
                        {
                            lock (lockObject)
                            {
                                DeleteBallFromField();
                                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                                Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            }
                            if (GameField[Ball.PositionY, Ball.PositionX].Skin == '\\')
                            {
                                Ball.Direction = "down";
                                goto Cycle;
                            }
                            else if (GameField[Ball.PositionY, Ball.PositionX].Skin == '/')
                            {
                                Ball.Direction = "up";
                                goto Cycle;
                            }
                        }
                    }
                }
                if (Ball.Direction == "left")
                {
                    while (GameField[Ball.PositionY, Ball.PositionX - 1].GetType() != typeof(Wall))
                    {
                        if (IsOver)
                        {
                            break;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX - 1].GetType() == typeof(Spike))
                        {
                            IsOver = true;
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            break;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX - 1].GetType() == typeof(Teleport))
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            Teleport teleport = (Teleport)GameField[Ball.PositionY, Ball.PositionX - 1];
                            Ball.PositionY = teleport.NextY;
                            Ball.PositionX = teleport.NextX;
                            continue;
                        }
                        DirecterMovementOfBall();
                        if (IsWinner == true)
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            goto Cycle;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Booster))
                        {
                            int[] speedValues = new int[] { 25, 100 };
                            Random random = new Random();
                            int randomSpeed = random.Next(0, 2);
                            HorizontalSpeed = speedValues[randomSpeed];
                        }
                        Thread.Sleep(HorizontalSpeed);
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Shield))
                        {
                            lock (lockObject)
                            {
                                DeleteBallFromField();
                                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                                Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            }
                            if (GameField[Ball.PositionY, Ball.PositionX].Skin == '\\')
                            {
                                Ball.Direction = "up";
                                goto Cycle;
                            }
                            else if (GameField[Ball.PositionY, Ball.PositionX].Skin == '/')
                            {
                                Ball.Direction = "down";
                                goto Cycle;
                            }
                        }
                    }
                }
                if (Ball.Direction == "up")
                {
                    while (GameField[Ball.PositionY - 1, Ball.PositionX].GetType() != typeof(Wall))
                    {
                        if (IsOver)
                        {
                            break;
                        }
                        if (GameField[Ball.PositionY - 1, Ball.PositionX].GetType() == typeof(Spike))
                        {
                            IsOver = true;
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            break;
                        }
                        if (GameField[Ball.PositionY - 1, Ball.PositionX].GetType() == typeof(Teleport))
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            Teleport teleport = (Teleport)GameField[Ball.PositionY - 1, Ball.PositionX];
                            Ball.PositionY = teleport.NextY;
                            Ball.PositionX = teleport.NextX;
                            continue;
                        }
                        DirecterMovementOfBall();
                        if (IsWinner == true)
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            goto Cycle;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Booster))
                        {
                            int[] speedValues = new int[] { 50, 200 };
                            Random random = new Random();
                            int randomSpeed = random.Next(0, 2);
                            VerticalSpeed = speedValues[randomSpeed];
                        }
                        Thread.Sleep(VerticalSpeed);
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Shield))
                        {
                            lock (lockObject)
                            {
                                DeleteBallFromField();
                                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                                Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            }
                            if (GameField[Ball.PositionY, Ball.PositionX].Skin == '\\')
                            {
                                Ball.Direction = "left";
                                goto Cycle;
                            }
                            else if (GameField[Ball.PositionY, Ball.PositionX].Skin == '/')
                            {
                                Ball.Direction = "right";
                                goto Cycle;
                            }
                        }
                    }
                }
                if (Ball.Direction == "down")
                {
                    while (GameField[Ball.PositionY + 1, Ball.PositionX].GetType() != typeof(Wall))
                    {
                        if (IsOver)
                        {
                            break;
                        }
                        if (GameField[Ball.PositionY + 1, Ball.PositionX].GetType() == typeof(Spike))
                        {
                            IsOver = true;
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            break;
                        }
                        if (GameField[Ball.PositionY + 1, Ball.PositionX].GetType() == typeof(Teleport))
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            Teleport teleport = (Teleport)GameField[Ball.PositionY + 1, Ball.PositionX];
                            Ball.PositionY = teleport.NextY;
                            Ball.PositionX = teleport.NextX;
                            continue;
                        }
                        DirecterMovementOfBall();
                        if (IsWinner == true)
                        {
                            DeleteBallFromField();
                            Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                            Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            goto Cycle;
                        }
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Booster))
                        {
                            int[] speedValues = new int[] { 50, 200 };
                            Random random = new Random();
                            int randomSpeed = random.Next(0, 2);
                            VerticalSpeed = speedValues[randomSpeed];
                        }
                        Thread.Sleep(VerticalSpeed);
                        if (GameField[Ball.PositionY, Ball.PositionX].GetType() == typeof(Shield))
                        {
                            lock (lockObject)
                            {
                                DeleteBallFromField();
                                Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                                Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                            }
                            if (GameField[Ball.PositionY, Ball.PositionX].Skin == '\\')
                            {
                                Ball.Direction = "right";
                                goto Cycle;
                            }
                            else if (GameField[Ball.PositionY, Ball.PositionX].Skin == '/')
                            {
                                Ball.Direction = "left";
                                goto Cycle;
                            }
                        }
                    }
                }
                if (IsOver)
                {
                    DeleteBallFromField();
                    Console.SetCursorPosition(Ball.PositionX, Ball.PositionY);
                    Console.Write(GameField[Ball.PositionY, Ball.PositionX].Skin);
                    break;
                }
                if (Ball.Direction == "right")
                {
                    Ball.Direction = "left";
                }
                else if (Ball.Direction == "left")
                {
                    Ball.Direction = "right";
                }
                else if (Ball.Direction == "up")
                {
                    Ball.Direction = "down";
                }
                else if (Ball.Direction == "down")
                {
                    Ball.Direction = "up";
                }
                continue;
            }
        }
        public void PlayerColoredPrint()
        {
            DeletePlayerFromField();
            Console.SetCursorPosition(Player.PositionX, Player.PositionY);
            if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(EnergyBullet))
            {
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "darkyellow");
            }
            else if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Shield))
            {
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "darkred");
            }
            else if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Wall))
            {
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "green");
            }
            else if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Booster))
            {
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "white", "magenta");
            }
            else if (GameField[Player.PositionY, Player.PositionX].GetType() == typeof(Teleport))
            {
                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "green", "darkblue");
            }
            else
            {
                Console.Write(GameField[Player.PositionY, Player.PositionX].Skin);
            }
        }
        public void PlayerController()
        {
            while (!IsOver || !IsWinner)
            {
                if (IsOver)
                {
                    break;
                }
                ConsoleKeyInfo playerDirection = Console.ReadKey(intercept: true);
                switch (playerDirection.Key.ToString())
                    {
                        case "RightArrow":
                        lock (lockObject)
                        {
                            PlayerColoredPrint();
                            if (Player.PositionX < Width - 2)
                            {
                                Player.Move("right");
                            }
                            DrawPlayer();
                        } 
                            break;
                        case "LeftArrow":
                        lock (lockObject)
                        {
                            PlayerColoredPrint();
                            if (Player.PositionX > 1)
                            {
                                Player.Move("left");
                            }
                            DrawPlayer();
                        }
                            break;
                        case "UpArrow":
                        lock (lockObject)
                        {
                            PlayerColoredPrint();
                            if (Player.PositionY > 1)
                            {
                                Player.Move("up");
                            }
                            DrawPlayer();
                        }
                            break;
                        case "DownArrow":
                        lock (lockObject)
                        {
                            PlayerColoredPrint();
                            if (Player.PositionY < Height - 2)
                            {
                                Player.Move("down");
                            }
                            DrawPlayer();
                        }
                            break;
                        case "F1":
                            if (AddShiledToField("left"))
                            {
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "darkred");
                            }
                        }
                            break;
                        case "F2":
                            if (AddShiledToField("right"))
                            {
                            lock (lockObject)
                            {
                                Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                                ColoredWrite(GameField[Player.PositionY, Player.PositionX].Skin.ToString(), "inline", "darkred");
                            }
                        }
                            break;
                        case "F3":
                            DeleteShieldFromField();
                        lock (lockObject)
                        {
                            Console.SetCursorPosition(Player.PositionX, Player.PositionY);
                            Console.Write(Player.Skin);
                        }
                        break;
                    case "Escape":
                        lock (lockObject)
                        {
                            Console.SetCursorPosition(0, Height);
                            CenteredColoredWrite(Width, "Game W I L L  B E  C L O S E D, press Enter to confirm", "wrap", "darkred");
                            IsClosed = true;
                        }
                        break;
                    case "Enter":
                        if (IsClosed)
                        {
                            IsOver = true;
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
    }

    internal class Game
    {
        public (string, Dictionary<string, int>?) StartGame(string level)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int Width, Height;
            if (level == "EASY")
            {
                Width = 80;
                Height = 20;
            } 
            else if (level == "NORMAL") {
                Width = 100;
                Height = 25;
            }
            else
            {
                Width = 120;
                Height = 30;
            }
            Field field = new Field(Height, Width);
            field.CreateField(level);
            field.AddEnergyBullets();
            if (level != "EASY")
            {
                field.AddSpikes();
            }
            if (level == "HARD")
            {
                field.AddBoosters();
                field.addTeleports();
            }
            field.DrawField();
            Thread BallThread = new Thread(field.StartMovementOfBall);
            BallThread.Start();
            field.PlayerController();
            field.StartMovementOfBall();
            Thread.Sleep(1000);
            Console.Clear();
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;
            Dictionary<string, int> time = new Dictionary<string, int>
            {
                {"minutes", elapsed.Minutes},
                {"seconds", elapsed.Seconds},
                {"milliseconds", elapsed.Milliseconds}
            };
            if (field.IsOver == true && field.IsWinner == false)
            {
                return ("DEFEAT", null);
            } 
            else if (field.IsOver == true && field.IsWinner == true)
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
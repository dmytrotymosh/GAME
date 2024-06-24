using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GAME;

namespace GAME.Elements
{
    public class Teleport : BaseElement
    {
        public int NextY { get; set; }
        public int NextX { get; set; }
        private Teleport connectedTeleport;
        public Teleport(char skin, int positionY, int positionX, ConsoleColor color, ConsoleColor bgcolor) : base(':', positionY, positionX, ConsoleColor.Green, ConsoleColor.DarkBlue) { }
        public void Connect(Teleport otherTeleport)
        {
            NextY = otherTeleport.PositionY;
            NextX = otherTeleport.PositionX;
            otherTeleport.NextX = PositionX;
            otherTeleport.NextY = PositionY;
            this.connectedTeleport = otherTeleport;
            otherTeleport.connectedTeleport = this;
        }
        public static Teleport CreateTeleport(int minX, int maxX, int minY, int maxY, Field field)
        {
            Random random = new Random();
            int x, y;
            do
            {
                x = random.Next(minX, maxX);
                y = random.Next(minY, maxY);
            }
            while (field.GameField[y, x].GetType() != typeof(EmptyCell));
            Teleport teleport = new Teleport(':', y, x, ConsoleColor.Green, ConsoleColor.DarkBlue);
            field.GameField[y, x] = teleport;
            return teleport;
        }
        public override void BallInteraction(Field field, Ball ball)
        {
            Teleport teleport = (Teleport)field.GameField[ball.PositionY, ball.PositionX];
            ball.PositionY = teleport.NextY;
            ball.PositionX = teleport.NextX;
        }
    }
}

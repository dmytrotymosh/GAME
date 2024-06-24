using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public enum MoveDirection
    {
        Left,
        Right,
        Top,
        Bottom
    }
    public abstract class BaseElement
    {
        public char Skin { get; set; }
        public int PositionY { get; set; }
        public int PositionX { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        public abstract void BallInteraction(Field field, Ball ball);
        public BaseElement(char skin, int positionY, int positionX, ConsoleColor color, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Skin = skin;
            PositionY = positionY;
            PositionX = positionX;
            Color = color;
            BgColor = bgColor;
        }
    }
}

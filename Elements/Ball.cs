using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Ball : BaseElement
    {
        public Ball(char skin, int positionY, int positionX, ConsoleColor color) : base('B', positionY, positionX, ConsoleColor.White) { }
        public MoveDirection Direction = MoveDirection.Bottom;
        public void Move(MoveDirection direction)
        {
            switch (direction)
            {
                case MoveDirection.Left:
                    PositionX -= 1;
                    break;
                case MoveDirection.Right:
                    PositionX += 1;
                    break;
                case MoveDirection.Top:
                    PositionY -= 1;
                    break;
                case MoveDirection.Bottom:
                    PositionY += 1;
                    break;
            }
        }
        public override void BallInteraction(Field field, Ball ball) { }
    }
}

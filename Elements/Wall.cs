using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Wall : BaseElement
    {
        public Wall(char skin, int positionY, int positionX, ConsoleColor color) : base(skin, positionY, positionX, ConsoleColor.Green) { }
        public override void BallInteraction(Field field, Ball ball)
        {
            if (ball.Direction == MoveDirection.Right)
            {
                ball.Direction = MoveDirection.Left;
            }
            else if (ball.Direction == MoveDirection.Left)
            {
                ball.Direction = MoveDirection.Right;
            }
            else if (ball.Direction == MoveDirection.Top)
            {
                ball.Direction = MoveDirection.Bottom;
            }
            else if (ball.Direction == MoveDirection.Bottom)
            {
                ball.Direction = MoveDirection.Top;
            }
        }
    }
}

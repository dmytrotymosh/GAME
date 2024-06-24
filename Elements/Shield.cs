using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Shield : BaseElement
    {
        public string Type { get; }
        public Shield(string type, int positionY, int positionX) : base(type == "left" ? '\\' : '/', positionY, positionX, ConsoleColor.DarkRed)
        {
            Type = type;
        }

        public override void BallInteraction(Field field, Ball ball)
        {
            if (this.Type == "right")
            {
                if (ball.Direction == MoveDirection.Left)
                {
                    ball.Direction = MoveDirection.Bottom;
                }
                else if (ball.Direction == MoveDirection.Right)
                {
                    ball.Direction = MoveDirection.Top;
                }
                else if (ball.Direction == MoveDirection.Top)
                {
                    ball.Direction = MoveDirection.Right;
                } 
                else if (ball.Direction == MoveDirection.Bottom)
                {
                    ball.Direction = MoveDirection.Left;
                }
            }
            else if (this.Type == "left")
            {
                if (ball.Direction == MoveDirection.Left)
                {
                    ball.Direction = MoveDirection.Top;
                }
                else if (ball.Direction == MoveDirection.Right)
                {
                    ball.Direction = MoveDirection.Bottom;
                }
                else if (ball.Direction == MoveDirection.Top)
                {
                    ball.Direction = MoveDirection.Left;
                }
                else if (ball.Direction == MoveDirection.Bottom)
                {
                    ball.Direction = MoveDirection.Right;
                }
            }
        }
    }
}

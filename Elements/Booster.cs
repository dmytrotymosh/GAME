using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Booster : BaseElement
    {
        public Booster(char skin, int positionY, int positionX, ConsoleColor color, ConsoleColor bgcolor) : base('?', positionY, positionX, ConsoleColor.White, ConsoleColor.Magenta) { }
        public override void BallInteraction(Field field, Ball ball)
        {
            if (ball.Direction == MoveDirection.Left || ball.Direction == MoveDirection.Right)
            {
                int[] speedValues = new int[] { 25, 100 };
                Random random = new Random();
                int randomSpeed = random.Next(0, 2);
                field.HorizontalSpeed = speedValues[randomSpeed];
            } 
            else
            {
                int[] speedValues = new int[] { 50, 200 };
                Random random = new Random();
                int randomSpeed = random.Next(0, 2);
                field.VerticalSpeed = speedValues[randomSpeed];
            }
        }
    }
}

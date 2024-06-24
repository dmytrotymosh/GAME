using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Player : BaseElement
    {
        public Player(char skin, int positionY, int positionX, ConsoleColor color) : base('P', positionY, positionX, ConsoleColor.White) { }
        public void Move(string direction)
        {
            switch (direction)
            {
                case "left":
                    PositionX -= 1;
                    break;
                case "right":
                    PositionX += 1;
                    break;
                case "up":
                    PositionY -= 1;
                    break;
                case "down":
                    PositionY += 1;
                    break;
            }
        }
        public override void BallInteraction(Field field, Ball ball)
        {
            
        }
    }
}

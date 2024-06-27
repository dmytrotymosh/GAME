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
        /*public void Move(string direction)
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
        }*/
        private readonly Dictionary<MoveDirection, (int dx, int dy)> directionOffsets = new Dictionary<MoveDirection, (int dx, int dy)>
        {
            { MoveDirection.Left, (-1, 0) },
            { MoveDirection.Right, (1, 0) },
            { MoveDirection.Top, (0, -1) },
            { MoveDirection.Bottom, (0, 1) }
        };
        public void Move(MoveDirection direction)
        {
            if (directionOffsets.TryGetValue(direction, out var offset))
            {
                PositionX += offset.dx;
                PositionY += offset.dy;
            }
        }
        public override void BallInteraction(Field field, Ball ball) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class Spike : BaseElement
    {
        public Spike(char skin, int positionY, int positionX, ConsoleColor color) : base('#', positionY, positionX, ConsoleColor.DarkBlue) { }
        public override void BallInteraction(Field field, Ball ball)
        {
            field.Status = GameStatus.Over;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class EmptyCell: BaseElement
    {
        public EmptyCell(char skin, int positionY, int positionX, ConsoleColor color) : base(' ', positionY, positionX, ConsoleColor.Black) { }
        public override void BallInteraction(Field field, Ball ball)
        {
        }
    }
}

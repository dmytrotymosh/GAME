using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Elements
{
    public class EnergyBullet : BaseElement
    {
        public EnergyBullet(char skin, int positionY, int positionX, ConsoleColor color) : base('@', positionY, positionX, ConsoleColor.DarkYellow) { }
        public override void BallInteraction(Field field, Ball ball)
        {
            field.GameField[ball.PositionY, ball.PositionX] = new EmptyCell(' ', ball.PositionY, ball.PositionX, ConsoleColor.Black);
            field.EnegryBulletsAmount -= 1;
            field.Points.Remove((ball.PositionY, ball.PositionX));
            if (field.EnegryBulletsAmount == 0)
            {
                field.Status = GameStatus.Winner;
            }
        }
    }
}

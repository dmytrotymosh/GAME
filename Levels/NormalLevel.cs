using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Levels
{
    public class NormalLevel : BaseLevel
    {
        public override int Width => 100;
        public override int Height => 25;

        public override void SetupField(Field field)
        {
            field.AddEnergyBullets();
            field.AddSpikes();
        }
    }
}

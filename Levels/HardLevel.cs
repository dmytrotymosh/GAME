using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Levels
{
    public class HardLevel : BaseLevel
    {
        public override int Width => 120;
        public override int Height => 30;

        public override void SetupField(Field field)
        {
            field.AddEnergyBullets();
            field.AddSpikes();
            field.AddBoosters();
            field.AddTeleports();
        }
    }
}

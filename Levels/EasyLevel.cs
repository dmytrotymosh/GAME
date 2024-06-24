using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Levels
{
    public class EasyLevel : BaseLevel
    {
        public override int Width => 80;
        public override int Height => 20;

        public override void SetupField(Field field)
        {
            field.AddEnergyBullets();
        }
    }
}

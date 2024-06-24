using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Levels
{
    public class EmulateLevel : EasyLevel
    {
        public override void Run(Field field)
        {
            field.Emulate();
        }
    }
}

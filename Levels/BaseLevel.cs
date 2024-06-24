using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAME.Levels
{
    public abstract class BaseLevel
    {
        public abstract int Width { get; }
        public abstract int Height { get; }
        public abstract void SetupField(Field field);
        public virtual void Run(Field field)
        {
            field.PlayerController();
        }
    }
}

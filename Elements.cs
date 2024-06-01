namespace GAME
{
    abstract class BaseElement
    {
        public char Skin { get; set; }
        public int PositionY { get; set; }
        public int PositionX { get; set; }
        public BaseElement(char skin, int positionY, int positionX)
        {
            Skin = skin;
            PositionY = positionY;
            PositionX = positionX;
        }
    }

    class EmptyCell : BaseElement
    {
        public EmptyCell(char skin, int positionY, int positionX) : base(' ', positionY, positionX) { }
    }

    class Ball : BaseElement
    {
        public Ball(char skin, int positionY, int positionX) : base('B', positionY, positionX) { }
        public string Direction = "right";
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
    }

    class Player : BaseElement
    {
        public Player(char skin, int positionY, int positionX) : base('P', positionY, positionX) { }
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
    }

    class EnergyBullet : BaseElement
    {
        public EnergyBullet(char skin, int positionY, int positionX) : base('@', positionY, positionX) { }
    }

    class Wall : BaseElement
    {
        public Wall(char skin, int positionY, int positionX) : base(skin, positionY, positionX) { }
    }

    class Spike : BaseElement
    {
        public Spike(char skin, int positionY, int positionX) : base('#', positionY, positionX) { }
    }

    class Shield : BaseElement
    {
        public Shield(char skin, int positionY, int positionX) : base(skin, positionY, positionX) { }
    }

    class Booster : BaseElement
    {
        public Booster(char skin, int positionY, int positionX) : base('?', positionY, positionX) { }
    }
    class Teleport : BaseElement
    {
        public int NextY { get; set; }
        public int NextX { get; set; }
        public Teleport(char skin, int positionY, int positionX, int nextY, int nextX) : base(':', positionY, positionX)
        {
            NextY = nextY;
            NextX = nextX;
        }
    }
}
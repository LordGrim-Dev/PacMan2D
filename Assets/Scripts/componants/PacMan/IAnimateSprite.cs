namespace PacMan
{
    internal interface IAnimateSprite
    {
        void StartAnimation();
        void StopAnimation();
        void Restart();

        bool Enabled { set; }
        bool SpriteEnabled { set; }
    }
}
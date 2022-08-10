namespace PacMan
{
    internal interface IPacManAnimation
    {
        void StartAnimation();

        void StopAnimation();

        void OnPause(bool inPasueStatus);
    }
}
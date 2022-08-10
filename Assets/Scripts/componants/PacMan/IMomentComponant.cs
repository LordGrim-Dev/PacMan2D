using UnityEngine;

namespace PacMan
{
    internal interface IMomentComponant
    {
        bool Enabled { get; set; }
        Vector2 CurrentDirection { get; }
        void OnAwake();
        void OnStart();

        void OnUpdate();
        void OnFixedUpdate();
        void UpdateSpeedMultiplier(float inSpeedMult);
        void UpdateSpeed(float inSpeed);
        void SetDirection(Vector2 direction, bool inForceUpdate = false);
        void ResetState();
    }
}
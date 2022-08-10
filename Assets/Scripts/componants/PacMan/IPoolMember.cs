
using UnityEngine;

namespace PacMan
{
    public interface IPoolMember
    {
        bool IsinUse { get; }
        void Show(Vector2 inDirection);
        void Hide();

    }
}
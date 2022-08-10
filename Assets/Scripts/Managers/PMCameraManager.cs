
namespace PacMan
{
    using UnityEngine;

    public class PMCameraManager : MonoBehaviour
    {

        [SerializeField]
        private Camera mMainCamera;

        [SerializeField]
        private SpriteRenderer m_SpriteToFitTo;

        [SerializeField]
        private float mAddingFactor = 1.5f;


        void Start()
        {
            Vector3 bounds = m_SpriteToFitTo.bounds.extents;

            float height = bounds.x / mMainCamera.aspect;

            if (height < bounds.y)
                height = bounds.y;

            float res = height + mAddingFactor;

            mMainCamera.orthographicSize = height + mAddingFactor;

        }
    }
}
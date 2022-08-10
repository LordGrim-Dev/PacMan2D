
using System;
using UnityEngine;

namespace PacMan
{
    public class PMGameSceneReferanceHolder : MonoBehaviour
    {
        private static PMGameSceneReferanceHolder s_Instance;
        public static PMGameSceneReferanceHolder Instance
        {
            get
            {
                return s_Instance;
            }
        }

        [SerializeField]
        LevelSceneRef m_LeveSceneRef;

        [SerializeField]
        GhostsSceneRef m_GhostRef;

        [SerializeField]
        UIReferance m_UIRef;

        [SerializeField]
        Boundaryelements m_BoundaryElements;

        public GhostsSceneRef GhostRef { get => m_GhostRef; }
        public LevelSceneRef LeveSceneRef { get => m_LeveSceneRef; }
        public Canvas Canvas { get => m_UIRef.UICanvas; }
        internal Boundaryelements BoundaryElements { get => m_BoundaryElements; }

        private void Awake()
        {
            if (s_Instance == null)
                s_Instance = this;
            else
                Destroy(gameObject);

        }
        

        public void UpdateValues(Vector2 inHomeInsidePos, Vector2 inHomeOutsideRefPos)
        {
            m_GhostRef.GhostHomeInside.transform.position = inHomeInsidePos;
            m_GhostRef.OutSide.transform.position = inHomeOutsideRefPos;
        }

    }
    

    [System.Serializable]
    public class GhostsSceneRef
    {
        [SerializeField]
        private Transform m_GhostHomeInside;

        [SerializeField]
        private Transform m_GhoseOutSide;

        public Transform GhostHomeInside { get => m_GhostHomeInside; }
        public Transform OutSide { get => m_GhoseOutSide; }
    }

    
    [System.Serializable]
    public class LevelSceneRef
    {

        [SerializeField]
        Transform m_PalletsParent;

        [SerializeField]
        Transform m_WallParent;

        [SerializeField]
        Transform m_GhostPointParent;

        public Transform WallParent { get => m_WallParent; }
        public Transform GhostMovementPointParent { get => m_GhostPointParent; }
        public Transform PalletsParent { get => m_PalletsParent; }
    }
    
    [System.Serializable]
    public class UIReferance
    {
        [SerializeField]
        Canvas m_UICanvas;

        public Canvas UICanvas { get => m_UICanvas; }
    }
    

    [Serializable]
    internal class Boundaryelements
    {
        [SerializeField]
        private Transform m_Left;

        [SerializeField]
        private Transform m_Right;

        [SerializeField]
        private Transform m_Up;

        [SerializeField]
        private Transform m_Down;

        public Transform Left { get => m_Left; }
        public Transform Right { get => m_Right; }
        public Transform Up { get => m_Up; }
        public Transform Down { get => m_Down; }
    }

}
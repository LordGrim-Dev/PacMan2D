using System;
using Game.Common;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Game.Common
{
    public class TouchManager : SingletonObserverBase
    {
        private TouchManager() { Initalise(); }
        private static TouchManager m_Instance;
        public static TouchManager Instance()
        {

            if (m_Instance == null)
            {
                m_Instance = new TouchManager();
            }
            return m_Instance;
        }
        private event Action<SwipeDirection> m_TouchUpdate;
        private Vector2 m_FingerDownPos;
        private Vector2 m_FingerUpPos;
        private bool m_IsDetectSwipeAfterRelease;

        private SwipeDirection m_PreviousSwipeDir;

        
        private void Initalise()
        {
            m_IsDetectSwipeAfterRelease = false;
            m_FingerDownPos = m_FingerUpPos = Vector2.zero;
            m_PreviousSwipeDir = SwipeDirection.none;
        }
        

        public void SubscribeToInput(Action<SwipeDirection> action)
        {
            m_TouchUpdate += action;
        }
        

        public void UnSubscribeToInput(Action<SwipeDirection> action)
        {
            m_TouchUpdate -= action;
        }
        

        public void OnUpdate()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

#if UNITY_ANDROID && !UNITY_EDITOR
		    TouchDevices ();
#else
            NonTouchDevices();
#endif

        }
        

        private void NonTouchDevices()
        {
            KeyBoardInput();
            MouseInput();
        }
        


        private void MouseInput()
        {
            // if not using the mouse, bail
            if (!Input.mousePresent)
            {
                return;
            }
            if (Input.GetMouseButtonUp(0))
            {
                m_FingerUpPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
                DetectSwipe();
            }
            // if (m_TouchBegan && !m_IsDetectSwipeAfterRelease)
            // {
            //     m_FingerDownPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //     DetectSwipe();
            // }
            if (Input.GetMouseButtonDown(0))
            {
                m_FingerUpPos = m_FingerDownPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
                DetectSwipe();
            }
        }
        

        private void KeyBoardInput()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                InformSwipe(SwipeDirection.eUp);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                InformSwipe(SwipeDirection.eDown);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InformSwipe(SwipeDirection.eLeft);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                InformSwipe(SwipeDirection.eRight);
            }
#endif
        }

        

        private void TouchDevices()
        {
            if (Input.touchCount < 0 || EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                return;
            }

            // else Go through touch

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    m_FingerUpPos = touch.position;
                    m_FingerDownPos = touch.position;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    if (!m_IsDetectSwipeAfterRelease)
                    {
                        m_FingerDownPos = touch.position;
                        DetectSwipe();
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    m_FingerDownPos = touch.position;
                    DetectSwipe();
                }
            }

        }
        

        void DetectSwipe()
        {
            float verticalDist = VerticalMoveValue();
            float horizontalDist = HorizontalMoveValue();

            if (verticalDist > PMConstants.SWIPE_THRESHOLD && verticalDist > horizontalDist)
            {
                // GameUtilities.ShowLog("Vertical Swipe Detected!");

                if (m_FingerDownPos.y - m_FingerUpPos.y > 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                InformSwipe(SwipeDirection.eUp);
#else
                    InformSwipe(SwipeDirection.eDown);
#endif
                }
                else if (m_FingerDownPos.y - m_FingerUpPos.y < 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                InformSwipe(SwipeDirection.eDown);
#else
                    InformSwipe(SwipeDirection.eUp);
#endif
                }
                m_FingerUpPos = m_FingerDownPos;

            }
            else if (horizontalDist > PMConstants.SWIPE_THRESHOLD && horizontalDist > verticalDist)
            {
                // GameUtilities.ShowLog("Horizontal Swipe Detected!");

                if (m_FingerDownPos.x - m_FingerUpPos.x > 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                    InformSwipe(SwipeDirection.eRight);
#else
                    InformSwipe(SwipeDirection.eLeft);
#endif

                }
                else if (m_FingerDownPos.x - m_FingerUpPos.x < 0)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
                InformSwipe(SwipeDirection.eLeft);
#else
                    InformSwipe(SwipeDirection.eRight);
#endif
                }
                m_FingerUpPos = m_FingerDownPos;

            }
            else
            {
#if DEBUG
                GameUtilities.ShowLog("No Swipe Detected!");

#endif
            }
        }
        

        float VerticalMoveValue()
        {
            return Mathf.Abs(m_FingerDownPos.y - m_FingerUpPos.y);
        }
        


        float HorizontalMoveValue()
        {
            return Mathf.Abs(m_FingerDownPos.x - m_FingerUpPos.x);
        }
        

        private void InformSwipe(SwipeDirection inDirection)
        {
            if (inDirection == SwipeDirection.none || inDirection == m_PreviousSwipeDir)
            {
                return;
            }

#if DEBUG 
            GameUtilities.ShowLog($"InformSwipe : m_PreviousSwipeDir{m_PreviousSwipeDir}");
#endif
            m_PreviousSwipeDir = inDirection;
            m_TouchUpdate?.Invoke(inDirection);
        }
        

        public override void OnDestroy()
        {
            m_Instance = null;
            m_TouchUpdate = null;

        }
        
    }
}
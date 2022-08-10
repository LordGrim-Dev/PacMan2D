using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Common;
using UnityEngine;

namespace PacMan
{
    public class PMFireBallPoolManager : SingletonObserverBase
    {

        private PMFireBallPoolManager() { Initialise(); }

        private static PMFireBallPoolManager s_Instance;

        public static PMFireBallPoolManager Instance()
        {
            if (s_Instance == null)
                s_Instance = new PMFireBallPoolManager();
            return s_Instance;
        }

        private PMFireBall m_FireBallPrefab;
        private Transform m_Parent;

        private Dictionary<int, PMFireBall> m_FireBallPool;

        private void Initialise()
        {
            m_FireBallPool = new Dictionary<int, PMFireBall>();
            m_Parent = PMGameSceneReferanceHolder.Instance.transform;
        }

        public PMFireBall GetFireBallByID(int inID)
        {
            PMFireBall fireBall = null;
            m_FireBallPool.TryGetValue(inID, out fireBall);
            return fireBall;
        }

        
        public PMFireBall GetAvailableBullets(Vector3 inPosition)
        {
            PMFireBall retval = null;

            int poolSize = m_FireBallPool.Count;

            foreach (KeyValuePair<int, PMFireBall> kvp in m_FireBallPool)
            {
                if (!kvp.Value.IsinUse)
                {
                    retval = kvp.Value;
                    break;
                }
            }

            if (retval == null)
            {
                retval = GameObject.Instantiate<PMFireBall>(m_FireBallPrefab, inPosition, Quaternion.identity, m_Parent.transform) as PMFireBall;

                m_FireBallPool.Add(retval.gameObject.GetInstanceID(), retval);
            }

            retval.transform.position = inPosition;

            return retval;
        }
        


        internal void OnResourceUpdate(PMFireBall inFireBallPrefab)
        {
            m_FireBallPrefab = inFireBallPrefab;
        }
        

        public override void OnDestroy()
        {
            s_Instance = null;
            foreach (KeyValuePair<int, PMFireBall> kvp in m_FireBallPool)
            {
                GameObject.Destroy(kvp.Value);
            }
            m_FireBallPool.Clear();
            m_FireBallPool = null;
            m_FireBallPrefab = null;
        }
        

    }
}
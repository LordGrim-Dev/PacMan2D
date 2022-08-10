using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PacMan
{
    public class PMGlobalManager : MonoBehaviour
    {
        private static PMGlobalManager s_Instance;
        public static PMGlobalManager Instance
        {
            get
            {
                return s_Instance;
            }
        }

        void Awake()
        {
            if (s_Instance == null)
            {
                s_Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }
}

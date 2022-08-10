
using System;
using System.Collections.Generic;
using Game.Common;
using UnityEngine;

namespace PacMan.UI
{
    public enum UserInterface
    {
        eIntroUI = 1001,
        eCounterUI = 1002,
        eGamePlayUI,
        eGameOverUI

    }

    public class UserInterfaceSystem : SingletonObserverBase
    {
        private static UserInterfaceSystem m_Instance;

        public static UserInterfaceSystem Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new UserInterfaceSystem();
                return m_Instance;
            }
        }

        Canvas m_ActiveParentcanvas;

        Dictionary<uint, BaseUI> m_UIMapper;

        private UserInterfaceSystem()
        {
            m_ActiveParentcanvas = PMGameSceneReferanceHolder.Instance.Canvas;

            m_UIMapper = new Dictionary<uint, BaseUI>();
        }
        
        public void ShowUi(BaseUI inUI)
        {
            inUI.gameObject.SetActive(true);
        }
    

        public void HideUI(uint inUI)
        {
            BaseUI ui = GetUI(inUI);
            ui.gameObject.SetActive(false);
        }

        public T LoadUI<T>(uint inUi) where T : BaseUI
        {
            string uiName = typeof(T).Name;

            BaseUI ui = GetUI(inUi);
            if (ui == null)
            {
                string uiPath = GetPath(uiName);
                GameObject newInstantiated = GameUtilities.GetGameObjectFromPath(uiPath, m_ActiveParentcanvas.transform);
                if (newInstantiated != null)
                {
                    ui = newInstantiated.GetComponent<T>();

                    newInstantiated.transform.SetParent(m_ActiveParentcanvas.transform);

                    SetUISizeToCanvas(ui);

                    AddUIToMapper(inUi, ui);
                }
            }
            ui?.gameObject.SetActive(false);

            return ui as T;
        }
        

        void SetUISizeToCanvas(BaseUI inUI)
        {
            if (m_ActiveParentcanvas != null)
            {
                var canvasRectTrans = m_ActiveParentcanvas.GetComponent<RectTransform>();

                if (canvasRectTrans != null)
                {
                    float width = canvasRectTrans.rect.width;
                    float height = canvasRectTrans.rect.height;

                    inUI.SetUpBackroundSize(new Vector2(width, height));
                }
            }
        }

        private void AddUIToMapper(uint inKey, BaseUI ui)
        {
            if (m_UIMapper.ContainsKey(inKey))
            {
                m_UIMapper[inKey] = ui;
            }
            else
                m_UIMapper.Add(inKey, ui);
        }
        
        private string GetPath(string uiName)
        {
            string fullPath = PMConstants.K_UI_PREFAB_PATH + uiName;
            return fullPath;
        }

        private BaseUI GetUI(uint inUi)
        {
            BaseUI ui = null;
            if (m_UIMapper.ContainsKey(inUi))
            {
                ui = m_UIMapper[inUi];
            }
            return ui;
        }
        
        internal void CleanUP()
        {
            foreach (KeyValuePair<uint, BaseUI> kvp in m_UIMapper)
            {
                GameObject.Destroy(kvp.Value);
            }
        }
        
        public override void OnDestroy()
        {
            m_Instance = null;
            m_ActiveParentcanvas = null;
            m_UIMapper.Clear();
            m_UIMapper = null;
        }
    }


}

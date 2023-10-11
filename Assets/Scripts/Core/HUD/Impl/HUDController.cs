using System.Collections.Generic;
using Core.Screens.Impl;
using UnityEngine;

namespace Core.HUD.Impl
{
    public sealed class HUDController : UIController<HUD>, IHUDController
    {
        private Dictionary<HUD, GameObject> _shownHudList;

        public HUDController()
        {
            _shownHudList = new Dictionary<HUD, GameObject>();
        }
        
        public void Show(HUD hud)
        {
            ShowImpl(hud);
        }

        public void Hide(HUD hud)
        {
            HideImpl(hud);
        }
        
        private void ShowImpl(HUD hud)
        {
            var instance = GetScreenInstance(hud);
            if (instance == null)
                return;

            _shownHudList.Add(hud, instance);
        }
        
        private void HideImpl(HUD hud)
        {
            if(!_shownHudList.ContainsKey(hud))
                return;

            Deactivate(hud);
        }
    }
}
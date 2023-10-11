using System;
using Core.Attributes;
using Core.Modules.UI;
using Core.Modules.UI.Layers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Screens.Impl
{
    public abstract class UIController<T> where T : UIControl
    {
        [Inject] protected IUILayersController UILayerController { get; set; }
        
        protected GameObject GetScreenInstance(T control)
        {
            if (control == null)
                throw new NullReferenceException();

            var config = control.UIConfig;
            
            var layer = UILayerController.GetLayerView<Transform>(config.Layer);
            return Instantiate(control, layer);
        }
        
        protected bool Deactivate(T control)
        {
            if (control == null)
                return false;

            var config = control.UIConfig;
            var layer = UILayerController.GetLayerView(config.Layer);
            var view = layer.transform.Find(control.Name);
            if (!view || !view.gameObject.activeSelf)
                return false;

            Object.DestroyImmediate(view.gameObject);

            return true;
        }
        
        private GameObject Instantiate(T control, Component parent)
        {
            var config = control.UIConfig;
            var prefab = Resources.Load<GameObject>(config.PrefabName);
            prefab.SetActive(true);

            var instance = Object.Instantiate(prefab, parent as Transform);
            instance.name = control.Name;

            return instance;
        }
    }
}
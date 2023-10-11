﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Modules.UI.Layers.Impl
{
    public sealed class UILayersController : IUILayersController
    {
        private readonly Dictionary<int, GameObject> _layers;
        
        public UILayersController()
        {
            _layers = new Dictionary<int, GameObject>();
        }

        public void RegisterLayer(int layerId, GameObject view)
        {
            if (_layers.ContainsKey(layerId))
                throw new NullReferenceException();
            _layers.Add(layerId, view);
        }

        public GameObject GetLayerView(int layerId)
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new NullReferenceException();
            return view;
        }
        
        public T GetLayerView<T>(int layerId) where T : Component
        {
            if (!_layers.TryGetValue(layerId, out var view))
                throw new NullReferenceException();

            if (typeof(T) == typeof(Transform))
                return view.transform as T;
            
            if (typeof(T) == typeof(GameObject))
                return view as T;

            throw new InvalidCastException();
        }
    }
}
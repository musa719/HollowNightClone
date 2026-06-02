using UnityEngine;
using System.Collections.Generic;

namespace HollowNight.Utilities
{
    /// <summary>
    /// Generic object pooling system for performance optimization
    /// </summary>
    public class ObjectPool<T> where T : Component
    {
        private Queue<T> availableObjects;
        private HashSet<T> activeObjects;
        private T prefab;
        private int poolSize;
        
        public ObjectPool(T prefab, int initialSize)
        {
            this.prefab = prefab;
            this.poolSize = initialSize;
            availableObjects = new Queue<T>(initialSize);
            activeObjects = new HashSet<T>();
            
            // Pre-allocate objects
            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(prefab);
                obj.gameObject.SetActive(false);
                availableObjects.Enqueue(obj);
            }
        }
        
        public T Get()
        {
            T obj;
            
            if (availableObjects.Count > 0)
            {
                obj = availableObjects.Dequeue();
            }
            else
            {
                obj = Object.Instantiate(prefab);
            }
            
            obj.gameObject.SetActive(true);
            activeObjects.Add(obj);
            return obj;
        }
        
        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            activeObjects.Remove(obj);
            availableObjects.Enqueue(obj);
        }
        
        public void Clear()
        {
            foreach (T obj in activeObjects)
            {
                Object.Destroy(obj.gameObject);
            }
            
            foreach (T obj in availableObjects)
            {
                Object.Destroy(obj.gameObject);
            }
            
            activeObjects.Clear();
            availableObjects.Clear();
        }
        
        public int GetAvailableCount() => availableObjects.Count;
        public int GetActiveCount() => activeObjects.Count;
    }
}

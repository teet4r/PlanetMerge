using Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonBehaviour<ObjectPoolManager>
{
    private class ObjectPool
    {
        private Transform _parent;
        private PoolObject _prefab; // 풀에 재사용할 오브젝트는 PoolObject를 상속해야 함
        private PoolObject[] _pool = new PoolObject[1];
        private int _top = -1;

        public ObjectPool(string prefabName, Transform parent)
        {
            _parent = parent;
            _prefab = Resources.Load<PoolObject>($"Prefabs/{prefabName}"); // 스크립트와 프리팹 이름은 동일하게
            _prefab.gameObject.SetActive(false);
        }

        public void CallDestructor() => _prefab.gameObject.SetActive(true);

        public PoolObject Release()
        {
            if (_top < 0)
            {
                var obj = Instantiate(_prefab, default, Quaternion.identity, _parent);

                obj.Initialize();

                return obj;
            }
            return _pool[_top--];
        }

        public void Return(PoolObject obj)
        {
            obj.gameObject.SetActive(false);

            ++_top;

            if (_top >= _pool.Length)
                _ResizePool();

            _pool[_top] = obj;
        }

        public void Clear()
        {
            while (_top >= 0)
            {
                var obj = _pool[_top];
                
                Destroy(obj.gameObject);
                _pool[_top] = null;
                --_top;
            }
        }

        private void _ResizePool()
        {
            int poolSize = _pool.Length;
            PoolObject[] newPool = new PoolObject[_pool.Length << 1];

            for (int i = 0; i < poolSize; ++i)
                newPool[i] = _pool[i];

            _pool = newPool;
        }
    }

    private static Transform _tr;
    private static Dictionary<Type, ObjectPool> _pools = new();

    protected override void Awake()
    {
        base.Awake();

        _tr = GetComponent<Transform>();
    }

    private void OnDestroy() => ClearAll();

    public static T Release<T>() where T : PoolObject
    {
        var type = typeof(T);
        
        if (!_pools.TryGetValue(type, out ObjectPool objectPool))
            _pools.Add(type, objectPool = new ObjectPool($"{type.Name}", _tr));

        return objectPool.Release() as T;
    }

    public static void Return<T>(T obj) where T : PoolObject
    {
        if (_pools.TryGetValue(typeof(T), out ObjectPool pool))
        {
            pool.Return(obj);
            return;
        }

        Destroy(obj.gameObject);
    }

    public static void HideAll()
    {
        var children = _tr.GetComponentsInChildren<PoolObject>();

        for (int i = 0; i < children.Length; ++i)
            children[i].Pool();
    }

    public static void ClearAll()
    {
        foreach (var pool in _pools.Values)
        {
            pool.Clear();
            pool.CallDestructor();
        }
        _pools.Clear();
    }
}

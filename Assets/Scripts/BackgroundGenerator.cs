using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> _prefabs;
    [SerializeField] private Transform _minPoint;
    [SerializeField] private Transform _maxPoint;
    [SerializeField] private Transform _parent;
    [SerializeField] private int _quantity = 20;

    private List<GameObject> _instances = new List<GameObject>();


    private void LateUpdate()
    {
        _instances.RemoveAll(x => x == null);
        for (int i = _instances.Count; i < _quantity; i++)
        {
            Spawn();
        }

        while (_instances.Count > _quantity)
        {
            DestroyInstance();
        }
    }

    [Button]
    public void Spawn()
    {
        GameObject prefab = _prefabs.GetRandom();
        Vector3 position = VectorHelper.Random(_minPoint.position, _maxPoint.position);

        GameObject instance = Instantiate(prefab, position, Quaternion.identity, _parent);

        _instances.Add(instance);
    }

    public void DestroyInstance()
    {
        Destroy(_instances[0]);
        _instances.RemoveAt(0);
    }
}

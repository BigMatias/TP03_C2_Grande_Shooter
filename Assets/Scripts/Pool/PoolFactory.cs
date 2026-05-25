using System;
using UnityEngine;

public static class PoolFactory
{
    public static IPool Create(Type componentType, GameObject prefab,
        Transform parent, int initialSize, int maxSize)
    {
        MonoBehaviour component = prefab.GetComponent(componentType) as MonoBehaviour;
        if (component == null) return null;
        
        Type wrapperType = typeof(PoolWrapper<>).MakeGenericType(componentType);
        IPool pool = (IPool)Activator.CreateInstance(
            wrapperType,
            component,   
            parent,      
            initialSize, 
            maxSize      
        );

        return pool;
    }
}

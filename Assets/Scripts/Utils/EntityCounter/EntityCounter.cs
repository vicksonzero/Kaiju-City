using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EntityCounter : MonoBehaviour
{
    [CanBeNull]
    private static EntityCounter _entityCounter;

    public static EntityCounter Inst => _entityCounter
        ? _entityCounter
        : FindObjectOfType<EntityCounter>();

    private Dictionary<string, HashSet<CountedEntity>> _store = new();

    public delegate void OnCountChanged(int count);

    public Dictionary<string, OnCountChanged> CountChangedDelegates = new();

    public void AddDelegate(string channel, OnCountChanged callback)
    {
        CountChangedDelegates.TryAdd(channel, callback);
        _store.TryAdd(channel, new HashSet<CountedEntity>());
        callback(_store[channel].Count);
    }

    public void Add(string[] channels, CountedEntity e)
    {
        foreach (var channel in channels)
        {
            _store.TryAdd(channel, new HashSet<CountedEntity>());
            _store[channel].Add(e);

            if (CountChangedDelegates.TryGetValue(channel, out var onCountChanged))
            {
                onCountChanged?.Invoke(_store[channel].Count);
            }
        }
    }

    public void Remove(string[] channels, CountedEntity e)
    {
        foreach (var channel in channels)
        {
            _store.TryAdd(channel, new HashSet<CountedEntity>());
            _store[channel].Remove(e);
            if (CountChangedDelegates.TryGetValue(channel, out var onCountChanged))
            {
                onCountChanged?.Invoke(_store[channel].Count);
            }
        }
    }
}
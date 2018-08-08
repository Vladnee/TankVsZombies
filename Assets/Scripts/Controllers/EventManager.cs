using System;
using System.Collections.Generic;
using UnityEngine;

public class Event<T>
{
    private List<Action<T>> _onChangeEvents = new List<Action<T>>();

    public event Action<T> OnChange
    {
        add { _onChangeEvents.Add(value); }
        remove { _onChangeEvents.Remove(value); }
    }

    public void OnChangeTrigger(T value)
    {
        _onChangeEvents.RemoveAll(x => x == null);
        foreach (Action<T> onChangeEvent in _onChangeEvents)
        {
            try
            {
                onChangeEvent.Invoke(value);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}

public class Event
{
    private List<Action> _onTriggersEvents = new List<Action>();

    public event Action OnTrigger
    {
        add { _onTriggersEvents.Add(value); }
        remove { _onTriggersEvents.Remove(value); }
    }

    public void Trigger()
    {
        _onTriggersEvents.RemoveAll(x => x == null);
        foreach (Action onChangeEvent in _onTriggersEvents)
        {
            try
            {
                onChangeEvent.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}

public static class EventManager
{
    public static Event<float> PlayerHealthChange = new Event<float>();
    public static Event<int> CounterKillsChange = new Event<int>();
    public static Event<int> SelectedWeaponChange = new Event<int>();
    public static Event GameOver = new Event();
}

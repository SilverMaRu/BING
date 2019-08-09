using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private static Action emptyAction = () => { };
    private static Dictionary<string, Delegate> eventNameDelegateParis = new Dictionary<string, Delegate>();
    private static Dictionary<object, Delegate> objDelegatePairs = new Dictionary<object, Delegate>();

    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="eventName"></param>
    /// <param name="newDelegate"></param>
    private static void AddDelegate(object obj, string eventName, Delegate newDelegate)
    {
        Delegate existingDelegate;
        //bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        //if(hasValue)
        if (eventNameDelegateParis.TryGetValue(eventName, out existingDelegate))
        {
            Delegate[] delegateArray = existingDelegate.GetInvocationList();
            bool hasSameDelegate = false;
            foreach(Delegate tempDelegate in delegateArray)
            {
                if (tempDelegate.Equals(newDelegate))
                {
                    hasSameDelegate = true;
                    break;
                }
            }
            if (!hasSameDelegate)
            {
                eventNameDelegateParis[eventName] = Delegate.Combine(existingDelegate, newDelegate);
            }
        }
        else
        {
            eventNameDelegateParis.Add(eventName, Delegate.Combine(emptyAction, newDelegate));
        }

        Delegate objDelegate;
        if (objDelegatePairs.TryGetValue(obj, out objDelegate))
        {
            objDelegatePairs[obj] = Delegate.Combine(objDelegate, newDelegate);
        }
        else
        {
            objDelegatePairs.Add(obj, newDelegate);
        }
    }

    public static void BindingEvent(object obj, string eventName, Action newAction)
    {
        AddDelegate(obj, eventName, newAction);
    }

    public static void BindingEvent<T>(object obj, string eventName, Action<T> newAction)
    {
        AddDelegate(obj, eventName, newAction);
    }

    public static void BindingEvent<T1, T2>(object obj, string eventName, Action<T1, T2> newAction)
    {
        AddDelegate(obj, eventName, newAction);
    }

    public static void BindingEvent<T1, T2, T3>(object obj, string eventName, Action<T1, T2, T3> newAction)
    {
        AddDelegate(obj, eventName, newAction);
    }

    public static void BindingEvent<T1, T2, T3, T4>(object obj, string eventName, Action<T1, T2, T3, T4> newAction)
    {
        AddDelegate(obj, eventName, newAction);
    }

    public static void OnEvent(string eventName)
    {
        Delegate existingDelegate;
        bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        if (hasValue && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            existingDelegate.DynamicInvoke();
            //Delegate[] delegates = existingDelegate.GetInvocationList();
            //foreach (Delegate tempDelegate in delegates)
            //{
            //    try
            //    {
            //        tempDelegate.DynamicInvoke();
            //    }
            //    catch (System.Reflection.TargetInvocationException)
            //    {
            //        Delegate.Remove(existingDelegate, tempDelegate);
            //    }
            //}
        }
    }

    public static void OnEvent<T>(string eventName, T arg)
    {
        Delegate existingDelegate;
        bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        if (hasValue && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            existingDelegate.DynamicInvoke(arg);
            //Delegate[] delegates = existingDelegate.GetInvocationList();
            //foreach (Delegate tempDelegate in delegates)
            //{
            //    try
            //    {
            //        tempDelegate.DynamicInvoke(arg);
            //    }
            //    catch (System.Reflection.TargetInvocationException)
            //    {
            //        Delegate.Remove(existingDelegate, tempDelegate);
            //    }
            //}
        }
    }

    public static void OnEvent<T1, T2>(string eventName, T1 arg0, T2 arg1)
    {
        Delegate existingDelegate;
        bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        if (hasValue && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            existingDelegate.DynamicInvoke(arg0, arg1);
            //Delegate[] delegates = existingDelegate.GetInvocationList();
            //foreach (Delegate tempDelegate in delegates)
            //{
            //    try
            //    {
            //        tempDelegate.DynamicInvoke(arg0, arg1);
            //    }
            //    catch (System.Reflection.TargetInvocationException)
            //    {
            //        Delegate.Remove(existingDelegate, tempDelegate);
            //    }
            //}
        }
    }

    public static void OnEvent<T1, T2, T3>(string eventName, T1 arg0, T2 arg1, T3 arg2)
    {
        Delegate existingDelegate;
        bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        if (hasValue && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            existingDelegate.DynamicInvoke(arg0, arg1, arg2);
            //Delegate[] delegates = existingDelegate.GetInvocationList();
            //foreach (Delegate tempDelegate in delegates)
            //{
            //    try
            //    {
            //        tempDelegate.DynamicInvoke(arg0, arg1, arg2);
            //    }
            //    catch (System.Reflection.TargetInvocationException)
            //    {
            //        Delegate.Remove(existingDelegate, tempDelegate);
            //    }
            //}
        }
    }

    public static void OnEvent<T1, T2, T3, T4>(string eventName, T1 arg0, T2 arg1, T3 arg2, T4 arg3)
    {
        Delegate existingDelegate;
        bool hasValue = eventNameDelegateParis.TryGetValue(eventName, out existingDelegate);
        if (hasValue && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            existingDelegate.DynamicInvoke(arg0, arg1, arg2, arg3);
            //Delegate[] delegates = existingDelegate.GetInvocationList();
            //foreach (Delegate tempDelegate in delegates)
            //{
            //    try
            //    {
            //        tempDelegate.DynamicInvoke(arg0, arg1, arg2, arg3);
            //    }
            //    catch (System.Reflection.TargetInvocationException)
            //    {
            //        Delegate.Remove(existingDelegate, tempDelegate);
            //    }
            //}
        }
    }

    private static void RemoveDelegate(string eventName, Delegate removeDelegate)
    {
        Delegate existingDelegate;
        if (eventNameDelegateParis.TryGetValue(eventName, out existingDelegate) && existingDelegate != null && existingDelegate.GetInvocationList().Length > 0)
        {
            eventNameDelegateParis[eventName] = Delegate.RemoveAll(existingDelegate, removeDelegate);
        }
    }

    public static void RemoveAction(string eventName, Action removeAction)
    {
        RemoveDelegate(eventName, removeAction);
    }

    public static void RemoveAction<T>(string eventName, Action<T> removeAction)
    {
        RemoveDelegate(eventName, removeAction);
    }

    public static void RemoveAction<T1,T2>(string eventName, Action<T1, T2> removeAction)
    {
        RemoveDelegate(eventName, removeAction);
    }

    public static void RemoveAction<T1, T2, T3>(string eventName, Action<T1, T2, T3> removeAction)
    {
        RemoveDelegate(eventName, removeAction);
    }

    public static void RemoveAction<T1, T2, T3, T4>(string eventName, Action<T1, T2, T3, T4> removeAction)
    {
        RemoveDelegate(eventName, removeAction);
    }

    public static void RemoveAction(object obj)
    {
        Delegate existingDelegate;
        if (objDelegatePairs.TryGetValue(obj, out existingDelegate))
        {
            Delegate[] delegateArray = existingDelegate.GetInvocationList();
            string[] eventNames = new string[eventNameDelegateParis.Keys.Count];
            eventNameDelegateParis.Keys.CopyTo(eventNames, 0);
            foreach (string tempEventName in eventNames)
            {
                foreach (Delegate tempDelegate in delegateArray)
                {
                    RemoveDelegate(tempEventName, tempDelegate);
                }
            }
        }
    }
}

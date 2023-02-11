using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBase<T> where T : class
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = Activator.CreateInstance(typeof(T),true) as T;
            }
            return instance;
        }
    }

}

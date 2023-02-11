using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ReflectionTools
{
    public static Type[] GetDerivedTypes(this Type baseType)
    {
        List<Type> types = new List<Type>();

        var assemblys = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var item in assemblys)
        {
            types.AddRange(item.GetTypes().Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t)));
        }

        return types.ToArray();
    }
}

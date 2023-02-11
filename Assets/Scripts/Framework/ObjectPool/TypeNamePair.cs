using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public struct TypeNamePair
    {
        private string name;
        private Type type;
        public string Name { get { return name; } }
        public Type Type { get { return type; } }

        public TypeNamePair(string name, Type type)
        {
            if (type == null)
            {
                throw new Exception("Type is invalid");
            }
            this.name = name;
            this.type = type;
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(name) ? type.FullName : $"{type.FullName}.{name}";
        }

        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode() ^ name.GetHashCode();
        }

        private bool Equals(TypeNamePair other)
        {
            return other.type == type && other.name == name;
        }
    }
}


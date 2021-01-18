using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public abstract class BaseEnumeration : IComparable
{
    /// <summary>
    /// Base constructor
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    protected BaseEnumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; }
    public string Name { get; }

    /// <summary>
    /// Override ToString to return Name
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return (Name);
    }

    /// <summary>
    /// Return all values in the enumeration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<T> GetAll<T>() where T : BaseEnumeration => typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly).Select(f => f.GetValue(null)).Cast<T>();

    /// <summary>
    /// Override Equals to compare two objects
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (!(obj is BaseEnumeration otherValue))
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    /// <summary>
    /// Override Equals to generate hash code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return (base.GetHashCode());
    }

    /// <summary>
    /// Compare the name of two objects
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(object other)
    {
        return (Id.CompareTo(((BaseEnumeration)other).Id));
    }
}

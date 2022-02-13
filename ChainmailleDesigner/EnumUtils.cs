// Chainmaille Designer  (c) 2022
// Created by Christopher Matthew Albrecht
// https://github.com/CMAlbrecht/ChainmailleDesigner
// File: EnumUtils.cs


// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License, version 3, as
// published by the Free Software Foundation.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace ChainmailleDesigner
{
  public static class EnumUtils
  {
    // Cache for descriptions to improve performance.
    // Enum type => enum value => description
    private static Dictionary<Type, Dictionary<Enum, string>>
      enumDescriptons = new Dictionary<Type, Dictionary<Enum, string>>();

    // Cache lock to mediate multi-thread cache access.
    private static ReaderWriterLock cacheLock = new ReaderWriterLock();

    private static Dictionary<Enum, string> DescriptionCacheForType(
      Type enumType)
    {
      Dictionary<Enum, string> result = null;
      cacheLock.AcquireReaderLock(-1);
      try
      {
        enumDescriptons.TryGetValue(enumType, out result);
        if (result == null)
        {
          // Not yet in the cache; we will need to upgrade to a writer lock.
          LockCookie lockCookie = cacheLock.UpgradeToWriterLock(-1);
          // Check again; it may have taken a while to get the lock upgrade.
          enumDescriptons.TryGetValue(enumType, out result);
          if (result == null)
          {
            // Still not in the cache, so add it to the cache.
            result = new Dictionary<Enum, string>();
            enumDescriptons.Add(enumType, result);
          }
          cacheLock.DowngradeFromWriterLock(ref lockCookie);
        }
      }
      finally
      {
        cacheLock.ReleaseReaderLock();
      }

      return result;
    }

    public static string[] GetAllDescriptions<T>()
      where T : struct
    {
      return GetAllDescriptions(typeof(T));
    }

    public static string[] GetAllDescriptions(Type enumType)
    {
      FieldInfo[] fieldInfos = enumType.GetFields(BindingFlags.Static |
        BindingFlags.Public | BindingFlags.NonPublic);
      string[] result = new string[fieldInfos.Length];
      for (int i = 0; i < fieldInfos.Length; i++)
      {
        FieldInfo fieldInfo = fieldInfos[i];
        DescriptionAttribute[] descriptionAttributes =
          (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
            typeof(DescriptionAttribute), false);
        result[i] = descriptionAttributes.Length > 0 ?
          descriptionAttributes[0].Description : string.Empty;
      }

      return result;
    }

    public static string GetDescription(this Enum enumValue)
    {
      string result = string.Empty;

      Type enumType = enumValue.GetType();
      Dictionary<Enum, string> descriptionsForType =
        DescriptionCacheForType(enumType);
      if (!descriptionsForType.TryGetValue(enumValue, out result))
      {
        // The value isn't in the cache yet. Get the writer lock.
        cacheLock.AcquireWriterLock(-1);
        try
        {
          // Check again; it may have taken a while to get the lock.
          if (!descriptionsForType.TryGetValue(enumValue, out result))
          {
            // Still not in the cache, so add it to the cache.
            string enumValueString = enumValue.ToString();
            FieldInfo fieldInfo = enumType.GetField(enumValueString);
            DescriptionAttribute[] descriptionAttributes =
              (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);
            result = descriptionAttributes.Length > 0 ?
              descriptionAttributes[0].Description : enumValueString;
            descriptionsForType.Add(enumValue, result);
          }
        }
        finally
        {
          cacheLock.ReleaseWriterLock();
        }
      }

      return result;
    }

    public static string GetEnumName(Type enumType, string description)
    {
      string result = description;
      FieldInfo[] fieldInfos = enumType.GetFields();
      foreach (FieldInfo fieldInfo in fieldInfos)
      {
        DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])
          fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptionAttributes.Length > 0 &&
            descriptionAttributes[0].Description == description)
        {
          result = fieldInfo.Name;
        }
      }

      return result;
    }

    public static object GetEnumValue(Type enumType, string description)
    {
      object result = null;

      FieldInfo[] fieldInfos = enumType.GetFields();
      foreach (FieldInfo fieldInfo in fieldInfos)
      {
        DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])
          fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (descriptionAttributes.Length > 0 &&
            descriptionAttributes[0].Description == description)
        {
          result = Enum.Parse(enumType, fieldInfo.Name, false);
        }
      }

      return result;
    }

    public static T ToEnum<T>(this string value)
      where T : struct
    {
      return (T)Enum.Parse(typeof(T), value, true);
    }

    public static Enum ToEnum(Type T, string value)
    {
      return (Enum)Enum.Parse(T, value, true);
    }

    public static T ToEnumFromDescription<T>(this string description)
      where T : struct
    {
      T? result = null;
      Type enumType = typeof(T);
      foreach (string value in Enum.GetNames(enumType))
      {
        FieldInfo fieldInfo = enumType.GetField(value);
        DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])
          fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        foreach (DescriptionAttribute descriptionAttribute
                 in descriptionAttributes)
        {
          if (descriptionAttribute.Description == description)
          {
            result = (T)Enum.Parse(enumType, value);
            break;
          }
        }
        if (result.HasValue)
        {
          break;
        }
      }
      if (!result.HasValue)
      {
        result = (T)Enum.Parse(enumType, description);
      }

      return result.Value;
    }

    public static T? ToNullableEnum<T>(this string value)
      where T : struct
    {
      return string.IsNullOrEmpty(value) ? (T?)null : ToEnum<T>(value);
    }

    public static T? ToNullableEnumFromDescription<T>(this string description)
      where T : struct
    {
      return string.IsNullOrEmpty(description) ||
        !GetAllDescriptions<T>().Contains(description) ?
        (T?)null : ToEnumFromDescription<T>(description);
    }

  }
}

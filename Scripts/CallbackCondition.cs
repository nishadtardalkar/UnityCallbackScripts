using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CallbackCondition : MonoBehaviour
{

    public enum Comparison
    {
        EQUAL,
        GREATER,
        GREATEREQUAL,
        LESSER,
        LESSEREQUAL,
        NOTEQUAL,
        CUSTOM
    }

    public enum DataType
    {
        FLOAT,
        STRING,
        OBJECT,
        CUSTOM
    }

    private class Task
    {
        public Action<object> Work;
        public object Data;
        public DataType DataType;
        public object Data1UnderObject;
        public object Data2UnderObject;
        public FieldInfo Data1;
        public FieldInfo Data2;
        public Comparison Comparison;
        public Func<object, object, bool> CustomComparator;
    }

    private static Hashtable tasks = new Hashtable();
    private List<object> toClear = new List<object>();

    /// <summary>
    /// <para>A static function to add a task to be called after certain condition is matched.</para>
    /// <para>STRING and OBJECT data types support only EQUAL and NOTEQUAL comparisons.</para>
    /// </summary>
    /// <param name="work">The actual action callback</param>
    /// <param name="data">The parameter for the callback method (if required)</param>
    /// <param name="dataType">Choose a data type from provided enum</param>
    /// <param name="data1obj">The object class holding the first data</param>
    /// <param name="data1">First data to be compared to</param>
    /// <param name="data2obj">The object class holding the second data</param>
    /// <param name="data2">Second data to be compared with</param>
    /// <param name="comparison">Choose a comparison type from provided enum</param>
    /// <param name="customComparator">If you choose comparison as CUSTOM then provide a function to compare with</param>
    /// <returns>
    /// If sucess, returns key used to cancel the task if needed using RemoveTask method. 
    /// If failed, returns null.
    /// </returns>
    public static object AddTask(Action<object> work, object data, DataType dataType, object data1obj, FieldInfo data1, object data2obj, FieldInfo data2, Comparison comparison, Func<object, object, bool> customComparator = null)
    {
        if ((dataType == DataType.OBJECT || dataType == DataType.STRING) && !(comparison == Comparison.EQUAL || comparison == Comparison.NOTEQUAL))
        {
            return null;
        }
                
        Task t = new Task();
        t.Work = work;
        t.Data = data;
        t.DataType = dataType;
        t.Data1UnderObject = data1obj;
        t.Data1 = data1;
        t.Data2UnderObject = data2obj;
        t.Data2 = data2;
        t.Comparison = comparison;
        t.CustomComparator = customComparator;

        int key = tasks.Count;
        while (tasks.ContainsKey(key))
        {
            key++;
        }
        tasks[key] = t;

        return key;
    }

    public static void RemoveTask(object key)
    {
        if (tasks.Contains(key))
        {
            tasks.Remove(key);
        }
    }

    public static void RemoveAllTasks()
    {
        tasks.Clear();
    }

    void Update()
    {
        toClear.Clear();
        foreach (object key in tasks.Keys)
        {
            Task t = (Task)tasks[key];
            object d1 = null, d2 = null;
            if (t.Data1 != null)
            {
                d1 = t.Data1.GetValue(t.Data1UnderObject);
            }
            if (t.Data2 != null)
            {
                d2 = t.Data2.GetValue(t.Data2UnderObject);
            }
            if (t.Comparison == Comparison.EQUAL)
            {
                if (t.DataType == DataType.FLOAT && (float)d1 == (float)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
                else if (t.DataType == DataType.STRING && (string)d1 == (string)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
                else if (t.DataType == DataType.OBJECT && d1 == d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.GREATER)
            {
                if (t.DataType == DataType.FLOAT && (double)d1 > (double)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.GREATEREQUAL)
            {
                if (t.DataType == DataType.FLOAT && (double)d1 >= (double)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.LESSER)
            {
                if (t.DataType == DataType.FLOAT && (double)d1 < (double)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.LESSEREQUAL)
            {
                if (t.DataType == DataType.FLOAT && (double)d1 <= (double)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.NOTEQUAL)
            {
                if (t.DataType == DataType.FLOAT && (double)d1 != (double)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
                else if (t.DataType == DataType.STRING && (string)d1 != (string)d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
                else if (t.DataType == DataType.OBJECT && d1 != d2)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
            else if (t.Comparison == Comparison.CUSTOM)
            {
                if (t.CustomComparator(d1, d2))
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
            }
        }
        for (int i = 0; i < toClear.Count; i++)
        {
            tasks.Remove(toClear[i]);
        }
    }
}

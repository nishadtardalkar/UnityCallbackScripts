using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackTimer : MonoBehaviour
{

    private class Task
    {
        public float ExecuteAt;
        public Action<object> Work;
        public object Data;
    }

    private static Hashtable tasks = new Hashtable();
    private static float checkAfter = 0f;
    private List<object> toClear = new List<object>();

    /// <summary>
    /// <para>Adds a task that gives a callback after certain time</para>
    /// </summary>
    /// <param name="work">Function to callback</param>
    /// <param name="delay">Delay for the callback</param>
    /// <param name="data">Paramter to the function</param>
    /// <returns>Key, which can be used to cancel the task using RemoveTask method</returns>
    public static object AddTask(Action<object> work, float delay, object data)
    {
        Task t = new Task();
        t.ExecuteAt = Time.time + delay;
        t.Data = data;
        t.Work = work;

        int key = tasks.Count;
        while (tasks.ContainsKey(key))
        {
            key++;
        }
        tasks[key] = t;

        if (t.ExecuteAt < checkAfter)
        {
            checkAfter = t.ExecuteAt;
        }

        return key;
    }

    public static void RemoveTask(object key)
    {
        if (tasks.ContainsKey(key))
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
        if (Time.time >= checkAfter)
        {

            checkAfter = float.MaxValue;
            toClear.Clear();
            foreach (object key in tasks.Keys)
            {
                Task t = (Task)tasks[key];
                if (Time.time >= t.ExecuteAt)
                {
                    t.Work(t.Data);
                    toClear.Add(key);
                }
                else
                {
                    checkAfter = Mathf.Min(checkAfter, t.ExecuteAt);
                }
            }
            foreach (object key in toClear)
            {
                tasks.Remove(key);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExceptionHandler : MonoBehaviour
{
    private void Awake()
    {
        Application.logMessageReceived += HandleException;
        DontDestroyOnLoad(gameObject);
    }

    private void HandleException(string log_string, string stack_trace, LogType log_type)
    {
        if (log_type == LogType.Exception)
        {

        }
    }
}

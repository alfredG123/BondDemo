using UnityEngine;

public class GeneralError
{
    /// <summary>
    /// Check if the variabel is null
    /// </summary>
    /// <param name="null_variable_name"></param>
    /// <param name="function_name"></param>
    public static void CheckIfNull(object variable_to_check, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the variable is null, report the error
        if (variable_to_check == null)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(variable_to_check) + ", is null.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is less than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfLess(int number_to_check, int limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is less than the limit, report the error
        if (number_to_check < limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is greater than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfGreater(int number_to_check, int limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is greater than the limit, report the error
        if (number_to_check > limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is equal to the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfEqual(int number_to_check, int limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is equal to the limit, report the error
        if (number_to_check == limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is less than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfLess(float number_to_check, float limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is less than the limit, report the error
        if (number_to_check < limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is greater than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfGreater(float number_to_check, float limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is greater than the limit, report the error
        if (number_to_check > limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Check if the number is equal to the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfEqual(float number_to_check, float limit, string function_name)
    {
        string message;

        VerifyFunctionName(function_name);

        message = string.Empty;

        // If the number is equal to the limit, report the error
        if (number_to_check == limit)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Verify the parent has the enough child to retrieve
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    /// <param name="function_name"></param>
    public static void VerifyChildCount(int index, GameObject parent, string function_name)
    {
        VerifyChildCount(index, parent.transform, function_name);
    }

    /// <summary>
    /// Verify the parent has the enough child to retrieve
    /// </summary>
    /// <param name="index"></param>
    /// <param name="parent"></param>
    /// <param name="function_name"></param>
    public static void VerifyChildCount(int index, Transform parent, string function_name)
    {
        int min_value;
        string message;

        VerifyFunctionName(function_name);

        min_value = 0;
        message = string.Empty;

        // Check the parameter
        if (index < min_value)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(index) + ", is invalid.";
        }
        else if (parent == null)
        {
            message = "In the function, " + function_name + ", the variable, " + nameof(parent) + ", is null.";
        }

        // Check if the index is valid
        else if (index >= parent.childCount)
        {
            message = "In the function, " + function_name + ", the index is greater than the child count.";
        }

        // Report the error, if the message is set
        if (!string.IsNullOrEmpty(message))
        {
            ReturnToTitleSceneForErrors(message);
        }
    }

    /// <summary>
    /// Verify the function name is set
    /// </summary>
    /// <param name="function_name"></param>
    private static void VerifyFunctionName(string function_name)
    {
        if (string.IsNullOrEmpty(function_name))
        {
            ReturnToTitleSceneForErrors("The function name is not set up.");
        }
    }

    /// <summary>
    /// Load the title scene, and report errors in the console 
    /// </summary>
    /// <param name="place_of_occurrence"></param>
    /// <param name="additional_message"></param>
    private static void ReturnToTitleSceneForErrors(string message_to_report)
    {
        // Report the bug in the console
        Debug.LogError(message_to_report);

        // Add some transitions

        // Load the title screen
        GeneralScene.LoadScene(Scene.Title);
    }
}

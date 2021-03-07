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
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the variable is null, report the error
        if (variable_to_check == null)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(variable_to_check) + ", is null.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    public static void CheckIfEmpty(string string_to_check, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the variable is null, report the error
        if (string.IsNullOrEmpty(string_to_check))
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(string_to_check) + ", is null or empty.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is less than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfLess(int number_to_check, int limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is less than the limit, report the error
        if (number_to_check < limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is greater than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfGreater(int number_to_check, int limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is greater than the limit, report the error
        if (number_to_check > limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is equal to the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfEqual(int number_to_check, int limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is equal to the limit, report the error
        if (number_to_check == limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is less than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfLess(float number_to_check, float limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is less than the limit, report the error
        if (number_to_check < limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is greater than the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfGreater(float number_to_check, float limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is greater than the limit, report the error
        if (number_to_check > limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Check if the number is equal to the specified limit
    /// </summary>
    /// <param name="number_to_check"></param>
    /// <param name="limit"></param>
    /// <param name="function_name"></param>
    public static void CheckIfEqual(float number_to_check, float limit, string function_name)
    {
        string result;

        VerifyFunctionName(function_name);

        result = string.Empty;

        // If the number is equal to the limit, report the error
        if (number_to_check == limit)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(number_to_check) + ", is invalid.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
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
        string result;

        VerifyFunctionName(function_name);

        min_value = 0;
        result = string.Empty;

        // Check the parameter
        if (index < min_value)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(index) + ", is invalid.";
        }
        else if (parent == null)
        {
            result = "In the function, " + function_name + ", the variable, " + nameof(parent) + ", is null.";
        }

        // Check if the index is valid
        else if (index >= parent.childCount)
        {
            result = "In the function, " + function_name + ", the index is greater than the child count.";
        }

        // Report the result
        ReturnToTitleSceneIfError(result);
    }

    /// <summary>
    /// Verify the function name is set
    /// </summary>
    /// <param name="function_name"></param>
    private static void VerifyFunctionName(string function_name)
    {
        if (string.IsNullOrEmpty(function_name))
        {
            ReturnToTitleSceneIfError("The function name is not set up.");
        }
    }

    /// <summary>
    /// Load the title scene, and report errors in the console 
    /// </summary>
    /// <param name="place_of_occurrence"></param>
    /// <param name="additional_message"></param>
    private static void ReturnToTitleSceneIfError(string message_to_report)
    {
        // If there is no error, return
        if (string.IsNullOrEmpty(message_to_report))
        {
            return;
        }

        // Report the bug in the console
        Debug.LogError(message_to_report);

        // Add some transitions

        // Load the title screen
        GeneralScene.LoadScene(GeneralScene.Scene.Title);
    }
}

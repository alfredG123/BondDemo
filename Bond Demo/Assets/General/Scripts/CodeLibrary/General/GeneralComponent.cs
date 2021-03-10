using UnityEngine;
using UnityEngine.UI;

public static class GeneralComponent
{
    /// <summary>
    /// Get a button component from the game object
    /// </summary>
    /// <param name="object_to_get"></param>
    /// <returns></returns>
    public static Button GetButton(GameObject object_to_get)
    {
        Button button;

        // If the current mode is testing, check all the parameters
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(object_to_get, "GetButton");
        }

        button = object_to_get.GetComponent<Button>();

        // If the current mode is testing, check if the button component is in the game object
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(button, "GetButton");
        }

        return (button);
    }

    /// <summary>
    /// Modified the text of the text component
    /// </summary>
    /// <param name="object_to_set"></param>
    /// <param name="text_to_set"></param>
    public static void SetText(GameObject object_to_set, string text_to_set)
    {
        Text text_object;

        // If the current mode is testing, check all the parameters
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(object_to_set, "SetText");
            GeneralError.CheckIfNull(text_to_set, "SetText");
        }

        text_object = object_to_set.GetComponent<Text>();

        // If the current mode is testing, check if the text component is in the game object
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(text_object, "SetText");
        }

        text_object.text = text_to_set;
    }

    /// <summary>
    /// Modified the sprite of the image component
    /// </summary>
    /// <param name="object_to_set"></param>
    /// <param name="sprite_to_set"></param>
    public static void SetSprite(GameObject object_to_set, Sprite sprite_to_set)
    {
        Image image_object;

        // If the current mode is testing, check all the parameters
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(object_to_set, "SetSprite");
            GeneralError.CheckIfNull(sprite_to_set, "SetSprite");
        }

        image_object = object_to_set.GetComponent<Image>();

        // If the current mode is testing, check if the image component is in the game object
        if (GeneralSetting.IsTestingEnabled())
        {
            GeneralError.CheckIfNull(image_object, "SetSprite");
        }

        image_object.sprite = sprite_to_set;
    }
}

using UnityEngine;
using UnityEngine.UI;

public static class GeneralComponent
{
    /// <summary>
    /// Modified the text of the text component
    /// </summary>
    /// <param name="object_to_set"></param>
    /// <param name="text_to_set"></param>
    public static void SetText(GameObject object_to_set, string text_to_set)
    {
        Text text_object;

        GeneralError.CheckIfNull(object_to_set, "SetText");
        GeneralError.CheckIfNull(text_to_set, "SetText");

        text_object = object_to_set.GetComponent<Text>();

        GeneralError.CheckIfNull(text_object, "SetText");

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

        GeneralError.CheckIfNull(object_to_set, "SetSprite");
        GeneralError.CheckIfNull(sprite_to_set, "SetSprite");

        image_object = object_to_set.GetComponent<Image>();

        GeneralError.CheckIfNull(image_object, "SetText");

        image_object.sprite = sprite_to_set;
    }
}

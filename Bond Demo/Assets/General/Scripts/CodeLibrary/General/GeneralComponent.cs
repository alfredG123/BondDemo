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

        button = object_to_get.GetComponent<Button>();

        return (button);
    }

    /// <summary>
    /// Modified the text of the text component
    /// </summary>
    /// <param name="game_object"></param>
    /// <param name="text_to_set"></param>
    public static void SetText(this GameObject game_object, string text_to_set)
    {
        Text text_object;

        text_object = game_object.GetComponent<Text>();

        text_object.text = text_to_set;
    }

    /// <summary>
    /// Modified the text of the text component
    /// </summary>
    /// <param name="text_object"></param>
    /// <param name="text_to_set"></param>
    public static void SetText(this Text text_object, string text_to_set)
    {
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

        image_object = object_to_set.GetComponent<Image>();

        image_object.sprite = sprite_to_set;
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AssetsLoader : MonoBehaviour
{
    public static AssetsLoader Assets { get; private set; }

    private static readonly List<GameObject> _RetrievedGameObjectList = new List<GameObject>();
    private static readonly List<Sprite> _RetrievedSpriteList = new List<Sprite>();

    #region INITIALIZATION
    /// <summary>
    /// Set the global static variable
    /// </summary>
    private void Awake()
    {
        if (Assets == null)
        {
            Assets = this;
        }
    }
    #endregion

    /// <summary>
    /// Get the transform from the Resourcs folder
    /// </summary>
    /// <param name="transform_name"></param>
    /// <param name="load_enum"></param>
    /// <returns></returns>
    public Transform LoadTransform(string transform_name, LoadObjectEnum load_enum)
    {
        return (LoadGameObject(transform_name, load_enum).transform);
    }

    /// <summary>
    /// Get the prefab from the Resourcs folder
    /// </summary>
    /// <param name="game_object_name"></param>
    /// <returns></returns>
    public GameObject LoadGameObject(string game_object_name, LoadObjectEnum load_enum)
    {
        string game_object_name_with_path;
        GameObject game_object;

        // Find the game object in the retrieved list
        game_object = _RetrievedGameObjectList.Where(obj => obj.name == game_object_name).FirstOrDefault();

        // If the game object is not in the list, load the prefab from the Resources Folder
        if (game_object == null)
        {
            game_object_name_with_path = load_enum.Path + "/" + game_object_name;

            game_object = Resources.Load<GameObject>(game_object_name_with_path);

            // Add the game object to the retrieved list for the future use
            _RetrievedGameObjectList.Add(game_object);
        }

        return (game_object);
    }

    /// <summary>
    /// Get the sprite from the Resourcs folder
    /// </summary>
    /// <param name="sprite_name"></param>
    /// <param name="load_enum"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string sprite_name, LoadObjectEnum load_enum)
    {
        string sprite_name_with_path;
        Sprite sprite;

        // Find the sprit in the retrieved list
        sprite = _RetrievedSpriteList.Where(image => image.name == sprite_name).FirstOrDefault();

        // If the sprite is not in the list, load the prefab from the Resources Folder
        if (sprite == null)
        {
            sprite_name_with_path = load_enum.Path + "/" + sprite_name;

            sprite = Resources.Load<Sprite>(sprite_name_with_path);

            // Add the sprite to the retrieved list for the future use
            _RetrievedSpriteList.Add(sprite);
        }

        return (sprite);
    }
}

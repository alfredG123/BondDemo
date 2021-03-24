using UnityEngine;

public class BaseGrid<T>
{
    private readonly float _cell_size;
    private readonly T[,] _grid_array;
    private readonly Vector2 _origin_point;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cell_size"></param>
    /// <param name="origin_point"></param>
    public BaseGrid(int width, int height, float cell_size, Vector2 origin_point)
    {
        Width = width;
        Height = height;
        _cell_size = cell_size;
        _origin_point = origin_point;

        _grid_array = new T[width, height];
    }

    #region Properties
    public int Width { get; private set; }

    public int Height { get; private set; }
    #endregion

    /// <summary>
    /// Store the object at the specified coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, T value)
    {
        // Verify the coordinate
        if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
        {
            return;
        }

        _grid_array[x, y] = value;
    }

    /// <summary>
    /// Store the object at the specified position
    /// </summary>
    /// <param name="position"></param>
    public void SetValue(Vector2 position, T value)
    {
        GetCoordinate(position, out int x, out int y);

        SetValue(x, y, value);
    }

    /// <summary>
    /// Get the object at the specifed coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public T GetValue(int x, int y)
    {
        if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
        {
            return (default);
        }

        return (_grid_array[x, y]);
    }

    /// <summary>
    /// Get the object at the specified position
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public T GetValue(Vector2 position)
    {
        GetCoordinate(position, out int x, out int y);

        return (GetValue(x, y));
    }

    /// <summary>
    /// Get the x and y coodinates on the grid based on the given position
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetCoordinate(Vector2 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - _origin_point).x / _cell_size);
        y = Mathf.FloorToInt((position - _origin_point).y / _cell_size);
    }

    /// <summary>
    /// Get the position on the screen based on the given x and y coodinates
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 GetPosition(int x, int y)
    {
        return (new Vector2(x, y) * _cell_size + _origin_point + new Vector2(_cell_size, _cell_size) * .5f);
    }

    /// <summary>
    /// Reset all cells to the default value
    /// </summary>
    public void ClearGrid()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                _grid_array[i, j] = default;
            }
        }
    }
}

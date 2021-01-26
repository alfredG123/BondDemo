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
    /// Convert the coordinate on the grid to a vector
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Vector2 ConvertCoordinateToPosition(int x, int y)
    {
        return (new Vector2(x, y) * _cell_size + _origin_point + new Vector2(_cell_size, _cell_size) * .5f);
    }

    /// <summary>
    /// Store the object at the specific coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, T value)
    {
        if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height))
        {
            return;
        }

        _grid_array[x, y] = value;
    }

    /// <summary>
    /// Get the object at the specific coordinate
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

    public T GetValue(Vector2 position)
    {
        GetGridPosition(position, out int x, out int y);

        return (GetValue(x, y));
    }

    private void GetGridPosition(Vector2 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - _origin_point).x / _cell_size);
        y = Mathf.FloorToInt((position - _origin_point).y / _cell_size);
    }

    /// <summary>
    /// Reset all cells
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

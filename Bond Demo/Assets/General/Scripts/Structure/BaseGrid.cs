using UnityEngine;

public class BaseGrid<T>
{
    private int _width;
    private int _height;
    private float _cell_size;
    private T[,] _grid_array;
    private Vector2 _origin_point;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="cell_size"></param>
    /// <param name="origin_point"></param>
    public BaseGrid(int width, int height, float cell_size, Vector2 origin_point)
    {
        _width = width;
        _height = height;
        _cell_size = cell_size;
        _origin_point = origin_point;

        _grid_array = new T[width, height];
    }

    #region Properties
    public int CellCount
    {
        get => (_width * _height);
    }
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
    /// Convert the position to the coordinate on the grid
    /// </summary>
    /// <param name="position"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void ConvertPositionToCoordinate(Vector2 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - _origin_point).x / _cell_size);
        y = Mathf.FloorToInt((position - _origin_point).y / _cell_size);
    }

    /// <summary>
    /// Store the object at the specific coordinate
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="value"></param>
    public void SetValue(int x, int y, T value)
    {
        if ((x < 0) || (x >= _width) || (y < 0) || (y >= _height))
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
        if ((x < 0) || (x >= _width) || (y < 0) || (y >= _height))
        {
            return (default(T));
        }

        return (_grid_array[x, y]);
    }

    public T GetValue(Vector2 position)
    {
        int x;
        int y;

        GetGridPosition(position, out x, out y);

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
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                _grid_array[i, j] = default(T);
            }
        }
    }
}

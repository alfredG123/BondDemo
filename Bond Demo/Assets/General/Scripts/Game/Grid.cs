using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    private int _width;
    private int _height;
    private float _cell_size;
    private T[,] _grid_array;
    private Vector2 _origin_point;

    public Grid(int width, int height, float cell_size, Vector2 origin_point)
    {
        _width = width;
        _height = height;
        _cell_size = cell_size;
        _origin_point = origin_point;

        _grid_array = new T[width, height];
    }

    public int Length
    {
        get => (_width * _height);
    }

    public Vector2 GetGridPositionInWorldPosition(int x, int y)
    {
        return (new Vector2(x, y) * _cell_size + _origin_point + new Vector2(_cell_size, _cell_size) * .5f);
    }

    private void GetGridPosition(Vector2 position, out int x, out int y)
    {
        x = Mathf.FloorToInt((position - _origin_point).x / _cell_size);
        y = Mathf.FloorToInt((position - _origin_point).y / _cell_size);
    }

    public void SetValue(int x, int y, T value)
    {
        if ((x < 0) || (x >= _width) || (y < 0) || (y >= _height))
        {
            return;
        }

        _grid_array[x, y] = value;
    }

    public void SetValue(Vector2 position, T value)
    {
        int x;
        int y;

        GetGridPosition(position, out x, out y);

        SetValue(x, y, value);
    }

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

        return(GetValue(x, y));
    }
}

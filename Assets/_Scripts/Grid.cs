using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid
{
    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public float cellSize { get; private set; }
    private float cellRadius;
    public int width { get; private set; }
    public int height { get; private set; }
    public Vector3 originPosition { get; private set; }

    private int[,] gridArray;

    public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        cellRadius = cellSize / 2;
        originPosition = _originPosition;

        gridArray = new int[width, height];

        DrawVisuals();
    }


    public void DrawVisuals(Transform parent = null)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Debug.Log("Draw visuals");
                Debug.DrawLine(CoordinatesToWorldPosition(x, y), CoordinatesToWorldPosition(x, y + 1), Color.white, 100);
                Debug.DrawLine(CoordinatesToWorldPosition(x, y), CoordinatesToWorldPosition(x + 1, y), Color.white, 100);
                CreateText(gridArray[x, y].ToString(), parent, CoordinatesToWorldPosition(x, y), 7, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                //Handles.Label(CoordinatesToWorldPosition(x, y), "0");
            }
        Debug.DrawLine(CoordinatesToWorldPosition(0, height), CoordinatesToWorldPosition(width, height), Color.white, 100);
        Debug.DrawLine(CoordinatesToWorldPosition(width, 0), CoordinatesToWorldPosition(width, height), Color.white, 100);
    }

    private TextMesh CreateText(string text, Transform parent, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("Debug_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = new Vector3(localPosition.x + cellRadius, localPosition.y, localPosition.z + cellRadius);
        transform.rotation = Quaternion.Euler(90, 0, 0);
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    public void SetValue(int value, int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
    }

    public void SetValue(int value, Vector2Int coordinates)
    {
        SetValue(value, coordinates.x, coordinates.y);
    }

    public void SetValue(int value, Vector3 worldPosition)
    {
        int x, y;
        WorldPositionToCoordinates(worldPosition, out x, out y);
        SetValue(value, x, y);
    }

    public int GetValue(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return gridArray[x, y];
        return 0;
    }

    public int GetValue(Vector3 position)
    {
        int x, y;
        WorldPositionToCoordinates(position, out x, out y);
        return GetValue(x, y);
    }


    public void WorldPositionToCoordinates(Vector3 worldPostion, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPostion - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPostion - originPosition).z / cellSize);
    }

    public Vector3 CoordinatesToWorldPosition(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public Vector3 CoordinatesToWorldPosition(Vector2Int coordinates)
    {
        return CoordinatesToWorldPosition(coordinates.x, coordinates.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid
{
    private readonly bool XY;

    private float cellSize;
    private float cellRadius;
    private int width;
    private int height;
    public Vector3 originPosition { get; private set; }

    private int[,] gridArray;
    private TextMesh[,] debugArray;

    public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition, bool _XY = false)
    {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        cellRadius = cellSize / 2;
        originPosition = _originPosition;
        gridArray = new int[width, height];
        debugArray = new TextMesh[width, height];
        XY = _XY;
    }

    public void DrawVisuals(Transform parent = null)
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Debug.DrawLine(CoorToWorld(x, y), CoorToWorld(x, y + 1), Color.white, 100);
                Debug.DrawLine(CoorToWorld(x, y), CoorToWorld(x + 1, y), Color.white, 100);

                if (debugArray[x, y] == null)
                    debugArray[x, y] = CreateText(gridArray[x, y].ToString(), parent, CoorToWorld(x, y), 7, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                else
                    debugArray[x, y].text = gridArray[x, y].ToString();
            }

        Debug.DrawLine(CoorToWorld(0, height), CoorToWorld(width, height), Color.white, 100);
        Debug.DrawLine(CoorToWorld(width, 0), CoorToWorld(width, height), Color.white, 100);
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
            gridArray[x, y] = value;
    }

    public void SetValue(int value, Vector2Int coordinates)
    {
        SetValue(value, coordinates.x, coordinates.y);
    }

    public void SetValue(int value, Vector3 worldPosition)
    {
        int x, y;
        WorldToCoor(worldPosition, out x, out y);
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
        WorldToCoor(position, out x, out y);
        return GetValue(x, y);
    }


    public void WorldToCoor(Vector3 worldPostion, out int x, out int y)
    {
        if (XY)
            WorldToCoorXY(worldPostion, out x, out y);
        else
            WorldToCoorXZ(worldPostion, out x, out y);
    }

    private void WorldToCoorXY(Vector3 worldPostion, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPostion - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPostion - originPosition).y / cellSize);
    }

    private void WorldToCoorXZ(Vector3 worldPostion, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPostion - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPostion - originPosition).z / cellSize);
    }

    public Vector3 CoorToWorld(int x, int y)
    {
        if (XY)
            return CoorToWorldXY(x, y);
        else
            return CoorToWorldXZ(x, y);
    }
    private Vector3 CoorToWorldXY(int x, int y)
    {
        return new Vector3(x, y, 0) * cellSize + originPosition;
    }

    private Vector3 CoorToWorldXZ(int x, int y)
    {
        return new Vector3(x, 0, y) * cellSize + originPosition;
    }

    public Vector3 CoorToWorld(Vector2Int coordinates)
    {
        return CoorToWorld(coordinates.x, coordinates.y);
    }
}

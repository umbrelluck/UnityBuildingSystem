using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGrid : MonoBehaviour
{
    public Grid grid { get; private set; }
    public Transform canvasParent;
    public int width, height;
    public float cellSize;

    public Transform prefab;
    private Transform flyingBuilding;
    private BuildingGrid bg;
    private bool available;

    [SerializeField] private LayerMask layer;

    private void Awake()
    {
        grid = new Grid(width, height, cellSize, Vector3.zero);
        grid.DrawVisuals(canvasParent);

        SetUpBuilding();
    }

    void SetUpBuilding()
    {
        if (flyingBuilding != null)
            Destroy(flyingBuilding.gameObject);
        else
        {
            flyingBuilding = Instantiate(prefab, new Vector3(-10, -10, -10), Quaternion.identity);
            bg = flyingBuilding.GetComponent<BuildingGrid>();
            bg.SetUp(cellSize);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SetUpBuilding();
        }

        if (flyingBuilding != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layer))
            {
                grid.WorldPositionToCoordinates(hit.point, out int x, out int y);
                Vector3 pos = grid.CoordinatesToWorldPosition(x, y);

                //check on boundaries
                available = true;
                if ((pos.x < (grid.CoordinatesToWorldPosition(0, 0).x)) || pos.x > (width - bg.width) * grid.cellSize)
                    available = false;
                if ((pos.z < (grid.CoordinatesToWorldPosition(0, 0).z)) || pos.z > (height - bg.height) * grid.cellSize)
                    available = false;

                //check on overlap
                //leftmost bottom coordinate
                //from here check on grid[x,y]=none else smth is there;
                x -= bg.width / 2;
                y -= bg.height / 2;
                for (int i = x; i < x + bg.width; i++)
                    for (int q = y; q < y + bg.height; q++)
                        if (grid.GetValue(i, q) != 0)
                        {
                            available = false;
                            break;
                        }

                flyingBuilding.transform.position = pos;

                bg.SetColor(available);

                if (Input.GetMouseButtonDown(0) && available)
                {
                    Build(x, y);
                }
            }
        }
    }

    private void Build(int x, int y)
    {
        for (int i = x; i < x + bg.width; i++)
            for (int q = y; q < y + bg.height; q++)
                grid.SetValue(9, i, q);
        bg.SetBuildColor();
        flyingBuilding = null;
    }
}

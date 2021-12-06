using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    public int width { get; private set; }
    public int height { get; private set; }

    [SerializeField] private Renderer render;
    public Vector3 offset { get; private set; }

    public void SetUp(float sellSize)
    {
        BoxCollider colider = gameObject.GetComponentInChildren<BoxCollider>();

        width = (int)((colider.bounds.max.x - colider.bounds.min.x) / sellSize) + 1;
        height = (int)((colider.bounds.max.z - colider.bounds.min.z) / sellSize) + 1;

        offset = new Vector3(width * sellSize / 2, 0, height * sellSize / 2);
    }


    public void SetColor(bool available)
    {
        if (available)
            render.material.color = Color.green;
        else
            render.sharedMaterial.color = Color.red;

    }
    public void SetBuildColor()
    {
        render.material.color = Color.white;
    }


}

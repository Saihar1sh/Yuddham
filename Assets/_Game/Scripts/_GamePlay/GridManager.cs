using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2 playAreaSize;
    [SerializeField] private int bridgeAreaSize;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject bridgePrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private List<Grid> gridList = new List<Grid>();

    private void Awake()
    {
        if (gridParent == null)
        {
            gridParent = new GameObject("GridParent").transform;
        }

        CreateGameArea();
    }

    private void CreateGameArea()
    {
        int bridgeIterator = 0;
        for (int i = 0; i < playAreaSize.x * playAreaSize.y; i++)
        {
            int x = (int)(i % playAreaSize.x);
            int y = (int)(i / playAreaSize.x);

            int bridgeFloor = Mathf.FloorToInt(playAreaSize.x / 4);
            if (bridgeIterator < bridgeAreaSize && (x == bridgeFloor || x == bridgeFloor * 3))
            {
                GameObject bridge1 = Instantiate(bridgePrefab, gridParent);
                GameObject bridge2 = Instantiate(bridgePrefab, gridParent);
                bridge1.transform.position = new Vector3(x, 0, playAreaSize.y/2 );
                bridge2.transform.position = new Vector3(x, 0, playAreaSize.y/2 + 1);
                bridgeIterator++;
                
            }
            if (y >= playAreaSize.y/2)
                y += bridgeAreaSize;
            
            GameObject grid = Instantiate(gridPrefab, gridParent);
            grid.transform.position = new Vector3(x, 0, y);
            gridList.Add(grid.GetComponent<Grid>());
        }
    }
}
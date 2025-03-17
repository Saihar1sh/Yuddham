using System;
using System.Collections;
using System.Collections.Generic;
using Arixen.ScriptSmith;
using UnityEngine;
using UnityEngine.Serialization;

namespace Yuddham
{
    public class GridManager : MonoGenericLazySingleton<GridManager>
    {
        [SerializeField] private Vector2 playAreaSize;
        [SerializeField] private int bridgeAreaSize;
        [SerializeField] private GridCube gridPrefab;
        [SerializeField] private GridCube bridgePrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private List<GridCube> gridList = new List<GridCube>();

        public Action<GridCube> OnHoverGrid;

        protected override void Awake()
        {
            base.Awake();
            if (gridParent == null)
            {
                gridParent = new GameObject("GridParent").transform;
            }
            gridList = new List<GridCube>();
            CreateGameArea();
        }
        

        [ContextMenu(nameof(CreateGameArea))]
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
                    GridCube bridge1 = Instantiate(bridgePrefab, gridParent);
                    GridCube bridge2 = Instantiate(bridgePrefab, gridParent);
                    bridge1.transform.position = new Vector3(x, 0, playAreaSize.y / 2);
                    bridge2.transform.position = new Vector3(x, 0, playAreaSize.y / 2 + 1);
                    bridgeIterator++;

                }

                if (y >= playAreaSize.y / 2)
                    y += bridgeAreaSize;

                GridCube grid = Instantiate(gridPrefab, gridParent);
                grid.transform.position = new Vector3(x, 0, y);
                grid.Init(new Vector2Int(x,y));
                gridList.Add(grid);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //Dispose();
        }
    }
}
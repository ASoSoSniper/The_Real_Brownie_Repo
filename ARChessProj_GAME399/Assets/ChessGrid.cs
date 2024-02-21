using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessGrid : MonoBehaviour
{
    [SerializeField] int boardSize = 8;

    [SerializeField] GameObject prefab;
    [SerializeField] Transform spawnPoint;
    [SerializeField] Material whiteMaterial;
    [SerializeField] Material blackMaterial;

    [SerializeField] float tileSpacing = 5f;

    public GameObject[,] tiles;


    // Start is called before the first frame update
    void Start()
    {
        tiles = new GameObject[boardSize, boardSize];

        for (int i = 0;  i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                GameObject tile = Instantiate(prefab, spawnPoint);

                float x = i * (tile.transform.localScale.x + tileSpacing);
                float z = j * (tile.transform.localScale.z + tileSpacing);
                tile.transform.localPosition = new Vector3(x, 0, z);

                if ((i + j) % 2 == 0)
                {
                    tile.GetComponentInChildren<MeshRenderer>().material = whiteMaterial;
                }
                else
                {
                    tile.GetComponentInChildren<MeshRenderer>().material = blackMaterial;
                }

                tiles[i, j] = tile;
            }
        }
    }

    public Vector3 GetPosition(char letter, int number)
    {
        return Vector3.zero;
    }

    public GameObject GetTile(GameObject tile)
    {
        for (int x = 0; x < Mathf.Sqrt(tiles.Length); x++)
        {
            for (int z = 0; z < Mathf.Sqrt(tiles.Length); z++)
            {
                if (tile.transform.parent.transform.position == tiles[x, z].transform.position)
                {
                    return tile;
                }
            }
        }
        return null;
    }

    public void GetTileCoordinates(GameObject tile, out int tileX, out int tileZ)
    {
        for (int x = 0; x < Mathf.Sqrt(tiles.Length); x++)
        {
            for (int z = 0; z < Mathf.Sqrt(tiles.Length); z++)
            {
                if (tile.transform.parent.transform.position == tiles[x, z].transform.position)
                {
                    tileX = x;
                    tileZ = z;
                    return;
                }
            }
        }

        tileX = 0;
        tileZ = 0;
        return;
    }

    public bool TileHasEnemy(GameObject tile)
    {
        bool hit = Physics.Raycast(tile.transform.position, tile.transform.position + Vector3.up * 5f);

        if (hit) return true;

        return false;
    }
}

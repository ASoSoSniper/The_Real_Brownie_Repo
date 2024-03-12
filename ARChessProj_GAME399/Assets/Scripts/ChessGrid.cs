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
    public bool TileExists(int x, int z)
    {
        bool validX = x >= 0 && x < Mathf.Sqrt(tiles.Length);
        bool validZ = z >= 0 && z < Mathf.Sqrt(tiles.Length);

        //Debug.Log(x + " = " + validX + ", " + z + " = " + validZ);
        if (validX && validZ)
        {
            if (tiles[x, z] != null)
            {
                return true;
            }
        }

        return false;
    }
    public GameObject GetTile(int x, int z)
    {
        if (!TileExists(x, z)) return null;

        return tiles[x, z];
    }

    public void GetTileCoordinates(GameObject tile, out int tileX, out int tileZ)
    {
        for (int x = 0; x < Mathf.Sqrt(tiles.Length); x++)
        {
            for (int z = 0; z < Mathf.Sqrt(tiles.Length); z++)
            {
                if (TileExists(x, z))
                {
                    if (tile.transform.parent.transform.position == tiles[x, z].transform.position)
                    {
                        tileX = x;
                        tileZ = z;
                        return;
                    }
                }
            }
        }

        tileX = 0;
        tileZ = 0;
        return;
    }

    public bool TileHasEnemy(GameObject tile, ChessPiece movingPiece)
    {
        RaycastHit result;
        bool hit = Physics.Raycast(tile.transform.GetChild(0).transform.position + -transform.up * 5f, transform.up, out result, 10f);

        if (!hit) return false;

        ChessPiece piece = result.collider.transform.GetComponent<ChessPiece>();
        if (piece) return piece.team != movingPiece.team;

        return false;
    }

    public ChessPiece.Teams GetPieceOnTile(GameObject tile)
    {
        RaycastHit result;
        bool hit = Physics.Raycast(tile.transform.GetChild(0).transform.position + -transform.up * 5f, transform.up, out result, 10f);
        Debug.DrawLine(tile.transform.GetChild(0).transform.position, tile.transform.GetChild(0).transform.position + transform.up * 5f, Color.red, 5f);

        if (!hit) return ChessPiece.Teams.None;

        ChessPiece piece = result.collider.transform.GetComponent<ChessPiece>();
        if (piece) return piece.team;

        return ChessPiece.Teams.None;
    }

    public ChessPiece GetChessPiece(int x, int z, ChessPiece.Teams teamToCheck)
    {
        if (!TileExists(x, z)) return null;
        RaycastHit result;
        bool hit = Physics.Raycast(GetTile(x,z).transform.GetChild(0).transform.position + -transform.up * 5f, transform.up, out result, 10f);

        if (!hit) return null;

        ChessPiece piece = result.collider.transform.GetComponent<ChessPiece>();
        if (!piece) return null;

        if (piece.team == teamToCheck) return piece;

        return null;
    }
    public ChessPiece GetChessPiece(GameObject tile, ChessPiece.Teams teamToCheck)
    {
        RaycastHit result;
        bool hit = Physics.Raycast(tile.transform.GetChild(0).transform.position + -transform.up * 5f, transform.up, out result, 10f);

        if (!hit) return null;

        ChessPiece piece = result.collider.transform.GetComponent<ChessPiece>();
        if (!piece) return null;

        if (piece.team == teamToCheck) return piece;

        return null;
    }

    public void ResetBoardHighlighting()
    {
        for (int x = 0; x < Mathf.Sqrt(tiles.Length); x++)
        {
            for (int z = 0; z < Mathf.Sqrt(tiles.Length); z++)
            {
                GameObject tile = GetTile(x, z);
                if (tile)
                {
                    tile.GetComponentInChildren<Highlight>().SetAsRouteTile(false);
                }
            }
        }
    }
}

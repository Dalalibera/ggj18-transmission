using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[ExecuteInEditMode]
public class TileData : MonoBehaviour
{

    public GameObject full1;
    public GameObject corner1;
    public GameObject half1;
    public GameObject quarter1;
    public GameObject grass1;

    public GameObject full2;
    public GameObject corner2;
    public GameObject half2;
    public GameObject quarter2;
    public GameObject grass2;

    public GameObject full3;
    public GameObject corner3;
    public GameObject half3;
    public GameObject quarter3;
    public GameObject grass3;

    public GameObject full4;
    public GameObject corner4;
    public GameObject half4;
    public GameObject quarter4;
    public GameObject grass4;

    [Serializable]
    public struct CollisionMeshRelation
    {
        public char TileId;
        public Mesh CollisionMesh;
    }

    public CollisionMeshRelation[] CollisionMeshes;

    [TextArea]
    public string Tiles;

    public void RecalculateTiles()
    {
        while(transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            if (Application.isEditor)
                DestroyImmediate(child.gameObject);
            else
                Destroy(child.gameObject);
        }

        if (string.IsNullOrEmpty(Tiles) || string.IsNullOrEmpty(Tiles.Trim()))
        {
            Debug.Log("Tiles is empty");
            return;
        }

        string[] lines = Regex.Split(Tiles, "\r?\n");

        int maxLineSize = lines[0].Length;

        foreach (string line in lines)
        {
            maxLineSize = Math.Max(maxLineSize, line.Length);
        }

        //tileModels.Add('F', Resources.Load("Tiles/tile-fullmountain-1", typeof(GameObject)) as GameObject);
        //tileModels.Add('C', Resources.Load("Tiles/tile-cornermountain-1", typeof(GameObject)) as GameObject);
        //tileModels.Add('H', Resources.Load("Tiles/tile-halfmountain-1", typeof(GameObject)) as GameObject);
        //tileModels.Add('Q', Resources.Load("Tiles/tile-quartermountain-1", typeof(GameObject)) as GameObject);
        //tileModels.Add('G', Resources.Load("Tiles/tile-grass-1", typeof(GameObject)) as GameObject);

        Dictionary<char, GameObject>[] tileModels = new Dictionary<char, GameObject>[4];
        tileModels[0] = new Dictionary<char, GameObject>();
        tileModels[1] = new Dictionary<char, GameObject>();
        tileModels[2] = new Dictionary<char, GameObject>();
        tileModels[3] = new Dictionary<char, GameObject>();

        tileModels[0].Add('F', full1);
        tileModels[1].Add('F', full2);
        tileModels[2].Add('F', full3);
        tileModels[3].Add('F', full4);

        tileModels[0].Add('G', grass1);
        tileModels[1].Add('G', grass2);
        tileModels[2].Add('G', grass3);
        tileModels[3].Add('G', grass4);

        tileModels[0].Add('Q', quarter1);
        tileModels[1].Add('Q', quarter2);
        tileModels[2].Add('Q', quarter3);
        tileModels[3].Add('Q', quarter4);

        tileModels[0].Add('C', corner1);
        tileModels[1].Add('C', corner2);
        tileModels[2].Add('C', corner3);
        tileModels[3].Add('C', corner4);
        
        tileModels[0].Add('H', half1);
        tileModels[1].Add('H', half2);
        tileModels[2].Add('H', half3);
        tileModels[3].Add('H', half4);

        char[,] tiles = new char[lines.Length, maxLineSize];

        int x = 0;
        int z = 0;
        foreach (string line in lines)
        {
            foreach (char c in line.ToUpper().ToCharArray())
            {
                tiles[z, x++] = c;
            }
            x = 0;
            z++;
        }

        TileCalculator calculator = new TileCalculator(tiles);
        int index = 0;

        Dictionary<char, Mesh> collisionMeshes = new Dictionary<char, Mesh>();
        foreach(CollisionMeshRelation rel in CollisionMeshes)
        {
            collisionMeshes.Add(rel.TileId, rel.CollisionMesh);
        }

        for (z = 0; z < tiles.GetLength(0); ++z)
        {
            for (x = 0; x < tiles.GetLength(1); ++x)
            {
                char tile = tiles[z, x];
                if(!tileModels[index % tileModels.Length].ContainsKey(tile))
                    continue;
                // create tile
                GameObject prefab = tileModels[index % tileModels.Length][tile];
                GameObject instance = Instantiate(prefab) as GameObject;
                instance.transform.parent = transform;
                // create collision mesh
                if(collisionMeshes.ContainsKey(tile))
                {
                    MeshCollider collider = instance.AddComponent<MeshCollider>();
                    collider.sharedMesh = collisionMeshes[tile];
                }
                // put it in place
                instance.transform.Translate(x * 4 * 10, 0, z * 4 * 10, Space.Self);
                instance.transform.Rotate(Vector3.up, calculator.CalculateTileRotation(x, z), Space.Self);
                index++;
            }
        }
    }

    private class TileCalculator
    {
        private char[,] tiles;
        private int width;
        private int height;

        public TileCalculator(char[,] tiles)
        {
            this.tiles = tiles;
            this.width = tiles.GetLength(1);
            this.height = tiles.GetLength(0);
        }

        public int CalculateTileRotation(int x, int z)
        {

            switch (tiles[z, x])
            {
                case 'H':
                    if (eastIs(x, z, 'G'))
                        return 90;
                    if (southIs(x, z, 'G'))
                        return 180;
                    if (westIs(x, z, 'G'))
                        return -90;
                    //if (northIs(x, z, 'G'))
                    //    return 0;
                    return 0;
                case 'C':
                    if (southeastIs(x, z, 'G'))
                        return 90;
                    if (southwestIs(x, z, 'G'))
                        return 180;
                    if (northwestIs(x, z, 'G'))
                        return -90;
                    //if (northeastIs(x, z, 'G'))
                    //    return 0;
                    //if (northIs(x, z, 'C', 'H', 'Q') && eastIs(x, z, 'C', 'H', 'Q'))
                    //    return 0;
                    //if (eastIs(x, z, 'C', 'H', 'Q') && southIs(x, z, 'C', 'H', 'Q'))
                    //    return 45;
                    //if (southIs(x, z, 'C', 'H', 'Q') && westIs(x, z, 'C', 'H', 'Q'))
                    //    return 45;
                    //if (westIs(x, z, 'C', 'H', 'Q') && northIs(x, z, 'C', 'H', 'Q'))
                    //    return 45;
                    return 0;
                case 'Q':
                    if (northIs(x, z, 'G') && eastIs(x, z, 'G'))
                        return 90;
                    if (eastIs(x, z, 'G') && southIs(x, z, 'G'))
                        return 180;
                    if (southIs(x, z, 'G') && westIs(x, z, 'G'))
                        return -90;
                    if (westIs(x, z, 'G') && northIs(x, z, 'G'))
                        return 0;
                    return 0;
                case 'F':
                case 'G':
                default:
                    return 0;
            }
        }

        private bool northeastIs(int x, int z, params char[] tile)
        {
            if (z == height - 1 || x == width - 1)
                return false;
            return Array.IndexOf(tile, tiles[z + 1, x + 1]) >= 0;
        }

        private bool northwestIs(int x, int z, params char[] tile)
        {
            if (z == height - 1 || x == 0)
                return false;
            return Array.IndexOf(tile, tiles[z + 1, x - 1]) >= 0;
        }

        private bool southeastIs(int x, int z, params char[] tile)
        {
            if (z == 0 || x == width - 1)
                return false;
            return Array.IndexOf(tile, tiles[z - 1, x + 1]) >= 0;
        }

        private bool southwestIs(int x, int z, params char[] tile)
        {
            if (z == 0 || x == 0)
                return false;
            return Array.IndexOf(tile, tiles[z - 1, x - 1]) >= 0;
        }

        private bool northIs(int x, int z, params char[] tile)
        {
            if (z == height - 1)
                return false;
            return Array.IndexOf(tile, tiles[z + 1, x]) >= 0;
        }

        private bool southIs(int x, int z, params char[] tile)
        {
            if (z == 0)
                return false;
            return Array.IndexOf(tile, tiles[z - 1, x]) >= 0;
        }

        private bool eastIs(int x, int z, params char[] tile)
        {
            if (x == width - 1)
                return false;
            return Array.IndexOf(tile, tiles[z, x + 1]) >= 0;
        }

        private bool westIs(int x, int z, params char[] tile)
        {
            if (x == 0)
                return false;
            return Array.IndexOf(tile, tiles[z, x - 1]) >= 0;
        }
    }
}

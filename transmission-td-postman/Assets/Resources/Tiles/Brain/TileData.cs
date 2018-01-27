using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class TileData : MonoBehaviour
{

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

        Dictionary<char, GameObject> tileModels = new Dictionary<char, GameObject>();
        tileModels.Add('F', Resources.Load("Tiles/tile-fullmountain", typeof(GameObject)) as GameObject);
        tileModels.Add('C', Resources.Load("Tiles/tile-cornermountain", typeof(GameObject)) as GameObject);
        tileModels.Add('H', Resources.Load("Tiles/tile-halfmountain", typeof(GameObject)) as GameObject);
        tileModels.Add('Q', Resources.Load("Tiles/tile-quartermountain", typeof(GameObject)) as GameObject);
        tileModels.Add('G', Resources.Load("Tiles/tile-grass", typeof(GameObject)) as GameObject);

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

        for (z = 0; z < tiles.GetLength(0); ++z)
        {
            for (x = 0; x < tiles.GetLength(1); ++x)
            {
                char tile = tiles[z, x];
                if(!tileModels.ContainsKey(tile))
                    continue;
                GameObject prefab = tileModels[tile];
                GameObject instance = Instantiate(prefab) as GameObject;
                instance.transform.parent = transform;
                instance.transform.Translate(x * 4, 0, z * 4, Space.Self);
                instance.transform.Rotate(Vector3.up, calculator.CalculateTileRotation(x, z), Space.Self);
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

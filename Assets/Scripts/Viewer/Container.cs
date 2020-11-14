using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MazeViewer.Maze;

public class Container : MonoBehaviour
{
    [SerializeField]
    private List<List<CellObj>> cellObjs;

    public float scale;
    public string path = "";
    public GameObject prefab;
    void Start()
    {
        try
        {
            var maze = MazeIO.ReadFromFile(path);
            // convert all to cellObjs;
            for(int i = 0; i < maze.Count; i++)
            {
                for(int j = 0; j < maze[0].Count; j++)
                {
                    var tempObj = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                    tempObj.transform.position += scale * (new Vector3(i, 0, j));
                    var component = tempObj.GetComponent<CellObj>();
                    component.SetState(maze[i][j]);
                }
            }
        }
        catch (FileNotFoundException e)
        {
            Debug.LogError("File not found!");
            throw;
        }
        
    }
}

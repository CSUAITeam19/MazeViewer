using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MazeViewer.Maze;

namespace MazeViewer.Viewer
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private List<List<GameObject>> cellObjs;

        [SerializeField] private CellFactory cellFactory;
        public float scale;
        public string path = "";
        public int maxMergedInOne = 256;
        public Material wallMaterial;
        [SerializeField] private Transform mergedMeshes;
        [SerializeField]  private GameObject rendererPrefeb;
        private Vector3 size;

        private void CombineWithNewMesh(MeshFilter target,  List<CombineInstance> instances)
        {
            var originalMesh = target.mesh;
            target.mesh = new Mesh();
            List<CombineInstance> toCombine = new List<CombineInstance>(instances);
            CombineInstance tempInstance = new CombineInstance
            {
                mesh = originalMesh,
                transform  = target.transform.localToWorldMatrix
            };
            toCombine.Add(tempInstance);
            target.mesh.CombineMeshes(toCombine.ToArray(),true,true);
        }

        IEnumerator MergeMesh()
        {
            int meshMerged = 0;
            GameObject targetObj = Instantiate(rendererPrefeb, mergedMeshes);
            var targetRenderer = targetObj.GetComponent<MeshRenderer>();
            var targetFilter = targetObj.GetComponent<MeshFilter>();
            var toAddInstances = new List<CombineInstance>();
            targetRenderer.sharedMaterial = wallMaterial;
            float beginTime = Time.time;
            
            foreach (var row in cellObjs)
            {
                // get all walls in row
                foreach(var obj in row)
                {
                    if ((obj.GetComponent<WallCell>()) != null)
                    {
                        CombineInstance tempInstance = new CombineInstance
                        {
                            mesh = obj.GetComponent<MeshFilter>().mesh, 
                            transform = obj.transform.localToWorldMatrix
                        };
                        toAddInstances.Add(tempInstance);
                    }
                }

                meshMerged += toAddInstances.Count;
                if(meshMerged < maxMergedInOne)
                {
                    CombineWithNewMesh(targetFilter, toAddInstances);
                }
                else
                {
                    // combine the instances in range
                    int inRangeCount = toAddInstances.Count - meshMerged + maxMergedInOne;
                    List<CombineInstance> instancesInRange =
                        new List<CombineInstance>(toAddInstances.GetRange(0, inRangeCount));
                    CombineWithNewMesh(targetFilter,instancesInRange);
                    // shift to new object, combine the overflowed part
                    targetObj = Instantiate(rendererPrefeb, mergedMeshes);
                    targetRenderer = targetObj.GetComponent<MeshRenderer>();
                    targetRenderer.sharedMaterial = wallMaterial;
                    targetFilter = targetObj.GetComponent<MeshFilter>();
                    toAddInstances.RemoveRange(0, inRangeCount);
                    CombineWithNewMesh(targetFilter, toAddInstances);
                    meshMerged = toAddInstances.Count;
                }
                toAddInstances.Clear();
                if (Time.time - beginTime > 0.02)
                {
                    beginTime = Time.time;
                    yield return null;
                }
            }
                
        }

        void Start()
        {
            var maze = MazeIO.ReadFromFile(path);
            // return if empty
            if (maze.Count < 1 || maze[0].Count < 1) return;
            // convert all to cellObjs;
            cellObjs = new List<List<GameObject>>();
            for (var i = 0; i < maze.Count; i++)
            {
                cellObjs.Add(new List<GameObject>());
                for (var j = 0; j < maze[0].Count; j++)
                {
                    cellObjs[i].Add(cellFactory.GetCellObj(new Vector2Int(i, j), maze[i][j]));
                    cellObjs[i][j].transform.SetParent(transform);
                    var icell = cellObjs[i][j].GetComponent<ICellObj>();
                    icell.Init();
                }
            }

            size = new Vector3(maze.Count, 0, maze[0].Count);
            // transform.position = -size / 2;
            
            // merge all walls
            StartCoroutine(MergeMesh());
        }
    }

}

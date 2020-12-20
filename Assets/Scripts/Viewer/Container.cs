using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MazeViewer.Maze;
using MazeViewer.UI;
using Unitilities;
using UnityEngine.Serialization;
using Logger = MazeViewer.UI.Logger;

namespace MazeViewer.Viewer
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private List<List<GameObject>> cellObjs;

        [SerializeField] private CellFactory cellFactory = default;
        [SerializeField] private EffectFactory effectFactory = default;
        [SerializeField] private ProgressMgr progressMgr = default;
        public float scale;
        [FormerlySerializedAs("path")] public string mazePath = "";
        public string resultPath = "";
        public int maxMergedInOne = 256;
        public Material wallMaterial;
        [SerializeField] private Transform mergedMeshes = default;
        [SerializeField] private GameObject rendererPrefeb = default;
        [SerializeField] private PathDrawer pathDrawer = default;
        private Vector3 size;
        private Vector2Int exitPos;

        private void CombineWithNewMesh(MeshFilter target, List<CombineInstance> instances)
        {
            var originalMesh = target.mesh;
            target.mesh = new Mesh();
            List<CombineInstance> toCombine = new List<CombineInstance>(instances);
            CombineInstance tempInstance = new CombineInstance
            {
                mesh = originalMesh,
                transform = target.transform.localToWorldMatrix
            };
            toCombine.Add(tempInstance);
            target.mesh.CombineMeshes(toCombine.ToArray(), true, true);
        }

        IEnumerator MergeMesh()
        {
            var timeWatcher = System.Diagnostics.Stopwatch.StartNew();
            int meshMerged = 0;
            GameObject targetObj = Instantiate(rendererPrefeb, mergedMeshes);
            var targetRenderer = targetObj.GetComponent<MeshRenderer>();
            var targetFilter = targetObj.GetComponent<MeshFilter>();
            var toAddInstances = new List<CombineInstance>();
            targetRenderer.sharedMaterial = wallMaterial;

            foreach(var row in cellObjs)
            {
                // get all walls in row
                foreach(var obj in row)
                {
                    if((obj.GetComponent<WallCell>()) != null)
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
                    CombineWithNewMesh(targetFilter, instancesInRange);
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
                // 超时则跳过这一帧
                if(timeWatcher.ElapsedMilliseconds > 5)
                {
                    timeWatcher.Restart();
                    yield return null;
                }
            }
            Logger.Instance.PrintInfo("迷宫场景已加载完毕");
        }

        /// <summary>
        /// 加载迷宫和搜索数据
        /// </summary>
        public void LoadAll()
        {
            if (!LoadMaze()) return;
            LoadResult();
        }

        /// <summary>
        /// 加载迷宫, 失败则返回false
        /// </summary>
        /// <returns></returns>
        public bool LoadMaze()
        {
            List<List<MazeState>> maze = new List<List<MazeState>>();
            try
            {
                maze = MazeIO.ReadFromFile(mazePath, out exitPos);
            }
            catch (FileNotFoundException)
            {
                Logger.Instance.PrintError($"无法打开路径为\"{mazePath}\"的迷宫文件!");
            }
            catch (Exception e)
            {
                Logger.Instance.PrintError($"加载迷宫时发生未知错误:{e}");
            }

            // return if empty
            if (maze.Count < 1 || maze[0].Count < 1)
                return false;
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
            return true;
        }
        
        /// <summary>
        /// 加载搜索结果
        /// </summary>
        public void LoadResult()
        {
            List<IRecovableOperation> operations = new List<IRecovableOperation>();
            List<Vector2Int> way = new List<Vector2Int>();
            int finalCost = 0;
            Logger.Instance.PrintInfo("正在载入搜索数据...");
            try
            {
                operations = MazeIO.ReadSearchDataFromFile(resultPath,
                    cellObjs.ConvertAll(row => row.ConvertAll(obj => obj.GetComponent<ICellObj>())), out way, out finalCost);
            }
            catch (FileNotFoundException)
            {
                Logger.Instance.PrintError($"无法打开路径为\"{resultPath}\"的搜索文件!");
            }
            catch (Exception)
            {
                Logger.Instance.PrintError("加载搜索数据时出现未知错误!");
            }

            operations.Add(new PathDrawOperation(way, pathDrawer));
            progressMgr.LoadOperationChain(new OperationChain(operations, -1));
            pathDrawer.HidePath();
            StatusBar.Instance.SetFinalCost(finalCost);
            Logger.Instance.PrintInfo("迷宫数据已加载");
        }

        /// <summary>
        /// 仅清除搜索结果
        /// </summary>
        public void ClearResult()
        {
            progressMgr.BackToBegin();
            EffectFactory.Instance.Initialize();
        }

        /// <summary>
        /// 清空整个场景
        /// </summary>
        public void ClearAll()
        {
            ClearResult();
            cellFactory.RecycleAll();
            // 清除所有生成的网格
            for(int i = 0; i < mergedMeshes.childCount; i++)
            {
                var tempObject = mergedMeshes.GetChild(i).gameObject;
                tempObject.GetComponent<MeshFilter>().mesh = new Mesh();
                // TODO: recycle gameObject
                Destroy(tempObject);
            }
            Logger.Instance.PrintInfo("迷宫已初始化");
        }

        private void Update()
        {
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.Z))
#else
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.Z))
#endif
            {
                progressMgr.BackStep();
            }
#if UNITY_EDITOR
            if(Input.GetKey(KeyCode.Y))
#else
            if((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKey(KeyCode.Y))
#endif
            {
                progressMgr.NextStep();
            }
        }

        [ContextMenu("Redo")]
        public void Redo()
        {
            progressMgr.BackStep();
        }

        [ContextMenu("Undo")]
        public void Undo()
        {
            progressMgr.NextStep();
        }

        /// <summary>
        /// 重载迷宫
        /// <para>等价于先调用ClearMaze();后调用LoadMaze();</para>
        /// </summary>
        public void ReloadMaze()
        {
            ClearAll();
            LoadAll();
        }

        public void UpdateMazePath(string path) => mazePath = path;

        public void UpdateResultPath(string path) => resultPath = path;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using MazeViewer.Viewer;
using Unitilities;
using UnityEngine;

namespace MazeViewer.Maze
{
    public static class MazeIO
    {
        public static List<List<MazeState>> ReadFromFile(string path, out Vector2Int exitPos)
        {
            StringReader stringReader;
            exitPos = new Vector2Int(1,0);
            
            // may throw FileNotFoundException
            stringReader = new StringReader(File.ReadAllText(path));
            
            var s = stringReader.ReadLine().Split(' ');
            var row = int.Parse(s[0]);
            var col = int.Parse(s[1]);
            var tempResult = new List<List<MazeState>>();
            exitPos =Vector2Int.zero;
            for(int i = 0; i < row; i++)
            {
                tempResult.Add(new List<MazeState>(col));
                var line = stringReader.ReadLine().Split(' ');
                for(int j = 0; j < col; j++)
                {
                    tempResult[i].Add((MazeState)int.Parse(line[j]));
                    if (tempResult[i][j] == MazeState.Exit) exitPos = new Vector2Int(i, j);
                }
            }
            return tempResult;
        }

        /// <summary>
        /// 从文件中读取搜索过程
        /// <para>假设文件可打开, 但尽可能兼容文件格式的微小错误</para>
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="cellList">实现搜索操作显示的单元的列表</param>
        /// <param name="way">输出的路径</param>
        /// <returns></returns>
        public static List<IRecovableOperation> ReadSearchDataFromFile(string path, List<List<ICellObj>> cellList,
            out List<Vector2Int> way)
        {
            StringReader stringReader;
            //try
            //{
            //    stringReader = new StringReader(File.ReadAllText(path));
            //}
            //catch (FileNotFoundException)
            //{
            //    Debug.LogError("Search file not found!");
            //    return new OperationChain();
            //}
            var operationList = new List<IRecovableOperation>();
            using(stringReader = new StringReader(File.ReadAllText(path)))
            {
                // parser--------------------------
                // 读取H值矩阵
                // 读入到一个记录中间变量的二维列表中, 避免直接对物体操作
                List<List<CellSearchData>> tempDataList = new List<List<CellSearchData>>();
                for(int i = 0; i < cellList.Count; i++)
                {
                    tempDataList.Add(new List<CellSearchData>());
                    List<string> splited = new List<string>(stringReader.ReadLine().Split(' '));
                    splited.RemoveAll(string.IsNullOrEmpty);
                    for(int j = 0; j < cellList[0].Count; j++)
                    {
                        CellSearchData temp = CellSearchData.defaultData;
                        
                        if(int.TryParse(splited[j], out temp.h))
                        {
                            tempDataList[i].Add(temp);
                        }
                        else
                        {
                            // exit this loop
                            Debug.LogError($"Can not parse h matrix line on ({i}, {j})");
                            // add range to fit the size
                            tempDataList[i].AddRange(Enumerable.Repeat(CellSearchData.defaultData,
                                cellList[i].Count - tempDataList[i].Count));
                        }
                    }
                }

                // 读取操作步骤
                string lineStr;
                while(!(lineStr = stringReader.ReadLine()).StartsWith("way"))
                {
                    var arguments = lineStr.Split(' ');
                    var metaOperCount = int.Parse(arguments[1]);
                    var stepOperation = new StepOperation();
                    for(int i = 0; i < metaOperCount; i++)
                    {
                        // read one line of meta operation
                        var operArg = new List<string>(stringReader.ReadLine().Split(' '));
                        operArg.RemoveAll(string.IsNullOrEmpty);

                        int row = int.Parse(operArg[1]);
                        int col = int.Parse(operArg[2]);
                        int cost = operArg.Count > 3 ? int.Parse(operArg[3]) : 0;

                        var lastData = tempDataList[row][col];
                        var nextData = lastData;
                        nextData.cost = cost;
                        switch(operArg[0])
                        {
                            case "add":
                                nextData.state = SearchState.Opened;
                                break;
                            case "vis":
                                nextData.state = SearchState.Closed;
                                break;
                            case "cost":
                                // nothing
                                break;
                            case "del":
                                nextData.state = SearchState.Idle;
                                break;
                        }
                        // add to stepOperation
                        stepOperation.Add(new MetaSearchOperation(cellList[row][col], lastData,
                            tempDataList[row][col] = nextData));
                    }
                    // add to result chain
                    operationList.Add(stepOperation);
                }
                // read way points are there
                way = new List<Vector2Int>();
                int wayCount = 0;
                try
                {
                    wayCount = int.Parse(lineStr.Split(' ')[1]);
                }
                catch(IndexOutOfRangeException)
                {
                    Debug.LogError("Can not parse way count!");
                }
                for(int i = 0; i < wayCount; i++)
                {
                    try
                    {
                        var splited = stringReader.ReadLine().Split(' ');
                        way.Add(new Vector2Int(int.Parse(splited[0]), int.Parse(splited[1])));
                    }
                    catch
                    {
                        way.Add(Vector2Int.zero);
                        Debug.LogError($"Can not parse way on {i}");
                    }
                }
                // end of parser-------------------
            }
            return operationList;
        }
    }
}


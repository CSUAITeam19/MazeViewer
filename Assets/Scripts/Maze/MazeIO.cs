using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
            try
            {
                stringReader = new StringReader(File.ReadAllText(path));
            }
            catch(FileNotFoundException)
            {
                exitPos = Vector2Int.zero;
                return new List<List<MazeState>>();
            }
            var s = stringReader.ReadLine().Split(' ');
            var col = int.Parse(s[0]);
            var row = int.Parse(s[1]);
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
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="cellList">实现搜索操作显示的单元的列表</param>
        /// <returns></returns>
        public static OperationChain ReadSearchDataFromFile(string path, List<List<ICellObj>> cellList, Vector2Int exitPos)
        {
            StringReader stringReader;
            try
            {
                stringReader = new StringReader(File.ReadAllText(path));
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("Search file not found!");
                return new OperationChain();
            }
            // 建立一个H值数组
            List<List<int>> hList = new List<List<int>>();
            for(int i = 0; i< cellList.Count; i++)
            {
                hList.Add(new List<int>());
                for(int j = 0; j < cellList[0].Count; j++)
                {
                    hList[i].Add(Mathf.Abs(i - exitPos.x) + Mathf.Abs(j - exitPos.y));
                }
            }
            
            var result = new OperationChain();

            // parser--------------------------
            string lineStr;
            while((lineStr = stringReader.ReadLine()) != null)
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
                    int cost = int.Parse(operArg[3]);
                    int h = hList[row][col];
                    switch (operArg[0])
                    {
                        case "add":
                            stepOperation.Add(MetaSearchOperationFactory.MakeOpenOperation(
                                cellList[row][col],
                                cost,
                                h)
                            );
                            break;
                        case "vis":
                            stepOperation.Add(MetaSearchOperationFactory.MakeCloseOperation(
                                cellList[row][col],
                                cost,
                                h)
                            );
                            break;
                        case "cost":
                            stepOperation.Add(MetaSearchOperationFactory.MakeCostRefreshOperation(
                                cellList[row][col],
                                cost)
                            );
                            break;
                    }

                }
                // add to result chain
                result.AddAndExcuteOperation(stepOperation, false);
                // Debug.Log("Step read.");
            }
            // end of parser-------------------
            stringReader.Close();
            return result;
        }
        ///// <summary>
        ///// 格式错误异常
        ///// </summary>
        //public class FormatException : Exception
        //{

        //}
    }
}


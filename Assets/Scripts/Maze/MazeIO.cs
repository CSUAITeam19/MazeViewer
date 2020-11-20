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
        public static List<List<MazeState>> ReadFromFile(string path)
        {
            StringReader stringReader;
            try
            {
                stringReader = new StringReader(File.ReadAllText(path));
            }
            catch(FileNotFoundException)
            {
                return new List<List<MazeState>>();
            }
            var s = stringReader.ReadLine().Split(' ');
            var col = int.Parse(s[0]);
            var row = int.Parse(s[1]);
            var tempResult = new List<List<MazeState>>();
            for(int i = 0; i < row; i++)
            {
                tempResult.Add(new List<MazeState>(col));
                var line = stringReader.ReadLine().Split(' ');
                for(int j = 0; j < col; j++)
                {
                    tempResult[i].Add((MazeState)int.Parse(line[j]));
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
        public static OperationChain ReadSearchDataFromFile(string path, List<List<ICellObj>> cellList)
        {
            StringReader stringReader;
            try
            {
                stringReader = new StringReader(File.ReadAllText(path));
            }
            catch (FileNotFoundException)
            {
                Debug.LogError("Search file not found!");
            }
            List<List<CellSearchData>> list = new List<List<CellSearchData>>();
            for(int i = 0; i< cellList.Count; i++)
            {
                list.Add(new List<CellSearchData>());
                for(int j = 0; j < cellList[0].Count; j++)
                {
                    list[i].Add(CellSearchData.defaultData);
                }
            }
            // TODO: Read
            return new OperationChain();
        }
    }
}


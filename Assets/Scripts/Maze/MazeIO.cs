using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MazeViewer.Maze
{
    public static class MazeIO
    {
        public static List<List<MazeState>> ReadFromFile(string path)
        {
            int row, col;
            StringReader fs = new StringReader(File.ReadAllText(path));
            var s = fs.ReadLine().Split(' ');
            row = int.Parse(s[0]);
            col = int.Parse(s[1]);
            var tempResult = new List<List<MazeState>>();
            for(int i = 0; i < row; i++)
            {
                tempResult.Add(new List<MazeState>(col));
                var line = fs.ReadLine().Split(' ');
                for(int j = 0; j < col; j++)
                {
                    tempResult[i].Add((MazeState) int.Parse(line[j]));
                }
            }
            return tempResult;
        }
    }
}


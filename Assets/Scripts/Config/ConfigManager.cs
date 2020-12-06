using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MazeViewer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MazeViewer.Viewer
{
    public class ConfigManager : MonoBehaviour
    {
        [Serializable]
        public struct Config
        {
            /// <summary>
            /// 迷宫编辑器的通信地址
            /// </summary>
            public string mazeEditorAddress;
            /// <summary>
            /// 迷宫文件的路径
            /// </summary>
            public string mazePath;
            /// <summary>
            /// 搜索结果文件的路径
            /// </summary>
            public string resultPath;
            /// <summary>
            /// 最大信息数量
            /// </summary>
            public int maxInfoCount;
            /// <summary>
            /// 信息显示时间
            /// </summary>
            public float infoLifeTime;
            public Config(string mazeEditorAddress, string mazePath, string resultPath, int maxInfoCount, float infoLifeTime)
            {
                this.mazeEditorAddress = mazeEditorAddress;
                this.mazePath = mazePath;
                this.resultPath = resultPath;
                this.maxInfoCount = maxInfoCount;
                this.infoLifeTime = infoLifeTime;
            }
            public static Config defaultConfig = new Config(">tcp://localhost:25565", 
                "../maze.txt", "../answer.txt",6,3.0f);
        }

        private string cfgPath = "./Config.json";
        private Config config;
        [SerializeField] private InputField mazePathField = default, resultPathField = default;
        [SerializeField] private Container container = default;

        public static ConfigManager instance;
        public Config Current => config;

        private void Awake()
        {
            config = Config.defaultConfig;
            try
            {
                config = JsonUtility.FromJson<Config>(File.ReadAllText(Path.Combine(Application.dataPath, cfgPath)));
            }
            catch(FileNotFoundException)
            {
                Debug.Log("Config file not found.");
                
            }
            catch(Exception e)
            {
                Debug.Log("Not handled except: " + e);
            }
            ApplyConfig();

            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogWarning("Cannot have duplicated ConfigManager in one scene!");
                return;
            }
        }

        private void Start()
        {
            var ins = MazeEditorProxy.instance;
            ins.mazePathChangeEvent += (changeEvent) => UpdateMazePath(changeEvent.data);
            ins.resultPathChangeEvent += (changeEvent) => UpdateResultPath(changeEvent.data);
            ins.mazeUpdateEvent += (updateEvent) => UpdateMaze();
        }

        private void ApplyConfig()
        {
            container.UpdateMazePath(mazePathField.text = config.mazePath);
            container.UpdateResultPath(resultPathField.text = config.resultPath);
            // save config
            using(var writer = File.CreateText(Path.Combine(Application.dataPath, cfgPath)))
            {
                writer.Write(JsonUtility.ToJson(config, true));
            }
        }

        /// <summary>
        /// 更新了迷宫路径
        /// </summary>
        /// <param name="path"></param>
        public void UpdateMazePath(string path)
        {
            config.mazePath = path;
            ApplyConfig();
        }

        /// <summary>
        /// 更新了结果路径
        /// </summary>
        /// <param name="path"></param>
        public void UpdateResultPath(string path)
        {
            config.resultPath = path;
            ApplyConfig();
        }

        /// <summary>
        /// 迷宫更新
        /// </summary>
        public void UpdateMaze()
        {
            Debug.Log("Update maze called!");
            container.ReloadMaze();
        }
    } 
}

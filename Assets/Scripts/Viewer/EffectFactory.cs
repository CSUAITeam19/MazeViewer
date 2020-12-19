using Unitilities;
using UnityEngine;

namespace MazeViewer.Viewer
{
    /// <summary>
    /// 特效工厂
    /// </summary>
    public class EffectFactory : MonoBehaviour
    {
        [SerializeField] private GameObjectPool activatedEffectPool = default;

        public static EffectFactory Instance
        {
            get;
            private set;
        }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            if(Instance != this)
            {
                Debug.LogError("Can not have duplicated EffectFactory in the same scene!");
            }
        }

        /// <summary>
        /// 获取激活特效物体
        /// </summary>
        /// <param name="position">position in world space</param>
        /// <param name="parent">parent of new GameObject</param>
        /// <returns></returns>
        public GameObject GetActivateEffect(Vector3 position, Transform parent)
        {
            if(activatedEffectPool.Pop(out GameObject temp))
            {
                if(parent != null)
                    temp.transform.SetParent(parent);
                temp.transform.position = position;
                return temp;
            }

            return null;
        }

        /// <summary>
        /// 回收激活特效物体
        /// </summary>
        /// <param name="toRecycle"></param>
        public void RecycleActivateEffect(GameObject toRecycle)
        {
            activatedEffectPool.Push(toRecycle);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            activatedEffectPool.PushAll();
        }
    }
}
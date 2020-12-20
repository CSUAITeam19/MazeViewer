# MazeViewer
- 该项目无特殊情况不会再更新核心代码
## 开发背景
- 数据结构作业: 基于搜索技术的迷宫寻路系统
- 在演示算法运行步骤的环节, 想到可以用Unity来进行3D的迷宫展示. 开发者有一定的技术基础的条件下, 可以较容易地实现算法分步展示
## 使用说明
- 基本操作
  - 左上角输入迷宫文件路径和搜索结果文件路径, 点击 "**加载迷宫**" 加载
  - 使用 **WASD** 和**方向键**控制移动, **鼠标**拖动视角, **空格键**上升, **Shift键**下降
  - 点击**右下角的切换按钮**切换到正交模式(平行俯视视角), **WASD** 和**方向键**控制移动, **鼠标**拖动画面, **滚轮**缩放
  - **上方的进度条**可以显示和控制搜索进度
  - 最外侧两个按钮分别是**倒放**和**正放**, 多次点击可加速 *(每次加速到原来的两倍)*
  - 次外层两个按钮分别是**上一步**和**下一步**
  - 在自动倒放或正放时, **中间的按钮**可以停止自动播放
- 进程间通信
  - 迷宫编辑器会自动通过套接字(默认是在25565端口使用的TCP连接)连接到本程序, 编辑器更新时触发本程序重新加载迷宫
    - 注: 仅能通过ZMQ进行通信, 该库在TCP的基础上进行了复杂的操作, 难以用原始的套接字进行模拟
## 实现细节
### 运行/开发环境
- 使用 **Unity 2019.4.3f1** 开发, 需要与 MazeEditor(暂未上传到GitHub) 及其附带的相关生成算法搭配使用
- 支持的平台: 目前仅确认 Windows 10 可以正常运行, 而移动端的操作逻辑未实现
- 使用 NetMQ 实现套接字通信, 通过VS自动复制导出nuget包的功能获取所需要的完整的dll链接库
### 性能优化
- 在生成迷宫时, 利用[自己实现搭的Unity轮子](https://github.com/LovelyCatHyt/Unitilities/)中的 GameObject 对象池节约 GC 的消耗, 在反复加载迷宫的情景可以节约大部分的加载时间
- 由于所有墙面在演示算法的过程中是完全静态的, 因此在加载时[将所有网格合并为整体](Assets/Scripts/Viewer/Container.cs#L46), 可以大幅减少 DrawCall 数量
  - 但是在方块数极多时, 网格会突然**无法正常渲染**. 因此在循环中加入了相关的判断来将方块分组, 分到不同的合并网格中
  - 另一方面, 在搜索过程中, 尽管地面可以反复变化, 但实际上也是相对静态的物体. 可以考虑将整个迷宫划分为规整的多个区块, 将同种材质的地面划入同一个网格中, 每次地面变化时再更新合并网格. **目前项目中并未实现**
  - 某些情况下场景中可能会有较多的粒子系统. 但目前未观察到明显的性能下降, 故不考虑
- 场景中大量的 3DText 会显著提高CPU占用率. 使用**文本到摄像头的距离**来筛选过远的文本, 将其剔除.
  - 显然使用**曼哈顿距离**比欧氏距离性能更佳
  - 由于需要每帧检测摄像头的方向来使文本旋转到正确的方向. 因此将这一计算操作移动到一个**单例**进行处理, 只有当角度变化超过阈值时才会触发全局的显示文本旋转
    - 使用一个哈希表(`HashSet<T>`)来记录当前活跃的文本, 只有 active 的文本需要考虑文本旋转, 在反复变换状态时, 哈希表的插入、查找、删除时间复杂度为 O(1), 这样可以避免一次调用过多的文本
    - 实际检测发现, 单例检测是否旋转并没有带来明显的性能提升, 因为未显示的文本仍然需要反复 `Update()` 来检测与摄像机的距离, 这一部分计算并没有消除.
- 进度条的拖动原本会导致大量的协程生成与停止, 因为在 v1.2.0 之前的所有地面状态变化的动画使用协程, 而每次更新都会导致动画的开/闭
  - 优化方案是[把动画更新的具体操作延迟到下一帧](Assets/Scripts/Viewer/CellObj/RouteCell.cs#L26). 这样当前帧的所有进度的变化都不涉及任何具体 Unity 的 API
  - 优化后, 进度条的任何变化消耗时间下降了若干数量级
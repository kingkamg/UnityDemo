# MyUnityDemo
# **简介**
**个人用于练习的一款第一人称生存游戏，需要实现的功能有:**  
+ 背包模块
+ 合成模块
+ 工具栏
+ 角色动画
+ 枪械模块
+ 地形模块
+ 建造系统
+ 采集模块

# **目前进度**
>## ***2019年9月5日***   
>### 完成了**背包模块**和**合成模块**的基础逻辑。   
>+ 具体采用了MVC结构，读取Json文件生成游戏道具。
>+ 合成模块UI交互。
>+ 物品槽及合成图谱（类似MC）生成。
>+ 物体的拖拽功能及拖拽目标的判定逻辑。
>+ 合成物品后的数值操作。
>### 结构图
>![logic](https://github.com/HmzMoonZy/UnityDemo/blob/dev/document/%E8%83%8C%E5%8C%85%E9%9D%A2%E6%9D%BF%E5%90%88%E6%88%90%E9%9D%A2%E6%9D%BF%E9%80%BB%E8%BE%91%E5%9B%BE.png?raw=true)
>### 已知BUG
>+ 背包面板内的ItemUI覆盖不完整，导致背包内图片有可能添加在同一物品槽内。
>+ 拆分物品的数值重置有缺漏,导致合成时在背包内对物体的拆分操作无法正常判断数值。
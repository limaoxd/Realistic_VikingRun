# Realistic_VikingRun
環境 : unity 20.3.21
本遊戲借鑒 temple_run , 一樣是在無限生成的地圖上蒐集金幣和逃生!
遊戲操作有 W , S , A , D ,SPACE.
W : 開始前進 (放開不會停)
S : 急煞 (沒什麼用 , 只是為了回到 idle 狀態)
A , D : 左右橫移移動 (要求 Bonus : 當跑到彎道和叉路 (對,我有做 T 字路) 的時候 , 橫移會由程式切換成轉彎 (人物的轉向和鏡頭的轉向我有做平滑過渡))

小提醒 : 看到要轉彎時可以持續壓者 A 或 D 鍵 , 不需要單點

SPACE : 跳躍

當玩家沒有停下來時 , 時間越久速度會越快 , 分數的計算方法是每吃一個金幣會根據當前速度來加分 !
理由是加速是提升玩家難度的方法 , 高風險也會有高回報 , 如果是以時間設計的話 , 玩家肯定不想提高速度而停下來緩速.

地圖除了有金幣以外 , 還會有酒桶當障礙物 (要求 Bonus : 金幣不會與酒桶重疊 , 金幣會浮在酒桶上方 , 玩家需要跳躍拾取)

當玩家不小心撞擊酒桶時 , 酒桶會觸發 rigbody 的 isKenetic 為 false 而被撞飛出去 , 玩家則會開啟 Ragdoll 的模式癱倒在地板上 2 秒 (這個我也想要 bonus qwq)

然後玩家在路上時可以拾取盾牌 , 盾牌會提供玩家 10 秒的無敵時間 , 跟金幣一樣不會和酒桶疊再一起

敵人是一隻飛龍 , 這個設計既強大 , 也很好的解決巡路的問題 , 因為它可以飛可以無視地板障礙 , 當玩家停下速度或是癱倒在地 , 只要距離夠近 , 飛龍會吐火把玩家殺死 .

我建構地圖的方式是採用 3 個 Raycast (dis = infinety), 前 左 右 , 只要其中一個方向的地方有偵測到碰撞 , 就封鎖那個方向的移動 , 這樣保證不會形成一個環 , 所以地圖的路線不可能撞在一起 (結果裝飾的建築有極低的機會會擋在路上 , 雖然沒碰撞箱).
然後我程式最精華的地方也在這裡 , 50幾行完成主架構 , 使用旋轉矩陣來換算地圖生成器該前往的方向!!
然後道路只要玩家離開了 trigger 大概一秒 , 地板會自動刪除 .

程式主要放在 Script 裡面 , 有些檔案是我在網路上找別人怎麼製作而載的 demo 碼.

音樂是採用了巫師3 :Drink Up, There's More 的 ost (昆特牌中毒神曲).

小小的意外 : 該專案的渲染使用了 HDRP , 並且我應用了 volumetric lighting , 等渲染 , 還有一些色偏的處理 , 結果可能是環境光的烘培撐不住時間換算大概 1 分中的時候 , 燈光的光照壞掉了 . 遊戲將進入夜晚模式 !? (還是看的到啦)

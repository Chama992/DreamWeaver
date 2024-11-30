using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController instance;

    #region stats
    [Header("玩家与游戏相关")]
    [Tooltip("玩家")]
    public Player player;

    [Tooltip("重置动画")]
    public UI_ResetAnim resetAnim;

    [Tooltip("黑洞")]
    public GameObject blackHole;

    [Tooltip("可交互半径")]
    public float interactRatio;

    [Tooltip("交互器")]
    public StandaloneInputModule InputModule;

    [Header("地图相关")]
    [Tooltip("全局碎片数据列表")]
    public List<Piece> pieces;

    [Tooltip("检查点碎片数据")]
    public Piece checkPoint;

    [Tooltip("关卡碎片生成位置偏移量")]
    public float pieceGenePositionRamdonBias;

    [Tooltip("黑洞生成位置偏移量")]
    public float blackHoleGenePositionRamdonBias;

    [Tooltip("关卡碎片生成量增加所需关卡数")]
    public int pieceGeneAmountAddLv;

    [Tooltip("关卡碎片生成上限")]
    public int pieceGeneMaxAmount;

    [Tooltip("水平块初始间距")]
    public float blockHorizontalInterval;

    [Tooltip("水平块间距线性增长比率")]
    public float blockHorizontalInterAddRate;

    [Tooltip("水平块间距增长所需关卡数")]
    public int blockHorizontalInterAddLv;

    [Tooltip("水平块最大间距")]
    public float blockHorizontalMaxInterval;

    [Tooltip("竖直块初始间距")]
    public float blockVerticalInterval;

    [Tooltip("竖直块间距线性增长比率")]
    public float blockVerticalInterAddRate;

    [Tooltip("竖直块间距增长所需关卡数")]
    public int blockVerticalInterAddLv;

    [Tooltip("竖直块最大间距")]
    public float blockVerticalMaxInterval;

    [Tooltip("水平块增加所需关卡数")]
    public int blockHorizontalAddLv;

    [Tooltip("水平块上限")]
    public int blockHorizontalMaxAmount;

    [Tooltip("竖直块增加所需关卡数")]
    public int blockVerticalAddLv;

    [Tooltip("竖直块上限")]
    public int blockVerticalMaxAmount;

    [Header("得分相关")]
    [Tooltip("单位分数")]
    public float unitScore;

    [Tooltip("一星长度倍率需求")]
    public float star1RequiredRate;

    [Tooltip("二星长度倍率需求")]
    public float star2RequiredRate;

    [Tooltip("三星长度倍率需求")]
    public float star3RequiredRate;

    /// <summary>
    /// 关卡起点
    /// </summary>
    public Vector3 levelStartPoint { get; private set; }

    /// <summary>
    /// 关卡中心
    /// </summary>
    public Vector3 levelCenterPoint { get; private set; }

    /// <summary>
    /// 关卡终点
    /// </summary>
    public Vector3 levelEndPoint { get; private set; }

    /// <summary>
    /// 可用碎片列表
    /// </summary>
    public List<Piece> enabledPieces { get; private set; } = new();

    /// <summary>
    /// 关卡碎片可生成位置
    /// </summary>
    public List<Vector3> pieceGenePositions { get; private set; } = new();

    /// <summary>
    /// 关卡所有生成的碎片
    /// </summary>
    public List<Piece> levelPieces { get; private set; } = new();

    /// <summary>
    /// 下一次清除的碎片
    /// </summary>
    public List<GameObject> readyToClear { get; private set; } = new();

    /// <summary>
    /// 下下一次清除的碎片
    /// </summary>
    public List<GameObject> readyToClear_2 { get; private set; } = new();

    /// <summary>
    /// 下下下一次清除的碎片
    /// </summary>
    public List<GameObject> readyToClear_3 { get; private set; } = new();

    /// <summary>
    /// 关卡碎片生成量
    /// </summary>
    public int pieceGeneAmount { get; private set; }

    /// <summary>
    /// 入梦深度
    /// </summary>
    public int level { get; private set; }

    /// <summary>
    /// 玩家解决方案，每次连接时更新
    /// </summary>
    public Stack<Vector3> Solution { get; private set; } = new();

    /// <summary>
    /// 本关已连接的线长，每次连接更新
    /// </summary>
    public float nodedLevelWeaveLength { get; private set; }

    /// <summary>
    /// 本关线长，每帧更新
    /// </summary>
    public float levelWeaveLength { get; private set; }

    /// <summary>
    /// 理论最优解线长，每关更新
    /// </summary>
    public float maxWeaveLength { get; private set; }

    /// <summary>
    /// 总线长，即已通关卡线长之和，每关更新
    /// </summary>
    public float overallWeaveLength { get; private set; }

    /// <summary>
    /// 每关的得分修饰系数
    /// </summary>
    public float scoreLevelModifier { get; private set; }

    /// <summary>
    /// 分数，每关更新
    /// </summary>
    public float score { get; private set; }

    /// <summary>
    /// 当前关评级，每帧更新
    /// </summary>
    public int stars { get; private set; }

    /// <summary>
    /// 当前关奖励
    /// </summary>
    public int bonus { get; private set; }

    /// <summary>
    /// 下一关生成黑洞数
    /// </summary>
    public int blackHoleAmount { get; private set; }

    /// <summary>
    /// 是否正在暂停
    /// </summary>
    public bool isPausing { get; private set; }

    /// <summary>
    /// 游戏是否正在进行
    /// </summary>
    public bool isGaming { get; private set; }

    /// <summary>
    /// 是否正在播放重置动画
    /// </summary>
    public bool isResetAnimating { get; set; }

    /// <summary>
    /// 是否正在播放转关卡动画
    /// </summary>
    public bool isReadyAnimating {  get; set; }
    #endregion

    #region Actions
    /// <summary>
    /// 游戏开始瞬间调用
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// 游戏暂停瞬间调用
    /// </summary>
    public Action onGamePause;

    /// <summary>
    /// 游戏继续瞬间调用
    /// </summary>
    public Action onGameContinue;

    /// <summary>
    /// 游戏结算瞬间调用
    /// </summary>
    public Action onGameEnd;

    /// <summary>
    /// 关卡过关瞬间调用
    /// </summary>
    public Action onLevelComplete;

    /// <summary>
    /// 道具完成选择瞬间调用
    /// </summary>
    public Action onLevelReady;

    /// <summary>
    /// 关卡开始瞬间调用
    /// </summary>
    public Action onLevelStart;

    /// <summary>
    /// 关卡重置瞬间调用
    /// </summary>
    public Action onLevelReset;

    #endregion

    #region Unity

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;

    }

    private void Update()
    {
        if (isGaming && !isResetAnimating && !isReadyAnimating)
        {
            RefreshLevelWeaveLength();
            RefreshStars();
        }

        if (isResetAnimating||isReadyAnimating)
        {
            InputModule.DeactivateModule();
        }
        else
        {
            InputModule.ActivateModule();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && isGaming)
        {
            if (isPausing)
            {
                ContinueGame();
            }
            else
            {
                PauseGame();
            }
        }

        //Debug
        if (Input.GetKeyDown(KeyCode.Q))
        {
            foreach (var nodes in Solution)
            {
                Debug.Log(nodes);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            RegenerateLevel();
        }

    }
    #endregion

    #region Generate
    /// <summary>
    /// 按照引用在指定位置生成地图，仅当需要针对性生成地图时独立调用，或用于随机生成方法中
    /// </summary>
    /// <param name="_piece">地图引用</param>
    /// <param name="_position">世界位置</param>
    /// <param name="_readyToBeClear">是否在关卡结束时清除</param>
    public Piece GeneratePiece(Piece _piece, Vector3 _position, bool _readyToBeClear)
    {
        Piece piece = FX.instance.SmoothSizeInstantiate(_piece.gameObject, _position).GetComponent<Piece>();
        if (_readyToBeClear)
            readyToClear_2.Add(piece.gameObject);
        else
            readyToClear_3.Add(piece.gameObject);
        return piece;
    }
    /// <summary>
    /// 在位置列表中随机选择位置生成列表中随机一个地图
    /// </summary>
    /// <remarks>
    /// 严禁向此方法中直接传全局地图数据列表和关卡位置列表！！！要传也使用深拷贝方法DeepCopyHelper.DeepCopy拷贝一份！！！
    /// 此方法亦不可用于对两个参数的列表的foreach枚举语句中
    /// </remarks>
    /// <param name="_pieceList">地图id列表</param>
    /// <param name="_positionList">位置列表</param>
    /// <param name="_readyToBeClear">是否在关卡结束时清除</param>
    /// <param name="_allowEmpty">随机生成是否包括不生成</param>
    /// <param name="_removeGenerated">是否删除列表中此次生成的碎片和位置</param>
    public Piece GenerateRandomPiece(List<Piece> _pieceList, List<Vector3> _positionList, bool _readyToBeClear, bool _allowEmpty, out Piece _extraDoor)
    {
        if (_pieceList == null || _positionList == null || _pieceList.Count == 0 || _positionList.Count == 0)
        {
            _extraDoor = null;
            return null;
        }
        int idIndex;
        if (_allowEmpty)
            idIndex = Random.Range(-1, _pieceList.Count);
        else
            idIndex = Random.Range(0, _pieceList.Count);
        int positionIndex = Random.Range(0, _positionList.Count);
        Piece newPiece = null;
        if (idIndex != -1)
        {
            newPiece = GeneratePiece(_pieceList[idIndex], _positionList[positionIndex], _readyToBeClear);
        }
        _positionList.RemoveAt(positionIndex);
        Piece extraPiece = null;
        if (idIndex != -1 && _pieceList[idIndex] is Piece_Door)
        {
            positionIndex = Random.Range(0, _positionList.Count);
            extraPiece = GeneratePiece(_pieceList[idIndex], _positionList[positionIndex], _readyToBeClear);
            ((Piece_Door)newPiece).relatedDoor = (Piece_Door)extraPiece;
            ((Piece_Door)extraPiece).relatedDoor = (Piece_Door)newPiece;
            _positionList.RemoveAt(positionIndex);
            _pieceList.RemoveAll(t => t is Piece_Door);
        }
        else
        {
            _pieceList.RemoveAt(idIndex);
        }
        _extraDoor = extraPiece;
        return newPiece;
    }

    /// <summary>
    /// 生成地图
    /// </summary>
    public void GenenrateMap()
    {

        if (level == 0)
        {
            levelPieces.Clear();
            Piece piece = GeneratePiece(checkPoint, levelStartPoint, true);
            if (piece != null)
                levelPieces.Add(piece);
        }
        else
        {
            Piece checkPoint = levelPieces.Find(t => t.transform.position == levelStartPoint);
            levelPieces.Clear();
            levelPieces.Add(checkPoint);
        }
        levelPieces.Add(GeneratePiece(checkPoint, levelEndPoint, false));
        List<Piece> enabledPieceCopy = enabledPieces.FindAll(t => true);
        List<Vector3> genePositionsCopy = pieceGenePositions.FindAll(t => true);
        for (int i = 0; i < pieceGeneAmount; i++)
        {
            if (Math.Min(pieceGeneAmount - i, genePositionsCopy.Count) < 2)
            {
                enabledPieceCopy.RemoveAll(t => t is Piece_Door);
            }
            Piece extraDoor = null;
            Piece newPiece = GenerateRandomPiece(enabledPieceCopy, genePositionsCopy, true, false, out extraDoor);
            if (newPiece != null)
                levelPieces.Add(newPiece);
            if (extraDoor != null)
                levelPieces.Add(extraDoor);
        }

        GenerateBlackHole();
    }

    /// <summary>
    /// 生成地图并播放动画
    /// </summary>
    public IEnumerator GenenrateMap_Smooth()
    {
        isReadyAnimating = true;
        if (level == 0)
        {
            levelPieces.Clear();
            Piece piece = GeneratePiece(checkPoint, levelStartPoint, true);
            if (piece != null)
                levelPieces.Add(piece);
        }
        else
        {
            Piece checkPoint = levelPieces.Find(t => t.transform.position == levelStartPoint);
            levelPieces.Clear();
            levelPieces.Add(checkPoint);
        }
        levelPieces.Add(GeneratePiece(checkPoint, levelEndPoint, false));
        yield return new WaitForSeconds(.4f);
        List<Piece> enabledPieceCopy = enabledPieces.FindAll(t => true);
        List<Vector3> genePositionsCopy = pieceGenePositions.FindAll(t => true);
        for (int i = 0; i < pieceGeneAmount; i++)
        {
            if (Math.Min(pieceGeneAmount - i, genePositionsCopy.Count) < 2)
            {
                enabledPieceCopy.RemoveAll(t => t is Piece_Door);
            }
            Piece extraDoor = null;
            Piece newPiece = GenerateRandomPiece(enabledPieceCopy, genePositionsCopy, true, false, out extraDoor);
            if (newPiece != null)
                levelPieces.Add(newPiece);
            if (extraDoor != null)
                levelPieces.Add(extraDoor);
            yield return new WaitForSeconds(.4f);
        }

        GenerateBlackHole();
        yield return new WaitForSeconds(1f);
        isReadyAnimating = false;
        ClearMap();
        StartLevel();
    }

    /// <summary>
    /// 在地图完成生成后生成黑洞
    /// </summary>
    public void GenerateBlackHole()
    {
        List<Piece> levelPiecesCopy = levelPieces.FindAll(t => !t.isCheckPoint);

        for (int i = 0; i < Math.Min(blackHoleAmount, levelPiecesCopy.Count); i++)
        {
            int index = Random.Range(0, levelPiecesCopy.Count);
            readyToClear_2.Add(FX.instance.SmoothSizeInstantiate(blackHole, levelPiecesCopy[index].transform.position+(Vector3)Random.insideUnitCircle * blackHoleGenePositionRamdonBias));
            levelPiecesCopy.RemoveAt(index);
        }
    }

    /// <summary>
    /// 清除之前的地图
    /// </summary>
    private void ClearMap()
    {
        for (int i = 0; i < readyToClear.Count; i++)
        {
            Destroy(readyToClear[i].gameObject);
        }
        readyToClear = readyToClear_2.FindAll(t => true);
        readyToClear_2 = readyToClear_3.FindAll(t => true);
        readyToClear_3.Clear();
    }

    #endregion

    #region Refresh&Calculate
    /// <summary>
    /// 依据当前入梦等级及起始地点刷新所有碎片生成点
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelPieceGenePosition()
    {
        List<Vector3> result = new();
        Vector3 startCorner = levelStartPoint + new Vector3(Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval), 0);
        for (int i = 0; i < level / blockHorizontalAddLv + 1; i++)
        {
            for (int j = 0; j < level / blockVerticalAddLv + 1; j++)
            {
                result.Add(startCorner + new Vector3(Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval) * i,
                    Math.Min(blockVerticalInterval * (1 + level / blockVerticalInterAddLv * blockVerticalInterAddRate), blockVerticalMaxInterval) * j) + (Vector3)(pieceGenePositionRamdonBias * Random.insideUnitCircle));
            }
        }
        pieceGenePositions = result;
    }

    /// <summary>
    /// 依据当前入梦等级刷新可生成的碎片
    /// </summary>
    private void RefreshEnabledPiece()
    {
        enabledPieces = pieces.FindAll(t => t.difficulty <= level);
    }

    /// <summary>
    /// 依据上次终点刷新起点位置，首次直接设为世界原点
    /// </summary>
    private void RefreshLevelStartPosition()
    {
        if (level == 0)
        {
            levelStartPoint = Vector3.zero;
        }
        else
        {
            levelStartPoint = levelEndPoint;
        }
    }

    /// <summary>
    /// 依据当前入梦等级及起始地点刷新终点位置
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelEndPosition()
    {
        levelEndPoint = levelStartPoint + new Vector3((Math.Min((level / blockHorizontalAddLv), blockHorizontalMaxAmount) + 2) * Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval),
            Math.Min((level / blockVerticalAddLv), blockVerticalMaxAmount) * Math.Min(blockVerticalInterval * (1 + level / blockVerticalInterAddLv * blockVerticalInterAddRate), blockVerticalMaxInterval));
    }

    /// <summary>
    /// 依据起点和终点刷新地图中心位置
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelCenterPosition()
    {
        levelCenterPoint = (levelStartPoint + levelEndPoint) / 2;
    }

    /// <summary>
    /// 依据当前入梦等级和刷新点总数刷新当前关卡最大碎片数
    /// </summary>
    private void RefreshPieceGeneMaxAmount()
    {
        pieceGeneAmount = Math.Min(Math.Min(pieceGeneMaxAmount, (1 + level / pieceGeneAmountAddLv)), pieceGenePositions.Count);
    }

    /// <summary>
    /// 依据玩家解决方案刷新当前关卡已连接节点总距离
    /// </summary>
    /// <returns></returns>
    private void RefreshNodedLevelWeaveLength()
    {
        if (Solution == null || Solution.Count == 0)
        {
            nodedLevelWeaveLength = 0;
            return;
        }

        Stack<Vector3> SolutionCopy = new();
        Vector3[] SolutionArrayCopy = new Vector3[Solution.Count];
        Solution.CopyTo(SolutionArrayCopy, 0);
        for (int i = 0; i < Solution.Count; i++)
        {
            SolutionCopy.Push(SolutionArrayCopy[i]);
        }
        Vector3 LastNode = SolutionCopy.Pop();
        float result = 0;
        while (SolutionCopy != null && SolutionCopy.Count != 0)
        {
            Vector3 NewNode = SolutionCopy.Pop();
            if ((NewNode != -Vector3.one) && (LastNode != -Vector3.one))
            {
                result += (LastNode - NewNode).magnitude;
            }
            LastNode = NewNode;
        }

        nodedLevelWeaveLength = result;
    }

    /// <summary>
    /// 刷新当前关卡总线长
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelWeaveLength()
    {
        if (Solution == null || Solution.Count == 0)
            return;
        levelWeaveLength = nodedLevelWeaveLength + (player.transform.position - Solution.Peek()).magnitude;
    }

    /// <summary>
    /// 依据已连接总长，刷新全局线长，首次设为0
    /// </summary>
    private void RefreshOverallWeaveLength()
    {
        if (level == 0)
            overallWeaveLength = 0;
        else
            overallWeaveLength += nodedLevelWeaveLength;
    }

    /// <summary>
    /// 依据当前起点、终点和所有碎片位置刷新最优解长度
    /// </summary>
    private void RefreshMaxWeaveLength()
    {
        Vector3 startNode = levelPieces.Find(t => t.transform.position == levelStartPoint).node.position;
        Vector3 endNode = levelPieces.Find(t => t.transform.position == levelEndPoint).node.position;
        List<Vector3> nodes = new();
        foreach (var node in levelPieces.FindAll(t => !t.isCheckPoint))
        {
            nodes.Add(node.transform.position);
        }
        //计算距离作为权重
        Dictionary<int, float[]> pathWeights = new Dictionary<int, float[]>();
        List<Vector3> allNodes = new List<Vector3>();
        allNodes.Add(startNode);
        allNodes.AddRange(nodes);
        allNodes.Add(endNode);
        for (int i = 0; i < allNodes.Count; i++)
        {
            pathWeights[i] = new float[allNodes.Count];
        }
        for (int i = 0; i < allNodes.Count; i++)
        {
            for (int j = i + 1; j < allNodes.Count; j++)
            {
                float distance = Vector2.Distance(allNodes[i], allNodes[j]);
                pathWeights[i][j] = distance;
                pathWeights[j][i] = distance;
            }
        }
        float[] originPathWeightToFinal = new float[allNodes.Count - 1];
        //防止直接从起始点跳到终点
        for (int i = 0; i < allNodes.Count - 1; i++)
        {
            originPathWeightToFinal[i] = pathWeights[i][allNodes.Count - 1];
            pathWeights[i][allNodes.Count - 1] = 0;
        }
        //初始化计算数组
        int[] path = new int[allNodes.Count];//用于记录路径
        float[] pathWeight = new float[allNodes.Count];//用于更新距离
        bool[] flag = new bool[allNodes.Count];
        for (int i = 0; i < allNodes.Count; i++)
        {
            flag[i] = false;
            pathWeight[i] = pathWeights[0][i];
            path[i] = 0;
        }
        flag[0] = true;
        //开始计算
        int calculateCount = 0;
        while (true)
        {
            //避免死循环
            calculateCount++;
            if (calculateCount > allNodes.Count)
            {
                Debug.Log("计算有误");
                break;
            }
            float maxDistance = float.MinValue;
            int maxIndex = -1;
            //找到当前权重最大的节点 记录距离以及其节点号
            for (int j = 0; j < pathWeight.Length; j++)
            {
                if (flag[j] == false && pathWeight[j] > maxDistance)
                {
                    maxDistance = pathWeight[j];
                    maxIndex = j;
                }
            }
            if (maxIndex == -1)
            {
                break;
            }
            //更新标记
            flag[maxIndex] = true;
            //更新权重数组
            for (int k = 0; k < allNodes.Count(); k++)
            {
                if (pathWeights[k][maxIndex] + pathWeight[maxIndex] > pathWeights[k][0] && flag[k] == false)
                {
                    pathWeight[k] = pathWeights[k][maxIndex] + pathWeight[maxIndex];
                    path[k] = maxIndex;
                }
            }
        }
        // pathWeight[^1] += originPathWeightToFinal[path[^1]];
        Debug.Log("最大距离:" + pathWeight[^1]);//找到一条貌似是最长的 可能比最长的短一点 但绝对不会长 不想改这个了
        maxWeaveLength = pathWeight[^1];
    }

    private void BfsSearchMaxLength(Vector2 startPoint, Vector2 endPoint, List<Vector2> points)
    {
        float max = float.MinValue;
        //计算距离作为权重
        Dictionary<int, float[]> pathWeights = new Dictionary<int, float[]>();
        List<Vector3> allNodes = new List<Vector3>();
        allNodes.Add(startPoint);
        allNodes.AddRange(points);
        allNodes.Add(endPoint);
        for (int i = 0; i < allNodes.Count; i++)
        {
            pathWeights[i] = new float[allNodes.Count];
        }
        for (int i = 0; i < allNodes.Count; i++)
        {
            for (int j = i + 1; j < allNodes.Count; j++)
            {
                float distance = Vector2.Distance(allNodes[i], allNodes[j]);
                pathWeights[i][j] = distance;
                pathWeights[j][i] = distance;
            }
        }

    }
    /// <summary>
    /// 刷新当前星数，每帧更新
    /// </summary>
    private void RefreshStars()
    {

        if (levelWeaveLength >= star3RequiredRate * maxWeaveLength)
        {
            stars = 3;
        }
        else if (levelWeaveLength >= star2RequiredRate * maxWeaveLength)
        {
            stars = 2;
        }
        else if (levelWeaveLength >= star1RequiredRate * maxWeaveLength)
        {
            stars = 1;
        }
        else
        {
            stars = 0;
        }
    }

    /// <summary>
    /// 刷新玩家分数，初始设为0；
    /// </summary>
    private void RefreshScore()
    {
        if (level == 0)
            score = 0;
        else
            score += scoreLevelModifier * Solution.Count * nodedLevelWeaveLength * unitScore;
    }

    /// <summary>
    /// 刷新每关重设的量
    /// </summary>
    private void RefreshOthers()
    {
        bonus = 0;
        scoreLevelModifier = 1;
    }

    #endregion

    #region Game&Level
    /// <summary>
    /// 开始游戏，初始化所有数值
    /// </summary>
    public void StartGame()
    {

        level = 0;
        isGaming = true;
        Time.timeScale = 1;
        player.gameObject.SetActive(true);
        levelPieces.Add(GeneratePiece(checkPoint, levelStartPoint, true));

        ReadyLevel();
        onGameStart?.Invoke();
    }
    /// <summary>
    /// 暂停游戏，仅在未暂停游戏时生效
    /// </summary>
    public void PauseGame()
    {
        if (isPausing)
            return;

        Time.timeScale = 0;
        isPausing = true;
        onGamePause?.Invoke();


    }
    /// <summary>
    /// 继续游戏，仅在暂停游戏后生效
    /// </summary>
    public void ContinueGame()
    {
        if (!isPausing)
            return;

        Time.timeScale = 1;
        isPausing = false;
        onGameContinue?.Invoke();
    }
    /// <summary>
    /// 结算当局游戏，仅在暂停游戏后生效
    /// </summary>
    public void EndGame()
    {
        if (!isPausing)
            return;
        isGaming = false;
        player.gameObject.SetActive(false);
        isPausing = true;
        onGameEnd?.Invoke();
    }
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// 退出游戏进程
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
    /// <summary>
    /// 完成当前关卡
    /// </summary>
    public void CompleteLevel()
    {
        level++;
        Time.timeScale = 0;
        isPausing = true;
        InGameUIManager.Instance.OpenRoguePropPanel(stars + bonus);
        onLevelComplete?.Invoke();
    }

    /// <summary>
    /// 道具已选完，准备开始下一关
    /// </summary>
    public void ReadyLevel()
    {
        Time.timeScale = 1;
        isPausing = false;
        RefreshLevelStartPosition();
        RefreshLevelEndPosition();
        RefreshLevelCenterPosition();
        RefreshLevelPieceGenePosition();
        RefreshPieceGeneMaxAmount();
        RefreshEnabledPiece();
        RefreshOverallWeaveLength();
        RefreshScore();
        RefreshOthers();
        Solution.Clear();
        RefreshNodedLevelWeaveLength();

        player.transform.position = levelPieces.Find(t => t.transform.position == levelStartPoint).node.position;
        onLevelReady?.Invoke();
    }

    /// <summary>
    /// 开始下一关
    /// </summary>
    public void StartLevel()
    {
        RefreshOverallWeaveLength();
        Solution.Push(levelPieces.Find(t => t.transform.position == levelStartPoint).node.position);
        RefreshMaxWeaveLength();
        player.GetComponentInChildren<PlayerNodeControl>().ResetLine();
        player.GetComponentInChildren<PlayerNodeControl>().SetLevelStartPoint(levelPieces.Find(t => t.transform.position == levelStartPoint).node.gameObject.GetInstanceID(), levelPieces.Find(t => t.transform.position == levelStartPoint));

        onLevelStart?.Invoke();
    }

    /// <summary>
    /// 重置当前关卡
    /// </summary>
    public void ResetLevel()
    {
        StartCoroutine(ResetLevelAnim());
    }

    private IEnumerator ResetLevelAnim()
    {
        isResetAnimating = true;
        resetAnim.gameObject.SetActive(true);
        onLevelReset?.Invoke();
        yield return new WaitForSeconds(2);
        Solution.Clear();
        Solution.Push(levelPieces.Find(t => t.transform.position == levelStartPoint).node.position);
        RefreshNodedLevelWeaveLength();
        RefreshScore();
        RefreshStars();
        RefreshOthers();
        player.transform.position = levelPieces.Find(t => t.transform.position == levelStartPoint).node.position;
        StartLevel();
        resetAnim.anim.SetBool("isStart", false);
        isResetAnimating = false;
        
    }

    /// <summary>
    /// 重新开始关卡并重新生成地图，返回是否允许重新生成
    /// </summary>
    public bool RegenerateLevel()
    {
        if (!GameController.instance.isGaming)
            return false;

        ResetLevel();
        ReadyLevel();
        return true;
    }


    #endregion

    #region Interface

    /// <summary>
    /// 依照给定值增加分数修饰系数
    /// </summary>
    /// <remarks>
    /// 一般加1而非加100
    /// </remarks>
    /// <param name="_addRate"></param>
    public void AddScoreModifier(float _addRate)
    {
        scoreLevelModifier += _addRate;
    }

    /// <summary>
    /// 依照给定值增加奖励
    /// </summary>
    /// <param name="_amount"></param>
    public void AddBonue(int _amount)
    {
        bonus += _amount;
    }

    /// <summary>
    /// 依照给定值储存黑洞
    /// </summary>
    /// <param name="_amount"></param>
    public void AddBlackHole(int _amount)
    {
        blackHoleAmount += _amount;
    }


    /// <summary>
    /// 连接节点。注意：通关也使用此方法完成，输入检查点碎片
    /// </summary>
    /// <param name="_piece">碎片引用</param>
    public void ConnectNode(Piece _piece)
    {
        if (isPausing)
            return;

        if (!_piece.allowLink || _piece.node == null)
            return;

        _piece.isLinked = true;
        Solution.Push(_piece.node.position);
        RefreshNodedLevelWeaveLength();
        if (_piece.transform.position == levelEndPoint)
        {
            CompleteLevel();
        }
    }
    /// <summary>
    /// 取消连接节点。返回成功与否
    /// </summary>
    /// <param name="_piece">碎片引用</param>
    /// <returns>是否成功取消连接</returns>
    public bool TryDisconnectNode(Piece _piece)
    {
        if (isPausing)
            return false;

        if (_piece.isCheckPoint || Solution.Peek() != _piece.node.position)
            return false;

        _piece.isLinked = false;
        Solution.Pop();
        RefreshNodedLevelWeaveLength();

        return true;
    }
    /// <summary>
    /// 连接门
    /// </summary>
    /// <param name="_door">门引用</param>
    public void ConnectDoor(Piece_Door _door)
    {
        Solution.Push(_door.door.position);
        Solution.Push(-Vector3.one);
        Solution.Push(_door.relatedDoor.door.position);
        RefreshNodedLevelWeaveLength();

    }
    /// <summary>
    /// 取消连接门，返回成功与否
    /// </summary>
    /// <param name="_door">门引用</param>
    /// <returns>是否成功取消连接</returns>
    public bool TryDisconnectDoor(Piece_Door _door)
    {
        if (Solution.Peek() != _door.door.position)
            return false;

        Solution.Pop();
        Solution.Pop();
        Solution.Pop();
        RefreshNodedLevelWeaveLength();

        return true;
    }
    #endregion
}

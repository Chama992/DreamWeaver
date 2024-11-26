using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{
    public static GameController instance;

    #region stats
    [Header("玩家相关")]
    [Tooltip("玩家")]
    public Player player;

    [Header("地图相关")]
    [Tooltip("全局碎片数据列表")]
    public List<Piece> pieces;

    [Tooltip("检查点碎片数据")]
    public Piece checkPoint;

    [Tooltip("关卡碎片生成位置偏移量")]
    public float pieceGenePositionRamdonBias;

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
    public Vector3 levelStartPoint {  get; private set; }

    /// <summary>
    /// 关卡中心
    /// </summary>
    public Vector3 levelCenterPoint {  get; private set; }

    /// <summary>
    /// 关卡终点
    /// </summary>
    public Vector3 levelEndPoint { get; private set; }

    /// <summary>
    /// 可用碎片列表
    /// </summary>
    public List<Piece> enabledPieces  { get { return new(); } private set { } }

    /// <summary>
    /// 关卡碎片生成位置
    /// </summary>
    public List<Vector3> pieceGenePositions { get { return new(); } private set { } }

    /// <summary>
    /// 进入下一关动画结束后清除的碎片
    /// </summary>
    public List<Piece> readyToClearPieces { get { return new(); } private set { } }

    /// <summary>
    /// 进入下一关先不清楚，加入到下下一关清除列表中的碎片
    /// </summary>
    public List<Piece> readyToReadyToClearPieces { get { return new(); } private set { } }

    /// <summary>
    /// 关卡碎片生成量
    /// </summary>
    public int pieceGeneAmount { get; private set; }

    /// <summary>
    /// 入梦深度
    /// </summary>
    public int level {  get; private set; }

    /// <summary>
    /// 玩家解决方案，每次连接时更新
    /// </summary>
    public Stack<Vector3> Solution { get { return new(); } private set { } }

    /// <summary>
    /// 本关已连接的线长，每次连接更新
    /// </summary>
    public float nodedLevelWeaveLength {  get; private set; }

    /// <summary>
    /// 本关线长，每帧更新
    /// </summary>
    public float levelWeaveLength {  get; private set; }

    /// <summary>
    /// 理论最优解线长，每关更新
    /// </summary>
    public float maxWeaveLength { get; private set; }

    /// <summary>
    /// 总线长，即已通关卡线长之和，每关更新
    /// </summary>
    public float overallWeaveLength {  get; private set; }

    /// <summary>
    /// 每关的得分修饰系数
    /// </summary>
    public float scoreLevelModifier {  get; private set; }

    /// <summary>
    /// 分数，每关更新
    /// </summary>
    public float score {  get; private set; }

    /// <summary>
    /// 当前关评级，每帧更新
    /// </summary>
    public int stars {  get; private set; }

    /// <summary>
    /// 是否正在暂停
    /// </summary>
    public bool isPausing { get; private set; }
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
    /// 关卡重置瞬间调用
    /// </summary>
    public Action onLevelReset;

    /// <summary>
    /// 关卡开始瞬间调用
    /// </summary>
    public Action onLevelStart;
    #endregion

    #region Unity

    private void Start()
    {
        if(instance != null)
            Destroy(gameObject);
        else
            instance = this;

    }

    private void Update()
    {
        RefreshLevelWeaveLength();
        RefreshStars();
    }
    #endregion

    #region Generate
    /// <summary>
    /// 按照id在指定位置生成地图，仅当需要针对性生成地图时独立调用，或用于随机生成方法中
    /// </summary>
    /// <param name="_id">地图id</param>
    /// <param name="_position">世界位置</param>
    /// <param name="_readyToBeClear">是否在关卡结束时清除</param>
    public Piece GeneratePiece(int _id,Vector3 _position,bool _readyToBeClear)
    {
        Piece piece = Instantiate(pieces.Find(t => t.id == _id).gameObject, _position, Quaternion.identity).GetComponent<Piece>();
        if (_readyToBeClear)
            readyToClearPieces.Add(piece);
        else
            readyToReadyToClearPieces.Add(piece);
        return piece;
    }
    /// <summary>
    /// 按照引用在指定位置生成地图，仅当需要针对性生成地图时独立调用，或用于随机生成方法中
    /// </summary>
    /// <param name="_piece">地图引用</param>
    /// <param name="_position">世界位置</param>
    /// <param name="_readyToBeClear">是否在关卡结束时清除</param>
    public Piece GeneratePiece(Piece _piece, Vector3 _position,bool _readyToBeClear)
    {
        Piece piece = Instantiate(_piece.gameObject, _position, Quaternion.identity).GetComponent<Piece>();
        if (_readyToBeClear)
            readyToClearPieces.Add(piece);
        else
            readyToReadyToClearPieces.Add(piece);
        return piece;
    }
    /// <summary>
    /// 在位置列表中随机选择位置生成列表中随机一个地图
    /// </summary>
    /// <remarks>
    /// 如果最后一个参数为真，严禁向此方法中直接传全局地图数据列表和关卡位置列表！！！要传也使用深拷贝方法DeepCopyHelper.DeepCopy拷贝一份！！！
    /// 此方法亦不可用于对两个参数的列表的foreach枚举语句中
    /// </remarks>
    /// <param name="_PieceList">地图id列表</param>
    /// <param name="_positionList">位置列表</param>
    /// <param name="_readyToBeClear">是否在关卡结束时清除</param>
    /// <param name="_allowEmpty">随机生成是否包括不生成</param>
    /// <param name="_removeGenerated">是否删除列表中此次生成的碎片和位置</param>
    public void GenerateRandomPiece(List<Piece> _PieceList,List<Vector3> _positionList,bool _readyToBeClear,bool _allowEmpty,bool _removeGenerated)
    {
        if (_PieceList == null || _positionList == null)
            return;
        int idIndex;
        if (_allowEmpty)
            idIndex = Random.Range(-1, _PieceList.Count);
        else
            idIndex = Random.Range(0, _PieceList.Count);
        int positionIndex = Random.Range(0, _positionList.Count);
        Piece newPiece = new();
        if (idIndex != -1)
        {
            newPiece = GeneratePiece(_PieceList[idIndex], _positionList[positionIndex],_readyToBeClear);
            if (_removeGenerated && !(_PieceList[idIndex] is Piece_Door))
                _PieceList.RemoveAt(idIndex);
        }
        if (_removeGenerated)
            _positionList.RemoveAt(positionIndex);
        if (_PieceList[idIndex] is Piece_Door)
        {
            positionIndex = Random.Range(0, _positionList.Count);
            Piece extraPiece = GeneratePiece(_PieceList[idIndex], _positionList[positionIndex], _readyToBeClear);
            ((Piece_Door)newPiece).relatedDoor = (Piece_Door)extraPiece;
            ((Piece_Door)extraPiece).relatedDoor = (Piece_Door)newPiece;
            if(_removeGenerated)
                _positionList.RemoveAt(positionIndex);
        }
        _PieceList.RemoveAll(t => t is Piece_Door);
    }

    public void GenenrateMap()
    {
        if(level == 0)
        {
            GeneratePiece(checkPoint, levelStartPoint,true);
        }
        GeneratePiece(checkPoint, levelEndPoint,false);
        List<Piece> enabledPieceCopy = DeepCopyHelper.DeepCopy(enabledPieces);
        List<Vector3> genePositionsCopy = DeepCopyHelper.DeepCopy(pieceGenePositions);
        for (int i = 0; i < pieceGeneAmount; i++)
        {
            GenerateRandomPiece(enabledPieceCopy, genePositionsCopy, true,false,true);
        }
    }

    /// <summary>
    /// 清除之前的地图
    /// </summary>
    private void ClearMap()
    {
        for (int i = 0;i < readyToClearPieces.Count;i++)
        {
            Destroy(readyToClearPieces[i].gameObject);
        }
        readyToClearPieces = DeepCopyHelper.DeepCopy(readyToReadyToClearPieces);
        readyToReadyToClearPieces.Clear();
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
        Vector3 startCorner = levelStartPoint + new Vector3(Math.Min(blockHorizontalInterval*(1+level/blockHorizontalInterAddLv*blockHorizontalInterAddRate),blockHorizontalMaxInterval), 0);
        for (int i = 0; i < level/blockHorizontalAddLv+1; i++)
        {
            for(int j = 0; j < level/blockVerticalAddLv+1; j++)
            {
                result.Add(startCorner + new Vector3(Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv*blockHorizontalInterAddRate), blockHorizontalMaxInterval) * i,
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
    private void RefreshStartPosition()
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
        levelEndPoint = levelStartPoint + new Vector3((Math.Min((level / blockHorizontalAddLv + 1), blockHorizontalMaxAmount)+2) * Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval),
            Math.Min((level / blockVerticalAddLv + 1), blockVerticalMaxAmount) * Math.Min(blockVerticalInterval * (1 + level / blockVerticalInterAddLv * blockVerticalInterAddRate), blockVerticalMaxInterval));
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
        if (Solution == null)
            return;

        Stack<Vector3> SolutionCopy = DeepCopyHelper.DeepCopy(Solution);
        Vector3 LastNode = SolutionCopy.Pop();
        float result = 0;
        while(SolutionCopy!= null)
        {
            Vector3 NewNode = SolutionCopy.Pop();
            if (NewNode != Vector3.positiveInfinity&&LastNode != Vector3.positiveInfinity)
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
        if(Solution.Count == 0)
            return;
        levelWeaveLength = nodedLevelWeaveLength + (player.transform.position - Solution.Peek()).magnitude;
    }

    /// <summary>
    /// 刷新全局线长，首次设为0
    /// </summary>
    private void RefreshOverallWeaveLength()
    {
        if (level == 0)
            overallWeaveLength = 0;
        else
            overallWeaveLength += nodedLevelWeaveLength;
    }

    /// <summary>
    /// 依据当前起点、终点和所有待删碎片位置刷新最优解长度
    /// </summary>
    private void RefreshMaxWeaveLength()
    {
        /*
        不行了，搞不出来，放着
        List<Vector3> generatedPiecesPos = new();
        generatedPiecesPos.Add(readyToClearPieces.Find(t => t.isCheckPoint == true).transform.position);
        foreach (var pos in readyToClearPieces.FindAll(t => t.isCheckPoint == false))
        {
            generatedPiecesPos.Add(pos.transform.position);
        }
        generatedPiecesPos.Add(readyToReadyToClearPieces.Find(t => t.isCheckPoint == true).transform.position);
        float[,] distance = new float[generatedPiecesPos.Count, generatedPiecesPos.Count];
        for (int i = 0; i < generatedPiecesPos.Count; i++)
        {
            for(int j = 0; j < i;j++)
            {
                distance[i,j] = (generatedPiecesPos[i] - generatedPiecesPos[j]).magnitude;
            }
        }
        */
        Debug.LogWarning("未完成最大路径算法。");
    }

    /// <summary>
    /// 刷新当前星数，每帧更新
    /// </summary>
    private void RefreshStars()
    {
        if(levelWeaveLength>=star3RequiredRate*maxWeaveLength)
        {
            stars = 3;
        }
        else if(levelWeaveLength>=star2RequiredRate*maxWeaveLength)
        {
            stars = 2;
        }
        else if(levelWeaveLength>=star3RequiredRate*maxWeaveLength)
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

    #endregion

    #region Game&Level
    /// <summary>
    /// 开始游戏，初始化所有数值
    /// </summary>
    public void StartGame()
    {
        level = 0;
        player.gameObject.SetActive(true);
        player.transform.position = Vector3.zero;
        RefreshStartPosition();
        RefreshLevelEndPosition();
        RefreshLevelCenterPosition();
        RefreshLevelPieceGenePosition();
        RefreshPieceGeneMaxAmount();
        RefreshEnabledPiece();
        RefreshOverallWeaveLength();
        RefreshScore();

        onGameStart?.Invoke();

        LevelStart();
    }
    /// <summary>
    /// 暂停游戏，仅在未暂停游戏时生效
    /// </summary>
    public void PauseGame()
    {
        if (isPausing)
            return;

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

        isPausing = false;
        onGameContinue?.Invoke();
    }
    /// <summary>
    /// 结算当局游戏，仅在暂停游戏后生效
    /// </summary>
    public void EndGame()
    {
        player.gameObject.SetActive(false);

        if (!isPausing)
            return;

        isPausing = false;
        onGameEnd?.Invoke();
    }
    /// <summary>
    /// 重置游戏
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
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
    public void LevelComplete()
    {
        level++;

        RefreshStartPosition();
        RefreshLevelEndPosition();
        RefreshLevelCenterPosition();
        RefreshLevelPieceGenePosition();
        RefreshPieceGeneMaxAmount();
        RefreshEnabledPiece();
        RefreshOverallWeaveLength();
        RefreshScore();

        onLevelComplete?.Invoke();
    }

    /// <summary>
    /// 开始下一关
    /// </summary>
    public void LevelStart()
    {
        onLevelStart?.Invoke();
    }

    /// <summary>
    /// 相机位置到位时调用，正式开始关卡；
    /// </summary>
    public void CameraMoved()
    {
        GenenrateMap();
        RefreshMaxWeaveLength();
        ClearMap();
    }
    /// <summary>
    /// 重置当前关卡
    /// </summary>
    public void ResetLevel()
    {
        onLevelReset?.Invoke();
    }


    #endregion

    #region Interface
    /// <summary>
    /// 连接节点。注意：通关也使用此方法完成，输入检查点碎片
    /// </summary>
    /// <param name="_piece">碎片引用</param>
    public void ConnectNode(Piece _piece)
    {
        Solution.Push(_piece.node.position);
        RefreshNodedLevelWeaveLength();
        if (_piece.isCheckPoint)
        {
            LevelComplete();
            Solution.Clear();
            Solution.Push(_piece.node.position);
            RefreshNodedLevelWeaveLength();
        }
    }
    /// <summary>
    /// 取消连接节点。返回成功与否
    /// </summary>
    /// <param name="_piece">碎片引用</param>
    /// <returns>是否成功取消连接</returns>
    public bool TryDisconnectNode(Piece _piece)
    {
        if (Solution.Peek() !=_piece.node.position||_piece.isCheckPoint)
            return false;

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
        Solution.Push(_door.doorPosition);
        Solution.Push(Vector3.positiveInfinity);
        Solution.Push(_door.relatedDoor.doorPosition);
        RefreshNodedLevelWeaveLength();
    }
    /// <summary>
    /// 取消连接门，返回成功与否
    /// </summary>
    /// <param name="_door">门引用</param>
    /// <returns>是否成功取消连接</returns>
    public bool TryDisconnectDoor(Piece_Door _door)
    {
        if(Solution.Peek()!=_door.doorPosition)
            return false;
 
        Solution.Pop();
        Solution.Pop();
        Solution.Pop();
        RefreshNodedLevelWeaveLength();

        return true;
    }
    #endregion
}

public class DeepCopyHelper
{
    /// <summary>
    /// 深拷贝方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_object"></param>
    /// <returns></returns>
    public static T DeepCopy<T>(T _object)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, _object);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return (T)formatter.Deserialize(memoryStream);
        }
    }
}

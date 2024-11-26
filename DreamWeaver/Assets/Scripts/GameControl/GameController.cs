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
    [Header("������")]
    [Tooltip("���")]
    public Player player;

    [Header("��ͼ���")]
    [Tooltip("ȫ����Ƭ�����б�")]
    public List<Piece> pieces;

    [Tooltip("������Ƭ����")]
    public Piece checkPoint;

    [Tooltip("�ؿ���Ƭ����λ��ƫ����")]
    public float pieceGenePositionRamdonBias;

    [Tooltip("�ؿ���Ƭ��������������ؿ���")]
    public int pieceGeneAmountAddLv;

    [Tooltip("�ؿ���Ƭ��������")]
    public int pieceGeneMaxAmount;

    [Tooltip("ˮƽ���ʼ���")]
    public float blockHorizontalInterval;

    [Tooltip("ˮƽ����������������")]
    public float blockHorizontalInterAddRate;

    [Tooltip("ˮƽ������������ؿ���")]
    public int blockHorizontalInterAddLv;

    [Tooltip("ˮƽ�������")]
    public float blockHorizontalMaxInterval;

    [Tooltip("��ֱ���ʼ���")]
    public float blockVerticalInterval;

    [Tooltip("��ֱ����������������")]
    public float blockVerticalInterAddRate;

    [Tooltip("��ֱ������������ؿ���")]
    public int blockVerticalInterAddLv;

    [Tooltip("��ֱ�������")]
    public float blockVerticalMaxInterval;

    [Tooltip("ˮƽ����������ؿ���")]
    public int blockHorizontalAddLv;

    [Tooltip("ˮƽ������")]
    public int blockHorizontalMaxAmount;

    [Tooltip("��ֱ����������ؿ���")]
    public int blockVerticalAddLv;

    [Tooltip("��ֱ������")]
    public int blockVerticalMaxAmount;

    [Header("�÷����")]
    [Tooltip("��λ����")]
    public float unitScore;

    [Tooltip("һ�ǳ��ȱ�������")]
    public float star1RequiredRate;

    [Tooltip("���ǳ��ȱ�������")]
    public float star2RequiredRate;

    [Tooltip("���ǳ��ȱ�������")]
    public float star3RequiredRate;

    /// <summary>
    /// �ؿ����
    /// </summary>
    public Vector3 levelStartPoint {  get; private set; }

    /// <summary>
    /// �ؿ�����
    /// </summary>
    public Vector3 levelCenterPoint {  get; private set; }

    /// <summary>
    /// �ؿ��յ�
    /// </summary>
    public Vector3 levelEndPoint { get; private set; }

    /// <summary>
    /// ������Ƭ�б�
    /// </summary>
    public List<Piece> enabledPieces  { get { return new(); } private set { } }

    /// <summary>
    /// �ؿ���Ƭ����λ��
    /// </summary>
    public List<Vector3> pieceGenePositions { get { return new(); } private set { } }

    /// <summary>
    /// ������һ�ض����������������Ƭ
    /// </summary>
    public List<Piece> readyToClearPieces { get { return new(); } private set { } }

    /// <summary>
    /// ������һ���Ȳ���������뵽����һ������б��е���Ƭ
    /// </summary>
    public List<Piece> readyToReadyToClearPieces { get { return new(); } private set { } }

    /// <summary>
    /// �ؿ���Ƭ������
    /// </summary>
    public int pieceGeneAmount { get; private set; }

    /// <summary>
    /// �������
    /// </summary>
    public int level {  get; private set; }

    /// <summary>
    /// ��ҽ��������ÿ������ʱ����
    /// </summary>
    public Stack<Vector3> Solution { get { return new(); } private set { } }

    /// <summary>
    /// ���������ӵ��߳���ÿ�����Ӹ���
    /// </summary>
    public float nodedLevelWeaveLength {  get; private set; }

    /// <summary>
    /// �����߳���ÿ֡����
    /// </summary>
    public float levelWeaveLength {  get; private set; }

    /// <summary>
    /// �������Ž��߳���ÿ�ظ���
    /// </summary>
    public float maxWeaveLength { get; private set; }

    /// <summary>
    /// ���߳�������ͨ�ؿ��߳�֮�ͣ�ÿ�ظ���
    /// </summary>
    public float overallWeaveLength {  get; private set; }

    /// <summary>
    /// ÿ�صĵ÷�����ϵ��
    /// </summary>
    public float scoreLevelModifier {  get; private set; }

    /// <summary>
    /// ������ÿ�ظ���
    /// </summary>
    public float score {  get; private set; }

    /// <summary>
    /// ��ǰ��������ÿ֡����
    /// </summary>
    public int stars {  get; private set; }

    /// <summary>
    /// �Ƿ�������ͣ
    /// </summary>
    public bool isPausing { get; private set; }
    #endregion

    #region Actions
    /// <summary>
    /// ��Ϸ��ʼ˲�����
    /// </summary>
    public Action onGameStart;

    /// <summary>
    /// ��Ϸ��ͣ˲�����
    /// </summary>
    public Action onGamePause;

    /// <summary>
    /// ��Ϸ����˲�����
    /// </summary>
    public Action onGameContinue;

    /// <summary>
    /// ��Ϸ����˲�����
    /// </summary>
    public Action onGameEnd;

    /// <summary>
    /// �ؿ�����˲�����
    /// </summary>
    public Action onLevelComplete;

    /// <summary>
    /// �ؿ�����˲�����
    /// </summary>
    public Action onLevelReset;

    /// <summary>
    /// �ؿ���ʼ˲�����
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
    /// ����id��ָ��λ�����ɵ�ͼ��������Ҫ��������ɵ�ͼʱ�������ã�������������ɷ�����
    /// </summary>
    /// <param name="_id">��ͼid</param>
    /// <param name="_position">����λ��</param>
    /// <param name="_readyToBeClear">�Ƿ��ڹؿ�����ʱ���</param>
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
    /// ����������ָ��λ�����ɵ�ͼ��������Ҫ��������ɵ�ͼʱ�������ã�������������ɷ�����
    /// </summary>
    /// <param name="_piece">��ͼ����</param>
    /// <param name="_position">����λ��</param>
    /// <param name="_readyToBeClear">�Ƿ��ڹؿ�����ʱ���</param>
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
    /// ��λ���б������ѡ��λ�������б������һ����ͼ
    /// </summary>
    /// <remarks>
    /// ������һ������Ϊ�棬�Ͻ���˷�����ֱ�Ӵ�ȫ�ֵ�ͼ�����б�͹ؿ�λ���б�����Ҫ��Ҳʹ���������DeepCopyHelper.DeepCopy����һ�ݣ�����
    /// �˷����಻�����ڶ������������б��foreachö�������
    /// </remarks>
    /// <param name="_PieceList">��ͼid�б�</param>
    /// <param name="_positionList">λ���б�</param>
    /// <param name="_readyToBeClear">�Ƿ��ڹؿ�����ʱ���</param>
    /// <param name="_allowEmpty">��������Ƿ����������</param>
    /// <param name="_removeGenerated">�Ƿ�ɾ���б��д˴����ɵ���Ƭ��λ��</param>
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
    /// ���֮ǰ�ĵ�ͼ
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
    /// ���ݵ�ǰ���εȼ�����ʼ�ص�ˢ��������Ƭ���ɵ�
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
    /// ���ݵ�ǰ���εȼ�ˢ�¿����ɵ���Ƭ
    /// </summary>
    private void RefreshEnabledPiece()
    {
        enabledPieces = pieces.FindAll(t => t.difficulty <= level);
    }

    /// <summary>
    /// �����ϴ��յ�ˢ�����λ�ã��״�ֱ����Ϊ����ԭ��
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
    /// ���ݵ�ǰ���εȼ�����ʼ�ص�ˢ���յ�λ��
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelEndPosition()
    {
        levelEndPoint = levelStartPoint + new Vector3((Math.Min((level / blockHorizontalAddLv + 1), blockHorizontalMaxAmount)+2) * Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval),
            Math.Min((level / blockVerticalAddLv + 1), blockVerticalMaxAmount) * Math.Min(blockVerticalInterval * (1 + level / blockVerticalInterAddLv * blockVerticalInterAddRate), blockVerticalMaxInterval));
    }

    /// <summary>
    /// ���������յ�ˢ�µ�ͼ����λ��
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelCenterPosition()
    {
        levelCenterPoint = (levelStartPoint + levelEndPoint) / 2;
    }

    /// <summary>
    /// ���ݵ�ǰ���εȼ���ˢ�µ�����ˢ�µ�ǰ�ؿ������Ƭ��
    /// </summary>
    private void RefreshPieceGeneMaxAmount()
    {
        pieceGeneAmount = Math.Min(Math.Min(pieceGeneMaxAmount, (1 + level / pieceGeneAmountAddLv)), pieceGenePositions.Count);
    }

    /// <summary>
    /// ������ҽ������ˢ�µ�ǰ�ؿ������ӽڵ��ܾ���
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
    /// ˢ�µ�ǰ�ؿ����߳�
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelWeaveLength()
    {
        if(Solution.Count == 0)
            return;
        levelWeaveLength = nodedLevelWeaveLength + (player.transform.position - Solution.Peek()).magnitude;
    }

    /// <summary>
    /// ˢ��ȫ���߳����״���Ϊ0
    /// </summary>
    private void RefreshOverallWeaveLength()
    {
        if (level == 0)
            overallWeaveLength = 0;
        else
            overallWeaveLength += nodedLevelWeaveLength;
    }

    /// <summary>
    /// ���ݵ�ǰ��㡢�յ�����д�ɾ��Ƭλ��ˢ�����Žⳤ��
    /// </summary>
    private void RefreshMaxWeaveLength()
    {
        /*
        �����ˣ��㲻����������
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
        Debug.LogWarning("δ������·���㷨��");
    }

    /// <summary>
    /// ˢ�µ�ǰ������ÿ֡����
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
    /// ˢ����ҷ�������ʼ��Ϊ0��
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
    /// ��ʼ��Ϸ����ʼ��������ֵ
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
    /// ��ͣ��Ϸ������δ��ͣ��Ϸʱ��Ч
    /// </summary>
    public void PauseGame()
    {
        if (isPausing)
            return;

        isPausing = true;
        onGamePause?.Invoke();
    }
    /// <summary>
    /// ������Ϸ��������ͣ��Ϸ����Ч
    /// </summary>
    public void ContinueGame()
    {
        if (!isPausing)
            return;

        isPausing = false;
        onGameContinue?.Invoke();
    }
    /// <summary>
    /// ���㵱����Ϸ��������ͣ��Ϸ����Ч
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
    /// ������Ϸ
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// �˳���Ϸ����
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
    /// ��ɵ�ǰ�ؿ�
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
    /// ��ʼ��һ��
    /// </summary>
    public void LevelStart()
    {
        onLevelStart?.Invoke();
    }

    /// <summary>
    /// ���λ�õ�λʱ���ã���ʽ��ʼ�ؿ���
    /// </summary>
    public void CameraMoved()
    {
        GenenrateMap();
        RefreshMaxWeaveLength();
        ClearMap();
    }
    /// <summary>
    /// ���õ�ǰ�ؿ�
    /// </summary>
    public void ResetLevel()
    {
        onLevelReset?.Invoke();
    }


    #endregion

    #region Interface
    /// <summary>
    /// ���ӽڵ㡣ע�⣺ͨ��Ҳʹ�ô˷�����ɣ����������Ƭ
    /// </summary>
    /// <param name="_piece">��Ƭ����</param>
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
    /// ȡ�����ӽڵ㡣���سɹ����
    /// </summary>
    /// <param name="_piece">��Ƭ����</param>
    /// <returns>�Ƿ�ɹ�ȡ������</returns>
    public bool TryDisconnectNode(Piece _piece)
    {
        if (Solution.Peek() !=_piece.node.position||_piece.isCheckPoint)
            return false;

        Solution.Pop();
        RefreshNodedLevelWeaveLength();

        return true;
    }
    /// <summary>
    /// ������
    /// </summary>
    /// <param name="_door">������</param>
    public void ConnectDoor(Piece_Door _door)
    {
        Solution.Push(_door.doorPosition);
        Solution.Push(Vector3.positiveInfinity);
        Solution.Push(_door.relatedDoor.doorPosition);
        RefreshNodedLevelWeaveLength();
    }
    /// <summary>
    /// ȡ�������ţ����سɹ����
    /// </summary>
    /// <param name="_door">������</param>
    /// <returns>�Ƿ�ɹ�ȡ������</returns>
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
    /// �������
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

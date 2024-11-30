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
    [Header("�������Ϸ���")]
    [Tooltip("���")]
    public Player player;

    [Tooltip("���ö���")]
    public UI_ResetAnim resetAnim;

    [Tooltip("�ڶ�")]
    public GameObject blackHole;

    [Tooltip("�ɽ����뾶")]
    public float interactRatio;

    [Tooltip("������")]
    public StandaloneInputModule InputModule;

    [Header("��ͼ���")]
    [Tooltip("ȫ����Ƭ�����б�")]
    public List<Piece> pieces;

    [Tooltip("������Ƭ����")]
    public Piece checkPoint;

    [Tooltip("�ؿ���Ƭ����λ��ƫ����")]
    public float pieceGenePositionRamdonBias;

    [Tooltip("�ڶ�����λ��ƫ����")]
    public float blackHoleGenePositionRamdonBias;

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
    public Vector3 levelStartPoint { get; private set; }

    /// <summary>
    /// �ؿ�����
    /// </summary>
    public Vector3 levelCenterPoint { get; private set; }

    /// <summary>
    /// �ؿ��յ�
    /// </summary>
    public Vector3 levelEndPoint { get; private set; }

    /// <summary>
    /// ������Ƭ�б�
    /// </summary>
    public List<Piece> enabledPieces { get; private set; } = new();

    /// <summary>
    /// �ؿ���Ƭ������λ��
    /// </summary>
    public List<Vector3> pieceGenePositions { get; private set; } = new();

    /// <summary>
    /// �ؿ��������ɵ���Ƭ
    /// </summary>
    public List<Piece> levelPieces { get; private set; } = new();

    /// <summary>
    /// ��һ���������Ƭ
    /// </summary>
    public List<GameObject> readyToClear { get; private set; } = new();

    /// <summary>
    /// ����һ���������Ƭ
    /// </summary>
    public List<GameObject> readyToClear_2 { get; private set; } = new();

    /// <summary>
    /// ������һ���������Ƭ
    /// </summary>
    public List<GameObject> readyToClear_3 { get; private set; } = new();

    /// <summary>
    /// �ؿ���Ƭ������
    /// </summary>
    public int pieceGeneAmount { get; private set; }

    /// <summary>
    /// �������
    /// </summary>
    public int level { get; private set; }

    /// <summary>
    /// ��ҽ��������ÿ������ʱ����
    /// </summary>
    public Stack<Vector3> Solution { get; private set; } = new();

    /// <summary>
    /// ���������ӵ��߳���ÿ�����Ӹ���
    /// </summary>
    public float nodedLevelWeaveLength { get; private set; }

    /// <summary>
    /// �����߳���ÿ֡����
    /// </summary>
    public float levelWeaveLength { get; private set; }

    /// <summary>
    /// �������Ž��߳���ÿ�ظ���
    /// </summary>
    public float maxWeaveLength { get; private set; }

    /// <summary>
    /// ���߳�������ͨ�ؿ��߳�֮�ͣ�ÿ�ظ���
    /// </summary>
    public float overallWeaveLength { get; private set; }

    /// <summary>
    /// ÿ�صĵ÷�����ϵ��
    /// </summary>
    public float scoreLevelModifier { get; private set; }

    /// <summary>
    /// ������ÿ�ظ���
    /// </summary>
    public float score { get; private set; }

    /// <summary>
    /// ��ǰ��������ÿ֡����
    /// </summary>
    public int stars { get; private set; }

    /// <summary>
    /// ��ǰ�ؽ���
    /// </summary>
    public int bonus { get; private set; }

    /// <summary>
    /// ��һ�����ɺڶ���
    /// </summary>
    public int blackHoleAmount { get; private set; }

    /// <summary>
    /// �Ƿ�������ͣ
    /// </summary>
    public bool isPausing { get; private set; }

    /// <summary>
    /// ��Ϸ�Ƿ����ڽ���
    /// </summary>
    public bool isGaming { get; private set; }

    /// <summary>
    /// �Ƿ����ڲ������ö���
    /// </summary>
    public bool isResetAnimating { get; set; }

    /// <summary>
    /// �Ƿ����ڲ���ת�ؿ�����
    /// </summary>
    public bool isReadyAnimating {  get; set; }
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
    /// �������ѡ��˲�����
    /// </summary>
    public Action onLevelReady;

    /// <summary>
    /// �ؿ���ʼ˲�����
    /// </summary>
    public Action onLevelStart;

    /// <summary>
    /// �ؿ�����˲�����
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
    /// ����������ָ��λ�����ɵ�ͼ��������Ҫ��������ɵ�ͼʱ�������ã�������������ɷ�����
    /// </summary>
    /// <param name="_piece">��ͼ����</param>
    /// <param name="_position">����λ��</param>
    /// <param name="_readyToBeClear">�Ƿ��ڹؿ�����ʱ���</param>
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
    /// ��λ���б������ѡ��λ�������б������һ����ͼ
    /// </summary>
    /// <remarks>
    /// �Ͻ���˷�����ֱ�Ӵ�ȫ�ֵ�ͼ�����б�͹ؿ�λ���б�����Ҫ��Ҳʹ���������DeepCopyHelper.DeepCopy����һ�ݣ�����
    /// �˷����಻�����ڶ������������б��foreachö�������
    /// </remarks>
    /// <param name="_pieceList">��ͼid�б�</param>
    /// <param name="_positionList">λ���б�</param>
    /// <param name="_readyToBeClear">�Ƿ��ڹؿ�����ʱ���</param>
    /// <param name="_allowEmpty">��������Ƿ����������</param>
    /// <param name="_removeGenerated">�Ƿ�ɾ���б��д˴����ɵ���Ƭ��λ��</param>
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
    /// ���ɵ�ͼ
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
    /// ���ɵ�ͼ�����Ŷ���
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
    /// �ڵ�ͼ������ɺ����ɺڶ�
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
    /// ���֮ǰ�ĵ�ͼ
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
    /// ���ݵ�ǰ���εȼ�����ʼ�ص�ˢ��������Ƭ���ɵ�
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
    /// ���ݵ�ǰ���εȼ�ˢ�¿����ɵ���Ƭ
    /// </summary>
    private void RefreshEnabledPiece()
    {
        enabledPieces = pieces.FindAll(t => t.difficulty <= level);
    }

    /// <summary>
    /// �����ϴ��յ�ˢ�����λ�ã��״�ֱ����Ϊ����ԭ��
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
    /// ���ݵ�ǰ���εȼ�����ʼ�ص�ˢ���յ�λ��
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelEndPosition()
    {
        levelEndPoint = levelStartPoint + new Vector3((Math.Min((level / blockHorizontalAddLv), blockHorizontalMaxAmount) + 2) * Math.Min(blockHorizontalInterval * (1 + level / blockHorizontalInterAddLv * blockHorizontalInterAddRate), blockHorizontalMaxInterval),
            Math.Min((level / blockVerticalAddLv), blockVerticalMaxAmount) * Math.Min(blockVerticalInterval * (1 + level / blockVerticalInterAddLv * blockVerticalInterAddRate), blockVerticalMaxInterval));
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
    /// ˢ�µ�ǰ�ؿ����߳�
    /// </summary>
    /// <returns></returns>
    private void RefreshLevelWeaveLength()
    {
        if (Solution == null || Solution.Count == 0)
            return;
        levelWeaveLength = nodedLevelWeaveLength + (player.transform.position - Solution.Peek()).magnitude;
    }

    /// <summary>
    /// �����������ܳ���ˢ��ȫ���߳����״���Ϊ0
    /// </summary>
    private void RefreshOverallWeaveLength()
    {
        if (level == 0)
            overallWeaveLength = 0;
        else
            overallWeaveLength += nodedLevelWeaveLength;
    }

    /// <summary>
    /// ���ݵ�ǰ��㡢�յ��������Ƭλ��ˢ�����Žⳤ��
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
        //���������ΪȨ��
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
        //��ֱֹ�Ӵ���ʼ�������յ�
        for (int i = 0; i < allNodes.Count - 1; i++)
        {
            originPathWeightToFinal[i] = pathWeights[i][allNodes.Count - 1];
            pathWeights[i][allNodes.Count - 1] = 0;
        }
        //��ʼ����������
        int[] path = new int[allNodes.Count];//���ڼ�¼·��
        float[] pathWeight = new float[allNodes.Count];//���ڸ��¾���
        bool[] flag = new bool[allNodes.Count];
        for (int i = 0; i < allNodes.Count; i++)
        {
            flag[i] = false;
            pathWeight[i] = pathWeights[0][i];
            path[i] = 0;
        }
        flag[0] = true;
        //��ʼ����
        int calculateCount = 0;
        while (true)
        {
            //������ѭ��
            calculateCount++;
            if (calculateCount > allNodes.Count)
            {
                Debug.Log("��������");
                break;
            }
            float maxDistance = float.MinValue;
            int maxIndex = -1;
            //�ҵ���ǰȨ�����Ľڵ� ��¼�����Լ���ڵ��
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
            //���±��
            flag[maxIndex] = true;
            //����Ȩ������
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
        Debug.Log("������:" + pathWeight[^1]);//�ҵ�һ��ò������� ���ܱ���Ķ�һ�� �����Բ��᳤ ����������
        maxWeaveLength = pathWeight[^1];
    }

    private void BfsSearchMaxLength(Vector2 startPoint, Vector2 endPoint, List<Vector2> points)
    {
        float max = float.MinValue;
        //���������ΪȨ��
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
    /// ˢ�µ�ǰ������ÿ֡����
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
    /// ˢ����ҷ�������ʼ��Ϊ0��
    /// </summary>
    private void RefreshScore()
    {
        if (level == 0)
            score = 0;
        else
            score += scoreLevelModifier * Solution.Count * nodedLevelWeaveLength * unitScore;
    }

    /// <summary>
    /// ˢ��ÿ���������
    /// </summary>
    private void RefreshOthers()
    {
        bonus = 0;
        scoreLevelModifier = 1;
    }

    #endregion

    #region Game&Level
    /// <summary>
    /// ��ʼ��Ϸ����ʼ��������ֵ
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
    /// ��ͣ��Ϸ������δ��ͣ��Ϸʱ��Ч
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
    /// ������Ϸ��������ͣ��Ϸ����Ч
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
    /// ���㵱����Ϸ��������ͣ��Ϸ����Ч
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
    /// ������Ϸ
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
    public void CompleteLevel()
    {
        level++;
        Time.timeScale = 0;
        isPausing = true;
        InGameUIManager.Instance.OpenRoguePropPanel(stars + bonus);
        onLevelComplete?.Invoke();
    }

    /// <summary>
    /// ������ѡ�꣬׼����ʼ��һ��
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
    /// ��ʼ��һ��
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
    /// ���õ�ǰ�ؿ�
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
    /// ���¿�ʼ�ؿ����������ɵ�ͼ�������Ƿ�������������
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
    /// ���ո���ֵ���ӷ�������ϵ��
    /// </summary>
    /// <remarks>
    /// һ���1���Ǽ�100
    /// </remarks>
    /// <param name="_addRate"></param>
    public void AddScoreModifier(float _addRate)
    {
        scoreLevelModifier += _addRate;
    }

    /// <summary>
    /// ���ո���ֵ���ӽ���
    /// </summary>
    /// <param name="_amount"></param>
    public void AddBonue(int _amount)
    {
        bonus += _amount;
    }

    /// <summary>
    /// ���ո���ֵ����ڶ�
    /// </summary>
    /// <param name="_amount"></param>
    public void AddBlackHole(int _amount)
    {
        blackHoleAmount += _amount;
    }


    /// <summary>
    /// ���ӽڵ㡣ע�⣺ͨ��Ҳʹ�ô˷�����ɣ����������Ƭ
    /// </summary>
    /// <param name="_piece">��Ƭ����</param>
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
    /// ȡ�����ӽڵ㡣���سɹ����
    /// </summary>
    /// <param name="_piece">��Ƭ����</param>
    /// <returns>�Ƿ�ɹ�ȡ������</returns>
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
    /// ������
    /// </summary>
    /// <param name="_door">������</param>
    public void ConnectDoor(Piece_Door _door)
    {
        Solution.Push(_door.door.position);
        Solution.Push(-Vector3.one);
        Solution.Push(_door.relatedDoor.door.position);
        RefreshNodedLevelWeaveLength();

    }
    /// <summary>
    /// ȡ�������ţ����سɹ����
    /// </summary>
    /// <param name="_door">������</param>
    /// <returns>�Ƿ�ɹ�ȡ������</returns>
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

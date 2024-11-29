using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Piece_Wheel : Piece
{
    [SerializeField] private Transform colliders;
    [SerializeField] private float ratio;
    [SerializeField] private float speed;
    private int amount;
    private List<Transform> cds = new();
    protected override void Start()
    {
        base.Start();
        cds.Clear();
        amount = colliders.childCount;
        Quaternion unitRotation = Quaternion.Euler(0, 0, 360 / amount);
        for (int i = 0; i < amount; i++)
        {
            cds.Add(colliders.GetChild(i));
            cds[i].name = "PlatForm " + i;
            if (cds[i].GetComponent<Collider2D>() != null)
            {
                cds[i].AddComponent<BoxCollider2D>();
            }
            Vector3 relativePosition = Vector3.right * ratio;
            for (int j = 0; j < i; j++)
            {
                relativePosition = unitRotation * relativePosition;
            }
            cds[i].transform.position = colliders.transform.position + relativePosition;
        }
    }

    List<Vector3> nextCdsRelaPosition = new();
    protected override void Update()
    {
        base.Update();
        nextCdsRelaPosition.Clear();
        for (int i = 0; i < amount; i++)
        {
            Transform nextCd = null;
            if (ramdomInt < 0.5)
            {
                if (i == 0)
                {
                    nextCd = cds[amount - 1];
                }
                else
                {
                    nextCd = cds[i - 1];
                }
            }
            else
            {
                if (i == amount - 1)
                {
                    nextCd = cds[0];
                }
                else
                {
                    nextCd = cds[i + 1];
                }
            }
            nextCdsRelaPosition.Add(nextCd.transform.position - colliders.transform.position);
        }
        for (int i = 0; i < amount; i++)
        {
            Transform thisCd = cds[i];
            Vector3 thisRelativePosition = thisCd.transform.position - colliders.transform.position;
            thisRelativePosition = Vector3.MoveTowards(thisRelativePosition, nextCdsRelaPosition[i], speed * 0.01f).normalized*ratio;
            thisCd.transform.position = thisRelativePosition + colliders.transform.position;
        }
    }
}

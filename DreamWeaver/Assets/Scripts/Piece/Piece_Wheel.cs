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
        amount = colliders.childCount;
        Quaternion unitRotation = Quaternion.Euler(0, 0, 360 / amount);
        Vector3 relativePosition = Vector3.right * ratio;
        for (int i = 0; i < amount; i++)
        {
            cds.Add(colliders.GetChild(i));
            cds[i].name = "PlatForm " + i;
            if (cds[i].GetComponent<Collider2D>() == null)
            {
                cds[i].AddComponent<BoxCollider2D>().usedByEffector = true;
            }
            if (cds[i].GetComponent<PlatformEffector2D>() == null)
            {
                cds[i].AddComponent<PlatformEffector2D>();
            }
            cds[i].transform.position = colliders.transform.position + relativePosition;
            relativePosition = unitRotation * relativePosition;
        }
    }

    List<Vector3> nextCdsRelaPosition = new();
    protected override void Update()
    {
        base.Update();
        if (transform.localScale != Vector3.one)
            return;

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
        bool isStanded = false;
        for (int i = 0; i < amount; i++)
        {
            Transform thisCd = cds[i];
            Vector3 thisRelativePosition = thisCd.transform.position - colliders.transform.position;
            thisRelativePosition = Vector3.RotateTowards(thisRelativePosition, nextCdsRelaPosition[i], speed * Time.deltaTime,0);
            thisCd.transform.position = thisRelativePosition + colliders.transform.position;

            if (cds[i].GetComponent<BoxCollider2D>().IsTouching(GameController.instance.player.GetComponent<Collider2D>()) && GameController.instance.player.transform.parent != cds[i])
            {
                GameController.instance.player.transform.parent = cds[i];
                isStanded = true;
            }
        }
        if(!isStanded)
        {
            GameController.instance.player.transform.parent = GameController.instance.transform.parent;
        }

    }
}

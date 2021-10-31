using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]private GameObject horizontalPathPrefab;
    [SerializeField] private GameObject verticalPathPrefab;
    [SerializeField] private GameObject leftUpPathPrefab;
    [SerializeField] private GameObject leftDownPathPrefab;
    [SerializeField] private GameObject rightUpPathPrefab;
    [SerializeField] private GameObject rightDownPathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        Vector3 startPos = lr.GetPosition(0);
        GameObject go;
        for(int i = 1; i < lr.positionCount; i++)
        {
            Vector3 newpos = lr.GetPosition(i);
            if (Mathf.Abs(startPos.x - newpos.x) > 0)
            {
                Vector3 start, end;
                if(startPos.x < newpos.x)
                {
                    start = startPos;
                    end = newpos;
                }else
                {
                    start = newpos;
                    end = startPos;
                }
                for (float x = start.x; x <= end.x; x++)
                {
                    go=Instantiate(horizontalPathPrefab,new Vector3(x,start.y,0f),Quaternion.identity);
                    go.transform.parent = transform;
                }
            }
            else if (Mathf.Abs(startPos.y - newpos.y) > 0)
            {
                Vector3 start, end;
                if (startPos.y < newpos.y)
                {
                    start = startPos;
                    end = newpos;
                }
                else
                {
                    start = newpos;
                    end = startPos;
                }
                for (float y = start.y; y <= end.y; y++)
                {
                    go = Instantiate(verticalPathPrefab, new Vector3(start.x, y, 0f), Quaternion.identity);
                    go.transform.parent = transform;
                }
            }
            if (i < lr.positionCount - 1)
            {
                //Debug.Log(startPos);
                //Debug.Log(newpos);
                //Debug.Log(lr.GetPosition(i + 1));
                if (newpos.x > startPos.x && newpos.y < lr.GetPosition(i + 1).y)
                {
                    go=Instantiate(leftUpPathPrefab, newpos+ new Vector3(0.5f,-0.5f,0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.x > startPos.x && newpos.y > lr.GetPosition(i + 1).y)
                {
                    go = Instantiate(leftDownPathPrefab, newpos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.y < startPos.y && newpos.x < lr.GetPosition(i + 1).x)
                {
                    go = Instantiate(rightUpPathPrefab, newpos + new Vector3(-0.5f, -0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.y < startPos.y && newpos.x > lr.GetPosition(i + 1).x)
                {
                    go = Instantiate(rightDownPathPrefab, newpos + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                
            }
            startPos = lr.GetPosition(i);
        }
    }

    public void OnDrawGizmos()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        Vector3 startPos = lr.GetPosition(0);
        for (int i = 1; i < lr.positionCount; i++)
        {
            Gizmos.DrawLine(startPos, lr.GetPosition(i));
            startPos = lr.GetPosition(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

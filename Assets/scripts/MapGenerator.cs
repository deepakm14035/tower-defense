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
    [SerializeField] private GameObject startPrefab;
    [SerializeField] private GameObject endPrefab;
    [SerializeField] private GameObject newPathPrefab;

    public bool useNewPath = true;

    // Start is called before the first frame update
    public void GenerateLevel(Level level)
    {
        //Level level = FindObjectOfType<LevelGenerator>().currentLevel;
        FindObjectOfType<LevelGenerator>().currentLevel = level;
        Camera.main.orthographicSize = level.cameraSize;
        //LineRenderer lr = GetComponent<LineRenderer>();
        //lr.SetPositions(level.path);
        int posCount = level.path.Length;
        Vector3[] arr = level.path;
        Vector3 startPos = arr[0];
        Instantiate(startPrefab, startPos, Quaternion.identity);
        Instantiate(endPrefab, arr[posCount - 1], Quaternion.identity);
        GameObject go;
        //Debug.Log("position count-" + lr.positionCount);
        for(int i = 1; i < posCount; i++)
        {
            Vector3 newpos = arr[i];
            if(Mathf.Abs(startPos.x - newpos.x) > 0 && Mathf.Abs(startPos.y - newpos.y) > 0)
            {
                Vector3 start, end;
                start = startPos;
                end = newpos;
                go = Instantiate(newPathPrefab, start + (end - start) / 2f, Quaternion.identity);
                go.GetComponent<SpriteRenderer>().size = new Vector2(Vector3.Magnitude( end - start),1f);
                go.transform.rotation= Quaternion.Euler(0,0, Mathf.Rad2Deg * Mathf.Atan((end.y-start.y)/(end.x-start.x)));
                Debug.Log(((end.y - start.y) / (end.x - start.x))+", "+ Mathf.Rad2Deg * Mathf.Atan((end.y - start.y) / (end.x - start.x)));
                go.transform.parent = transform;
            }
            else if (Mathf.Abs(startPos.x - newpos.x) > 0)
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
                if (!useNewPath) {
                    for (float x = start.x; x <= end.x; x++)
                    {
                        go = Instantiate(horizontalPathPrefab, new Vector3(x, start.y, 0f), Quaternion.identity);
                        go.transform.parent = transform;
                    }
                }
                else {
                    go = Instantiate(newPathPrefab, new Vector3(start.x + (end.x - start.x) / 2f, start.y, 0f), Quaternion.identity);
                    go.GetComponent<SpriteRenderer>().size = new Vector2(end.x - start.x+1f, 1f);
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
                if (!useNewPath) {
                    for (float y = start.y; y <= end.y; y++)
                    {
                        go = Instantiate(verticalPathPrefab, new Vector3(start.x, y, 0f), Quaternion.identity);
                        go.transform.parent = transform;
                    }
                }
                else
                {
                    go = Instantiate(newPathPrefab, new Vector3(start.x, start.y + (end.y - start.y)/2f, 0f), Quaternion.identity);
                    go.GetComponent<SpriteRenderer>().size = new Vector2(1f,end.y-start.y+1f);
                    go.transform.parent = transform;
                }

            }
            if (i < arr.Length - 1)
            {
                //Debug.Log(startPos);
                //Debug.Log(newpos);
                //Debug.Log(lr.GetPosition(i + 1));
                if (newpos.x > startPos.x && newpos.y < arr[i + 1].y && !useNewPath)
                {
                    go=Instantiate(rightUpPathPrefab, newpos+ new Vector3(0.5f,-0.5f,0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.x > startPos.x && newpos.y > arr[i + 1].y && !useNewPath)
                {
                    go = Instantiate(leftDownPathPrefab, newpos + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.x < startPos.x && newpos.y < arr[i + 1].y && !useNewPath)
                {
                    go = Instantiate(rightUpPathPrefab, newpos + new Vector3(-0.5f, -0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.x < startPos.x && newpos.y > arr[i + 1].y && !useNewPath)
                {
                    go = Instantiate(rightDownPathPrefab, newpos + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.y < startPos.y && newpos.x < arr[i + 1].x && !useNewPath)
                {
                    go = Instantiate(rightUpPathPrefab, newpos + new Vector3(-0.5f, -0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.y < startPos.y && newpos.x > arr[i + 1].x && !useNewPath)
                {
                    go = Instantiate(leftUpPathPrefab, newpos + new Vector3(0.5f, -0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else if (newpos.y > startPos.y && newpos.x < arr[i + 1].x && !useNewPath)
                {
                    go = Instantiate(rightDownPathPrefab, newpos + new Vector3(-0.5f, 0.5f, 0), Quaternion.identity);
                    go.transform.parent = transform;
                }
                else
                {
                    go = Instantiate(newPathPrefab, newpos, Quaternion.identity);
                    go.transform.parent = transform;
                }

            }
            startPos = arr[i];
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

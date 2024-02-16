using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
        LoadBarriers();
        
    }

    [SerializeField] Vector3 middleStartPos = Vector2.zero;
    [SerializeField] Vector3 cameraStartPos = Vector2.zero;
    [SerializeField] bool isMovingCamera = false;
    [SerializeField] Vector3 mousePos = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        OnMouseActive();
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 10;
        if(Camera.main.orthographicSize < 0 )
        {
            Camera.main.orthographicSize = 0;
        }
        mousePos = Input.mousePosition / (250 / Camera.main.orthographicSize);
        mousePos.z = -10;
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            middleStartPos = mousePos;
            cameraStartPos = Camera.main.transform.position;
            isMovingCamera = true;
        } else if(Input.GetKeyUp(KeyCode.Mouse2)) {
            isMovingCamera = false;
        } else if(isMovingCamera)
        {
            Camera.main.transform.position = cameraStartPos - (mousePos - middleStartPos);
        }
    }

    [SerializeField] public bool wasMouseDown = false;
    [SerializeField] Vector3 mouseStartPos = Vector3.zero;
    [SerializeField] GameObject selectedObject = null;
    [SerializeField] Vector3 selectedObjectStartPos = Vector3.zero;
    [SerializeField] List<GameObject> barriers = new List<GameObject>();
    [SerializeField] bool gridSnap = true;
    [SerializeField] float gridPrecision = 0.1f;

    public static ObjectController INSTANCE;

    private void OnMouseDown()
    {

        

    }

    private void LoadBarriers()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Barrier");
        for(int i = 0; i < objects.Length; i++)
        {
            barriers.Add(objects[i]);
        }
    }

    

    private void OnMouseActive()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (GameObject barrier in barriers)
            {
                BoxCollider2D bc = barrier.GetComponent<BoxCollider2D>();
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (bc.bounds.Contains(mousePos))
                {
                    return;
                }
            }
            //Mouse Down Event
            if (!wasMouseDown)
            {
                Persistent.hasMoused = true;
                mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                
                
                mouseStartPos.z = 0;
                GameObject[] objects = GameObject.FindGameObjectsWithTag("Moveable");
                for (int i = 0; i < objects.Length; i++)
                {
                    GameObject go = objects[i];
                    BoxCollider2D bc = go.GetComponent<BoxCollider2D>();
                    if (bc.bounds.Contains(mouseStartPos))
                    {
                        selectedObject = go;
                        selectedObjectStartPos = selectedObject.transform.position;
                        break;
                    }
                }
            }
            //Mouse Held Event
            if (selectedObject != null)
            {
                selectedObject.transform.position = selectedObjectStartPos + (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStartPos);

                float x = selectedObject.transform.position.x;
                float y = selectedObject.transform.position.y;
                if (gridSnap)
                {
                    x = Mathf.Round(x * (1f/ gridPrecision)) / (1f / gridPrecision);
                    y = Mathf.Round(y * (1f / gridPrecision)) / (1f / gridPrecision);
                }

                selectedObject.transform.position = new Vector3(x, y, 0);
            }
            wasMouseDown = true;
        }
        else
        {
            //Mouse Down Event
            if (wasMouseDown) 
            {
                selectedObject = null;
            }
            wasMouseDown = false;
        }
    }
}

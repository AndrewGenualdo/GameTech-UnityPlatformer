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

    // Update is called once per frame
    void Update()
    {
        OnMouseActive();
        Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 10;
        if(Camera.main.orthographicSize < 0 )
        {
            Camera.main.orthographicSize = 0;
        }
    }

    [SerializeField] public bool wasMouseDown = false;
    [SerializeField] Vector3 mouseStartPos = Vector3.zero;
    [SerializeField] GameObject selectedObject = null;
    [SerializeField] Vector3 selectedObjectStartPos = Vector3.zero;
    [SerializeField] List<GameObject> barriers = new List<GameObject>();

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
                selectedObject.transform.position = new Vector3(selectedObject.transform.position.x, selectedObject.transform.position.y, 0);
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

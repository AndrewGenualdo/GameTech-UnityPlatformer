using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        OnMouseActive();
    }

    [SerializeField] public bool wasMouseDown = false;
    [SerializeField] Vector3 mouseStartPos = Vector3.zero;
    [SerializeField] GameObject selectedObject = null;
    [SerializeField] Vector3 selectedObjectStartPos = Vector3.zero;

    public static ObjectController INSTANCE;

    private void OnMouseDown()
    {
        


    }

    

    private void OnMouseActive()
    {
        if (Input.GetMouseButton(0))
        {
            //Mouse Down Event
            if (!wasMouseDown)
            {
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

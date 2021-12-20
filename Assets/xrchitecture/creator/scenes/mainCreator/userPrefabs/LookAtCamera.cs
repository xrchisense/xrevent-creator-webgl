using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private Sprite spritetoTurn;
    // Start is called before the first frame update
    void Start()
    {
        spritetoTurn = GetComponent<Sprite>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}

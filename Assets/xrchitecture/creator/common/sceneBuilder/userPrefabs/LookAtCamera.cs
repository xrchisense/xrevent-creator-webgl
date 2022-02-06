using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    private Sprite spritetoTurn;
    private Transform transformtoTurn;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Sprite>(out spritetoTurn);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}

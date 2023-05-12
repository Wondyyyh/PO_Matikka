using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    private CarRoad carRoad; //Reff
    public float Tpos;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        carRoad = FindObjectOfType<CarRoad>(); //Find reff
    }
    void Update()
    {        
        if (Tpos < 0.832f) // Max VALUE - 0.001
            Tpos += Time.deltaTime / speed;
        else Tpos = 0.0f;
        carRoad.MoveCar(Tpos, this.gameObject);
    }
}

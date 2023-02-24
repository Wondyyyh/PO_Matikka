using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPath : MonoBehaviour
{
    public GameObject A;
    public GameObject B;
    public GameObject C;
    public GameObject D;

    [Range(0f,1f)]
    public float T = 0.0f;

    private void OnDrawGizmos()
    {
        //P‰‰pisteet                //-
        Vector3 PtA= A.transform.position;
        Vector3 PtB= B.transform.position;
        Vector3 PtC= C.transform.position;
        Vector3 PtD= D.transform.position;
        //Viivat p‰‰pisteiden v‰lille
        Gizmos.color = Color.gray;
        Gizmos.DrawLine(PtA, PtB);
        Gizmos.DrawLine(PtB, PtC);
        Gizmos.DrawLine(PtC, PtD); //-


        //Lerp uudet pisteet viivastolle //--
        Vector3 PtX = (1 - T) * PtA + T * PtB;
        Vector3 PtY = (1 - T) * PtB + T * PtC;
        Vector3 PtZ = (1 - T) * PtC + T * PtD;
        //Viivojen piirto pisteiden v‰lille
        Gizmos.color = Color.black;
        Gizmos.DrawLine(PtX, PtY);
        Gizmos.DrawLine(PtY, PtZ);
        //Pallojen piirto pisteisiin
        Gizmos.color= Color.magenta;
        Gizmos.DrawSphere(PtX, 0.07f);
        Gizmos.DrawSphere(PtY, 0.07f);
        Gizmos.DrawSphere(PtZ, 0.07f); //--


        //Lerp uudet pisteet viivastolle //---
        Vector3 PtR = (1 - T) * PtX + T * PtY;
        Vector3 PtS = (1 - T) * PtY + T * PtZ;
        //Viivojen piirto pisteiden v‰lille
        Gizmos.color = Color.black;
        Gizmos.DrawLine(PtR, PtS);
        //Pallojen piirto pisteisiin
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(PtR, 0.05f);
        Gizmos.DrawSphere(PtS, 0.05F); //---


        //Lerp the BIG O
        Vector3 PtO = (1 - T) * PtR + T * PtS;
        Gizmos.color = Color.red; 
        Gizmos.DrawSphere(PtO, 0.07F); // piirret‰‰n the BIG O

    }


}

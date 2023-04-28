using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPoint : MonoBehaviour
{
    public Transform contorl0;
    public Transform contorl1;

    public Transform Anchor { get { return gameObject.transform; } }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(Anchor.position, contorl0.position);


        // Lasketaan keskipiste contorl0:n ja contorl1:n välillä
        Vector3 midpoint = Anchor.position;

        // Lasketaan vektori kontrollipisteiden välillä
        Vector3 vector = contorl1.position - contorl0.position;

        // Lasketaan vektorin pituus
        float length = vector.magnitude;

        // Muutetaan vektorin pituus, jotta control-pisteet ovat samalla etäisyydellä toisistaan
        float newLength = length / 2f;

        // Muutetaan vektorin suunta keskipisteestä control1:een
        Vector3 newVector = vector.normalized * newLength;

        // Asetetaan control1:n paikka keskipisteen ja uuden vektorin avulla
        contorl1.position = midpoint + newVector;

        // Asetetaan control0:n paikka keskipisteen ja uuden vektorin vastakkaisen suunnan avulla
        contorl0.position = midpoint - newVector;

        Gizmos.DrawLine(Anchor.position, contorl1.position);

    }
}

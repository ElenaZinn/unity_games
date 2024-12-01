using UnityEngine;
using System.Collections;

public class ReturnPlayer : MonoBehaviour
{

    public Transform ReturnPoint;


    public void OnTriggerEnter(Collider other)
    {

        other.transform.position = ReturnPoint.position;

    }
}
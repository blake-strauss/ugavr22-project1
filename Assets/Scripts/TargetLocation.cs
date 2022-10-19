using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using TMPro;

public class TargetLocation : MonoBehaviour
{
    [SerializeField] Color foundColor;
    [SerializeField] Color notFoundColor;
    [SerializeField] GameObject targetCollider;
    public SearchObject target;
    public bool isFound;
    
    // Start is called before the first frame update
    void Start()
    {
        targetCollider.GetComponent<Renderer>().material.color = notFoundColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        SearchObject so = rb.GetComponent<SearchObject>();
        if (so == null) return;
        if (so == target)
        {
            isFound = true;
            targetCollider.GetComponent<Renderer>().material.color = foundColor;
            other.transform.position = transform.position;
            other.transform.rotation = transform.rotation;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            so.pingAudio = false; //stop playing audio
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;
        SearchObject so = rb.GetComponent<SearchObject>();
        if (so == null) return;
        if(so == target)
        {
            isFound = false;
            targetCollider.GetComponent<Renderer>().material.color = notFoundColor;
        }
    }
}

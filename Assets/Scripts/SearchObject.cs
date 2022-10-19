using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SearchObject : MonoBehaviour
{
    [SerializeField] float soundInterval; //seconds
    public bool pingLocation = true;
    public AudioSource audioSource;
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > soundInterval && pingLocation)
        {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            timer = 0;
        }
    }
}

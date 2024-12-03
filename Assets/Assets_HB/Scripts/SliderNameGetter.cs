using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderNameGetter : MonoBehaviour
{
    public string parentName;
    public GameObject root;
    // Start is called before the first frame update
    private void Awake()
    {
        parentName = root.name;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnTriggerIn : MonoBehaviour
{
    private Explodable _explodable;
    private Vector3 explodePos;

    private void Awake()
    {
        _explodable = GetComponent<Explodable>();
        explodePos = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnExplode()
    {
        _explodable.explode();
        ExplosionForceInScene ef = GameObject.FindObjectOfType<ExplosionForceInScene>();
        ef.doExplosion(explodePos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "PlayerBody")
        {
            explodePos = other.transform.position;
            OnExplode();
        }
    }
}

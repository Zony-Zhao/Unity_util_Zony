using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace util
{
    public class AnimatorRandomParameter:MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Animator>().SetFloat("random",Random.value);
        }
    }
}
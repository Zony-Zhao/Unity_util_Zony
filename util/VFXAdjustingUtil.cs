using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace animals.scripts.util
{
    public static class VFXAdjustingUtil
    {
        public static List<ParticleSystem> getAllParticleSystems(this ParticleSystem parent)
        {
            return parent.GetComponentsInChildren<ParticleSystem>(true).ToList();
        }
    }
}
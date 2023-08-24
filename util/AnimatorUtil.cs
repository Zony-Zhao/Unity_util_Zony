using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace animals.scripts.util
{
    public static class AnimatorUtil
    {
        public static List<AnimatorControllerParameter> GetFloatParameters(this Animator animator)
        {
            var floatParameters = new List<AnimatorControllerParameter>();
            foreach (var parameter in animator.parameters)
                // Debug.Log("Parameter Name: " + parameter.name);
                if (parameter.type == AnimatorControllerParameterType.Bool)
                {
                    // Debug.Log("Default Bool: " + parameter.defaultBool);
                }
                else if (parameter.type == AnimatorControllerParameterType.Float)
                {
                    floatParameters.Add(parameter);
                }
                else if (parameter.type == AnimatorControllerParameterType.Int)
                {
                    // Debug.Log("Default Int: " + parameter.defaultInt);
                }

            return floatParameters;
        }

        public static bool CheckParameterExist(this Animator animator, int nameHash)
        {
            foreach (var parameter in animator.parameters)
                if (parameter.nameHash == nameHash)
                    return true;

            return false;
        }

        public static void SetAllFloatZero(this Animator animator)
        {
            animator.GetFloatParameters().ForEach(p => animator.SetFloat(p.nameHash, 0));
        }

        public static void SetAllFloatZero(this Animator animator, string exception)
        {
            animator.GetFloatParameters().Where(p => p.name != exception).ToList()
                .ForEach(p => animator.SetFloat(p.nameHash, 0));
        }
    }
}
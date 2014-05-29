using System.Collections;
using UnityEngine;

namespace UberIncludes
{
    public enum SHAPE { QUAD };
    public enum EMITION_SHAPE { 
    LINEAR_SINGLE_POINT, LINEAR_CIRCULAR_AREA, 
    FIREWORKS_C, FIREWORKS_C_RANDOM, 
    FIREWORKS_R, FIREWORKS_IGLU, 
    FIREWORKS_DISK };

    public static class UberValues
    {
        public const float CIRCULAR_RADIUS = 1f;
    }   

    public static class UberFunctions
    {
        internal static Vector3 genCircularPos(Vector3 position)
        {
            float xVar = Random.Range(position.x - UberValues.CIRCULAR_RADIUS, position.x + UberValues.CIRCULAR_RADIUS);
            float yVar = Random.Range(position.y - UberValues.CIRCULAR_RADIUS, position.y + UberValues.CIRCULAR_RADIUS);
            float zVar = Random.Range(position.z - UberValues.CIRCULAR_RADIUS, position.z + UberValues.CIRCULAR_RADIUS);
            return new Vector3(xVar, yVar, zVar);
        }

        internal static float randomSize(float _Size, float _MaxVariableSize)
        {
            return Random.Range(_Size, _MaxVariableSize);
        }

        internal static Vector3 eulerVelocity(Vector3 velocity, Vector3 aceleration)
        {
            return (velocity + aceleration * Time.deltaTime);
        }

        internal static Vector3 circularVelocity(float velocity, float theta, float phi)
        {
            Vector3 returnVal = new Vector3();
            returnVal.x = velocity * Mathf.Cos(theta) * Mathf.Sin(phi);
            returnVal.y = velocity * Mathf.Cos(phi);
            returnVal.z = velocity * Mathf.Sin(theta) * Mathf.Sin(phi);
            return returnVal;
        }
    }
}


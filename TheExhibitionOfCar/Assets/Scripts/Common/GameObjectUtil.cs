using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtil  {

    public static Vector3 MultiplyEachElement(this Vector3 sourceVector3, Vector3 targetVector3)
    {
        return new Vector3(sourceVector3.x*targetVector3.x,sourceVector3.y* targetVector3.y,sourceVector3.z* targetVector3.z);
    }
}

using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaManager : Singleton<AreaManager>
{
    [SerializeField] private Vector2[] path;
    public Vector2[] Path => path;
}

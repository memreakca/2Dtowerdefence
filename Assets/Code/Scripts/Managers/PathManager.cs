using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class PathManager : MonoBehaviour
{
    public static PathManager main;

    public Transform startPoint;
    public Transform[] path;
    public Tilemap pathTileMap;

    private void Awake()
    {
        main = this;
    }

}

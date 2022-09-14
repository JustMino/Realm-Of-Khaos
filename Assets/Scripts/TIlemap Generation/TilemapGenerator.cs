using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
  public int curheight;
  public GameObject spawn;
  public List<GameObject> tiles;
  public GameObject boss;
  private Transform TileParent;
    // Start is called before the first frame update
    void Start()
    {
      tiles = new List<GameObject>();
      Object[] gos = Resources.LoadAll("Tile Prefabs");
      foreach(GameObject go in gos)
      {
        tiles.Add(go);
      }
      TileParent = GameObject.Find("Generated").transform;
      GameObject tile;
      for (int i = 0; i < 5; i++)
      {
        tile = Instantiate(tiles[Random.Range(0, tiles.Count)], TileParent);
        tile.transform.position = new Vector3 (i*9*5 + TileParent.position.x, curheight*5, 0);
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

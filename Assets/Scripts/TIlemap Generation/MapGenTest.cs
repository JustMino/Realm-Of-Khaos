using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenTest : MonoBehaviour
{

  public TileBase[] tiles;
  public TileBase[] oneup;
  public TileBase[] onedown;
  public TileBase[] walltb;
  public Tilemap ground;
  public Tilemap grounddeco;
  public Tilemap walltm;
  int totaltiles = 0;
  public List<int> heights = new List<int>();
  GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
      gm = GameObject.Find("GameManager").GetComponent<GameManager>();
      ground = GameObject.Find("Ground").GetComponent<Tilemap>();
      grounddeco = GameObject.Find("Ground Deco").GetComponent<Tilemap>();
      walltm = GameObject.Find("Wall").GetComponent<Tilemap>();
      GroundGen();
      UnderFloorGen();
      WallGen();
      UnderGroundGen();
    }

    void GroundGen()
    {
      var position = new Vector3Int (0, 0, 0);
      for (int i = 0; i < Random.Range(gm.minheightchange, gm.maxheightchange); i++)
      {
        for (int o = 0; o < Random.Range(gm.mingroundlength, gm.maxgroundlength); o++)
        {
          // ground.SetTile(position, tiles[1]);
          grounddeco.SetTile(position, tiles[0]);
          position.x++;
          totaltiles++;
          heights.Add(position.y);
        }
        position.y = position.y + Random.Range(0,2)*2-1;
      }
    }

    void UnderFloorGen()
    {
      var position = new Vector3Int (0, 0, 0);
      for (int i = 0; i < totaltiles -1; i++)
      {
        int dif = heights[i + 1] - heights[i];
        if (dif == 0)
        {
          ground.SetTile(position, tiles[1]);
        }
        else if (dif == 1)
        {
          ground.SetTile(position, tiles[1]);
          position.x++;
          ground.SetTile(position, oneup[0]);
          position.y++;
          ground.SetTile(position, oneup[1]);
          i++;
        }
        else if (dif == -1)
        {
          ground.SetTile(position, onedown[0]);
          position.y--;
          ground.SetTile(position, onedown[1]);
        }
        position.x++;
      }
      ground.SetTile(position, tiles[1]);
    }

    void UnderGroundGen()
    {
      var position = new Vector3Int (gm.fillstartxpos, gm.fillstartypos, 0);
      for (int i = 0; i < totaltiles + Mathf.Abs(gm.fillstartxpos) + gm.extrawalls; i++)
      {
        if (i > 10)
        {
          while(ground.GetTile(position) == null && position.y <= gm.wallheight + heights[heights.Count - 1])
          {
            ground.SetTile(position, tiles[2]);
            position.y++;
          }
        }
        else
        {
          while(ground.GetTile(position) == null && position.y <= gm.wallheight)
          {
            ground.SetTile(position, tiles[2]);
            position.y++;
          }
        }
        position.x++;
        position.y = gm.fillstartypos;
      }
    }

    void WallGen()
    {
      var position = new Vector3Int (-1, 0, 0);
      ground.SetTile(position, walltb[4]);
      position.y++;
      for (int i = 0; i < gm.wallheight; i++)
      {
        walltm.SetTile(position, walltb[0]);
        ground.SetTile(position, walltb[2]);
        position.y++;
      }
      position = new Vector3Int (totaltiles, heights[heights.Count - 1], 0);
      ground.SetTile(position, walltb[5]);
      position.y++;
      for (int i = 0; i < gm.wallheight; i++)
      {
        walltm.SetTile(position, walltb[1]);
        ground.SetTile(position, walltb[3]);
        position.y++;
      }
    }
}

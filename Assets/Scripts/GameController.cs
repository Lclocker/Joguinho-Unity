using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

  public int totalScore;
  public Text scoreText;
  private int vidas = 0;
  private int kills = 0;

  public static GameController instance;
  // Start is called before the first frame update
  void Awake()
  {
    if (instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }

  }

  void Start()
  {
    UpdateVidasText();
    UpdateKillsText();
  }

  void Update()
  {

  }

  public void SetKills(int kill)
  {
    kills += kill;
    UpdateKillsText();
    if (kills % 5 == 0)
    {
      //spawner.spawn.SetSpawnRate(-1);
    }
  }

  public void SetVidas(int vida)
  {
    vidas += vida;
    if (vidas >= 0)
    {
      UpdateVidasText();
    }
  }

  public int GetVidas()
  {
    return vidas;
  }

  public void UpdateVidasText()
  {
    //scoreText.text = totalScore.ToString();
    //GameObject.Find("vidasText").GetComponent<Text>().text = vidas.ToString();
  }

  public void UpdateKillsText()
  {
    //scoreText.text = totalScore.ToString();
    GameObject.Find("killsText").GetComponent<Text>().text = kills.ToString();
  }
}

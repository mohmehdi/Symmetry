using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static int Level=0;                  //to fill pattern with its data >> and its static for refrence between Scenes
    #region UI controler
    public List<Button> levelBTNs;              //list of level buttons and each one set the (static Level)

    public GameObject loadSceneBarPanel;        //the loading bar gameobject >> enable it before loading scene
    public Slider loadsceneBar;                 //Loading bar

    private bool flag=false;                    //to decide Hide main or level_select >> change when (level select button) and (back to main button)
    public GameObject BTN_Panel;                //enable when flag = flase 
    public GameObject Level_Select_Panel;       //enable when flag = true 

    public Slider Score;                        //Score Bar
    private float percent;                      //filles Score Bar

    public Sprite[] DrawOrShow = new Sprite[2];//change LookDrawBTN_Image button image to draw or look
    public Image LookDrawBTN_Image;            

    public Text BestRecord;                     
    public Text ThisRecord;
    private int thisRecord=0;
    private int[] record;

    public GameObject rewardPanel;              //enable this panel after level finished
    private List<string> rewardMSG;             
    public Text rewardText;                     //show message from rewardMSG randomly
    #endregion 
    #region Audio                   
    public AudioSource CheckSound;              //this will be played after you finished level

    #endregion
    #region Gameobject controls
    public GameObject pattHider;                //a plane in level 20 that covers pattern   
    public MGrid pattern;                       //refrence to pattern
    public MGrid pad;                           //refrence to pad
    private GridData data;                      //data loads by SetDatalevel or by save file >> a size and Matrix 

    public Animator pathAnim;                   //anamator of cylender called path that is patterns parent  so pattern rotates when path rotate
    public GameObject path;
    public Animator camAnim;                    //to centerize camera when pattern rotates behind pad
    #endregion
    private void Awake()
    {
        data = new GridData();                  //make new data
        Input.backButtonLeavesApp = true;       //so back in android works
    }
    private void Start()
    {
        LoadLevel();                     //load pattern data and generate new grid 
        rewardMSG = new List<string> { "Bravo", "Amazing", "Perfect", "Good Job", "Awesome", "Astounding","Good job","Exelent"};
    }
    private void LockBTNS()             //active or deactive level selection buttons
    {
        BTN_Panel.SetActive(false);
        int L= SaveStatus.Load().level;   //get level and deactive all buttons grather than it             
        //Debug.Log(L);                                
        for (int i = 0; i < levelBTNs.Count; i++)      
        {                                              
            if (i+1>L)                                 
            {                                          
                levelBTNs[i].interactable=false;       
            }                                          
        }                                              
            Level_Select_Panel.SetActive(true);        
    }
    public void PanelLevel(bool flag)   //switch between main panel and level select panel
    {
        if (flag)
        {
            LockBTNS();
        }
        else
        {
            Level_Select_Panel.SetActive(false);
            BTN_Panel.SetActive(true);
        }
    }
    public void LoadScene(int level)    //level_select_buttons set the value and it will start loading bar
    {
        Level = level;
        loadSceneBarPanel.SetActive(true);
        StartCoroutine(LoadBar());
    }
    private IEnumerator LoadBar()       //do loading opperation and set Slider value
    {
        AsyncOperation opp = new AsyncOperation();
        //load level by index
        if (Level == 0)
        {
           opp=  SceneManager.LoadSceneAsync("Main");
        }
        else if (Level >0 || Level ==-2)
        {
             opp = SceneManager.LoadSceneAsync("Levels");
        }
        else if (Level == -1)
        {
             opp = SceneManager.LoadSceneAsync("DrawSave");
        }
        while (!opp.isDone)
        {
            float progress = Mathf.Clamp01(opp.progress / 0.9f);
            loadsceneBar.value = progress;
            yield return null;
        }
    }

    #region Loading level
    private void LoadLevel()
    {
        if (Level != 0 && Level != -1)
        {
            SetData_level();                    //first set the data
            LoadPattern();                      //load pattern by data we just have now

            pad.DeleteGrid();                 
            pad.GenerateGrid(data.size);         
            pad.GetComponent<MGrid>().enabled = false; //pad is not intractable first
            record = SaveStatus.Load().record;         //set record from save for best Score value
        }
        if (Level == 20)
        {
            pattHider.SetActive(true);                  //panel that should hide pattern
        }
    }           
    private void SetData_level()
    {
        if (Level == -1)
        {
            data.size = 5;
            //saved data from levelmake sceane
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == -2)
        {
            string path = Application.persistentDataPath + "/GridData.save";
            if (File.Exists(path))
                data = GridSaveControler.Load();
            else
            {
                data.size = 5;
                //saved data from levelmake sceane
                data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}
            };
            }
        }
        //..............................LEVELS DATA START FROM HERE...............//
        else if (Level == 1)
        {
            data.size = 5;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,4,0,0,0},
            {0,0,0,0,5,0,0,0,0,0},
            {0,0,0,0,0,0,5,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 2)
        {
            data.size = 5;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,3,0,0,0,0,5,0,0},
            {3,3,3,3,3,3,5,4,5,0},
            {0,0,3,0,0,0,0,5,0,0},
            {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 3)
        {
            data.size = 5;
            data.grid = new int[,] {
          {0,0,0,0,0,0,0,0,0,0},
          {0,0,0,0,1,1,1,0,0,0},
          {0,0,0,0,1,0,0,0,0,0},
          {0,1,1,1,1,1,1,0,0,0},
          {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 4)
        {
            data.size = 5;
            data.grid = new int[,] {
           {0,0,0,0,0,0,0,0,0,0},
           {0,0,5,4,4,4,2,0,0,0},
           {0,0,5,3,3,3,2,0,0,0},
           {0,0,5,4,4,4,2,0,0,0},
           {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 5)            //
        {
            data.size = 6;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,0,0,0},
            {0,0,0,0,0,6,6,6,1,0,0,0},
            {0,0,0,0,0,6,1,1,1,0,0,0},
            {0,0,0,0,0,6,6,6,6,6,6,6},
            {0,0,0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 6)        //
        {
            data.size = 6;
            data.grid = new int[,] {
            {0,0,0,0,0,0,5,5,5,0,0,0},
            {0,0,0,0,4,0,5,5,0,0,0,0},
            {0,0,0,0,4,4,5,2,2,2,0,0},
            {0,0,0,0,4,4,4,3,2,2,0,0},
            {0,0,0,0,0,0,3,3,0,2,0,0},
            {0,0,0,0,0,3,3,3,0,0,0,0}
            };
        }
        else if (Level == 7)        //
        {
            data.size = 7;
            data.grid = new int[,] {
            {1,1,3,3,3,1,1,1,1,1,1,1,1,1},
            {1,1,1,3,1,1,1,3,3,3,3,1,1,1},
            {1,1,1,3,1,1,1,3,3,3,3,1,1,1},
            {1,1,1,3,1,1,1,3,1,1,3,1,1,1},
            {1,1,1,3,3,1,3,3,1,1,3,1,1,1},
            {1,1,1,3,3,3,3,3,1,1,3,1,1,1},
            {1,1,1,1,1,1,1,1,1,3,3,3,1,1}
            };
        }
        else if (Level == 8)
        {
            data.size = 8;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,1,5,1,0,0,0,0},
            {0,0,1,1,1,0,0,0,1,5,5,1,0,0,0,0},
            {0,0,1,5,1,1,0,0,1,5,5,1,0,0,0,0},
            {0,0,1,5,5,1,1,1,5,5,1,1,0,0,0,0},
            {0,0,1,5,5,5,5,1,5,5,1,5,1,1,1,0},
            {0,0,1,1,5,5,5,5,5,5,5,5,5,5,5,1}
            };
        }
        else if (Level == 9)
        {
            data.size = 9;
            data.grid = new int[,] {
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1},
            {1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,1},
            {1,3,3,3,3,3,3,3,3,3,3,1,1,1,3,3,3,1},
            {1,3,3,3,3,3,3,3,3,3,3,1,1,1,3,3,3,1},
            {1,3,3,3,1,1,1,1,1,3,3,1,1,1,3,3,3,1},
            {1,3,3,3,1,1,1,1,1,3,3,1,1,1,3,3,3,1},
            {1,3,3,3,3,3,1,1,1,1,1,3,3,3,3,3,3,1},
            {1,3,3,3,3,3,1,1,1,1,1,3,3,3,3,3,3,1}
            };
        }
        else if (Level == 10)
        {
            data.size = 10;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,5,5,5,5,1,1,1,0,0,0,0,0,0,0},
            {0,0,1,1,5,5,5,5,5,5,1,5,5,1,1,0,0,0,0,0},
            {0,1,5,5,5,5,5,5,5,1,1,5,5,5,5,1,0,0,0,0},
            {0,1,5,5,5,5,5,5,1,1,1,5,5,5,5,5,1,0,0,0},
            {1,5,5,5,4,5,5,5,1,0,1,5,5,5,5,5,1,0,0,0},
            {1,5,5,5,4,5,5,1,0,0,1,5,5,5,5,5,5,1,0,0},
            {1,5,5,5,4,5,5,5,1,1,1,5,5,5,5,5,5,1,0,0}
            };
        }
        else if (Level == 11)
        {
            data.size = 7;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,4,4,4,4,4,2,2,2,2,2,0,0},
            {0,0,4,4,4,4,4,2,2,2,2,2,2,0},
            {5,0,2,2,2,2,2,2,0,0,0,0,1,2},
            {5,2,2,2,2,2,2,2,0,0,1,0,1,2},
            {5,2,2,2,2,2,2,2,0,0,0,0,2,2},
            {5,2,2,2,2,2,2,5,5,2,2,2,2,0}
            };
        }
        else if (Level == 12)
        {
            data.size = 7;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,1,1,1,1,1,0,0,0},
            {0,0,0,0,0,1,4,4,4,4,4,1,0,0},
            {0,0,0,0,1,4,4,4,4,4,4,4,1,0},
            {0,0,0,1,4,4,4,4,4,4,4,4,4,1},
            {0,0,1,4,4,4,4,4,4,4,4,4,4,1},
            {0,1,4,4,4,4,4,4,4,4,4,1,1,0}
            };
        }
        else if (Level == 13)
        {
            data.size = 10;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,1,4,0,1,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,5,1,4,4,1,5,1,1,0,0,0,0,0,0,0,0,0},
            {0,0,1,5,1,1,1,1,5,5,1,1,0,0,0,0,0,0,0,0},
            {0,0,1,5,5,5,5,5,5,5,5,5,1,1,0,0,0,0,0,0},
            {0,0,1,5,5,5,5,5,5,5,5,5,5,1,1,0,0,0,0,0}
            };
        }
        else if (Level == 14)
        {
            data.size = 9;
            data.grid = new int[,] {
            {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
            {2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2},
            {2,2,2,2,2,2,2,2,1,1,1,1,2,2,2,2,2,2},
            {2,2,7,0,2,2,2,1,1,1,1,1,1,1,2,2,2,2},
            {2,2,2,2,0,2,2,2,0,0,0,0,0,0,2,2,2,2},
            {0,2,2,2,2,0,2,0,0,0,0,7,7,0,0,2,2,2},
            {0,0,2,0,0,0,2,0,0,0,0,7,7,0,0,0,2,2},
            {0,0,0,0,0,0,4,0,0,1,1,0,0,0,0,0,2,2},
            {2,2,2,0,0,0,4,0,0,1,1,0,0,0,0,0,2,2}
            };
        }
        else if (Level == 15)
        {
            data.size = 15;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,3,3,3,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,3,3,3,3,1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,3,3,1,1,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0},
            {0,1,0,0,0,1,1,1,1,1,1,0,0,0,0,1,1,1,0,0,3,0,0,0,0,3,3,0,0,0},
            {0,1,1,1,1,5,5,1,0,0,1,0,0,0,0,1,1,1,0,3,3,3,3,3,0,3,0,0,0,0},
            {0,1,1,5,5,5,5,1,0,0,1,0,0,0,0,1,1,0,3,3,3,3,3,3,3,3,0,0,0,0},
            {0,1,1,5,5,5,5,1,0,1,1,0,0,0,0,0,1,0,3,3,4,3,3,1,3,3,3,0,0,0},
            {0,0,0,1,1,1,1,1,0,1,1,0,0,0,0,0,1,0,3,3,4,3,3,3,3,3,3,0,0,0}
            };
        }
        else if (Level == 16)
        {
            data.size = 15;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,1,1,1,5,5,5,5,5,5,5,1,1,1,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,1,5,5,5,5,5,5,5,5,5,5,5,5,5,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,5,5,5,5,5,5,1,1,1,1,5,5,5,5,5,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,5,5,5,5,5,5,1,1,1,1,1,1,1,5,5,5,5,1,0,0,0,0,0,0,0},
            {0,0,0,0,1,5,5,5,5,5,1,1,1,1,1,1,1,1,1,5,5,5,1,0,0,0,0,0,0,0},
            {0,0,0,1,5,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,5,5,5,1,0,0,0,0,0,0},
            {0,0,0,1,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,1,0,0,0,0,0,0},
            {0,0,1,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,5,5,1,0,0,0,0,0},
            {0,1,1,5,5,5,5,1,1,1,5,1,1,1,1,5,5,1,1,1,1,1,5,5,1,0,0,0,0,0},
            {0,1,5,5,5,5,1,1,1,5,5,1,1,1,1,5,5,5,5,1,1,1,1,5,5,1,0,0,0,0},
            {0,1,5,5,5,5,1,5,5,5,5,1,1,1,1,1,5,5,5,5,5,1,1,5,5,1,0,0,0,0},
            {0,1,5,5,5,5,5,5,5,5,5,1,1,1,1,1,1,5,5,5,5,5,5,5,5,1,0,0,0,0},
            {0,1,5,5,5,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,5,5,5,5,1,0,0,0,0},
            {0,1,5,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,5,5,5,5,5,5,1,0,0,0,0}
            };
        }
        else if (Level == 17)
        {
            data.size = 15;
            data.grid = new int[,] {
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,1,7,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,2,2,1,7,7,7,7,7,7,7},
            {7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,1,1,7,7,1,2,2,2,1,7,7,7,7,7,7},
            {7,7,7,1,1,1,1,1,1,1,7,7,7,7,1,4,4,1,1,1,2,2,2,2,1,7,7,7,7,7},
            {7,7,1,4,4,4,4,4,4,4,1,7,7,1,4,4,4,4,4,4,1,2,2,4,1,7,7,7,7,7},
            {7,1,4,4,4,1,1,1,1,4,4,1,7,1,4,4,1,2,4,4,4,1,4,4,4,1,7,7,7,7},
            {7,1,4,4,4,4,1,0,0,1,4,4,1,4,4,4,2,1,2,4,4,1,1,4,4,4,1,7,7,7},
            {1,4,4,4,4,4,4,1,0,1,4,4,1,4,4,4,4,4,1,4,4,1,2,1,4,4,4,1,7,7},
            {1,4,4,4,4,4,4,4,1,4,4,4,4,1,4,4,4,4,4,1,1,0,0,1,4,4,4,4,1,7},
            {1,4,4,4,4,4,4,4,4,4,4,4,4,1,4,4,4,4,4,4,0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 18)
        {
            data.size = 15;
            data.grid = new int[,] {
           {5,5,5,7,7,5,5,5,7,7,7,7,5,5,5,5,7,7,1,4,4,4,4,4,4,4,7,4,4,4},
            {7,7,5,7,7,5,7,5,7,7,7,7,5,7,7,5,7,7,1,7,7,7,7,7,7,4,7,4,7,4},
            {7,7,5,7,7,5,7,5,7,5,5,5,5,7,7,5,7,7,1,4,4,4,4,4,4,4,7,4,7,4},
            {7,7,5,5,5,5,7,5,5,5,7,7,7,7,7,5,5,5,1,4,7,7,7,7,7,7,7,4,7,4},
            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,4,4,4,4,4,4,4,7,4,7,4},
            {7,7,7,7,7,7,7,7,7,2,2,2,2,2,2,2,1,5,1,7,7,7,7,7,7,4,7,4,7,4},
            {7,7,7,7,7,7,7,7,7,2,1,3,1,1,1,2,1,5,1,4,4,4,4,4,4,4,7,4,7,4},
            {7,2,2,2,2,2,2,2,7,2,1,3,1,2,1,2,1,5,1,4,7,7,7,7,7,7,7,4,7,4},
            {7,2,7,7,7,7,7,2,7,2,1,3,1,2,2,2,1,5,1,4,4,4,4,4,4,4,4,4,7,4},
            {7,2,7,2,2,2,7,2,7,2,1,3,1,1,1,1,1,5,1,7,7,7,7,7,7,7,7,7,7,4},
            {7,2,7,2,7,2,7,2,7,2,1,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4},
            {7,2,7,2,7,7,7,2,7,2,1,3,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {7,2,7,2,2,2,2,2,7,2,1,3,3,7,3,3,3,3,3,7,3,3,3,7,3,3,7,3,7,3},
            {7,2,7,7,7,7,7,7,7,2,1,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3},
            {7,2,2,2,2,2,2,2,2,2,1,3,3,7,3,3,3,3,3,7,3,3,3,7,3,3,7,3,7,3}
            };
        }
        else if (Level == 19)
        {
            data.size = 15;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,1,5,5,5,5,5,1,1,5,5,5,5,5,1,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,2,2,2,1,5,1,2,2,2,2,2,2,2,2,1,5,1,2,2,2,1,0,0,0,0},
            {0,0,0,0,1,2,1,1,1,5,1,1,1,1,1,1,1,1,1,1,5,1,1,1,2,1,0,0,0,0},
            {0,0,0,0,1,2,1,0,1,5,1,0,1,5,1,1,5,1,0,1,5,1,0,1,2,1,0,0,0,0},
            {0,0,0,0,1,2,1,0,1,5,1,0,1,5,1,1,5,1,0,1,5,1,0,1,2,1,0,0,0,0},
            {0,0,0,0,1,2,1,0,1,5,1,0,1,5,1,1,5,1,0,1,5,1,0,1,2,1,0,0,0,0},
            {1,1,1,1,1,2,1,1,1,1,1,1,1,5,1,1,5,1,1,1,1,1,1,1,2,1,1,1,1,1},
            {1,4,4,4,1,2,1,4,4,4,4,4,1,5,1,1,5,1,4,4,4,4,4,1,2,1,4,4,4,1},
            {1,4,1,1,1,2,1,1,1,1,1,4,1,5,1,1,5,1,4,1,1,1,1,1,2,1,1,1,4,1},
            {1,4,1,0,1,2,1,0,1,1,1,1,1,5,1,1,5,1,4,1,5,1,0,1,2,1,0,1,4,1},
            {1,4,1,0,1,2,1,0,1,5,5,5,5,5,1,1,5,5,5,5,5,1,0,1,2,1,0,1,4,1},
            {1,4,1,0,1,2,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,2,1,0,1,4,1},
            {1,4,1,0,1,2,1,0,0,0,1,4,1,0,0,0,0,1,4,1,0,0,0,1,2,1,0,1,4,1}
            };
        }
        else if (Level == 20)
        {
            data.size = 15;
            data.grid = new int[,] {
           {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,0,2,0,0,4,0,0,1,0,4,0,4,4,4,0,1,0,0,5,0,0,1,0,0,4,4,0,1},
            {1,0,0,0,1,5,5,0,0,1,0,4,0,1,0,4,0,1,0,0,1,5,0,1,0,0,1,4,0,1},
            {1,0,0,0,2,0,3,0,0,1,0,4,4,4,0,4,0,1,0,0,3,0,0,1,0,0,4,0,0,1},
            {1,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,0,4,5,3,0,0,0,1,0,0,4,4,4,0,0,1,0,0,5,0,0,1,0,0,4,0,0,1},
            {1,0,0,0,5,0,0,0,0,1,0,0,4,0,0,0,0,1,0,5,1,3,0,1,0,4,1,4,0,1},
            {1,0,0,0,1,2,0,0,0,1,0,0,4,1,4,0,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,0,2,0,0,0,0,0,1,0,0,0,0,4,0,0,1,0,5,3,0,0,1,0,0,4,0,0,1},
            {1,0,0,0,0,0,0,0,0,1,0,0,4,4,4,0,0,1,0,5,1,0,0,1,0,4,1,0,0,1},
            {1,0,3,0,2,0,0,0,0,1,0,0,0,0,0,0,0,1,0,0,5,0,0,1,0,0,4,0,0,1},
            {1,0,5,5,1,0,0,0,0,1,0,4,0,4,4,4,0,1,0,0,0,0,0,1,0,0,0,0,0,1},
            {1,0,4,0,0,2,0,0,0,1,0,4,0,1,0,4,0,1,0,3,1,5,0,1,0,4,1,4,0,1},
            {1,0,0,0,0,0,0,0,0,1,0,4,4,4,0,4,0,1,0,0,5,0,0,1,0,0,4,0,0,1}
            };
        }
    }
    private void LoadPattern()
    {
        pattern.DeleteGrid();
        pattern.GenerateGrid(data.size);

        for (int i = 0; i < data.size; i++)
        {
            for (int j = 0; j < data.size* 2; j++)
            {
                if (data.grid[i, j] == 0)
                {
                    data.grid[i, j] = 0;
                }
                else
                {
                    pattern.Set_Mat_ID(data.grid[i, j]);
                    pattern.SetTileMatColor();
                    Vector3 temp;
                    temp = new Vector3(i * pattern.cell_Size.x + pattern.cell_Size.x / 2, j * pattern.cell_Size.y + pattern.cell_Size.y / 2, pattern.transform.position.z);
                    temp = new Vector3(temp.x - pattern.transform.localScale.x / 2, temp.y - pattern.transform.localScale.y / 2);
                    temp += pattern.transform.position;
                    GameObject go = Instantiate(pattern.TilePrefab, temp, Quaternion.identity);

                    go.transform.localScale = pattern.cell_Size;
                    go.transform.SetParent(pattern.transform);
                }
            }
        }
        if (Level != -1)
        {
            pattern.enabled = false;
            pattern.transform.SetParent(path.transform);
        }
    }
    #endregion
    
    public void SetPadActive()
    {
        flag = !flag;
        pathAnim.SetBool("Look", flag);
        camAnim.SetBool("Look", flag);
        if (flag)
        {
            LookDrawBTN_Image.sprite = DrawOrShow[0];
        }
        else
        {
            thisRecord++;

            LookDrawBTN_Image.sprite = DrawOrShow[1];

        }
        pad.GetComponent<MGrid>().enabled = flag;

    }
    public void Check()
    {
        int count = 0;
        for (int i = 0; i < data.size; i++)
        {
            for (int j = 0; j < data.size*2; j++)
            {
                if (data.grid[i,j] == pad.Tiles[data.size -i -1,j])
                {
                    count++;
                }
            }
        }
        float p = (float) count / (data.size * (data.size*2) ) ;
        percent = p;
        if (percent == 1)
        {
           

            System.Random rand = new System.Random();
            rewardPanel.SetActive(true);
            rewardText.text = rewardMSG[rand.Next(rewardMSG.Count)];
            if (thisRecord<record[Level] || record[Level]==-1)
            {
                record[Level] = thisRecord;
                int temp = SaveStatus.Load().level;
                if (temp<=Level)
                {
                    SaveStatus.Save(record,Level+1);
                }
                else
                {
                    SaveStatus.Save(record, temp);
                }
            }
            CheckSound.Play();
        }
        else
        {
            //adad.ShowClosableVideoAd();
        }
        StartCoroutine( scoreBar());
    }
    IEnumerator scoreBar()
    {
       if (Level != -2)
        if (record[Level]!=-1)
        {
                BestRecord.text ="Best :"+ record[Level].ToString();
        }
        ThisRecord.text ="Your Score :"+ thisRecord.ToString();
        while (Mathf.Abs( percent - Score.value) >0.01f)
        {
            if (percent > Score.value)
            {
                Score.value += 0.02f;
            }
            else
            {
                Score.value -= 0.02f;
            }
            yield return null;
        }
    }
    
     public  void Quit()
    {
         Application.Quit();
    }
}
public static class SaveStatus
{
    public static void Save(int[] record,int level)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerStatus.save";

            FileStream stream = new FileStream(path, FileMode.Create);


        PlayerStatus data = new PlayerStatus
        {
            record = record,
            level = level
        };
        ;
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static PlayerStatus Load()
    {
        string path = Application.persistentDataPath + "/PlayerStatus.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerStatus data = formatter.Deserialize(stream) as PlayerStatus;
            stream.Close();

            return data;
        }
        else
        {
           // Debug.LogError("Save not found");
            int[] tmp = new int[50];
            for (int i = 0; i < 10; i++)
            {
                tmp[i] = -1;
            }
            Save(tmp,1);
            return new PlayerStatus {record=tmp};
        }
    }
}
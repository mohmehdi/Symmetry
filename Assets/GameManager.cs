using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int Level=0;
    public MGrid pattern;
    public MGrid pad;

     public  Animator corner1;
     public  Animator corner2;
    public GameObject BTN_Panel;
    public GameObject Level_Select_Panel;

    public GameObject path;

    private GridData data;

    //ui elements
    private void Awake()
    {
        data = new GridData();
    }
    private void Start()
    {
        LoadLevel();
    }
    private void Delay()
    {
            BTN_Panel.SetActive(false);
            Level_Select_Panel.SetActive(true);
    }
    public void PanelLevel(bool flag)
    {
        if (flag)
        {
            Invoke("Delay", 0.5f);
            
             corner1.SetBool("Small", false);
             corner2.SetBool("Small", false);
        }
        else
        {
            Level_Select_Panel.SetActive(false);
            BTN_Panel.SetActive(true);
            corner1.SetBool("Small", true);
            corner2.SetBool("Small", true);
        }
    }
    public void LoadScene(int level)
    {
        Level = level;

        Invoke("loadScene",1);
    }
    private void loadScene()
    {
        //load level by index
        if (Level == 0)
        {
            SceneManager.LoadSceneAsync("Main");
        }
        else if (Level >= 1 && Level <= 5)
        {
            SceneManager.LoadSceneAsync("Levels");
        }
        else if (Level == -1)
        {
            SceneManager.LoadSceneAsync("DrawSave");
        }
    } 
    private void LoadLevel()
    {
        if (Level != 0)
        {
            SetData_level(Level);
            LoadPattern();
        }
    }
    private void SetData_level(int l)
    {
        if (Level == -1)            //DrawSave scene
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

            //saved data from levelmake sceane
       else if (Level==1)
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
            {1,1,1,2,3,0,0,0,0,0},
            {0,0,0,0,0,0,4,0,0,0},
            {0,0,0,0,5,0,0,0,0,0},
            {0,0,0,0,0,0,5,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}
            };
        }
        else if (Level == 3)
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
        else if (Level == 4)
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
        else if (Level == 5)
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
        levelSpecial();
    }
    private void levelSpecial()
    {
        if (Level != -1)
        {
            pattern.enabled = false;
        }
    }
    public void Check()
    {
        AnimatePlane();
        pad.enabled = false;
        int count = 0;
        for (int i = 0; i < data.size; i++)
        {
            for (int j = 0; j < data.size*2; j++)
            {
                if (data.grid[i,j] == pad.Tiles[i,j])
                {
                    count++;
                }
            }
        }
        float percent = (float) count / (data.size * (data.size*2) ) ;
        Debug.Log(percent);
    }
    private void AnimatePlane()
    {
        pad.gameObject.transform.SetParent(path.transform);
    }
}

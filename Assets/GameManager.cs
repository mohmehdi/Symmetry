using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManager : MonoBehaviour
{
    public static int Level=0;
    public MGrid pattern;
    public MGrid pad;
    private bool flag=false;
    public Slider Score;
    public Text DrawOrShow;
    public GameObject BTN_Panel;
    public GameObject Level_Select_Panel;
    public Text BestRecord;
    public Text ThisRecord;
    private int thisRecord=0;
    private int[] record;
    public GameObject path;

    private GridData data;
    private float percent;
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

        }
        else
        {
            Level_Select_Panel.SetActive(false);
            BTN_Panel.SetActive(true);
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

    #region Loading level
    private void LoadLevel()
    {
        if (Level != 0)
        {
            SetData_level(Level);
            LoadPattern();
        }
        if (Level <= 5 && Level >= 1)
        {
            pad.GetComponent<MGrid>().enabled = false;
            record = SaveStatus.Load().record;
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
        if (Level != -1)
        {
            pattern.enabled = false;
            pattern.transform.SetParent(path.transform);
        }
    }
    #endregion
    
    public void SetPadActive()
    {
        //Debug.Log(LookCounter);
       // SaveStatus.Save(record);
        flag = !flag;
        if (flag)
        {
            DrawOrShow.text = "Look";
        }
        else
        {
            thisRecord++;

            DrawOrShow.text = "Draw";
        }
            pad.GetComponent<MGrid>().enabled = flag;
            pattern.gameObject.SetActive(!flag);
    }
    public void Check()
    {
        // pad.enabled = false;

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
            if (thisRecord<record[Level])
            {
            record[Level] = thisRecord;
            }
        }
        StartCoroutine( scoreBar());
    }
    IEnumerator scoreBar()
    {
        BestRecord.text =record[Level].ToString();
        ThisRecord.text = thisRecord.ToString();
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

}
public static class SaveStatus
{
    public static void Save(int[] record)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/PlayerStatus.save";

            FileStream stream = new FileStream(path, FileMode.Create);


        PlayerStatus data = new PlayerStatus
        {
            record =record
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
            Debug.LogError("Save not found");
            return new PlayerStatus {record=new int[5] };
        }
    }
}
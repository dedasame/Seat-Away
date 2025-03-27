using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject selectedSeat;
    public GameObject selectedGrid;
    public GameObject[] levels;
    public Color[] colors; //seat colors
    public Color unmovableColor;
    public GameObject seatPrefab;
    public GameObject personPrefab;
    public bool isChecked;
    public int selectedPersonIndex; //hedef person index
    public bool levelCompleted;
    public bool levelFailed;
    public GameObject completedScreen;
    //public GameObject failedScreen;
    public GameObject settingScreen;
    public bool checkPersonPos;
    public Color gridColor0; //dark
    public Color gridColor1; //light
    public TextMeshProUGUI levelText; //basangicta leveli yazdir
    public AudioSource sound;
    /*
    public TextMeshProUGUI timeText; //sure icin fonksiyonda kullanilacak
    public float timer; //sure icin
    */
    public static GameManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        //PlayerPrefs.SetInt("level",0);
        //PlayerPrefs.SetInt("levelIndex",1);

        if(!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level",0);
        }
        if(!PlayerPrefs.HasKey("levelIndex"))
        {
            PlayerPrefs.SetInt("levelIndex",1);
        }
        if(PlayerPrefs.GetInt("level") > levels.Length-1) PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level")%levels.Length+1);
        levels[PlayerPrefs.GetInt("level")].SetActive(true);
        //CheckGrids();
        levelText.text = "LEVEL " + PlayerPrefs.GetInt("levelIndex").ToString();
        //timer = levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().timeLimit;
        if(PlayerPrefs.GetInt("sound") == 0) AudioListener.pause = true;
        else AudioListener.pause = false;
    }
    void Update()
    {
        if(!levelCompleted && isChecked) TakeSeatAndMove();
        if(levelCompleted && completedScreen.activeSelf)
        {
            if(Input.GetMouseButtonDown(0))
            {
                NextLevel();
            }
        }
        if(!levelCompleted && levelFailed)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RestartLevel();
            }
        }
        if(!levelCompleted && checkPersonPos && !isChecked)
        {
            if(levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex].GetComponent<PersonScript>().path.Count == 0)
            {
                checkPersonPos = false;
                isChecked = true;
            }
        }
        /*
        if(timer > 0 && !levelCompleted)
        {
            timer = timer - (Time.deltaTime* 0.1f);
            timeText.text = timer.ToString();
        }
        else if(timer <=0 && !levelCompleted)
        {
            failedScreen.SetActive(true);
            Time.timeScale = 0;
        }
        */
    }
   public void TakeSeatAndMove()
   {
        if(Input.GetMouseButtonDown(0) && isChecked) //Seat seçimi
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,1<<7)) //Seat seçimi
            {    
                if(hit.collider.tag == "Seat" && hit.collider.gameObject.GetComponent<SeatScript>().canMove)
                {
                    selectedSeat = hit.collider.gameObject;
                }
            }
        }
        if(Input.GetMouseButton(0) && selectedSeat) //Seat hareketi
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit,100f,1<<6)) //Grid seçimi
            {
                if(hit.collider.tag == "Grid" && hit.collider.gameObject.GetComponent<GridScript>().seat == null)
                {
                    selectedGrid = hit.collider.gameObject;
                    if(SeatSearch.instance.GeneratePath(selectedSeat.GetComponent<SeatScript>().grid,selectedGrid.GetComponent<GridScript>())!=null )
                    {
                        selectedSeat.transform.position = UnityEngine.Vector3.MoveTowards(selectedSeat.transform.position, selectedGrid.transform.position, 1f);
                    }
                    else
                    {
                        selectedGrid = null;
                    }
                }        
            }
        }
        if(Input.GetMouseButtonUp(0)) //Seat birakma ve gride yerlestirme
        {
            if(selectedGrid)
            {
                selectedSeat.GetComponent<SeatScript>().grid.seat = null;
                selectedSeat.transform.position = selectedGrid.transform.position;
                selectedGrid.GetComponent<GridScript>().seat = selectedSeat;
                selectedSeat.GetComponent<SeatScript>().grid = selectedGrid.GetComponent<GridScript>();
            }
            selectedSeat = null;
            selectedGrid = null;
            isChecked = false; //hamle yapma izni kapatildi
            CheckGrids(); //Grid Bulma
        }
   }
   public void CheckGrids()
   {
        GridScript targetGrid = GridSearch.Instance.FindGrid(levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().startGrid,
            levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex].GetComponent<PersonScript>());
        if(targetGrid) //bir grid var ise
        {
            //path olustur a* ile
            List<GridScript> path = AStarPathFinding.instance.GeneratePath(levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().startGrid,targetGrid);
            //person path'i ata
            levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex].GetComponent<PersonScript>().path = path;
            //seat'e person ata
            targetGrid.seat.GetComponent<SeatScript>().person = levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex].GetComponent<PersonScript>();
            levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex].transform.parent = targetGrid.seat.transform;
            selectedPersonIndex++; //->> listedeki personlarin pozisyonlari ayarlansın!!
            for(int i = 0 ; i< 10; i++)
            {
                if(selectedPersonIndex + i < levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons.Count)
                {
                    levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons[selectedPersonIndex+i].GetComponent<PersonScript>().pos= 
                        levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().positions[i];
                }
            }
            if(selectedPersonIndex < levels[PlayerPrefs.GetInt("level")].GetComponent<LevelScript>().persons.Count)
            {
                Invoke("CheckGrids", .3f); //sonraki person için tekrar kontrol et
                //CheckGrids();
            }
            else
            {
                levelCompleted = true;
                Invoke("LevelComplete",1f);
            }
        }
        else
        {
            //herhangi bir grid bulunamazsa direkt hamle yapma izni açılacak
            if(selectedPersonIndex == 0) isChecked = true;
            else checkPersonPos = true;
        }
   }
   public void NextLevel()
   {
        PlayerPrefs.SetInt("level",PlayerPrefs.GetInt("level")+1);
        PlayerPrefs.SetInt("levelIndex",PlayerPrefs.GetInt("levelIndex")+1);
        SceneManager.LoadScene(this.gameObject.scene.buildIndex);
   }
   public void LevelComplete()
   {
        completedScreen.SetActive(true);
   }

    //Buttons
   public void RestartLevel()
   {
        SceneManager.LoadScene(this.gameObject.scene.buildIndex);
   }
   public void SettingButton() //ayarlar icin canvas ac
   {
        settingScreen.SetActive(true);
   }
   public void CloseSettingMenuButton()
   {
        settingScreen.SetActive(false);
   }
   public void SetSound()
   {
        if(PlayerPrefs.GetInt("sound") == 0)
        {
            PlayerPrefs.SetInt("sound",1);
            AudioListener.pause = false;
        }
        else
        {
            PlayerPrefs.SetInt("sound",0);
            AudioListener.pause = true;
        }
   }
   public void MainMenuButton()
   {
        SceneManager.LoadScene(0);
   }
}

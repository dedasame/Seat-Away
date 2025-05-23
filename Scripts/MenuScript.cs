using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public GameObject person;
    public GameObject seat;
    public Color[] colors;
    void Start()
    {
        SetColor(Random.Range(0, 10));
        //Level ?? -> menu
        //PlayerPrefs.GetInt("levelIndex");
    }
    public void SetColor(int index)
    {
        person.GetComponentInChildren<Renderer>().material.color = colors[index] *1.2f;
        seat.GetComponent<SeatScript>().seat.GetComponent<Renderer>().material.color = colors[index];
    }
    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
}
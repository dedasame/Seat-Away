using System.Collections.Generic;
using UnityEngine;

public class LevelScript : MonoBehaviour
{
    public int[] personList; //renk sirasi
    public Vector3[] positions; //ilk 3 + 1
    public List<GameObject> persons; //yolcular
    public GridScript startGrid; //baslangic gridi
    public float timeLimit; //sure siniri
    void Start()
    {
        CreatePersons();
    }
    public void CreatePersons()
    {
        for(int i = 0; i < personList.Length; i++)
        {
            GameObject person;
            if(i<10)
            {
                person = Instantiate(GameManager.Instance.personPrefab, positions[i], Quaternion.identity);
            }
            else 
            {
                person = Instantiate(GameManager.Instance.personPrefab, positions[9], Quaternion.identity);
            }
            person.GetComponent<PersonScript>().color = personList[i];
            persons.Add(person);
            person.GetComponent<PersonScript>().SetColor(personList[i]);
        }
    }
}
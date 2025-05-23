using UnityEngine;
public class SeatScript : MonoBehaviour
{
    public int seatMaxPerson;
    public bool canMove; //can seat move
    public GameObject seat; //seat object
    public int color; //seat color
    public GridScript grid; //seat grid
    public PersonScript person; //person on the seat
    public void SetColor()
    {
        if(!canMove) seat.GetComponent<Renderer>().material.color = GameManager.Instance.unmovableColor;
        else seat.GetComponent<Renderer>().material.color = GameManager.Instance.colors[color];
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
        if(other.tag == "Grid")
        {
            grid = other.GetComponent<GridScript>();
            grid.seat = gameObject;
        }
    }
}
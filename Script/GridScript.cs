using UnityEngine;

public class GridScript : MonoBehaviour
{
    public bool createSeat;
    public int seatColor;
    public bool canMove;


    public GridScript[] neighbours; //komsular
    public GameObject seat;


    public bool gridColor; //false -> 0 true -> 1

    //A* 
    public float gScore,hScore;
    public GridScript cameFrom;
    public float FScore()
    {
        return gScore + hScore;
    }


    void Start()
    {
        FindNeighbours();
        if(createSeat) CreateSeat();
        if(gridColor) GetComponentInChildren<SpriteRenderer>().color = GameManager.Instance.gridColor1;
        else GetComponentInChildren<SpriteRenderer>().color = GameManager.Instance.gridColor0;
    }

    public void CreateSeat()
    {
        seat = Instantiate(GameManager.Instance.seatPrefab, transform.position, Quaternion.identity);
        seat.GetComponent<SeatScript>().grid = this;
        seat.GetComponent<SeatScript>().canMove = canMove;
        seat.GetComponent<SeatScript>().color = seatColor;
        seat.GetComponent<SeatScript>().SetColor();
    }
    public void FindNeighbours()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.up,out hit, 1f, 1 << 6)) neighbours[0] = hit.collider.gameObject.GetComponent<GridScript>();
        if(Physics.Raycast(transform.position, Vector3.right,out hit, 1f, 1 << 6)) neighbours[1] = hit.collider.gameObject.GetComponent<GridScript>();
        if(Physics.Raycast(transform.position, Vector3.down,out hit, 1f, 1 << 6)) neighbours[2] = hit.collider.gameObject.GetComponent<GridScript>();
        if(Physics.Raycast(transform.position, Vector3.left,out hit, 1f, 1 << 6)) neighbours[3] = hit.collider.gameObject.GetComponent<GridScript>();
    }






}

using System.Collections.Generic;
using UnityEngine;

public class GridSearch : MonoBehaviour
{
    public static GridSearch Instance;
    private void Awake()
    {
        Instance = this;
    }
    public GridScript FindGrid(GridScript startCell, PersonScript person)
    {
        Queue<GridScript> queue = new Queue<GridScript>();
        HashSet<GridScript> visited = new HashSet<GridScript>();
        queue.Enqueue(startCell); //baslangici kuyruga ekler
        visited.Add(startCell); //ziyaret edilen
        while (queue.Count > 0)
        {
            GridScript current = queue.Dequeue();
            if(current.seat != null && !current.seat.GetComponent<SeatScript>().person && current.seat.GetComponent<SeatScript>().color == person.color)
            {
                if(AStarPathFinding.instance.GeneratePath(startCell,current) != null) //ulasilabilir ise
                {
                    return current; // Aranan hücre bulundu
                }
            }
            if(current.seat == null) //koltuk yok ise komsulari ekler
            {
                foreach (GridScript neighbor in current.neighbours)
                {
                    if(neighbor && !visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor); //kuyruga ekler
                        visited.Add(neighbor);
                    }   
                }
            }
        }
        return null; // Aranan hücre bulunamadı
    }
}



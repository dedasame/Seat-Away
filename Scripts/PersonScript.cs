using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public int color;
    public List<GridScript> path; 
    public Vector3 pos;
    public void SetColor(int color)
    {
        GetComponentInChildren<Renderer>().material.color = GameManager.Instance.colors[color];
        GetComponentInChildren<Renderer>().material.color *= 1.2f;
    }
    void Update()
    {
        if(path != null)
        {
            if(path.Count > 0)
            {
                transform.GetComponentInChildren<Animator>().SetTrigger("run");
                transform.position = Vector3.MoveTowards(transform.position, path[0].transform.position, 7.5f * Time.deltaTime);
                if(Vector3.Distance(transform.position,path[0].transform.position) < 0.01f)
                {
                    transform.GetComponentInChildren<Animator>().SetTrigger("sit");
                    GameManager.Instance.sound.Play();
                    path.RemoveAt(0);
                }
            }
        }
        if(pos != Vector3.zero)
        {
            transform.GetComponentInChildren<Animator>().SetTrigger("run");
            transform.position = Vector3.MoveTowards(transform.position, pos, 7.5f * Time.deltaTime);
            if(Vector3.Distance(transform.position,pos) < 0.01f)
            {
                transform.GetComponentInChildren<Animator>().SetTrigger("idle");
                pos = Vector3.zero;
            }
        }
    }
}
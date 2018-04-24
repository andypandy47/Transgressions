using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public int ID { get; set; }
    public string LevelName { get; set; }
    public bool Completed { get; set; }
    public string Rank { get; set; }
    public bool Locked { get; set; }

    public Level(int id, string levelName, bool completed, string rank, bool locked)
    {
        this.ID = id;
        this.LevelName = levelName;
        this.Completed = completed;
        this.Rank = rank;
        this.Locked = locked;
    }

    public void Complete()
    {
        this.Completed = true;
    }

    public void Complete(string rank)
    {
        this.Completed = true;
        this.Rank = rank;
    }

    public void Lock()
    {
        this.Locked = true;
    }

    public void Unlock()
    {
        this.Locked = false;
    }
}

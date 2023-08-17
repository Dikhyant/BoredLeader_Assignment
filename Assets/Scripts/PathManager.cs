using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour, IPathProvider
{
    List<Path> paths;
    Path chosenPath;

    void Awake() {
        AssignIdToPaths();
        chosenPath = paths[0];
    }

    void AssignIdToPaths() {
        if(paths == null) return;
        if(paths.Count == 0) return;
        for(int i = 0; i < paths.Count; i++) {
            paths[i].Id = i;
        }
    }

    public Path GetChosenPath()
    {
        if(paths.Count == 0) return null;
        
        return chosenPath;
    }
}

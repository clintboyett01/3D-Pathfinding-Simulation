using System.Collections.Generic;
using UnityEngine;

public class Node: MonoBehaviour {
    public List < Node > adjacentNodes;
    public List < Edge > edges;
    public int index;
}
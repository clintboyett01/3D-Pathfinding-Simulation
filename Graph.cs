using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

class Graph: MonoBehaviour {
    private int size;
    private Node[] nodes;
    private Edge[, ] edges;
    private int graphIndex = 0;
    public Material Selected;
    public Material Path;
    public Camera camera;
    int start = 0;
    int end = 0;
    int count = 0;
    AnimationHandler animationHandler;
    private readonly int NO_PARENT = -1;
    private int[] parents;

    public Graph(int size) {
        this.size = size;
        edges = new Edge[size, size];
        nodes = new Node[size];

        GraphGenerator graphGenerator = FindObjectOfType < GraphGenerator > ();
        Selected = graphGenerator.Selected;

        Path = graphGenerator.Path;
        animationHandler = FindObjectOfType < AnimationHandler > ();

        parents = new int[size];

        parents[start] = NO_PARENT;
    }

    public void AddNode(Node node) {
        nodes[graphIndex] = node;
        node.index = graphIndex;
        graphIndex++;
    }

    public void ConnectNodesFully(Node n1, Node n2, Edge e) {
        if (e == null) {
            return;
        }
        if (edges[n1.index, n2.index] != null) {
            Destroy(e.gameObject);
            return;
        }
        if (edges[n2.index, n1.index] != null) {
            Destroy(e.gameObject);
            return;
        }
        edges[n1.index, n2.index] = e;
        edges[n2.index, n1.index] = e;
    }

    public List < Node > CheckForUnconnectedNodes() {
        List < Node > nodes = new List < Node > ();
        for (int i = 0; i < size; i++) {
            bool hasEdge = false;
            for (int k = 0; k < size; k++) {
                if (edges[i, k] != null) {
                    hasEdge = true;
                    break;
                }
            }
            if (!hasEdge) {
                nodes.Add(this.nodes[i]);
            }
        }
        return nodes;
    }

    private void PrintPath(int currentVertex,
        int[] parents) {

        if (currentVertex == NO_PARENT) {
            return;
        }
        animationHandler.enqueueAnimation(nodes[currentVertex].gameObject, Path);
        PrintPath(parents[currentVertex], parents);
        if (currentVertex - 1 == NO_PARENT) {
            return;
        }
        if (edges[nodes[currentVertex].index, nodes[parents[currentVertex]].index] == null) {
            return;
        }
        animationHandler.enqueueAnimation(edges[nodes[currentVertex].index, nodes[parents[currentVertex]].index].gameObject, Path);

    }

    bool LessThanWithBuffer(float float1, float float2) {
        if (Math.Abs(float1 - float2) < 0.00001) {
            return true;
        }
        if (float1 < float2) {
            return true;
        }
        return false;
    }

    public void dijkstra() {
        camera = FindObjectOfType < Camera > ();
        int nVertices = size;
        end = (int) UnityEngine.Random.Range(start + 1, size);
        if (end > size) {
            end = size - 1;
        }

        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(nodes[start].gameObject.transform.position - camera.transform.position), 1000000);
        camera.transform.position = camera.transform.position - camera.transform.forward * 100;
        animationHandler.enqueueAnimation(nodes[start].gameObject, Path);
        animationHandler.enqueueAnimation(nodes[end].gameObject, Path);
        float[] shortestDistances = new float[nVertices];

        bool[] added = new bool[nVertices];

        for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++) {
            shortestDistances[vertexIndex] = float.MaxValue;
            added[vertexIndex] = false;
        }

        shortestDistances[start] = 0;

        for (int i = 1; i < nVertices; i++) {
            int nearestVertex = -1;
            float shortestDistance = float.MaxValue;
            for (int vertexIndex = 0; vertexIndex < nVertices - 1; vertexIndex++) {
                if (!added[vertexIndex] &&
                    shortestDistances[vertexIndex] <
                    shortestDistance) {
                    nearestVertex = vertexIndex;
                    shortestDistance = shortestDistances[vertexIndex];
                }
            }

            if (nearestVertex == -1) {
                Debug.Log("No connection");
                return;
            }
            added[nearestVertex] = true;

            for (int vertexIndex = 0; vertexIndex < nVertices; vertexIndex++) {
                if (edges[nearestVertex, vertexIndex] == null) {
                    continue;
                }
                float edgeDistance = edges[nearestVertex, vertexIndex].transform.localScale.y;

                if (edgeDistance > 0 &&
                    ((shortestDistance + edgeDistance) <
                        shortestDistances[vertexIndex])) {
                    parents[vertexIndex] = nearestVertex;
                    shortestDistances[vertexIndex] = shortestDistance +
                        edgeDistance;

                    animationHandler.enqueueAnimation(edges[nearestVertex, vertexIndex].gameObject, Selected);
                }
                if (vertexIndex == end && edges[nearestVertex, vertexIndex] != null) {
                    PrintPath(vertexIndex, parents);
                    return;
                }
            }
            if (nearestVertex != start && nearestVertex != end)
                animationHandler.enqueueAnimation(nodes[nearestVertex].gameObject, Selected);
        }
    }

    public void AStar() {
        camera = FindObjectOfType < Camera > ();
        end = (int) UnityEngine.Random.Range(start + 1, size);
        if (end > size) {
            end = size - 1;
        }
        Debug.Log("camera" + camera);
        Debug.Log("node" + nodes[start].gameObject);
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(nodes[start].gameObject.transform.position - camera.transform.position), 1000000);
        camera.transform.position = camera.transform.position - camera.transform.forward * 100;
        animationHandler.enqueueAnimation(nodes[start].gameObject, Path);
        animationHandler.enqueueAnimation(nodes[end].gameObject, Path);

        float[] shortestDistances = new float[size];
        float[] heuristicDistance = new float[size];

        bool[] added = new bool[size];
        for (int vertexIndex = 0; vertexIndex < size; vertexIndex++) {
            shortestDistances[vertexIndex] = float.MaxValue;
            added[vertexIndex] = false;
            heuristicDistance[vertexIndex] = Vector3.Distance(nodes[vertexIndex].gameObject.transform.position, nodes[end].gameObject.transform.position);
        }

        shortestDistances[start] = 0;

        for (int i = 1; i < size; i++) {
            int nearestVertex = -1;
            float shortestDistance = float.MaxValue;
            float shortestHeuristicDistance = float.MaxValue;
            for (int vertexIndex = 0; vertexIndex < size - 1; vertexIndex++) {
                if (!added[vertexIndex] && lessThanWithBuffer(shortestDistances[vertexIndex] + heuristicDistance[vertexIndex], shortestHeuristicDistance)) {
                    nearestVertex = vertexIndex;
                    shortestHeuristicDistance = (float) Math.Round(shortestDistances[vertexIndex] + heuristicDistance[vertexIndex], 5);
                    shortestDistance = shortestDistances[vertexIndex];
                }
            }

            added[nearestVertex] = true;

            for (int vertexIndex = 0; vertexIndex < size; vertexIndex++) {
                if (edges[nearestVertex, vertexIndex] == null) {
                    continue;
                }
                float edgeDistance = (float) edges[nearestVertex, vertexIndex].transform.localScale.y;

                if (edgeDistance > 0 &&
                    ((shortestDistance + edgeDistance) <
                        shortestDistances[vertexIndex])) {
                    parents[vertexIndex] = nearestVertex;
                    shortestDistances[vertexIndex] = shortestDistance +
                        edgeDistance;

                    animationHandler.enqueueAnimation(edges[nearestVertex, vertexIndex].gameObject, Selected);
                }
                if (vertexIndex == end && edges[nearestVertex, vertexIndex] != null) {
                    PrintPath(vertexIndex, parents);
                    return;
                }
            }
            if (nearestVertex != start && nearestVertex != end)
                animationHandler.enqueueAnimation(nodes[nearestVertex].gameObject, Selected);
        }
    }

    public void BFS() {
        camera = FindObjectOfType < Camera > ();
        end = (int) UnityEngine.Random.Range(start + 1, size);
        if (end > size) {
            end = size - 1;
        }
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(nodes[start].gameObject.transform.position - camera.transform.position), 1000000);
        camera.transform.position = camera.transform.position - camera.transform.forward * 100;
        animationHandler.enqueueAnimation(nodes[start].gameObject, Path);
        animationHandler.enqueueAnimation(nodes[end].gameObject, Path);

        int[] parents = new int[size];

        parents[start] = NO_PARENT;

        bool[] visited = new bool[size];
        for (int i = 0; i < visited.Length; i++) {
            visited[i] = false;
        }
        List < int > q = new List < int > ();
        q.Add(start);

        visited[start] = true;

        int vis;
        while (q.Count != 0) {
            vis = q[0];
            if (vis != start && vis != end)
                animationHandler.enqueueAnimation(nodes[vis].gameObject, Selected);

            q.Remove(q[0]);

            for (int i = 0; i < size; i++) {
                if (edges[vis, i] != null && (!visited[i])) {
                    q.Add(i);
                    visited[i] = true;
                    parents[i] = vis;
                    animationHandler.enqueueAnimation(edges[vis, i].gameObject, Selected);

                    if (i == end) {
                        PrintPath(i, parents);
                        return;
                    }
                }
            }
        }
    }

    public void DepthFirstSearch(int start, int end, bool[] visited) {
        end = (int) UnityEngine.Random.Range(start + 1, size);
        camera = FindObjectOfType < Camera > ();
        visited[start] = true;
        camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, Quaternion.LookRotation(nodes[start].gameObject.transform.position - camera.transform.position), 1000000);
        camera.transform.position = camera.transform.position - camera.transform.forward * 100;
        for (int i = 0; i < size; i++) {
            if (edges[start, i] != null && (!visited[i])) {
                parents[i] = start;
                animationHandler.enqueueAnimation(edges[start, i].gameObject, Selected);
                if (i == end) {
                    PrintPath(i, parents);
                    throw new System.Exception("Path found");
                }
                DepthFirstSearch(i, end, visited);
            }
        }

    }

}
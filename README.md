# 3D Pathfinding Simulation Through Procedurally Generated Graphs
![Screenshot 2024-02-02 152522](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/29aa9fb7-bc39-4c5e-aba7-bc93d127d21f)

## Introduction or Project Overview
I made this project in 2021, but was using Unity Version Control at the time instead of GitHub. This project is 3D pathfinding simulation that uses procedurally generated graphs. I designed and developed this project using Unity and C#, it aims to visualize the workings of various pathfinding algorithms within dynamically created 3D spaces. The purpose of this project is to offer insights into algorithmic pathfinding by simulating different scenarios and graph configurations in a visually engaging manner.

## Key Features
The 3D Pathfinding Simulation offers several distinctive features, including:
- **Procedurally Generated 3D Graphs**: Graphs are generated dynamically, offering a new experience with each simulation run.
- **Multiple Pathfinding Algorithms**: Implements widely used algorithms such as Dijkstra, A*, Breadth-First Search (BFS), and Depth-First Search (DFS) to solve pathfinding problems.
- **Real-Time Visualization**: Watch algorithms in action as they navigate through the graph, highlighting paths and decision points.
- **Customizable Simulation Parameters**: Users can adjust various parameters such as graph style and algorithm selection, allowing for a tailored simulation experience.

## Technologies Used
This project is built using several key technologies and frameworks:
- **Unity**: Powers the 3D simulation and visualization aspects, providing a rich and interactive environment.
- **C#**: All scripting and algorithm implementation are done using C#, making use of Unity's extensive libraries.
- **Poisson Disk Sampling**: Utilized for node distribution within the graphs, ensuring a minimum distance between nodes for both 2D and 3D simulations.

## Poisson 3D Generator Parameters
In the simulation, the Poisson 3D generator plays a vital role in creating a procedurally generated environment with customizable graph properties:

- **Radius of Distance Between Discs**: Defines the minimum separation between any two nodes, controlling the density of the graph.
- **Maximum Edge Distance**: Determines the maximum length for edges between nodes, influencing the graph's connectivity.
- **Region Size**: Specifies the total volume in which nodes are distributed, affecting the scope and scale of the generated graph.
- **Rejection Samples**: The number of attempts the algorithm makes to place a new node while adhering to the minimum distance rule, impacting the distribution's uniformity and density.

These parameters allow for extensive customization of the graph's appearance and structure, offering various scenarios for pathfinding simulations.

## Live Demonstrations and Downloads
- **Online Demo**: Explore the simulation through [this online demo](https://clintboyett.com/Simulation). Get a firsthand look at how different algorithms navigate complex 3D spaces. The demo provides an interactive experience, allowing users to select algorithms, generate graphs, and visualize pathfinding in real-time. Due to browser limits, the online demo is much slower than the executable.
- **Executable Link**: For a local experience, download the executable from [here](https://drive.google.com/file/d/16PbRPjn8tNsxWuzauV97KgaVRHJG8tTX/view). Compatible with Windows, the executable offers an offline mode to explore and interact with the simulation. **Note**: To return to the main menu, use ALT-F4 and relaunch the application.

## Contact Information
For further information, questions, or feedback, please visit [my website](https://clintboyett.com/). Here you'll find more about my work, other projects, and ways to get in touch.
![Screenshot 2024-02-02 111007](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/1b1d7cfc-5c48-44b7-84e1-e207a7fddbe5)
![Screenshot 2024-02-02 111027](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/be793c07-baf3-4e7d-9c7b-7a35d099fbd2)
![Screenshot 2024-02-02 135353](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/d8aa6356-37db-477c-a208-30170dad53ac)
![Screenshot 2024-02-02 135307](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/6c142977-90d0-4637-a5b7-ea0fedf1bf7a)
![Screenshot 2024-02-02 135244](https://github.com/clintboyett01/3D-Pathfinfing-Simulation/assets/37661122/c60d5981-eded-4aea-a355-3aee6007beba)

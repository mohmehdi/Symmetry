# Symmetry

![SYM](https://user-images.githubusercontent.com/52382093/190011739-c8e1a83b-9810-4785-826b-696f8be1bd8a.gif)


<details>
  <summary>Spoiler, this is the last level</summary>
  
![image](https://user-images.githubusercontent.com/52382093/190009427-aa4ddfb2-7246-4700-a7c0-62fdaafec6ce.png)  
</details>

Game is available on : [itch](https://mohmehdi.itch.io/symmetry)        
for Linux, Windows, Android, Web   

### Prerequisites

Unity3D

## How to Make New Levels

there are 4 scenes  
- **Main**  
- **Levels**  
- **DrawSave**  
- **MakeLevel Dev**      
    
use MakeLevel_Dev to make new levels   
after you finished drawing click save button (load it to make sure)     
this will create a text file include data you need   
just rememmber size of the grid you want to save
add an if statment at SetData_level() method in GameManager script or replace with one of levels 
for example for change level 2 data you can change size and grid with new values   
```
        else if (Level == 2)  
        {
            data.size = 5;
            data.grid = new int[,] {
            {0,0,0,0,0,0,0,0,0,0},     	//
            {0,0,3,0,0,0,0,5,0,0},   	//
            {3,3,3,3,3,3,5,4,5,0},		//   new data will paste here
            {0,0,3,0,0,0,0,5,0,0},		//
            {0,0,0,0,0,0,0,0,0,0}		//
            };
        }
```
** if you want something special for a level like i did in level 20 do it on LoadLevel() method in GameManager script 	:slightly_smiling_fac **




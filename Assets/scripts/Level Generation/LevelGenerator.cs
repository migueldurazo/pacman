using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

    public enum Maze {

        Test, Sample1
    };


    /*
		Patterns are used to determine what prefab will be used
		Given a level text file such as:
		%%%%
		%  %
		%  %
		%%%%
		It will first put an edge to it:
		eeeeee
		e%%%%e
		e%  %e
		e%  %e
        e%%%%e
		eeeeee
		And then it will determine what each of those 1s and 0s
		prefab is by looking at its 4 directions: top, bottom, left and right
		for isntance:
		eee
		e%%
		e%
		Looking at the center of that 3x3 square the pattern we want is given
		by its 4 directions in the order of top,bottom,left,right.
		So the pattern is e,1,e,1
		For that pattern, a prefab is chosen, not all patterns will have prefabs
		if a pattern is found that does not have a prefab, an empty prefab
		will be used. Each prefab is a 8x8 sprite with or without colliders.
		
		To represent each pattern a dictionary chain will be used, top will
		be mapped to bottom, bottom to left and left to right, finally right
		is mapped to the prefab.
	
	
	*/

    public IAgent agent;
    public string levelData;

	// Use this for initialization
	void Start () {
		
		Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> 
		patterns = new Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>>();
		
		GameObject emptySquare = (GameObject) Resources.Load("prefabs/Empty", typeof(GameObject));
		GameObject pacman = (GameObject) Resources.Load("prefabs/Pacman", typeof(GameObject));
        GameObject pacdot = (GameObject)Resources.Load("prefabs/Pacdot", typeof(GameObject));

        loadPatterns(patterns);
		
		List<List<char>> original = new List<List<char>>();
		List<List<char>> maze = new List<List<char>>();
		List<List<char>> characters = new List<List<char>>();
		List<List<bool>> food = new List<List<bool>>();


        List<string> file = readTextFile( levelData );

        //TextAsset mytxtData = (TextAsset)Resources.Load("levels/" + file.Name.Substring(0,file.Name.Length-4));


        foreach ( string s in file ){
			
			List<char> line = new List<char>(s.ToCharArray());
			
			original.Add(line);
			
		}
		
		int  matrixLength = original.Count;
		int matrixRowLength = original[0].Count;
		
		Debug.Log("Matrix is "+matrixLength+" * "+matrixRowLength);
		
		//add a row at the top and at the bottom with 'e' (edge)
		List<char> eRow = new List<char>();
		for( int i = 0 ; i < matrixRowLength; i++ ){
			
			eRow.Add('e');
			
		}
		
		original.Insert(0,new List<char>(eRow));
		original.Add(new List<char>(eRow));
		
		//add a 'e' as first and last element to every row.
		foreach( List<char> row in original ){
			
			row.Add( 'e' );
			row.Insert(0, 'e' );
			
		}
		
			
		matrixLength = original.Count;
		matrixRowLength = original[0].Count;
		
		Debug.Log("Matrix is now "+matrixLength+" * "+matrixRowLength);
		
		foreach( List<char> row in original ){
			
			characters.Add(row);
			
			maze.Add( getMazeLine(row) );
			
			food.Add( getFoodFlags(row) );
			
		}
	
		
		printMatrix(maze);
		
		printMatrix(characters);
		
		drawLevel( patterns, maze, characters, food, emptySquare, pacman, pacdot);
		
		
//		Instantiate (patterns['e']['1']['e']['1'], new Vector3(0f, 0f, 0f), Quaternion.identity);
		
	
	}
	
	List<char> getMazeLine( List<char> line ){
		
		//Convert every f (food) and p(pacman) to 0
		
		List<char> newLine = new List<char>();
		
		foreach( char c in line ){
			
			char realC = c;
			
			if( c=='.' || c=='P' || c=='G' ){
				
				realC = ' ';
				
			}
			
			newLine.Add(realC);
			
		}
		
		return newLine;
		
	}
	
	List<bool> getFoodFlags ( List<char> line ){
		
		//If char is f set true, false otherwise
		
		List<bool> foodFlags = new List<bool>();
		
		foreach( char c in line ){
			
			if(  c == '.' ){
				
				foodFlags.Add(true);
				
			}else{
				
				foodFlags.Add(false);
				
			}
			
		}
		
		return foodFlags;
		
	}



    
	void drawLevel(Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> patterns,
		List<List<char>> maze, List<List<char>> characters, List<List<bool>> food, GameObject emptySquare,
			GameObject pacman, GameObject pacdot ){

        Level level = new Level();


        int height = maze.Count;
		int width = maze[0].Count;

        //This data structure helps to see if a piece of food is already in place, initialized to false
        List<List<bool>> foodIsPlaced = createSameDimensionStructure(food);
       

        float prefabSize = emptySquare.GetComponent<Renderer>().bounds.size.x;
		
		Debug.Log("size "+prefabSize);
		
		float startX = ((float)width/2f)*prefabSize*-1f;
		float startY = ((float)height/2f)*prefabSize;
		
		float currentX = startX;
		float currentY = startY;
		
		float distance = prefabSize;

        int boardXCoordinate = 0;
        
		
		for( int i = 0 ; i < maze.Count ; i++ ){
			
			currentX = startX;

            List<Place> boardRow = new List<Place>();

            int boardYCoordinate = 0;

            for ( int j = 0 ; j < maze[i].Count ; j++ ){
				
				char current = maze[i][j];
				
				if( current != 'e' ){

                    //valid area
					
					GameObject prefab = emptySquare;
					
					if(  current != ' ' ){
						
						char top = maze[i-1][j];
						char bottom = maze[i+1][j];
						char left = maze[i][j-1];
						char right = maze[i][j+1];
						
						Debug.Log("Getting prefab for "+top+","+bottom+","+left+","+right);
						
						try{
							
							prefab = patterns[top][bottom][left][right];
							
						}catch(System.NullReferenceException nrfe){
							
							Debug.Log("The pattern "+top+","+bottom+","+left+","+right+" Does not have a prefab");
							
							prefab = emptySquare;
							
						}	
						
					}

					//Current prefab position
					Vector3 position = new Vector3 (currentX, currentY, 0f);
					Instantiate (prefab,position, Quaternion.identity);
                    
                    //use to instantiate pacman, pacdots and ghosts
                    Vector3 entityPosition = new Vector3(currentX + prefabSize / 2f, currentY - prefabSize / 2f, 0f);

                    Place place = new Place(false);
                    place.Level = level;
                    place.X = boardXCoordinate;
                    place.Y = boardYCoordinate++;
                    

                    if (maze[i][j] == ' ' && maze[i][j + 1] == ' ' && maze[i + 1][j] == ' '
                        && maze[i + 1][j + 1] == ' ')
                    {

                        place.Valid = true;

                        //Found an empty spot where pacman can be placed
                        //Use a structure to store pacman position and coordinates, new object required

                        place.PacmanPosition = entityPosition;

                    }

                    boardRow.Add(place);

                    if ( characters[i][j] == 'P' && characters[i][j+1] == 'P' && characters[i+1][j]=='P'
						&& characters[i+1][j+1] == 'P'){
						
						//Vector3 pacPos = new Vector3 (currentX+prefabSize/2f, currentY-prefabSize/2f, 0f);						
						
                        GameObject pacmanInstance = (GameObject)Instantiate (pacman, entityPosition, Quaternion.identity);

                        PacmanMovement pacmanMovement = pacmanInstance.GetComponent<PacmanMovement>();

                        pacmanMovement.Agent = agent;

                        pacmanMovement.Level = level;

                        pacmanMovement.CurrentPlace = place;

                        Debug.Log(place.X + ":" + place.Y);
						
					}

                    if (food[i][j] && food[i][j + 1]  && food[i + 1][j] 
                        && food[i + 1][j + 1] && !foodIsPlaced[i][j] && !foodIsPlaced[i][j + 1]
                        && !foodIsPlaced[i + 1][j] && !foodIsPlaced[i + 1][j + 1]     )
                    {

                        //Food gets placed
                        
                        GameObject pacdotInstance = (GameObject)Instantiate(pacdot, entityPosition, Quaternion.identity);

                        foodIsPlaced[i][j] = foodIsPlaced[i][j + 1] = foodIsPlaced[i + 1][j] = foodIsPlaced[i + 1][j + 1] = true;

                    }

                    currentX += distance;
					
				}
				
			}

            if (boardRow.Count > 0)
            {
                level.Board.Add(boardRow);
                boardXCoordinate++;
            }
			currentY-=distance;
			
		}
		
		
	}
	
	void loadPatterns(Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> patterns ){
		
		//WALLS
		//wall top left corner
		addPattern( new char[]{'e','%','e','%'}, (GameObject) Resources.Load("prefabs/WCLT", typeof(GameObject)), patterns);
		//wall top clear
		addPattern( new char[]{'e',' ','%','%'}, (GameObject) Resources.Load("prefabs/WTC", typeof(GameObject)), patterns);
		//wall top obstacle
		addPattern( new char[]{'e','%','%','%'}, (GameObject) Resources.Load("prefabs/WTO", typeof(GameObject)), patterns);
		//wall top right corner
		addPattern( new char[]{'e','%','%','e'}, (GameObject) Resources.Load("prefabs/WCRT", typeof(GameObject)), patterns);
		//wall right clear
		addPattern( new char[]{'%','%',' ','e'}, (GameObject) Resources.Load("prefabs/WRC", typeof(GameObject)), patterns);
		//wall right obstacle
		addPattern( new char[]{'%','%','%','e'}, (GameObject) Resources.Load("prefabs/WRO", typeof(GameObject)), patterns);
		//wall bottom right corner
		addPattern( new char[]{'%','e','%','e'}, (GameObject) Resources.Load("prefabs/WCRB", typeof(GameObject)), patterns);
		//wall bottom clear
		addPattern( new char[]{' ','e','%','%'}, (GameObject) Resources.Load("prefabs/WBC", typeof(GameObject)), patterns);
		//wall bottom obstacle
		addPattern( new char[]{'%','e','%','%'}, (GameObject) Resources.Load("prefabs/WBO", typeof(GameObject)), patterns);
		//wall bottom left corner
		addPattern( new char[]{'%','e','e','%'}, (GameObject) Resources.Load("prefabs/WCLB", typeof(GameObject)), patterns);
		//wall left clear
		addPattern( new char[]{'%','%','e',' '}, (GameObject) Resources.Load("prefabs/WLC", typeof(GameObject)), patterns);
		//wall left obstacle
		addPattern( new char[]{'%','%','e','%'}, (GameObject) Resources.Load("prefabs/WLO", typeof(GameObject)), patterns);
		
		//OBSTACLES
		//Obstacle top left corner
		addPattern( new char[]{' ','%',' ','%'}, (GameObject) Resources.Load("prefabs/OCLT", typeof(GameObject)), patterns);
		//Obstacle top right corner
		addPattern( new char[]{' ','%','%',' '}, (GameObject) Resources.Load("prefabs/OCRT", typeof(GameObject)), patterns);
		//Obstacle bottom left corner
		addPattern( new char[]{'%',' ',' ','%'}, (GameObject) Resources.Load("prefabs/OCLB", typeof(GameObject)), patterns);
		//Obstacle bottom right corner
		addPattern( new char[]{'%',' ','%',' '}, (GameObject) Resources.Load("prefabs/OCRB", typeof(GameObject)), patterns);
		//Obstacle top
		addPattern( new char[]{' ','%',' ',' '}, (GameObject) Resources.Load("prefabs/OT", typeof(GameObject)), patterns);
		//Obstacle bottom
		addPattern( new char[]{'%',' ',' ',' '}, (GameObject) Resources.Load("prefabs/OB", typeof(GameObject)), patterns);
		//Obstacle left
		addPattern( new char[]{' ',' ',' ','%'}, (GameObject) Resources.Load("prefabs/OL", typeof(GameObject)), patterns);
		//Obstacle right
		addPattern( new char[]{' ',' ','%',' '}, (GameObject) Resources.Load("prefabs/OR", typeof(GameObject)), patterns);
		//Obstacle wall vertical
		addPattern( new char[]{'%','%',' ',' '}, (GameObject) Resources.Load("prefabs/OWV", typeof(GameObject)), patterns);
		//Obstacle wall horizontal
		addPattern( new char[]{' ',' ','%','%'}, (GameObject) Resources.Load("prefabs/OWH", typeof(GameObject)), patterns);
		//Obstacle wall top
		addPattern( new char[]{' ','%','%','%'}, (GameObject) Resources.Load("prefabs/OWT", typeof(GameObject)), patterns);
		//Obstacle wall bottom
		addPattern( new char[]{'%',' ','%','%'}, (GameObject) Resources.Load("prefabs/OWB", typeof(GameObject)), patterns);
		//Obstacle wall left
		addPattern( new char[]{'%','%',' ','%'}, (GameObject) Resources.Load("prefabs/OWL", typeof(GameObject)), patterns);
		//Obstacle wall right
		addPattern( new char[]{'%','%','%',' '}, (GameObject) Resources.Load("prefabs/OWR", typeof(GameObject)), patterns);
		
		//EMPTY
		addPattern( new char[]{'%','%','%','%'}, (GameObject) Resources.Load("prefabs/Empty", typeof(GameObject)), patterns);
		
	}
	
	//Given a char array, it will map the directions to the prefab.
	
	void addPattern( char[] pat, GameObject prefab,
	Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> patterns ){ 
		
		if( pat.Length == 4 ){
			
			if( !patterns.ContainsKey( pat[0] ) ){
				
				patterns.Add( pat[0], new Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>() );
				
			}
			
			if( !patterns[pat[0]].ContainsKey( pat[1] ) ){
				
				patterns[pat[0]].Add( pat[1], new Dictionary<char,Dictionary<char,GameObject>>() );		
				
			}
			
			if( !patterns[pat[0]][pat[1]].ContainsKey( pat[2] ) ){
				
				patterns[pat[0]][pat[1]].Add( pat[2], new Dictionary<char,GameObject>() );			
				
			}
			
			if( !patterns[pat[0]][pat[1]][pat[2]].ContainsKey( pat[3] ) ){
				
				patterns[pat[0]][pat[1]][pat[2]].Add( pat[3] , prefab );			
				
			}
			
		}else{
			
				Debug.Log("Pattern must have 4 characters");
			
		}
		
	}
	
	void printMatrix( List<List<char>> charMatrix ){
		
		string rowstring = "";
		
		foreach( List<char> row in charMatrix ){
			
			foreach( char c in row ){
				
				rowstring += c;
				
			}
			
			rowstring += '\n';
			
		}
		
		Debug.Log(rowstring);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	

	List<string> readTextFile(string contents)
	{
		
	   List<string> list = new List<string>();

        byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(contents);
        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
        MemoryStream stream = new MemoryStream(byteArray);

        StreamReader inp_stm = new StreamReader(  stream);

	   int characterNumber = 0;
	   
	   while(!inp_stm.EndOfStream)
	   {
		   string inp_ln = inp_stm.ReadLine( );

           inp_ln.Trim();
		   
		   if( characterNumber == 0 ){
			   
				characterNumber = inp_ln.Length;
			   
		   }else{
			   
			   if( inp_ln.Length != characterNumber ){
				   
					Debug.Log("File is wrongly built, number of characters dont match");
					break;
				   
			   }
			   
		   }
		   
		   list.Add( inp_ln );
		   
	   }

	   inp_stm.Close( );  
	   
	   return list;
	}

    private List<List<bool>> createSameDimensionStructure(List<List<bool>> copyFrom)
    {

        List<List<bool>> result = new List<List<bool>>();

        copyFrom.ForEach(item => {
            result.Add(new List<bool>());
            item.ForEach(flag => { result[result.Count - 1].Add(false); });
        });

        return result;
        
    }
	
	
}

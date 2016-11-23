using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    
    /*
		
		
        A pre-process is to add double spaces for pacman and food tiles, 
        the reason is that pacman is a 16x16 prefab and each level prefab tile
        is 8x8, if a pacmanChar or '.' or a space is too tight (8x8) this will be expanded
        Example:
        %%%%%%
        %P % %
        % .  %
        %%%%%%
        First, it will detect tight spaces and add a symbol to it 'T'
        %%%%%%
        %P %T%
        % .T %
        %%%%%%
        The 'T' is added when a tile is between '%' horizontally or vertically
        The level is expanded by duplicating each column containing either
        a pacmanChar, '.' or 'T'
        %%%%%%%%%%
        %PP  %%TT%
        %  ..TT  %
        %%%%%%%%%%
        Then, each row containing pacmanChar, 'T' or '.' is duplicated
        %%%%%%%%%%
        %PP  %%TT%
        %PP  %%TT%
        %  ..TT  %
        %  ..TT  %
        %%%%%%%%%%
        This level is now adjusted for pacman to move freely
        
        Given a level text file such as:
		%%%%
		%  %
		%  %
		%%%%
		It will put an edge to it, in order to create patterns.
		eeeeee
		e%%%%e
		e%  %e
		e%  %e
        e%%%%e
		eeeeee

        Patterns are used to determine what prefab will be used
		After this pre process it will determine what each of those 1s and 0s
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

    private char pacmanChar = 'P';
    private char foodChar = '.';
    private char tightSpaceChar = 'T';
    private char emptySpaceChar = ' ';
    private char wallChar = '%';
    private char edgeChar = 'e';
    private char powerUpChar = 'o';
    private char ghostChar = 'G';
    

    public IAgent agent;
    public string levelData;
    public Level generatedLevel;
    private Camera CameraVisual;
    private GameObject Camera;
    private List<GameObject> ghosts = new List<GameObject>();

    int ghostIndex = 0;

    public GameObject multiAgentGO = null;

    bool multiAgent = false;
    // Use this for initialization
    void Start() {

        if (agent != null)
        {
            if( agent is MultiAgent)
            {
                multiAgent = true;
            }
        }

        Camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraVisual = Camera.GetComponent<Camera>();

        Dictionary<char, Dictionary<char, Dictionary<char, Dictionary<char, GameObject>>>>
        patterns = new Dictionary<char, Dictionary<char, Dictionary<char, Dictionary<char, GameObject>>>>();

        GameObject emptySquare = (GameObject)Resources.Load("prefabs/Empty", typeof(GameObject));
        GameObject pacman = (GameObject)Resources.Load("prefabs/Pacman", typeof(GameObject));
        GameObject pacdot = (GameObject)Resources.Load("prefabs/Pacdot", typeof(GameObject));
        GameObject powerup = (GameObject)Resources.Load("prefabs/PowerUp", typeof(GameObject));

        GameObject blinky = (GameObject)Resources.Load("prefabs/Blinky", typeof(GameObject));
        ghosts.Add(blinky);

        loadPatterns(patterns);

        List<List<char>> original = new List<List<char>>();
        List<List<char>> maze = new List<List<char>>();
        List<List<char>> characters = new List<List<char>>();
        List<List<bool>> food = new List<List<bool>>();

        //The file is represented as a list of strings, each string is a line on the file
        List<string> file = readTextFile(levelData);

        //The original input file is preserved as character arrays
        foreach (string s in file) {

            List<char> line = new List<char>(s.ToCharArray());
            original.Add(line);

        }

        List<List<char>> preProcessed = preProcessLevel(original);

        foreach (List<char> row in preProcessed) {

            characters.Add(row);

            maze.Add(getMazeLine(row));

            food.Add(getFoodFlags(row));

        }


        printMatrix(maze);

        printMatrix(characters);

        generatedLevel = drawLevel(patterns, maze, characters, food,
            emptySquare, pacman, pacdot, powerup, ghosts);

    }


    //Surrounds level with 'e' and expands rows and columns according to
    //pacman size
    List<List<char>> preProcessLevel(List<List<char>> original)
    {

        if (original == null || original.Count == 0)
        {
            return null;
        }

        List<List<char>> transformedList = levelExpansion(original);

        transformedList = addEdge(transformedList);

        return transformedList;

    }

    private int getGhostIndex()
    {
        int aux = ghostIndex++;

        if (ghostIndex == ghosts.Count) ghostIndex = 0;

        return aux;
    }

    private List<List<char>> levelExpansion(List<List<char>> source)
    {
        List<List<char>> transformedList = new List<List<char>>();

        int matrixLength = source.Count;
        int matrixRowLength = source[0].Count;

        //copy original matrix
        for (int i = 0; i < matrixLength; i++)
        {
            transformedList.Add(new List<char>(source[i]));

        }


            //insert 'T's on tight spaces
            //iterate each row
        for (int i = 0; i < matrixLength; i++)
        {

            for (int j = 1; j < matrixRowLength - 1; j++)
            {

                char lastChar = source[i][j - 1];
                char currentChar = source[i][j];
                char nextChar = source[i][j + 1];

                if (currentChar == emptySpaceChar && lastChar == wallChar && nextChar == wallChar)
                {

                    //add tight space
                    transformedList[i][j] = tightSpaceChar;

                }


            }

        }
        //iterate through each column
        for ( int i = 0; i < matrixRowLength; i++)
        {

            for( int j = 1; j < matrixLength-1; j++)
            {
                
                char lastChar = source[j - 1][i];
                char currentChar = source[j][i];
                char nextChar = source[j + 1][i];

                if( currentChar == emptySpaceChar && lastChar == wallChar && nextChar == wallChar)
                {

                    //add tight space
                    transformedList[j][i] = tightSpaceChar;

                }

            
            }

        }

        //iterate through each column for expansion
        //backwards since that way new columns could be inserted
        //otherwise out of index exceptions could be thrown
        for (int i = matrixRowLength - 1; i >= 0; i--)
        {

            for (int j = 0; j < matrixLength; j++)
            {
                char character = transformedList[j][i];

                if (isExpandableCharacter(character))
                {

                    //duplicate column
                    for (int k = 0; k < matrixLength; k++)
                    {
                        transformedList[k].Insert(i, transformedList[k][i]);

                    }

                    break;
                }

            }

        }
        matrixLength = transformedList.Count;
        matrixRowLength = transformedList[0].Count;

        //iterate through each row for expansion
        //backwards since that way new rows could be inserted from the bottom up
        //otherwise out of index exceptions could be thrown
        for (int i = matrixLength  - 1; i >= 0; i--)
        {

            for (int j = 0; j < matrixRowLength; j++)
            {

                char character = transformedList[i][j];

                if (isExpandableCharacter(character))
                {
                    //duplicate row
                    transformedList.Insert(i, new List<char>(transformedList[i]));
                    break;
                }

            }

        }

        matrixLength = transformedList.Count;
        matrixRowLength = transformedList[0].Count;

        //remove 'T'
        for (int i = 0; i < matrixLength; i++)
        {

            for (int j = 0; j < matrixRowLength; j++)
            {
                if( transformedList[i][j] == tightSpaceChar)
                {

                    transformedList[i][j] = emptySpaceChar;

                }

            }

        }

        return transformedList;

    }

    private bool isExpandableCharacter(char c)
    {
        return c == pacmanChar || c == tightSpaceChar || c == foodChar || c == ghostChar;
    }
    
    private List<List<char>> addEdge(List<List<char>> source)
    {
        List<List<char>> transformedList = new List<List<char>>();

        int matrixLength = source.Count;
        int matrixRowLength = source[0].Count;

        List<char> eRow = new List<char>();
        for (int i = 0; i < matrixRowLength+2; i++)
        {

            eRow.Add(edgeChar);

        }

        //add edge space at the top
        transformedList.Add(eRow);

        //add a 'e' as first and last element to every row.
        foreach (List<char> row in source)
        {

            List<char> newRow = new List<char>(row);

            newRow.Insert(0, edgeChar);
            newRow.Add(edgeChar);

            transformedList.Add(newRow);

        }

        //add edge space at the bottom
        transformedList.Add(eRow);

        return transformedList;

    }

    List<char> getMazeLine( List<char> line ){
		
		//Convert every .(food), P(pacman),G(ghost) and o(Power up) to empty
		
		List<char> newLine = new List<char>();
		
		foreach( char c in line ){
			
			char realC = c;
			
			if( c==foodChar || c==pacmanChar || c==ghostChar || c==powerUpChar ){
				
				realC = emptySpaceChar;
				
			}
			
			newLine.Add(realC);
			
		}
		
		return newLine;
		
	}
	
	List<bool> getFoodFlags ( List<char> line ){
		
		//If char is f set true, false otherwise
		
		List<bool> foodFlags = new List<bool>();
		
		foreach( char c in line ){
			
			if(  c == foodChar ){
				
				foodFlags.Add(true);
				
			}else{
				
				foodFlags.Add(false);
				
			}
			
		}
		
		return foodFlags;
		
	}



    
	Level drawLevel(Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> patterns,
		List<List<char>> maze, List<List<char>> characters, List<List<bool>> food, GameObject emptySquare,
			GameObject pacman, GameObject pacdot, GameObject powerup, List<GameObject> ghosts ){

        Level level = new Level();

        int height = maze.Count;
		int width = maze[0].Count;

        //This data structure helps to see if a piece of food is already in place, initialized to false
        List<List<bool>> foodIsPlaced = createSameDimensionStructure(food);
       
        float prefabSize = emptySquare.GetComponent<Renderer>().bounds.size.x;
		
		float startX = ((float)width/2f)*prefabSize*-1f;
		float startY = ((float)height/2f)*prefabSize;
		
        if( width >= 74 && height >=74)
        {
            CameraVisual.fieldOfView = 155;
        }else
        if( width >= 55 && height >= 53)
        {
            CameraVisual.fieldOfView = 143;
        }

		float currentX = startX;
		float currentY = startY;
		
		float distance = prefabSize;

        int boardXCoordinate = 0;

        List<IAgent> agentMovements = new List<IAgent>();
		
		for( int i = 0 ; i < maze.Count ; i++ ){
			
			currentX = startX;

            List<Place> boardRow = new List<Place>();

            int boardYCoordinate = 0;

            for ( int j = 0 ; j < maze[i].Count ; j++ ){
				
				char current = maze[i][j];
				
				if( current != edgeChar ){

                    //valid area
					
					GameObject prefab = emptySquare;
					
					if(  current != emptySpaceChar ){
						
						char top = maze[i-1][j];
						char bottom = maze[i+1][j];
						char left = maze[i][j-1];
						char right = maze[i][j+1];
						
						try{
							
							prefab = patterns[top][bottom][left][right];
							
						}catch(System.NullReferenceException nrfe){
							
							Debug.Log("The pattern "+top+","+bottom+","+left+","+right+" Does not have a prefab");
							
							prefab = emptySquare;
							
						}catch( KeyNotFoundException knf)
                        {
                            Debug.Log("The pattern " + top + "," + bottom + "," + left + "," + right + " Does not have a prefab");

                            prefab = emptySquare;

                        }	
						
					}

					//Current prefab position
					Vector3 position = new Vector3 (currentX, currentY, 0f);
					Instantiate (prefab,position, Quaternion.identity);
                    
                    //use to instantiate pacman, pacdots, powerups and ghosts
                    Vector3 entityPosition = 
                        new Vector3(currentX + prefabSize / 2f,
                                    currentY - prefabSize / 2f, 0f);

                    Place place = new Place();
                    place.Valid = false;
                    place.HasFood = false;
                    place.Level = level;
                    place.X = boardXCoordinate;
                    place.Y = boardYCoordinate++;
                    place.EntityPosition = entityPosition;

                    if (food[i][j] && food[i][j + 1] && food[i + 1][j]
                        && food[i + 1][j + 1] && !foodIsPlaced[i][j] && !foodIsPlaced[i][j + 1]
                        && !foodIsPlaced[i + 1][j] && !foodIsPlaced[i + 1][j + 1])
                    {

                        //Food gets placed

                        GameObject pacdotInstance = (GameObject)Instantiate(pacdot, entityPosition, Quaternion.identity);

                        foodIsPlaced[i][j] = foodIsPlaced[i][j + 1] = foodIsPlaced[i + 1][j] = foodIsPlaced[i + 1][j + 1] = true;

                        place.HasFood = true;

                        level.FoodCount++;

                    }

                    if (maze[i][j] == emptySpaceChar && maze[i][j + 1] == emptySpaceChar && maze[i + 1][j] == emptySpaceChar
                        && maze[i + 1][j + 1] == emptySpaceChar)
                    {

                        place.Valid = true;

                        //Found an empty spot where pacman can be placed

                    }

                    boardRow.Add(place);

                    if ( characters[i][j] == pacmanChar && characters[i][j+1] == pacmanChar && characters[i+1][j]==pacmanChar
						&& characters[i+1][j+1] == pacmanChar){
						
						//Vector3 pacPos = new Vector3 (currentX+prefabSize/2f, currentY-prefabSize/2f, 0f);						
						
                        GameObject pacmanInstance = (GameObject)Instantiate (pacman, entityPosition, Quaternion.identity);

                        if ( multiAgent)
                        {
                            IAgent pacmanAgent = ((MultiAgent)agent).pacmanAgent.copy();
                            pacmanAgent.gameObject = pacmanInstance;
                            pacmanAgent.agentPlace = place;
                            level.PacmanPosition = place;
                            pacmanInstance.GetComponent<PacmanMovement>().enabled = false;
                            agentMovements.Insert(0, pacmanAgent);
                        }
                        else
                        {
                            PacmanMovement pacmanMovement = pacmanInstance.GetComponent<PacmanMovement>();
                            pacmanMovement.Agent = agent;
                            level.PacmanPosition = place;
                            pacmanMovement.Level = level;
                        }
						
					}

                    if (characters[i][j]         == ghostChar &&
                        characters[i][j + 1]     == ghostChar && 
                        characters[i + 1][j]     == ghostChar &&
                        characters[i + 1][j + 1] == ghostChar   )
                    {

                        //Vector3 pacPos = new Vector3 (currentX+prefabSize/2f, currentY-prefabSize/2f, 0f);						
                        GameObject ghost = 
                            (GameObject)Instantiate(ghosts[getGhostIndex()],
                            entityPosition, Quaternion.identity);

                        if (multiAgent)
                        {
                            IAgent ghostAgent =   ((MultiAgent)agent).ghostAgent.copy();
                            ghostAgent.agentPlace = place;
                            ghostAgent.gameObject = ghost;
                            level.GhostPositions.Add(place);
                            level.GhostScaredTimes.Add(0);
                            level.GhostOriginPosition.Add(place);
                            ghost.GetComponent<GhostMovement>().setLevel(level);
                            ghost.GetComponent<GhostMovement>().setOrigin(entityPosition);
                            ghost.GetComponent<GhostMovement>().setOriginPlace(place,level);
                            ghost.GetComponent<GhostMovement>().setGhostIndex(level.GhostPositions.Count - 1);
                            ghost.GetComponent<GhostMovement>().setMovement(multiAgentGO.GetComponent<MultiAgentMovement>());
                            agentMovements.Add(ghostAgent);
                        }
                        else
                        {
                            /*
                            PacmanMovement pacmanMovement =
                            ghost.GetComponent<PacmanMovement>();
                            pacmanMovement.Agent = new RandomAgent();
                            pacmanMovement.Level = level;
                            */
                        }

                    }

                    if (characters[i][j]         == powerUpChar && 
                        characters[i][j + 1]     == powerUpChar && 
                        characters[i + 1][j]     == powerUpChar && 
                        characters[i + 1][j + 1] == powerUpChar   )
                    {

                        GameObject powerUpInstance = (GameObject)Instantiate(powerup, entityPosition, Quaternion.identity);
                        powerUpInstance.GetComponent<Powerup>().setLevel(level);
                        powerUpInstance.GetComponent<Powerup>().setMultiAgentMovement(multiAgentGO.GetComponent<MultiAgentMovement>());
                        place.HasPowerUp = true;
                        level.PowerupPositions.Add(place);
                        level.PowerUpCount++;
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

        foreach (List<Place> row in level.Board)
        {
            foreach (Place place in row)
            {
                if (place.HasFood)
                    level.FoodPositions.Add(place);
            }
        }


        if (multiAgent)
        {

            if (multiAgentGO)
            {

                if( agentMovements[0] is MiniMaxAgent)
                {
                    ((MiniMaxAgent)agentMovements[0]).setAgentNumber(agentMovements.Count);
                }

                if (agentMovements[0] is ExpectiMaxAgent)
                {
                    ((ExpectiMaxAgent)agentMovements[0]).setAgentNumber(agentMovements.Count);
                }

                multiAgentGO.GetComponent<MultiAgentMovement>().setAgents(agentMovements);
                multiAgentGO.GetComponent<MultiAgentMovement>().Level = level;
                multiAgentGO.GetComponent<MultiAgentMovement>().enabled = true;
                multiAgentGO.SetActive(true);
            }
        }

        return level;
		
		
	}
	
	void loadPatterns(Dictionary<char,Dictionary<char,Dictionary<char,Dictionary<char,GameObject>>>> patterns ){
		
		//WALLS
		//wall top left corner
		addPattern( new char[]{edgeChar,wallChar,edgeChar,wallChar}, (GameObject) Resources.Load("prefabs/WCLT", typeof(GameObject)), patterns);
		//wall top clear
		addPattern( new char[]{edgeChar,emptySpaceChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/WTC", typeof(GameObject)), patterns);
		//wall top obstacle
		addPattern( new char[]{edgeChar,wallChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/WTO", typeof(GameObject)), patterns);
		//wall top right corner
		addPattern( new char[]{edgeChar,wallChar,wallChar,edgeChar}, (GameObject) Resources.Load("prefabs/WCRT", typeof(GameObject)), patterns);
		//wall right clear
		addPattern( new char[]{wallChar,wallChar,emptySpaceChar,edgeChar}, (GameObject) Resources.Load("prefabs/WRC", typeof(GameObject)), patterns);
		//wall right obstacle
		addPattern( new char[]{wallChar,wallChar,wallChar,edgeChar}, (GameObject) Resources.Load("prefabs/WRO", typeof(GameObject)), patterns);
		//wall bottom right corner
		addPattern( new char[]{wallChar,edgeChar,wallChar,edgeChar}, (GameObject) Resources.Load("prefabs/WCRB", typeof(GameObject)), patterns);
		//wall bottom clear
		addPattern( new char[]{emptySpaceChar,edgeChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/WBC", typeof(GameObject)), patterns);
		//wall bottom obstacle
		addPattern( new char[]{wallChar,edgeChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/WBO", typeof(GameObject)), patterns);
		//wall bottom left corner
		addPattern( new char[]{wallChar,edgeChar,edgeChar,wallChar}, (GameObject) Resources.Load("prefabs/WCLB", typeof(GameObject)), patterns);
		//wall left clear
		addPattern( new char[]{wallChar,wallChar,edgeChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/WLC", typeof(GameObject)), patterns);
		//wall left obstacle
		addPattern( new char[]{wallChar,wallChar,edgeChar,wallChar}, (GameObject) Resources.Load("prefabs/WLO", typeof(GameObject)), patterns);
		
		//OBSTACLES
		//Obstacle top left corner
		addPattern( new char[]{emptySpaceChar,wallChar,emptySpaceChar,wallChar}, (GameObject) Resources.Load("prefabs/OCLT", typeof(GameObject)), patterns);
		//Obstacle top right corner
		addPattern( new char[]{emptySpaceChar,wallChar,wallChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OCRT", typeof(GameObject)), patterns);
		//Obstacle bottom left corner
		addPattern( new char[]{wallChar,emptySpaceChar,emptySpaceChar,wallChar}, (GameObject) Resources.Load("prefabs/OCLB", typeof(GameObject)), patterns);
		//Obstacle bottom right corner
		addPattern( new char[]{wallChar,emptySpaceChar,wallChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OCRB", typeof(GameObject)), patterns);
		//Obstacle top
		addPattern( new char[]{emptySpaceChar,wallChar,emptySpaceChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OT", typeof(GameObject)), patterns);
		//Obstacle bottom
		addPattern( new char[]{wallChar,emptySpaceChar,emptySpaceChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OB", typeof(GameObject)), patterns);
		//Obstacle left
		addPattern( new char[]{emptySpaceChar,emptySpaceChar,emptySpaceChar,wallChar}, (GameObject) Resources.Load("prefabs/OL", typeof(GameObject)), patterns);
		//Obstacle right
		addPattern( new char[]{emptySpaceChar,emptySpaceChar,wallChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OR", typeof(GameObject)), patterns);
		//Obstacle wall vertical
		addPattern( new char[]{wallChar,wallChar,emptySpaceChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OWV", typeof(GameObject)), patterns);
		//Obstacle wall horizontal
		addPattern( new char[]{emptySpaceChar,emptySpaceChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/OWH", typeof(GameObject)), patterns);
		//Obstacle wall top
		addPattern( new char[]{emptySpaceChar,wallChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/OWT", typeof(GameObject)), patterns);
		//Obstacle wall bottom
		addPattern( new char[]{wallChar,emptySpaceChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/OWB", typeof(GameObject)), patterns);
		//Obstacle wall left
		addPattern( new char[]{wallChar,wallChar,emptySpaceChar,wallChar}, (GameObject) Resources.Load("prefabs/OWL", typeof(GameObject)), patterns);
		//Obstacle wall right
		addPattern( new char[]{wallChar,wallChar,wallChar,emptySpaceChar}, (GameObject) Resources.Load("prefabs/OWR", typeof(GameObject)), patterns);
		
		//EMPTY
		addPattern( new char[]{wallChar,wallChar,wallChar,wallChar}, (GameObject) Resources.Load("prefabs/Empty", typeof(GameObject)), patterns);
		
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

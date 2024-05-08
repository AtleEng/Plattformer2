using System.Numerics;
using System.Text.Json;

namespace Engine
{
    //Class that handle loading in levels
    public static class LoadingManager
    {
        static GameEntity empty = new(); //Use this entity to store all levelObj in to
        static int currentLevel = 0; //The index of the current level
        static public int CurrentLevel //read only, use method Load(int i);
        {
            get
            {
                return currentLevel;
            }
        }
        static Vector2 levelSize = Vector2.Zero; //LevelSize is determend by the level
        static public Vector2 LevelSize
        {
            get
            {
                return levelSize;
            }
        }

        static string prePath = @"Game\Project\Levels\"; //In this folder must all levels be
        static Dictionary<int, Type> entitysInLevel = new() //Dictionery of all diffrent levelObj <=> index
        {
            {1, typeof(Block)},
            {2, typeof(JumpPad)},
            {3, typeof(WalkEnemy)},
            {4, typeof(JumpingEnemy)},
            {5, typeof(RandomEnemy)},
            {6, typeof(FlyingEnemy)},
            {7, typeof(KillZone)},
            {8, typeof(Player)},
            {9, typeof(Portal)},
        };
        //TODO I know I can use a list but it is for readability

        public static Dictionary<int, string> levels = new() //Dictionery of the levelIndex <=> FilePath
        {
            {1, "Level1"},
            {2, "Level2"},
            {3, "Level3"},
            {4, "Level4"},
            {5, "Level5"}
        };
        static List<GameEntity> levelEntities = new(); //All Entitys in level

        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public static void StartLoading() //Creates the "Folder" entity
        {
            empty.name = "Level";
            EntityManager.SpawnEntity(empty);
        }

        public static void SaveLevel(string path, int[,] level) //Used this to save the first level, not used in the moment
        {
            // omvandla 2D array till en jagged array (för att json inte stödjer det)
            int[][] jaggedArray = ConvertToJaggedArray(level);

            // omvandla jagged array to JSON
            string json = JsonSerializer.Serialize(jaggedArray);

            // Spara json på filen
            File.WriteAllText(Path.Combine(prePath, $"{path}.json"), json);

            Console.WriteLine($"JSON saved to {path}");
        }

        static public void Load(int i) //Method to load levels
        {
            ClearLevel(); //Clear everything in level
            if (levels.ContainsKey(i)) //Check so i is valid
            {
                currentLevel = i; //i is the currentLevel

                int[,]? level = LoadLevel(levels[i]); //Load from path
                if (level != null) //Check so it is fine
                {
                    levelSize = new(level.GetLength(1), level.GetLength(0)); //The level sets the levelSize
                    SpawEntitiesInLevel(level); //Spawn in all entitys
                }
            }
        }
        public static int[,]? LoadLevel(string path)
        {
            try
            {
                // Load JSON string from file
                string json = File.ReadAllText(Path.Combine(prePath, $"{path}.json"));

                // Deserialize JSON to jagged array
                int[][]? jaggedArray = JsonSerializer.Deserialize<int[][]>(json, options);

                // Convert jagged array to 2D array
                if (jaggedArray != null)
                {
                    int[,] level = ConvertTo2DArray(jaggedArray);

                    if (level != null)
                    {
                        System.Console.WriteLine($"{path} loaded");
                        return level;
                    }
                    else
                    {
                        Console.WriteLine($"Deserialization failed, {path} was not loaded");
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error loading {path}: {e.Message}");
                return null;
            }
        }

        static void SpawEntitiesInLevel(int[,] level) //Spaw in all entitys
        {
            //loop the whole grid
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    if (level[x, y] > 0) //0 == air
                    {
                        GameEntity? entity = GetEntityInstance(level[x, y]); //Use method to create a instace from type
                        if (entity != null) //Check so it worked, otherwise it will become air
                        {
                            levelEntities.Add(entity); //add entity to tracker list
                            Vector2 spawPos = new Vector2(y, x); //Put it in the right place
                            //Spawn as child of empty
                            EntityManager.SpawnEntity(entity, spawPos, Vector2.One, empty.transform);
                        }
                    }
                }
            }
        }
        static void ClearLevel() //Clear the whole level
        {
            foreach (GameEntity entity in levelEntities)
            {
                EntityManager.DestroyEntity(entity); //Destroy all entitys
            }
            levelEntities.Clear(); //Clear the tracking list
        }
        // Method to spawn GameEntity based on int key
        static GameEntity? GetEntityInstance(int key)
        {
            // Check if the key exists in the dictionary
            if (entitysInLevel.ContainsKey(key))
            {
                Type entityType = entitysInLevel[key];
                // Use reflection to create an instance of the specified type
                GameEntity? newEntity = (GameEntity?)Activator.CreateInstance(entityType);

                if (newEntity == null) { System.Console.WriteLine($"Error, the number {key} is wrong"); }
                // Return the spawned GameEntity
                return newEntity;
            }
            else
            {
                Console.WriteLine($"Entity: {key} is not");
                return null;
            }
        }

        // Convert 2D array to jagged array (Because json is dumb)
        static int[][] ConvertToJaggedArray(int[,] array)
        {
            int rows = array.GetLength(0); //Get int[this,] value
            int cols = array.GetLength(1); //Get int[,that] value

            int[][] jaggedArray = new int[rows][]; //Create a jagged arry and set amount of rows

            for (int x = 0; x < rows; x++) //Loop all rows
            {
                jaggedArray[x] = new int[cols]; //set columes
                for (int y = 0; y < cols; y++) //Loop all columes
                {
                    jaggedArray[x][y] = array[x, y]; //Set all values of jagged array
                }
            }
            return jaggedArray; //return jagged array
        }
        // Convert jagged array to 2D array (Because json is dumb)
        static int[,] ConvertTo2DArray(int[][] jaggedArray)
        {
            int rows = jaggedArray.Length; //Get amount of rows from jagged array
            int cols = jaggedArray[0].Length; //Get amount of columes from jagged array

            int[,] array = new int[rows, cols]; //Create the 2d array

            for (int x = 0; x < rows; x++) //Loop all positions
            {
                for (int y = 0; y < cols; y++)
                {
                    array[x, y] = jaggedArray[x][y]; //Set value at position
                }
            }
            return array; //return the 2d array
        }
    }
}
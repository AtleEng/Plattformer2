using System.Numerics;
using System.Text.Json;

namespace Engine
{
    //Class that handle loading in levels
    public static class LevelEditor
    {
        static GameEntity empty = new(); //Use this entity to store all levelObj in to
        
        static Vector2 levelSize = Vector2.Zero; //LevelSize is determend by the level

        static string prePath = @"Game\Project\Levels\"; //In this folder must all levels be

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

        static void DisplayLevel(int[,] level) //Spaw in all entitys
        {
            //loop the whole grid
            for (int x = 0; x < level.GetLength(0); x++)
            {
                for (int y = 0; y < level.GetLength(1); y++)
                {
                    if (level[x, y] > 0) //0 == air
                    {
                        
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
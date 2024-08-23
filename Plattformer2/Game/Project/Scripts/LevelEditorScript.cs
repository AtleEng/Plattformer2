using System.Numerics;
using CoreEngine;
using UI;
using System.Text.Json;

namespace Engine
{
    //Class that handle loading in levels
    public class LevelEditorScript : Component
    {
        string currentPath = "";
        List<ButtonObject> optionsButtons = new();

        public UIText pathText = new();

        int[,] currentLevel = new int[19, 14];

        List<GameEntity> levelEntitys = new();

        override public void Start() //Creates the "Folder" entity
        {
            SpawnOptionsButtons();
        }
        void SpawnOptionsButtons()
        {
            Dictionary<string, Action> buttonDictionary = new()
            {
                { "Back", () => Core.ChangeRotEntity(new StartScene()) },
                { "Open", () => Open() },
                { "New", () => OpenNew() },
                { "Save", () => Save() },
                { "Play", () => PlayLevel() }
            };

            int i = 0;
            foreach (var keyValuePair in buttonDictionary)
            {
                ButtonObject button = new ButtonObject(keyValuePair.Value);
                button.uIText.text = keyValuePair.Key;
                button.uIText.fontSize = 30;
                button.uIText.gameEntity.transform.position.Y = 5;
                EntityManager.SpawnEntity(button, new Vector2(75 + (150 * i), 25), new Vector2(150, 50), gameEntity.transform);
                i++;
            }
        }
        public void Open()
        {
            currentPath = OpenDialog.GetFile();
            pathText.text = currentPath;
            System.Console.WriteLine(currentPath);

            currentLevel = LoadLevel(currentPath);
            DisplayLevel();
        }
        public void OpenNew()
        {
            currentPath = "";
            pathText.text = currentPath;

            currentLevel = new int[19, 14];
            DisplayLevel();
        }
        public void Save()
        {
            if (currentPath == "")
            {
                currentPath = OpenDialog.GetDirectory();
            }

            LoadingManager.SaveLevel(currentPath, currentLevel);
            pathText.text = currentPath;
        }
        int[,]? LoadLevel(string path)
        {
            try
            {
                // Load JSON string from file
                string json = File.ReadAllText(path);

                // Deserialize JSON to jagged array
                int[][]? jaggedArray = JsonSerializer.Deserialize<int[][]>(json, LoadingManager.options);

                // Convert jagged array to 2D array
                if (jaggedArray != null)
                {
                    int[,] level = LoadingManager.ConvertTo2DArray(jaggedArray);

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

        void DisplayLevel()
        {
            ClearLevel();
            //loop the whole grid
            for (int x = 0; x < currentLevel.GetLength(0); x++)
            {
                for (int y = 0; y < currentLevel.GetLength(1); y++)
                {
                    if (currentLevel[x, y] > 0) //0 == air
                    {
                        GameEntity? entity = GetImageFromIndex(currentLevel[x, y]); //Use method to create a instace from type
                        if (entity != null) //Check so it worked, otherwise it will become air
                        {
                            levelEntitys.Add(entity); //add entity to tracker list
                            Vector2 spawPos = new Vector2(y, x); //Put it in the right place
                            //Spawn as child of empty
                            EntityManager.SpawnEntity(entity, spawPos, Vector2.One);
                        }
                    }
                }
            }
        }
        void ClearLevel() //Clear the whole level
        {
            foreach (GameEntity entity in levelEntitys)
            {
                EntityManager.DestroyEntity(entity); //Destroy all entitys
            }
            levelEntitys.Clear(); //Clear the tracking list
        }
        // Method to spawn GameEntity based on int key
        GameEntity? GetImageFromIndex(int key)
        {
            // Check if the key exists in the dictionary
            if (LoadingManager.entitysInLevel.ContainsKey(key))
            {
                Type entityType = LoadingManager.entitysInLevel[key];
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
        void PlayLevel()
        {

        }
    }
}
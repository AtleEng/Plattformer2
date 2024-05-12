using System;
using System.Numerics;
using System.Text.Json;
using Raylib_cs;
using UI;

namespace Engine
{
    //Class that handle loading in levels
    public class LevelMenuManager : GameEntity
    {
        static string prePath = @"Game\Project\Levels\"; //In this folder must all levels be

        static List<GameEntity> levelEntities = new(); //All Entitys in level

        List<ButtonObject> levelButtons = new();

        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        public override void OnInnit() //Creates the "Folder" entity
        {
            List<List<string>> allFiles = GetFiles();
            SpawnFolderButtons(allFiles);
        }
        public void SpawnFolderButtons(List<List<string>> allFiles)
        {
            int posIndex = 0;
            for (int i = 0; i < allFiles.Count; i++)
            {
                int index = i; // Create a local variable to capture the current value of i
                if (allFiles[index].Count > 0)
                {
                    System.Console.WriteLine(index);
                    Action action = () => SpawnFilesButtons(allFiles[index]);
                    ButtonObject folderButton = new ButtonObject(action);

                    EntityManager.SpawnEntity(folderButton, new Vector2(550, (100 * posIndex) + 50), new Vector2(300, 100), transform);
                    posIndex++;
                }
            }
        }
        public void SpawnFilesButtons(List<string> folder)
        {
            for (int i = 0; i < levelButtons.Count; i++)
            {
                EntityManager.DestroyEntity(levelButtons[i]);
            }
            levelButtons.Clear();

            int posIndex = 0;
            for (int i = 0; i < folder.Count; i++)
            {
                int index = i; // Create a local variable to capture the current value of i
                if (folder[index] != null)
                {
                    Action action = () => LoadLevel(folder[index]);
                    ButtonObject levelButton = new ButtonObject(action);

                    // Split the directory path by directory separator
                    string[] directories = folder[index].Split(Path.DirectorySeparatorChar);

                    // The "Levels" directory will be the second-to-last element in the array
                    string levelsDirectoryName = directories[directories.Length - 1];

                    if (levelsDirectoryName.Contains(".json"))
                    {
                        levelsDirectoryName = levelsDirectoryName.Split(".")[0];
                    }

                    levelButton.uIText.text = levelsDirectoryName;

                    EntityManager.SpawnEntity(levelButton, new Vector2(900, (100 * posIndex) + 50), new Vector2(300, 100), transform);
                    levelButtons.Add(levelButton);
                    posIndex++;
                }
            }
        }

        public void LoadLevel(string path)
        {
            System.Console.WriteLine($"Loading file: {path}");
        }

        public void OpenBrowse()
        {
            // Specify the directory path
            string directoryPath = @"Game\Project\Levels\";

            // Get all folders in the directory
            string[] folders = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);

            // Display the list of folders and their files
            foreach (string folder in folders)
            {
                Console.WriteLine($"Files in Folder: {folder}");

                // Get all JSON files in the current folder
                string[] jsonFiles = Directory.GetFiles(folder, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    Console.WriteLine(folder + @"\" + jsonFile);
                }
            }
        }
        public List<List<string>> GetFiles()
        {
            List<List<string>> files = new List<List<string>>();
            // Specify the directory path
            string directoryPath = @"Game\Project\Levels\";

            // Get all folders in the directory
            string[] folders = Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories);

            // Display the list of folders and their files
            foreach (string folder in folders)
            {
                Console.WriteLine($"Files in Folder: {folder}");

                List<string> filesInFolder = new();

                // Get all JSON files in the current folder
                string[] jsonFiles = Directory.GetFiles(folder, "*.json");

                foreach (string jsonFile in jsonFiles)
                {
                    Console.WriteLine(jsonFile);

                    filesInFolder.Add(jsonFile);
                }
                files.Add(filesInFolder);
            }
            return files;
        }
    }
}
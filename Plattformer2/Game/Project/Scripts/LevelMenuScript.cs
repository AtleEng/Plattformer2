using System.Numerics;
using CoreEngine;

namespace Engine
{
    //Class that handle loading in levels
    public class LevelMenuScript : Component
    {
        List<ButtonObject> folderButtons = new();
        List<ButtonObject> levelButtons = new();

        override public void Start() //Creates the "Folder" entity
        {
            SpawnOptionsButtons();

            List<List<string>> allFiles = GetFiles();
            SpawnFolderButtons(allFiles);
        }
        void SpawnOptionsButtons()
        {
            ButtonObject button = new ButtonObject(() => Core.ChangeRotEntity(new StartScene()));
            button.uIText.text = "Back";
            EntityManager.SpawnEntity(button, new Vector2(175, 725), new Vector2(300, 100), gameEntity.transform);
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

                    // Split the directory path by directory separator
                    string[] directories = allFiles[index][0].Split(Path.DirectorySeparatorChar);

                    // The "Levels" directory will be the second-to-last element in the array
                    string folderDirectoryName = directories[directories.Length - 2];

                    folderButton.uIText.text = folderDirectoryName;

                    EntityManager.SpawnEntity(folderButton, new Vector2(550, (125 * posIndex) + 75), new Vector2(300, 100), gameEntity.transform);
                    folderButtons.Add(folderButton);
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

                    EntityManager.SpawnEntity(levelButton, new Vector2(900, (125 * posIndex) + 75), new Vector2(300, 100), gameEntity.transform);
                    levelButtons.Add(levelButton);
                    posIndex++;
                }
            }
        }

        public void LoadLevel(string path)
        {
            System.Console.WriteLine($"Loading file: {path}");
            PlayScene playScene = new PlayScene(path);
            Core.ChangeRotEntity(playScene);
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
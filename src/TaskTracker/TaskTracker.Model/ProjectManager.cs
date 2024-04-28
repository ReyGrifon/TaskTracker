using Newtonsoft.Json;

namespace TaskTracker.Model
{
    internal class ProjectManager
    {
        private static string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _path = $"{_appDataPath}\\TaskTracker";
        private static string _fileName = "\\TaskTracker.notes";
        public string Path
        {
            set
            {
                _path = value;
            }
            get
            {
                return _path;
            }
        }
        /// <summary>
        /// Сохраняет проект по пути _folderPath.
        /// </summary>
        /// <param name="project"></param>
        public void SaveProject(Project project)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            if (!File.Exists(_path + _fileName))
            {
                var fileStream = File.Create(_path + _fileName);
                fileStream.Close();
            }
            File.WriteAllText(_path + _fileName, JsonConvert.SerializeObject(project));
        }

        /// <summary>
        /// Загружает данные из пути _folderPath.
        /// </summary>
        /// <returns></returns>
        public Project LoadProject()
        {
            try
            {
                Project project;
                if (!File.Exists(_path + _fileName))
                {
                    return new Project();
                }
                using (StreamReader reader = new StreamReader(_path + _fileName))
                {
                    string jsonData = reader.ReadToEnd();
                    project = JsonConvert.DeserializeObject<Project>(jsonData);
                }
                return project ?? new Project();
            }
            catch (Exception e)
            {
                return new Project();
            }

        }
    }
}

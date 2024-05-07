using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace TaskTracker.Model
{
    public static class JsonTaskConvert
    {
        private static string _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string _path = $"{_appDataPath}\\TaskTracker";
        private static string _fileName = "\\TaskTracker.notes";
        private static string _filePath = _path + _fileName;
        public static string Path
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
        public static void SaveProject(List<Task> project)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            if (!File.Exists(_filePath))
            {
                var fileStream = File.Create(_filePath);
                fileStream.Close();
            }
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(project));
        }
        /// <summary>
        /// Загружает данные из пути _folderPath.
        /// </summary>
        /// <returns></returns>
        public static List<Task> LoadProject()
        {
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Task>>(json);
            }
            catch (Exception e)
            {
                return new List<Task>();
            }

        }
    }
}

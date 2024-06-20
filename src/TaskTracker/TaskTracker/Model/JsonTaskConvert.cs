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
        private static string _fileTasksName = "\\TaskTrackerTasks.notes";
        private static string _fileBranchName = "\\TaskTrackerBranch.notes";
        private static string _filePath;
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
        public static void SaveProject(object project)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            if (project.GetType() == typeof(List<Task>)) 
            {
                _filePath = _path + _fileTasksName;
            }
            else
            {
                _filePath = _path + _fileBranchName;
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
        public static object LoadProject(Type projectType)
        {
            try
            {
                if (projectType == typeof(Task))
                {
                    _filePath = _path + _fileTasksName;
                }
                else
                {
                    _filePath = _path + _fileBranchName;
                }
                string json = File.ReadAllText(_filePath);
                if (projectType == typeof(Task))
                {
                    return JsonConvert.DeserializeObject<List<Task>>(json);
                }
                else
                {
                    return JsonConvert.DeserializeObject<List<TaskBranch>>(json);
                }
            }
            catch (Exception e)
            {
                if (projectType == typeof(Task))
                {
                    return new List<Task>();
                }
                else
                {
                    return new List<TaskBranch>();
                }
            }

        }
    }
}

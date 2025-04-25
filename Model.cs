using System;
using System.Collections.Generic;
using System.IO;

public class Model
{
  public string DirectoryPath1 { get; set; }
  public string DirectoryPath2 { get; set; }

  public Model(string dir1, string dir2)
  {
    DirectoryPath1 = dir1;
    DirectoryPath2 = dir2;
  }

  private Dictionary<string, FileInfo> GetFiles(string directory)
  {
    var files = new Dictionary<string, FileInfo>();
    foreach (var file in new DirectoryInfo(directory).GetFiles("*", SearchOption.AllDirectories))
    {
      var relativePath = file.FullName.Substring(DirectoryPath1.Length + 1);
      files[relativePath] = file;
    }
    return files;
  }

  public List<string> CompareDirectories()
  {
    var changes = new List<string>();

    var filesDir1 = GetFiles(DirectoryPath1);
    var filesDir2 = GetFiles(DirectoryPath2);

    foreach (var file in filesDir1)
    {
      if (!filesDir2.ContainsKey(file.Key))
      {
        changes.Add($"Файл \"{file.Key}\" создан");
      }
      else if (filesDir2[file.Key].LastWriteTime != file.Value.LastWriteTime)
      {
        changes.Add($"Файл \"{file.Key}\" изменен");
      }
    }

    foreach (var file in filesDir2)
    {
      if (!filesDir1.ContainsKey(file.Key))
      {
        changes.Add($"Файл \"{file.Key}\" удален");
      }
    }

    return changes;
  }

  public void SynchronizeDirectories()
  {
    var filesDir1 = GetFiles(DirectoryPath1);
    var filesDir2 = GetFiles(DirectoryPath2);

    foreach (var file in filesDir1)
    {
      var targetPath = Path.Combine(DirectoryPath2, file.Key);
      if (!filesDir2.ContainsKey(file.Key) || filesDir2[file.Key].LastWriteTime < file.Value.LastWriteTime)
      {
        File.Copy(file.Value.FullName, targetPath, true);
      }
    }

    foreach (var file in filesDir2)
    {
      var targetPath = Path.Combine(DirectoryPath1, file.Key);
      if (!filesDir1.ContainsKey(file.Key) || filesDir1[file.Key].LastWriteTime < file.Value.LastWriteTime)
      {
        File.Copy(file.Value.FullName, targetPath, true);
      }
    }
  }
}
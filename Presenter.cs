using System;
using System.Collections.Generic;

public class Presenter
{
    private readonly View _view;
    private Model _model;

    public Presenter(View view)
    {
        _view = view;
        _view.SyncRequested += OnSyncRequested;
    }

    private void OnSyncRequested(object sender, EventArgs e)
    {
        try
        {
            var dir1 = _view.DirectoryPath1;
            var dir2 = _view.DirectoryPath2;

            if (!System.IO.Directory.Exists(dir1) || !System.IO.Directory.Exists(dir2))
            {
                _view.ShowMessage("Одна из указанных директорий не существует.");
                return;
            }

            _model = new Model(dir1, dir2);

            var changes = _model.CompareDirectories();
            _view.ShowChanges(changes);

            if (changes.Count > 0)
            {
                _model.SynchronizeDirectories();
                _view.ShowMessage("Синхронизация завершена.");
            }
            else
            {
                _view.ShowMessage("Директории уже синхронизированы.");
            }
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"Ошибка: {ex.Message}");
        }
    }
}
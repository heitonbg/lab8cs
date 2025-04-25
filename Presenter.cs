using System;

public class Presenter
{
  private readonly Model _model;
  private readonly IDirectorySyncView _view;

  public Presenter(Model model, IDirectorySyncView view)
  {
    _model = model;
    _view = view;
  }

  public void CompareAndSynchronize()
  {
    try
    {
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
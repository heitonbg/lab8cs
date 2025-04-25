using System;
using System.Collections.Generic;
using System.Windows.Forms;

public interface IDirectorySyncView
{
  void ShowChanges(List<string> changes);
  void ShowMessage(string message);
}

public class View : Form, IDirectorySyncView
{
  private TextBox _logTextBox;
  private Button _syncButton;
  private TextBox _dir1TextBox;
  private TextBox _dir2TextBox;
  private Label _dir1Label;
  private Label _dir2Label;

  public View()
  {
    InitializeComponents();
  }

  private void InitializeComponents()
  {
    this.Text = "Синхронизация директорий";
    this.Size = new System.Drawing.Size(600, 400);

    _dir1Label = new Label { Text = "Директория 1:", Location = new System.Drawing.Point(10, 20), AutoSize = true };
    _dir1TextBox = new TextBox { Location = new System.Drawing.Point(100, 20), Width = 300 };

    _dir2Label = new Label { Text = "Директория 2:", Location = new System.Drawing.Point(10, 50), AutoSize = true };
    _dir2TextBox = new TextBox { Location = new System.Drawing.Point(100, 50), Width = 300 };

    _syncButton = new Button { Text = "Синхронизировать", Location = new System.Drawing.Point(10, 80), Width = 120 };
    _syncButton.Click += SyncButton_Click;

     _logTextBox = new TextBox
    {
      Location = new System.Drawing.Point(10, 120),
      Width = 560,
      Height = 200,
      Multiline = true,
      ReadOnly = true,
      ScrollBars = ScrollBars.Vertical
    };

    this.Controls.Add(_dir1Label);
    this.Controls.Add(_dir1TextBox);
    this.Controls.Add(_dir2Label);
    this.Controls.Add(_dir2TextBox);
    this.Controls.Add(_syncButton);
    this.Controls.Add(_logTextBox);
  }

  public void ShowChanges(List<string> changes)
  {
    _logTextBox.Clear();
    foreach (var change in changes)
    {
      _logTextBox.AppendText(change + Environment.NewLine);
    }
  }

  public void ShowMessage(string message)
  {
    MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
  }

  private void SyncButton_Click(object sender, EventArgs e)
  {
    var dir1 = _dir1TextBox.Text;
    var dir2 = _dir2TextBox.Text;

    if (!System.IO.Directory.Exists(dir1) || !System.IO.Directory.Exists(dir2))
    {
      ShowMessage("Одна из указанных директорий не существует.");
      return;
    }

    var model = new Model(dir1, dir2);
    var presenter = new Presenter(model, this);
    presenter.CompareAndSynchronize();
  }
}
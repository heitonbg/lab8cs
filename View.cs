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
  private TextBox logTextBox;
  private Button syncButton;
  private TextBox dir1TextBox;
  private TextBox dir2TextBox;
  private Label dir1Label;
  private Label dir2Label;

  public View()
  {
    InitializeComponents();
  }

  private void InitializeComponents()
  {
    this.Text = "Синхронизация директорий";
    this.Size = new System.Drawing.Size(600, 400);

    dir1Label = new Label { Text = "Директория 1:", Location = new System.Drawing.Point(10, 20), AutoSize = true };
    dir1TextBox = new TextBox { Location = new System.Drawing.Point(100, 20), Width = 300 };

    dir2Label = new Label { Text = "Директория 2:", Location = new System.Drawing.Point(10, 50), AutoSize = true };
    dir2TextBox = new TextBox { Location = new System.Drawing.Point(100, 50), Width = 300 };

    syncButton = new Button { Text = "Синхронизировать", Location = new System.Drawing.Point(10, 80), Width = 120 };
    syncButton.Click += SyncButton_Click;

    logTextBox = new TextBox
    {
      Location = new System.Drawing.Point(10, 120),
      Width = 560,
      Height = 200,
      Multiline = true,
      ReadOnly = true,
      ScrollBars = ScrollBars.Vertical
    };

    this.Controls.Add(dir1Label);
    this.Controls.Add(dir1TextBox);
    this.Controls.Add(dir2Label);
    this.Controls.Add(dir2TextBox);
    this.Controls.Add(syncButton);
    this.Controls.Add(logTextBox);
  }

  public void ShowChanges(List<string> changes)
  {
    logTextBox.Clear();
    foreach (var change in changes)
    {
      logTextBox.AppendText(change + Environment.NewLine);
    }
  }

  public void ShowMessage(string message)
  {
    MessageBox.Show(message, "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
  }

  private void SyncButton_Click(object sender, EventArgs e)
  {
    var dir1 = dir1TextBox.Text;
    var dir2 = dir2TextBox.Text;

    if (!System.IO.Directory.Exists(dir1) || !System.IO.Directory.Exists(dir2))
    {
      ShowMessage("Одна из указанных директорий не существует.");
      return;
    }

    var model = new DirectorySyncModel(dir1, dir2);
    var presenter = new DirectorySyncPresenter(model, this);
    presenter.CompareAndSynchronize();
  }
}
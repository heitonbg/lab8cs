using System;
using System.Windows.Forms;

static class Program
{
  [STAThread]
  static void Main()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);

    var view = new View();
    Application.Run(view);
  }
}
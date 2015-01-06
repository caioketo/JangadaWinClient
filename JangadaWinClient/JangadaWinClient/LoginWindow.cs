using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TomShane.Neoforce.Controls;

namespace JangadaWinClient
{
    public class LoginWindow
    {
        Window loginWindow;
        Manager manager;
        TextBox loginBox;
        TextBox passBox;
        TomShane.Neoforce.Controls.Console consoleWindow;

        public LoginWindow(Manager manager)
        {
            this.manager = manager;
            consoleWindow = (TomShane.Neoforce.Controls.Console)manager.GetControl("ConsoleWindow");
        }

        public void Init()
        {
            loginWindow = new Window(manager);
            loginWindow.Init();
            loginWindow.Text = "Login";
            loginWindow.Width = 250;
            loginWindow.Height = 250;
            loginWindow.Center();

            Label loginLbl = new Label(manager);
            loginLbl.Init();
            loginLbl.Text = "User:";
            loginLbl.Top = 10;
            loginLbl.Left = 10;

            loginBox = new TextBox(manager);
            loginBox.Init();
            loginBox.Top = 30;
            loginBox.Left = 10;
            loginBox.Width = 200;

            Label passLbl = new Label(manager);
            passLbl.Init();
            passLbl.Text = "Password:";
            passLbl.Top = 60;
            passLbl.Left = 10;

            passBox = new TextBox(manager);
            passBox.Init();
            passBox.Top = 80;
            passBox.Left = 10;
            passBox.Width = 200;
            passBox.Mode = TextBoxMode.Password;

            Button enterBtn = new Button(manager);
            enterBtn.Init();
            enterBtn.Top = 120;
            enterBtn.Left = 70;
            enterBtn.Width = 100;
            enterBtn.Text = "Login";
            enterBtn.Click += new TomShane.Neoforce.Controls.EventHandler(this.loginBtn_click);

            loginWindow.Add(enterBtn);
            loginWindow.Add(loginLbl);
            loginWindow.Add(loginBox);
            loginWindow.Add(passLbl);
            loginWindow.Add(passBox);

            manager.Add(loginWindow);
            loginWindow.Hide();
        }

        public void Show()
        {
            loginWindow.Show();
            loginBox.Focused = true;
            
        }

        public void Hide()
        {
            loginWindow.Hide();
        }

        private void loginBtn_click(object sender, TomShane.Neoforce.Controls.EventArgs e)
        {
            consoleWindow.MessageBuffer.Add(new ConsoleMessage("Teste", "Attempting to login...", 0));
            consoleWindow.MessageBuffer.Add(new ConsoleMessage("Teste", "User: " + loginBox.Text + " Pass: " + passBox.Text, 0));
        }
    }
}

using Jangada;
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
        Window connectingWindow;
        Manager manager;
        TextBox loginBox;
        TextBox passBox;
        Panel charList;

        public LoginWindow(Manager manager)
        {
            this.manager = manager;
        }

        public void Init()
        {
            charList = new Panel(manager);
            charList.Width = 200;
            charList.Init();
            charList.Left = (manager.ScreenWidth / 2) - (charList.Width / 2);
            charList.Top = (manager.ScreenHeight - charList.Height) / 2;
            charList.Hide();
            manager.Add(charList);

            connectingWindow = new Window(manager);
            connectingWindow.Init();
            connectingWindow.Text = "";
            connectingWindow.Width = 250;
            connectingWindow.Height = 100;
            connectingWindow.Center();
            connectingWindow.Hide();

            Label connectingLbl = new Label(manager);
            connectingLbl.Text = "Connecting ...";
            connectingLbl.Top = 10;
            connectingLbl.Left = 10;
            connectingLbl.Width = 250;
            connectingLbl.Init();
            connectingWindow.Add(connectingLbl);

            manager.Add(connectingWindow);

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
            loginWindow.Hide();
            connectingWindow.Show();
            Jangada.getInstance().AddLog("Attempting to connect...");
            TCPClient.getInstance().StartConnect();
            Jangada.getInstance().AddLog("Connected.");
            Jangada.getInstance().AddLog("Sending LoginPacket (User: " + loginBox.Text + " Pass: " + passBox.Text + ")");
            Networkmessage.Builder newMessage = Networkmessage.CreateBuilder();
            newMessage.LoginPacket = LoginPacket.CreateBuilder()
                 .SetLogin(loginBox.Text)
                 .SetPassword(passBox.Text)
                 .Build();
            newMessage.Type = Networkmessage.Types.Type.LOGIN;
            Messages messagesToSend = Messages.CreateBuilder().AddNetworkmessage(newMessage.Build()).Build();
            TCPClient.getInstance().Send(messagesToSend);
        }

        public void ShowCharList(List<Character> characters)
        {
            int top = 0;
            foreach (Character character in characters)
            {
                Button btn = new Button(manager);
                btn.Init();
                btn.Text = character.Name + " | " + character.Info;
                btn.Top = top;
                btn.Height = 20;
                btn.Width = 200;
                charList.Add(btn);
                top += 20;
            }
            connectingWindow.Hide();
            charList.Show();
        }
    }
}

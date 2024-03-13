using CloudOSLib;
using CloudOSLib.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Transactions;
using Sys = Cosmos.System;

namespace CloudOS
{
    public class Kernel : Sys.Kernel
    {
        CloudOSLibrary OSLib;

        protected override void BeforeRun()
        {
            OSLib = new();
            OSLib.windowsSystem.enableCursor = true;

            CloudOSWindow loginWindow = new();
            loginWindow.text = "Welcome to CloudOS.";
            loginWindow.width = 300;
            loginWindow.height = 200;
            loginWindow.x = OSLib.windowsSystem.Width / 2 - (loginWindow.width / 2);
            loginWindow.y = OSLib.windowsSystem.Height / 2 - (loginWindow.height / 2);
            TextLabel welcomeText = new();
            welcomeText.Text = "Enter ID and Password to Login to CloudOS.";
            welcomeText.Position = new(25, 25);
            loginWindow.controls.Add(welcomeText);
            Button okBtn = new();
            okBtn.Text = "OK";
            okBtn.Position = new(25, 35);
            okBtn.Size = new(50, 12);
            okBtn.OnClick.AddListener(new((ClickEvent e) => {
                OSLib.windowsSystem.EnableUI = true;
                OSLib.windowsSystem.windows.Remove(loginWindow);
                CloudOSWindow window = new();
                window.text = "CloudOS 테스트 버전";
                window.x = 20;
                window.y = 20;
                TextLabel label = new TextLabel();
                label.Text = "This is label. 글자입니다.";
                label.Position = new System.Drawing.Point(10, 10);
                window.controls.Add(label);
                InputBox inputBox = new InputBox();
                inputBox.Text = "This is input box. 입력 박스입니다.";
                inputBox.Size = new(200, 100);
                inputBox.Position = new(10, 40);
                window.controls.Add(inputBox);
                OSLib.windowsSystem.windows.Add(window);
            }));
            loginWindow.controls.Add(okBtn);
            OSLib.windowsSystem.windows.Add(loginWindow);
        }

        protected override void Run() { }
    }
}

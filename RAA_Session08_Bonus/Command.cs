#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using NLog.Config;
using Slack.Webhooks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NLog.Slack;
using NLog;

#endregion

namespace RAA_Session08_Bonus
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            // write to journal file
            app.WriteJournalComment("This is my first journal comment", true);

            //string journalPath = @"C:\Users\micha\AppData\Local\Autodesk\Revit\Autodesk Revit 2022\Journals";
            string journalPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                @"\Autodesk\Revit\Autodesk Revit 2022\Journals";

            Process.Start(journalPath);

            // step 1. create a channel in Slack for your add-in notifications
            //          call it "Revit Add-in Notifications" or something like that

            // step 2. create a new app in Slack
            //          File > Settings & Administration > Manage Apps
            //          Click "Build" in upper right corner
            //          Click "Create New App" then click "From scratch"
            //          Give your app a name like "Revit Add-in Messenger" and select a workshop it will

            // step 3. install the Slack Webhooks NuGet package
            //          
            //          In Visual Studio, go to Tools > NuGet Package Manager > Manage NuGet Packages for Solution to open the package manager
            //          Search for "slack webhooks" and select the Slack.Webhooks package. 
            //          Select your project in the list and click "Install"

            // step 4. write your Slack connection code
            string slackURL = @"https://hooks.slack.com/services/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";

            //SlackClient slackClient = new SlackClient(slackURL);
            //SlackMessage slackMessage = new SlackMessage();

            //slackMessage.Text = "This is my first Slack message test!!!!";
            //slackClient.Post(slackMessage);

            var config = new LoggingConfiguration();
            var slackTarget = new SlackTarget
            {
                Layout = "${message}",
                WebHookUrl = slackURL,
            };

            config.AddTarget(slackTarget);

            var slackTargetRules = new LoggingRule("*", NLog.LogLevel.Debug, slackTarget);
            config.LoggingRules.Add(slackTargetRules);

            LogManager.Configuration = config;

            Logger.De




            return Result.Succeeded;
        }
    }
}

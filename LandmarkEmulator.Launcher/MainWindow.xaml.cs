using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace LandmarkEmulator.Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IConfiguration Configuration { get; private set; }

        private string authServer = "http://127.0.0.1:5000";
        private string sessionKey = "";
        private DateTime sessionExpires;
        private string landmarkLocation = "";

        private class AuthenticateResponse
        {
            public string SessionKey { get; set; }
            public DateTime SessionKeyExpiration { get; set; }
            public string Message { get; set; }
        }

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("LauncherConfig.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
            authServer = (string)Configuration.GetValue(typeof(string), "Server");

            Btn_Play.Visibility = Visibility.Hidden;
        }

        private async void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            await AttemptAuthentication();
        }

        private async Task AttemptAuthentication()
        {
            Btn_Login.IsEnabled = false;

            HttpClient client = new HttpClient();
            
            /// POST ///
            var response = await client.PostAsync($"{authServer}/authenticate?username={Txt_Username.Text}&password={Txt_Password.Password}", null);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var deserializedObject = JsonConvert.DeserializeObject<AuthenticateResponse>(result);
                if (deserializedObject.SessionKey != null)
                    sessionKey = deserializedObject.SessionKey;

                Btn_Login.Visibility = Visibility.Hidden;
                Btn_Play.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("Failed to Authenticate. Please try again.", "Failed!");

            Btn_Login.IsEnabled = true;

            await Task.CompletedTask;
        }

        private async void Btn_Play_Click(object sender, RoutedEventArgs e)
        {
            if (landmarkLocation == "")
                HandleFileSelect();
            if (landmarkLocation == "")
                return;

            if (sessionExpires < DateTime.Now)
                await AttemptAuthentication();

            Process p = new Process();
            p.StartInfo.FileName = landmarkLocation;
            p.StartInfo.Arguments = $"SessionId={sessionKey} CasSessionId=0 STEAM_ENABLED=0";
            p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(landmarkLocation);
            p.Start();
        }

        public void HandleFileSelect()
        {
            OpenFileDialog? dialog = new OpenFileDialog();
            
            dialog.Filter = "Landmark Applications|Landmark64.exe";

            var result = dialog.ShowDialog();

            if (result == true)
                landmarkLocation = dialog.FileName;
            else
                MessageBox.Show("Landmark Location wasn't set! Please try again.", "Failed!");
        }
    }
}

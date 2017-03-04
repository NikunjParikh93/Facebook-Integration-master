using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using Facebook;

namespace FBDesktopApplication
{

    /// <summary>
    /// Interaction logic for FBDialog.xaml
    /// </summary>
    public partial class FBDialog : Window
    {

        #region Public properties
        public string access_token;
        public DateTime token_expires;
        public string granted_scopes;
        public string denied_scopes;
        public string error;
        public string error_reason;
        public string error_description;
        #endregion
        public FBDialog()
        {
            InitializeComponent();
            this.Closed += FBDialog_Closed;
        }

        private void FBDialog_Closed(object sender, EventArgs e)
        {
        }

        private void Fbdialog_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.AbsolutePath == "/connect/login_success.html")
            {
                if (e.Uri.Query.Contains("error"))
                {
                    ExtractURLInfo("?", e.Uri.Query);
                }
                else
                {                    
                    ExtractURLInfo("#", e.Uri.Fragment);
                }
                this.Close();
            }

        }

        private void Fbdialog_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
           
        }
        
        private void ExtractURLInfo(string inpTrimChar, string urlInfo)
        {
            string fragments = urlInfo.Trim(char.Parse(inpTrimChar)); // Trim the hash or the ? mark
            string[] parameters = fragments.Split(char.Parse("&")); // Split the url fragments / query string 

            // Extract info from url
            foreach (string parameter in parameters)
            {
                string[] name_value = parameter.Split(char.Parse("=")); // Split the input

                switch (name_value[0])
                {
                    case "access_token":
                        this.access_token = name_value[1];
                        break;
                    case "expires_in":
                        double expires = 0;
                        if (double.TryParse(name_value[1], out expires))
                        {
                            this.token_expires = DateTime.Now.AddSeconds(expires);
                        }
                        else
                        {
                            this.token_expires = DateTime.Now;
                        }
                        break;
                    case "granted_scopes":
                        this.granted_scopes = WebUtility.UrlDecode(name_value[1]);
                        //MessageBox.Show(granted_scopes);
                        break;
                    case "denied_scopes":
                        this.denied_scopes = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error":
                        this.error = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_reason":
                        this.error_reason = WebUtility.UrlDecode(name_value[1]);
                        break;
                    case "error_description":
                        this.error_description = WebUtility.UrlDecode(name_value[1]);
                        break;
                    default:
                        break;
                }
            }
        }
    

    private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string returnUrl = WebUtility.UrlEncode("https://www.facebook.com/connect/login_success.html");
            string scopes = WebUtility.UrlDecode("user_photos,user_videos,user_likes,publish_actions");//WebUtility.UrlEncode(ConfigurationManager.AppSettings["scope"]);
            string appID = ConfigurationManager.AppSettings["appid"];
            string url = String.Format
                ("https://www.facebook.com/dialog/oauth?client_id={0}&redirect_uri={1}&response_type=token%2Cgranted_scopes&scope={2}&display=popup", appID, returnUrl, scopes);
            FbdialogWebBrowser.Navigate(url);

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Facebook;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Model;


namespace FBDesktopApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Facebook.FacebookClient client = null;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FBDialog fbdialog = new FBDialog();
            btnLogin.Visibility = Visibility.Collapsed;
            switch (fbdialog.ShowDialog())
            {
                case true:
                    GetDp(fbdialog);
                    break;
                case false:
                    GetDp(fbdialog);
                    break;
            }
        }

        private void GetDp(FBDialog fbd)
        {
            try
            {
                client = new FacebookClient();
                client.AccessToken = fbd.access_token;
                dynamic me = client.Get("me?fields=picture.width(300),email,name,gender,birthday");
                if (me != null)
                {
                    btnLogin.Visibility = Visibility.Collapsed;
                    txtPost.Visibility = Visibility.Visible;
                    btnPost.Visibility = Visibility.Visible;
                    lblPost.Visibility = Visibility.Visible;
                    rectDetail.Visibility = Visibility.Visible;
                    Uri uri = new Uri(Convert.ToString(me.picture.data.url), UriKind.Absolute);
                    ImageSource imgSource = new BitmapImage(uri);
                    dpImage.Source = imgSource;
                    lblName.Content = "Hello " + me[2] + " !!";
                    lblEmail.Content = "You logged in with email id :" + me[1];
                    lblGender.Content = "Gender: " + me[3];
                    string birthday = "Birthday : " + me[4];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem while connecting to Facebook, please try again..");
                btnLogin.Visibility = Visibility.Collapsed;
            }
        }

        private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            if (client != null)
            {
                try
                {
                    //code for friend list

                    var friendListData = client.Get("/me/friends");
                    JObject friendListJson = JObject.Parse(friendListData.ToString());

                    List<FbUser> fbUsers = new List<FbUser>();
                    foreach (var friend in friendListJson["data"].Children())
                    {
                        FbUser fbUser = new FbUser();
                        fbUser.Id = friend["id"].ToString().Replace("\"", "");
                        fbUser.Name = friend["name"].ToString().Replace("\"", "");
                        fbUsers.Add(fbUser);
                    }






                    // code for post
                    dynamic parameters = new ExpandoObject();
                    parameters.message = txtPost.Text;
                    dynamic me = client.Get("me/permissions");
                    //dynamic me = client.Get("me?fields=picture,id,name,likes");
                    //Dictionary<string, string> data = new Dictionary<string, string>();
                    dynamic result = client.Post("me/feed", parameters);
                    var id = result.id;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error while performing post operation : " + ex.Message);
                }
            }
        }
    }
}

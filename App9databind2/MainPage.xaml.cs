using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App9databind2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // create list
        List<clsDog> _mylist;
       

        public MainPage()
        {
            this.InitializeComponent();
            // add event handler for loading the page
            //this.Loaded += MainPage_Loaded;
         
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // get the global string
            // like fptr in c.  myapp is like a file pointer
            App myapp = (Application.Current) as App;
            tblTitle.Text = myapp.myTitle;

            // if the list is already full, exit
            if (_mylist != null)
                return;

            // instantiate the list
            _mylist = new List<clsDog>();

            // otherwise, fill the list.
            loadLocalData();
            //tblTitle.Text = _mylist.Count().ToString() + " Dog Breeds";
            // binds the list to the listview
            lvDogs.ItemsSource = _mylist;
        }

        private async void loadLocalData()
        {
            // get the text file (json)
            // FILE fptr;  fptr = fopen("myDogs.txt", "r+");
            // printf("Hello \n World");    &amp; &lt; 
            // async and await - 
            var dogFile = await
                Package.Current.InstalledLocation.GetFileAsync("Data\\myDogs.txt");

            // read the contents (all the contents s a string)
            var fileContent = await FileIO.ReadTextAsync(dogFile);

            // convert contents to a json array of generic object
            var dogJson = JsonArray.Parse(fileContent);

            // convert those to objects of type clsDog
            createListOfDogs(dogJson);
        }

        private void createListOfDogs(JsonArray dogJson)
        {
            foreach (var item in dogJson)
            {
                // get the object (like the sender)
                var obj = item.GetObject();
                // create new dog object to map to
                clsDog dog = new clsDog();

                // get each key value pair and sort it to the 
                // appropriate elements of the class
                foreach (var key in obj.Keys)
                {
                    IJsonValue value;
                    if (!obj.TryGetValue(key, out value))
                        continue;

                    switch (key)
                    {
                        case "breed":
                            dog.breed = value.GetString();
                            break;
                        case "origin":
                            dog.origin = value.GetString();
                            break;
                        case "category":
                            dog.category = value.GetString();
                            break;
                        case "activity":
                            dog.activity = value.GetString();
                            break;
                        case "grooming":
                            dog.grooming = value.GetString();
                            break;
                        case "image":
                            dog.imgBreed = value.GetString();
                            break;
                    } // end switch
                } // end foreach (var key in obj.Keys)

                _mylist.Add(dog);

            } // end foreach (var item in array)
        }

        private void StackPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // show some stuff on the screen for that dog.
            tblBreed.Text = _mylist[lvDogs.SelectedIndex].breed;
            tblActivity.Text = _mylist[lvDogs.SelectedIndex].activity;
            tblCategory.Text = _mylist[lvDogs.SelectedIndex].category;
            tblGrooming.Text = _mylist[lvDogs.SelectedIndex].grooming;

            // add an image using a default if it doesn't exist.
            string imageFileName = "ms-appx:///Images/images.jpe";
            // need the images in a folder
            // create a uri with a path to the image
            // ms-appx:///Images/cairn.jpg
            // check if the image selected exists.
            // Images/filename.jpg
            if (File.Exists(_mylist[lvDogs.SelectedIndex].imgBreed))
            {
                imageFileName = "ms-appx:///" +
                _mylist[lvDogs.SelectedIndex].imgBreed;
            }

            Uri myuri = new Uri(imageFileName, UriKind.Absolute);
            // create a bitmap image with that uri
            BitmapImage myBitmap = new BitmapImage(myuri);
            // set the source of imgOne to the bitmap.
            imgOne.Source = myBitmap;

            fpFlyoutDetails.Visibility = Visibility.Visible;

        }

        private void FlyoutPresenter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutPresenter curr = (FlyoutPresenter)sender;
            if( curr.Visibility == Visibility.Visible )
            {
                curr.Visibility = Visibility.Collapsed;
            }

        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // goto the next page.
            this.Frame.Navigate(typeof(Page2));
        }
    }
}

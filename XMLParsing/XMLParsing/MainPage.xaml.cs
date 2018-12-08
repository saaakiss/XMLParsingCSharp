using HtmlAgilityPack;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using XMLParsing.Model;

namespace XMLParsing
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GetXml();
        }

        public async void GetXml()
        {
            if (NetworkCheck.IsInternet())
            {
                Uri geturi = new Uri("http://demo.msensis.com/paokbc/lineup.xml");
                HttpClient client = new HttpClient(new NativeMessageHandler());
                HttpResponseMessage responseGet = await client.GetAsync(geturi);
                string response = await responseGet.Content.ReadAsStringAsync();

                List<Athlete> AthleteList = new List<Athlete>();
                XDocument doc = XDocument.Parse(response);
                foreach(var item in doc.Descendants("athlete"))
                {
                    Achievements a = new Achievements()
                    {
                        Achievement = "N/A"
                    };

                    Athlete itemAthl = new Athlete(a);

                    itemAthl.Label = item.Element("label").Value.ToString();
                    itemAthl.Number = item.Element("number").Value.ToString().Trim();
                    itemAthl.FullName = item.Element("fullname").Value.ToString().Trim();
                    itemAthl.Nationality = item.Element("nationality").Value.ToString().Trim();
                    itemAthl.Position = item.Element("position").Value.ToString().Trim();
                    itemAthl.Height = item.Element("height").Value.ToString().Trim();
                    itemAthl.Birthday = item.Element("birthday").Value.ToString().Trim();
                    itemAthl.ImgUrl = item.Element("img_url").Value.ToString().Trim();

                    //Debug.WriteLine("-----------------------------------");
                    try
                    {
                        var html = new HtmlDocument();
                        html.LoadHtml(item.Element("achievements").Value.ToString());
                        var ps = html.DocumentNode.Descendants("p");
                        string noHTML="";
                        foreach (var p in ps)
                        {
                            var textContent = p.InnerText;
                            noHTML = noHTML + " " + Regex.Replace(textContent, @"<[^>]+>|&nbsp;", "").Trim().ToString();
                        }

                        //Debug.WriteLine(noHTML);
                        itemAthl.AthlAchievements.Achievement = noHTML;
                        //Debug.WriteLine(itemAthl.AthlAchievements.Achievement);

                    }
                    catch
                    {
                        itemAthl.AthlAchievements.Achievement = "N/A";
                        //Debug.WriteLine(itemAthl.AthlAchievements.Achievement);
                    }

                    AthleteList.Add(itemAthl);

                }

                /*Debug.WriteLine("--------------------------");
                foreach (var athlete in AthleteList)
                {
                    
                    Debug.WriteLine(athlete.AthlAchievements.Achievement);
                }*/

                listviewAthletes.ItemsSource = AthleteList;

            }
            else
            {

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("XmlParsing!", "No network is available.", "Ok");
                });
            }

            ProgressLoader.IsRunning = false;



        }
    }
}

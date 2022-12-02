using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;


namespace beanngservereditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		public IDictionary<string, string> maps = new Dictionary<string, string>();

		public IDictionary<string, string> fileContent= new Dictionary<string, string>();
		public MainWindow()
		{
			InitializeComponent();
			maps.Add("Automation Test Track","automation_test_track");
			maps.Add("Cliff", "cliff");
			maps.Add("Derby Arenas", "derby_arenas");
			maps.Add("East Coast, USA", "east_coast_usa");
			maps.Add("ETK Driver Experience Center","etk_driver_experience-center");
			maps.Add("Grid, Small, Pure", "grid_small_pure");
			maps.Add("Grid V2", "gridmap_v2");
			maps.Add("Hirochi Raceway", "hirochi_raceway");
			maps.Add("Industrial Site", "indutrial_site");
			maps.Add("Italy", "italy");
			maps.Add("Jungle Rock Island", "jungle_rock_island");
			maps.Add("Small Island, USA", "small_island_usa");
			maps.Add("Utah, USA", "utah_usa");
			maps.Add("West Coast, USA", "west_coast_usa");

			foreach (KeyValuePair<string, string> kvp in maps)
			{ 
			  cbxMap.Items.Add(kvp.Key);
			}

			fileContent.Add("ServerName","");
			fileContent.Add("AuthKey","");
			fileContent.Add("MaxCars", "");
			fileContent.Add("MaxPlayers", "");
			fileContent.Add("Map", "");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{ 
			string sFilePath = "";
			string sFileContent = "";
			OpenFileDialog file = new OpenFileDialog();
			if (file.ShowDialog() == true)
			{
				sFilePath = file.FileName;
				txtFilePath.Text = sFilePath;
			}

			if (File.Exists(sFilePath))
			{ 
			   readFile(sFilePath);  
			}
		}


		private void readFile(string sFilePath)
		{ 
		  string[] lines = File.ReadAllLines(sFilePath);

			foreach (string line in lines)
			{
				if (line.Contains("Name"))
				{ 
				  fileContent["ServerName"] = extractText(line);
					txtServerName.Text = fileContent["ServerName"];
				}
				if (line.Contains("AuthKey"))
				{
					fileContent["AuthKey"] = extractText(line);
					txtAuthKey.Text = fileContent["AuthKey"];
				}
				if (line.Contains("MaxCars"))
				{
					fileContent["MaxCars"] = extractText(line);
					txtMax_Cars.Text = fileContent["MaxCars"];
				}
				if (line.Contains("MaxPlayers"))
				{
					fileContent["MaxPlayers"] = extractText(line);
					txtMax_Players.Text = fileContent["MaxPlayers"];
				}
				if (line.Contains("Map"))
				{
					fileContent["Map"] = extractMap(line);
					cbxMap.SelectedItem = fileContent["Map"];
				}

			}
		}

		private string extractText(string sLine)
		{ 
			string text = "";
			bool start = true;
			int iStart = 0;
			int iEnd = 0;
			if (sLine.Contains('"'))
			{
			  iStart = sLine.IndexOf('"')+1;
        text = sLine.Remove(0,iStart);
				text = text.Remove(text.LastIndexOf('"'));
			}
			else
			{
				for (int i = 0; i < sLine.Length; i++)
				{
					if (Char.IsDigit(sLine[i]))
					{ 
					  text = sLine[i].ToString();
					}
				}				
			}
			return text;
		}


		private string extractMap(string sLine)
		{ 
		  string text = "None";
			foreach (KeyValuePair<string, string> kvp in maps)
			{
				if (sLine.Contains(kvp.Value))
				{ 
				  text = kvp.Key;
				}
			}
			return text;
		}

		private void btnSubmit_Click(object sender, RoutedEventArgs e)
		{
			string[] lines = File.ReadAllLines(txtFilePath.Text);
			bool bFlag = true;
			string sServerName = "";
			string sAuthKey = "";
			string sMaxCars = "";
			string sMaxPlayers = "";
			string sMap = "";
			try
			{
			  sServerName = txtServerName.Text;
			}
			catch (Exception ex)
			{ 
			  MessageBox.Show(ex.Message); 
				bFlag = false;
			}
			/////////
			try
			{
			  sAuthKey = txtAuthKey.Text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				bFlag = false;
			}
			/////////
			try
			{
			  sMaxCars = txtMax_Cars.Text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				bFlag = false;
			}
			/////////
			try
			{
			  sMaxPlayers = txtMax_Players.Text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				bFlag = false;
			}
			/////////
			foreach (KeyValuePair<string, string> kvp in maps)
			{
				if (cbxMap.SelectedItem.Equals(kvp.Key))
				{
				 sMap = kvp.Value;
				}
			}
			/////////
			if (bFlag)
			{
				string sServerLine = "Name = \"" + sServerName + "\"";
				string sAuthLine = "AuthKey = \"" + sAuthKey + "\"";
				string sMaxCarsLine = "MaxCars = " + sMaxCars.ToString();
				string sMaxPlayersLine = "MaxPlayers =" + sMaxPlayers;
				string sMapLine = "Map = /levels/" + sMap + "/info.json";

				for (int h = 4; h < lines.Length; h++)
				{
					if (lines[h].Contains("Name"))
					{
						lines[h] = sServerLine;
					}
					if (lines[h].Contains("AuthKey ="))
					{
						lines[h] = sAuthLine;
					}
					if (lines[h].Contains("MaxCars"))
					{
						lines[h] = sMaxCarsLine;
					}
					if (lines[h].Contains("MaxPlayers"))
					{
						lines[h] = sMaxPlayersLine;
					}
					if (lines[h].Contains("Map"))
					{
						lines[h] = sMapLine;
					}

				  File.Delete(txtFilePath.Text);
					File.WriteAllLines(txtFilePath.Text,lines);

				}
			}

		}
	}
}

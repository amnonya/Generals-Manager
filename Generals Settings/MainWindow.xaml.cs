﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Generals_Settings
{
    /*
    AntiAliasing = 1
    DrawScrollAnchor = yes
    MoveScrollAnchor = yes
    UnlockMaxFPS = yes
    UseCamera = yes
    SaveCamera = yes
    */


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The Generals data folder name.
        /// </summary>
        private const string GENERALS_FOLDER = "Command and Conquer Generals Data";

        /// <summary>
        /// The options file name.
        /// </summary>
        private const string OPTIONS_FILE_NAME = "Options.ini";

        /// <summary>
        /// The Generals - Zero Hour data folder name.
        /// </summary>
        private const string ZERO_HOUR_FOLDER = "Command and Conquer Generals Zero Hour Data";

        public string Version
        {
            get
            {
                return string.Format("Version {0} (Game Version 1.04)", Assembly.GetEntryAssembly().GetName().Version.ToString(2));
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            string folderName = string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                (/*rbtnGenerals.Checked ? GENERALS_FOLDER :*/ ZERO_HOUR_FOLDER));

            string fileName = string.Format("{0}\\{1}", folderName, OPTIONS_FILE_NAME);
            DataContext = LoadINIFile(fileName);
            FillComboBoxes();
        }

        private static Dictionary<string, string> LoadINIFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            Dictionary<string, string> iniFile = new Dictionary<string, string>(lines.Length);
            foreach (string line in lines)
            {
                string[] split = line.Split(new string[] { " = " }, StringSplitOptions.None);
                iniFile.Add(split[0], split[1]);
            }
            return iniFile;
        }

        private static void SaveINIFile(Dictionary<string, string> dict, string path)
        {
            List<string> lines = new List<string>(dict.Count);
            foreach (KeyValuePair<string, string> kvp in dict)
            {
                lines.Add(string.Format("{0} = {1}", kvp.Key, kvp.Value));
            }
            File.WriteAllLines(path, lines);
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            string folderName = string.Format("{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                (/*rbtnGenerals.Checked ? GENERALS_FOLDER :*/ ZERO_HOUR_FOLDER));

            string fileName = string.Format("{0}\\{1}", folderName, OPTIONS_FILE_NAME);
            SaveINIFile((Dictionary<string, string>)DataContext, fileName);
            Close();
        }

        private void btnAdvancedDisplay_Click(object sender, RoutedEventArgs e)
        {
            AdvancedDisplayWindow adw = new AdvancedDisplayWindow();
            adw.Owner = this;
            adw.DataContext = DataContext;
            adw.Version = Version;
            if (adw.ShowDialog().GetValueOrDefault(false))
            {
                DataContext = adw.DataContext;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnDefaults_Click(object sender, RoutedEventArgs e)
        {
            sliderBrightness.Value = 50;
            chkLanguageFilter.IsChecked = true;
            sliderMusic.Value = 55;
            cmbResolution.SelectedItem = "800 600";
            chkRetaliation.IsChecked = true;
            sliderSoundFX.Value = 79;
            sliderScrollSpeed.Value = 50;
            chkSendDelay.IsChecked = false;
            cmbDetail.SelectedItem = "High";
            chkAlternateMouseSetup.IsChecked = false;
            chkDoubleClickGuard.IsChecked = false;
            sliderVoice.Value = 70;
            /* Advanced display options seem to not be included in defaults.
            ["AntiAliasing"] = "1";
            ["BuildingOcclusion"] = "yes";
            ["DynamicLOD"] = "yes";
            ["ExtraAnimations"] = "yes";
            ["HeatEffects"] = "yes";
            ["MaxParticleCount"] = "5000";
            ["ShowSoftWaterEdge"] = "yes";
            ["ShowTrees"] = "yes";
            ["TextureReduction"] = "0";
            ["UseCloudMap"] = "yes";
            ["UseLightMap"] = "yes";
            ["UseShadowDecals"] = "yes";
            ["UseShadowVolumes"] = "yes";
            */
        }

        private void FillComboBoxes()
        {
            foreach (string resolution in Screen.GetResolutions())
            {
                cmbResolution.Items.Add(resolution);
            }
            cmbDetail.Items.Add("High");
            cmbDetail.Items.Add("Medium");
            cmbDetail.Items.Add("Low");
            cmbDetail.Items.Add("Custom");

            foreach (IPAddress ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    cmbOnlineIP.Items.Add(ip.ToString());
                    cmbLanIP.Items.Add(ip.ToString());
                }
            }
        }
    }
}
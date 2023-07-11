using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;


namespace AWS
{
    // <summary>
    // Interaction logic for MainWindow.xaml
    // </summary>
    public partial class MainWindow : Window
    {
        private string selectedFolderPath = "";
        private string selectedOutputFolderPath = "";

        public MainWindow()
        {
            InitializeComponent();

        }

        private void UpdateUI(string message)
        {
            // Update a text box or another UI element with the provided message
            // For example:
            OutputTextBox.AppendText(message + Environment.NewLine);
            OutputTextBox.ScrollToEnd();

        }

        private void Install_Python_Button_ClicK(object sender, RoutedEventArgs e)
        {
            UpdateUI("Downloading Python Installer");

            // Define the URL to download the Python installer
            string pythonInstallerUrl = "https://www.python.org/ftp/python/3.11.4/python-3.11.4-amd64.exe";

            // Define the path where the installer will be saved
            string installerPath = "C:\\Temp\\python-installer.exe";

            // Download the Python installer
            using (WebClient client = new WebClient())
            {
                client.DownloadFile(pythonInstallerUrl, installerPath);
            }
            UpdateUI("Installing Python");

            // Run the Python installer
            Process.Start(installerPath);

        }

        private void Install_Required_Packages_Button_ClicK(object sender, RoutedEventArgs e)
        {
            UpdateUI("Installing Python Packages");

            InstallRequiredPackages();
        }

        private void InstallRequiredPackages()
        {
            // Specify the packages to install
            string[] packages = { "xlsxwriter", "pandas" };

            // Run pip commands to install the packages
            foreach (string package in packages)
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "pip3",
                    Arguments = $"install {package}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process())
                {
                    process.StartInfo = psi;
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();

                    // Display the installation output, if needed
                    UpdateUI(output);
                }
            }
        }

        private void Select_Folder_Button_ClicK(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                selectedFolderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                // Do something with the selected folder path
                // e.g., display it in a text box
                SelectedFolderTextBox.Text = selectedFolderPath;
            }

        }

        private void Select_Output_Folder_Button_ClicK(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                ValidateNames = false,
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select Folder"
            };

            if (dialog.ShowDialog() == true)
            {
                selectedOutputFolderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                // Do something with the selected folder path
                // e.g., display it in a text box
                SelectedOutputFolderTextBox.Text = selectedOutputFolderPath;
            }
        }


        private void RunPythonScript(string scriptFileName, string configFile, string outputFile)
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", scriptFileName);

            if (File.Exists(scriptPath))
            {
                if (!string.IsNullOrEmpty(selectedOutputFolderPath))
                {
                    // Create the output folder if it doesnt exist
                    Directory.CreateDirectory(selectedOutputFolderPath);

                    // Get the path for the exported Excel file
                    string excelFilePath = Path.Combine(selectedOutputFolderPath, outputFile);

                    // Launch PowerShell and execute the Python script with the output folder path and the excelFilePath as arguments
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = $"\"{scriptPath}\" \"{configFile}\" \"{selectedOutputFolderPath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = new Process())
                    {
                        process.StartInfo = psi;
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        // Display the output, if needed
                        UpdateUI(output);

                        // Get the path of the exported Excel file
                        UpdateUI($"Exported Excel file path: {excelFilePath}");
                    }
                }
                else
                {
                    UpdateUI("Output folder not selected.");
                }
            }
            else
            {
                UpdateUI($"Python script not found: {scriptPath}");
            }
        }

        private void RunPythonScript2(string scriptFileName, string config1File, string config2File, string outputFile)
        {
            string scriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts", scriptFileName);

            if (File.Exists(scriptPath))
            {
                if (!string.IsNullOrEmpty(selectedOutputFolderPath))
                {
                    // Create the output folder if it doesnt exist
                    Directory.CreateDirectory(selectedOutputFolderPath);

                    // Get the path for the exported Excel file
                    string excelFilePath = Path.Combine(selectedOutputFolderPath, outputFile);

                    // Launch PowerShell and execute the Python script with the output folder path and the excelFilePath as arguments
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = "python",
                        Arguments = $"\"{scriptPath}\" \"{config1File}\" \"{config2File}\" \"{selectedOutputFolderPath}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = new Process())
                    {
                        process.StartInfo = psi;
                        process.Start();
                        string output = process.StandardOutput.ReadToEnd();
                        process.WaitForExit();

                        // Display the output, if needed
                        UpdateUI(output);

                        // Get the path of the exported Excel file
                        UpdateUI($"Exported Excel file path: {excelFilePath}");
                    }
                }
                else
                {
                    UpdateUI("Output folder not selected.");
                }
            }
            else
            {
                UpdateUI($"Python script not found: {scriptPath}");
            }
        }


        private void Subnet_Analysis_Button_ClicK(object sender, RoutedEventArgs e)
        {
            string scriptFileName = "Subnet_Analysis.py";
            string configFile = Path.Combine(selectedFolderPath, "describe_subnets.json");
            string outputFile = "subnet_analysis.xlsx"; // Specify the desired output file name

            if (File.Exists(configFile))
            {
                UpdateUI("Analyzing Subnets");
                RunPythonScript(scriptFileName, configFile, outputFile);
                

            }
            else
            {
                UpdateUI($"Configuration file not found: {configFile}");
            }
        }

        private void ACL_Analysis_Button_ClicK(object sender, RoutedEventArgs e)
        {
            string scriptFileName = "ACL_Analysis.py";
            string configFile = Path.Combine(selectedFolderPath, "describe_network_acls.json");
            string outputFile = "acl_analysis.xlsx"; // Specify the desired output file name

            if (File.Exists(configFile))
            {
                UpdateUI("Analyzing ACLs");

                RunPythonScript(scriptFileName, configFile, outputFile);

            }
            else
            {
                UpdateUI($"Configuration file not found: {configFile}");
            }
        }

        private void VPC_Analysis_Button_ClicK(object sender, RoutedEventArgs e)
        {
            string scriptFileName = "VPC_Analysis.py";
            string vpcFile = Path.Combine(selectedFolderPath, "describe_vpcs.json");
            string vpcEndpointsFile = Path.Combine(selectedFolderPath, "describe_vpc_endpoints.json");
            string outputFile = "vpc_analysis.xlsx"; // Specify the desired output file name

            if (File.Exists(vpcFile) && File.Exists(vpcEndpointsFile))
            {
                UpdateUI("Analyzing VPCs");

                RunPythonScript2(scriptFileName, vpcFile, vpcEndpointsFile, outputFile);

            }
            else if (File.Exists(vpcFile) && !File.Exists(vpcEndpointsFile))
            {
                UpdateUI("VPC Endpoints configuration files not found.");
            }
            else if (!File.Exists(vpcFile) && File.Exists(vpcEndpointsFile))
            {
                UpdateUI("VPC configuration file not found.");
            }
            else
            {
                UpdateUI("Both VPC configuration files not found.");
            }

        }


        private void Security_Groups_Analysis_Button_ClicK(object sender, RoutedEventArgs e)
        {
            string scriptFileName = "Security_Groups_Analysis.py";
            string configFile = Path.Combine(selectedFolderPath, "describe_security_groups.json");
            string outputFile = "security_group_analysis.xlsx"; // Specify the desired output file name

            if (File.Exists(configFile))
            {
                UpdateUI("Analyzing Security Groups");

                RunPythonScript(scriptFileName, configFile, outputFile);

            }
            else
            {
                UpdateUI($"Configuration file not found: {configFile}");
            }
        }

        private void Route_Table_Analysis_Button_ClicK(object sender, RoutedEventArgs e)
        {
            string scriptFileName = "Route_Table_Analysis.py";
            string configFile = Path.Combine(selectedFolderPath, "describe_route_tables.json");
            string outputFile = "route_table_analysis.xlsx"; // Specify the desired output file name

            if (File.Exists(configFile))
            {
                UpdateUI("Analyzing Route Tables");

                RunPythonScript(scriptFileName, configFile, outputFile);

            }
            else
            {
                UpdateUI($"Configuration file not found: {configFile}");
            }
        }

        private void Exit_Button_ClicK(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();

        }

    }
}

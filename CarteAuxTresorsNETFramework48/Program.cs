using CarteAuxTresorsNETFramework48.FileIO;
using CarteAuxTresorsNETFramework48.Interfaces;
using CarteAuxTresorsNETFramework48.Models;
using CarteAuxTresorsNETFramework48.Services;
using CarteAuxTresorsNETFramework48.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CarteAuxTresorsNETFramework48
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."));
            LoggerConsoleHelper.LogWarn("Debut des simulations");

            // v1 un seul fichier
            //string inputPath = Path.Combine(basePath, "InputFile", "input.txt");
            //string outputPath = Path.Combine(basePath, "OutputFile", $"{DateTime.Now:yyyyMMdd-HHmmss}-output.txt");
            //v2 on boucle sur les fichiers dans InputFile
            string inputDir = Path.Combine(projectPath, "InputFile");
            string outputDir = Path.Combine(projectPath, "OutputFile");

            IOrientationService orientationService = new OrientationService();

            // Initialisation des mouvements
            var mouvements = new List<IMouvement>
            {
                new MouvementService(orientationService),       // pour Mouvement.A
                new TournerGaucheService(orientationService),   // pour Mouvement.G
                new TournerDroiteService(orientationService)    // pour Mouvement.D
            };

            var mouvementMap = mouvements.ToDictionary(m => m.Type);

            foreach (var inputFilePath in Directory.GetFiles(inputDir, "*.txt"))
            {
                try
                {
                    string inputFileName = Path.GetFileNameWithoutExtension(inputFilePath);
                    string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    string outputFileName = $"ResultOutput_{inputFileName}-{timestamp}.txt";
                    string outputPath = Path.Combine(outputDir, outputFileName);

                    LoggerConsoleHelper.LogInfo("Lecture du fichier d'entrée : " + inputFileName);
                    var inputParser = new FileInputParser();
                    Deroulement data = inputParser.Parse(inputFilePath);

                    LoggerConsoleHelper.LogInfo($"Début simulation avec {data.Aventuriers.Count} aventurier(s)");
                    var simulation = new DeroulementService(mouvementMap);
                    simulation.Executer(data);

                    LoggerConsoleHelper.LogInfo("Ecriture du résultat dans le fichier de sortie : " + outputPath);
                    var writer = new FileOutputWriter();
                    writer.Write(outputPath, data);
                    LoggerConsoleHelper.LogSuccess($"✅ Simulation terminée. Résultat écrit dans :{outputPath}");
                }
                catch (Exception ex)
                {
                    LoggerConsoleHelper.LogError($"❌ Erreur pour le fichier {inputFilePath} : {ex.Message}");
                }
            }
            LoggerConsoleHelper.LogWarn("Fin des simulations");
            NLog.LogManager.Shutdown();
        }
    }
}

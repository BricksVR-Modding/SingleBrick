using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(BricksVR.BuildInfo.Description)]
[assembly: AssemblyDescription(BricksVR.BuildInfo.Description)]
[assembly: AssemblyCompany(BricksVR.BuildInfo.Company)]
[assembly: AssemblyProduct(BricksVR.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + BricksVR.BuildInfo.Author)]
[assembly: AssemblyTrademark(BricksVR.BuildInfo.Company)]
[assembly: AssemblyVersion(BricksVR.BuildInfo.Version)]
[assembly: AssemblyFileVersion(BricksVR.BuildInfo.Version)]
[assembly: MelonInfo(typeof(BricksVR.SingleBrick), BricksVR.BuildInfo.Name, BricksVR.BuildInfo.Version, BricksVR.BuildInfo.Author, BricksVR.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("BricksVR", "BricksVR")]
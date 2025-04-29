using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class CrimeStats{//A class to represent the crime statistics for a year
public int Year{get; set;}
public int Population{get; set;}
public int ViolentCrime{get; set;}
public int Murder{get; set; }
public int Rape{get; set;}
public int Robbery{get; set;}
public int AggravatedAssault{get; set;}
public int PropertyCrime{get; set;}
public int Burglary{get; set;}
public int Theft{get; set;}
public int MotorVehicleTheft{get; set;}
}

class Program{
static void Main(string[] args){
if(args.Length != 2){//Ensurse the correct number of command line arguments
    Console.WriteLine("Usage: CrimeAnalyzer <crime_csv_file_path> <report_file_path>");
    return;
}
    string csvPath = args[0];
    string reportPath = args[1];
    
    try{//Loads crime data and generates the report
    List<CrimeStats> crimeData = LoadCrimeData(csvPath);
    string report = GenerateReport(crimeData);
    File.WriteAllText(reportPath, report);
    Console.WriteLine("Report generated successfully.");
}
    catch(Exception ex){
    Console.WriteLine($"Error: {ex.Message}");
}
    }

static List<CrimeStats> LoadCrimeData(string csvPath){//Loads crime data from the csv file
    var crimeData = new List<CrimeStats>();
    using (var reader = new StreamReader(csvPath)){
        string line;
        int lineNumber = 0;

while((line = reader.ReadLine()) != null){
        lineNumber++;
    if(lineNumber == 1) continue; 

        var values = line.Split(',');
    if(values.Length != 11)
        throw new Exception($"Row {lineNumber} contains {values.Length} values. It should contain 11.");

    try{
        var crimeStats = new CrimeStats{//Creates and adds a new CrimeStats object
        Year = int.Parse(values[0]),
        Population = int.Parse(values[1]),
        ViolentCrime = int.Parse(values[2]),
        Murder = int.Parse(values[3]),
        Rape = int.Parse(values[4]),
        Robbery = int.Parse(values[5]),            AggravatedAssault = int.Parse(values[6]),
        PropertyCrime = int.Parse(values[7]),
        Burglary = int.Parse(values[8]),
        Theft = int.Parse(values[9]),
        MotorVehicleTheft = int.Parse(values[10])
};
        crimeData.Add(crimeStats);
}
    catch(FormatException){
        throw new Exception($"Row {lineNumber} contains invalid data.");
}
}
}

    return crimeData; //Returns crimeData
}

    static string GenerateReport(List<CrimeStats> crimeData){ //Generates a report based on the crime data
        var report = new System.Text.StringBuilder();
        var years = crimeData.Select(c => c.Year);
        report.AppendLine($"Crime Analyzer Report");
        report.AppendLine($"Period: {years.Min()}–{years.Max()} ({years.Count()} years)");
        //Adds statistics to the report
        var murderYears = from c in crimeData where c.Murder < 15000 select c.Year;
        report.AppendLine($"Years murders per year < 15000: {string.Join(", ", murderYears)}");

        var robberyYears = from c in crimeData where c.Robbery > 500000 select $"{c.Year} = {c.Robbery}";
        report.AppendLine($"Robberies per year > 500000: {string.Join(", ", robberyYears)}");

        var violentCrime2010 = crimeData.FirstOrDefault(c => c.Year == 2010);
    if(violentCrime2010 != null){
        double perCapita = (double)violentCrime2010.ViolentCrime / violentCrime2010.Population;
        report.AppendLine($"Violent crime per capita rate (2010): {perCapita}");
}       //Adds average murders per year
        report.AppendLine($"Average murder per year (all years): {crimeData.Average(c => c.Murder):F2}");
        report.AppendLine($"Average murder per year (1994–1997): {crimeData.Where(c => c.Year >= 1994 && c.Year <= 1997).Average(c => c.Murder):F2}");
        report.AppendLine($"Average murder per year (2010–2013): {crimeData.Where(c => c.Year >= 2010 && c.Year <= 2013).Average(c => c.Murder):F2}");

        report.AppendLine($"Minimum thefts per year (1999–2004): {crimeData.Where(c => c.Year >= 1999 && c.Year <= 2004).Min(c => c.Theft)}");
        report.AppendLine($"Maximum thefts per year (1999–2004): {crimeData.Where(c => c.Year >= 1999 && c.Year <= 2004).Max(c => c.Theft)}");

        var highestMotorTheft = crimeData.OrderByDescending(c => c.MotorVehicleTheft).First();
        report.AppendLine($"Year of highest number of motor vehicle thefts: {highestMotorTheft.Year}");

    return report.ToString(); //Returns the generated report
}
}
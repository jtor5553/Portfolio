using System;
using System.Collections.Generic;
using System.IO;



namespace AnalyzeMusicPlaylist
{
class Program{
    static void Main(string[] args){
    //Checks for command line arguments
        if(args.Length != 2){
            Console.WriteLine("Error: Provide the file paths.");
            return;
        }

    string musicPlaylistPath = args[0];
    string reportFilePath = args[1];

        
        //reads and parse the tab-delimited data file
            var songs = ReadMusicPlaylist(musicPlaylistPath);

        //generates the report based off of the parsed data 
            GenerateReport(songs, reportFilePath);
        }
    

        static List<Song> ReadMusicPlaylist(string path){
            var songs = new List<Song>();
            int lineNumber = 1;

            using(var reader = new StreamReader(path)){
        //skips the header line
            reader.ReadLine();

            string line;
            while((line = reader.ReadLine()) != null){
            lineNumber++;
            var values = line.Split('\t');   
            
            if(values.Length != 8){//checks to see if there is a correct number of columns
            Console.WriteLine($"Row {lineNumber} contains {values.Length} values. It should contain 8.");
            continue;
        }

            var song = new Song
            {
            Name = values[0],
            Artist = values[1],
            Album = values[2],
            Genre = values[3],
            Size = int.Parse(values[4]),
            Time = int.Parse(values[5]),
            Year = int.Parse(values[6]),
            Plays = int.Parse(values[7])
    };
            songs.Add(song);
        
    
    }
}

        return songs;
        }

    static void GenerateReport(List<Song> songs, string reportPath){
        using(var writer = new StreamWriter(reportPath)){

        writer.WriteLine("Music Playlist Report\n");

//songs that have 200 or more plays
        writer.WriteLine("Songs with 200 or more plays:");

    foreach(var song in songs){
        if(song.Plays >= 200){
            writer.WriteLine(song);
        }
    }
        writer.WriteLine();

//number of alternative songs
        int altSongsCount = 0;
        foreach(var song in songs){
            if(song.Genre == "Alternative"){altSongsCount++;}
        }
        writer.WriteLine($"Number of Alt songs: {altSongsCount}\n");

//number of Hip-Hop/Rap songs
        int hipHopSongsCount = 0;
        foreach(var song in songs){
            if(song.Genre == "Hip-Hop/Rap"){hipHopSongsCount++;}
        }
        writer.WriteLine($"Number of Hip/Rap songs: {hipHopSongsCount}\n");

//songs from album "Welcome to the Fishbowl"
        writer.WriteLine($"Songs from album 'Welcome to the Fishbowl':");
        foreach(var song in songs){
        if(song.Album == "Welcome to the Fishbowl"){
            writer.WriteLine(song);
        }
    }
    writer.WriteLine();

//songs from before 1970
    writer.WriteLine("Songs from before 1970:");
    foreach(var song in songs){
        if(song.Year < 1970){writer.WriteLine(song);}
    }
    writer.WriteLine();

//song with names longer than 85 characters
        writer.WriteLine("Song names longer than 85 characters:");
    foreach(var song in songs){
        if(song.Name.Length > 85){writer.WriteLine(song.Name);}
    }
        writer.WriteLine();

//longest song time
    Song longestSong = null;
    foreach(var song in songs){
        if(longestSong == null || song.Time > longestSong.Time){longestSong = song}
    }
    writer.WriteLine($"Longest song: {longestSong}");

//unique genres
    writer.WriteLine("\nUnique Genres:");
    var genreSet = new HashSet<string>();

    foreach(var song in songs){
        genreSet.Add(song.Genre);
        }
    foreach(var genre in genreSet){writer.WriteLine(genre);}
    writer.WriteLine();

    //number of songs produced each year
        writer.WriteLine("Yearly Number of Songs in Playlist:");
        var yearCounts = new Dictionary<int, int>();
        
        foreach(var song in songs){
        if(yearCounts.ContainsKey(song.Year)){
            yearCounts[song.Year]++;
        }else{
            yearCounts[song.Year] = 1;
        }
        }
        foreach(var year in yearCounts.Keys){
        writer.WriteLine($"{year}: {yearCounts[year]}");
        }
    }
    writer.WriteLine();

//total plays per year
    writer.WriteLine("Total Plays Per Year:");
    var playsPerYear = new Dictionary<int, int>();
    foreach(var song in songs){
        if(playsPerYear.ContainsKey(song.Year)){
                playsPerYear[song.Year] += song.Plays;
        }else{
            playsPerYear[song.Year] = song.Plays;
        }
    }
    foreach(var year in playsPerYear.Keys){
        writer.WriteLine($"{year}: {playsPerYear[year]}");
    }

//unique artists
        writer.WriteLine("\nUnique Artists:");
        var artistSet = new HashSet<string>();

    foreach(var song in songs){
        artistSet.Add(song.Artist);
    }
    foreach(var artist in artistSet){
        writer.WriteLine(artist);
        }
        }
        }
    }

    public class Song{
        public string Name{get; set;}
        public string Artist{get; set;}
        public string Album{get; set;}
        public string Genre{get; set;}
        public int Size{get; set;}
        public int Time{get; set;}
        public int Year{get; set;}
        public int Plays{get; set;}

        public override string ToString(){
            return string.Format("Name: {0}, Artist: {1}, Album: {2}, Genre: {3}, Size: {4}, Time: {5}, Year: {6}, Plays: {7}",
            Name, Artist, Album, Genre, Size, Time, Year, Plays);
    }
    }
}
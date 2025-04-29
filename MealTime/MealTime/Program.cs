namespace MealTime;



2class Program
{
    static void Main(string[] args)
    {

        /*Prompt the user to enter the time.*/
Console.WriteLine("Enter the time of day\n");

        /*Get the user response.*/
string time = Console.ReadLine();

        /*Send the user response to the convertTime  function to convert the time to a floating point value.*/
float timeInFloat = convertTime(string time);
        /*Use the converted time to determine whether it is  Breakfast time, Lunch time, or Dinner time. 
        Breakfast between 7:00 and 8:00, Lunch between 12:00 and 13:00,  Dinner between 18:00 and 19:00
        Output the appropriate message to the user.
        End program*/
float breakfastStart, breakfastEnd, lunchStart, lunchEnd, dinnerStart, dinnerEnd;
    breakfastStart = 7.0;
    breakfastEnd = 8.0;
    lunchStart = 12.0;
    lunchEnd = 13.0;
    dinnerStart = 18.0;
    dinnerEnd = 19.0;

        /* 
        The meal times are breakfast between 7:00 and 8:00, lunch between 12:00 and 13:00, and dinner between 
        18:00 and 19:00. Assume that the user’s input will be formatted in 24-hour time as #:## or ##:##. And 
        assume that each meal’s time range is inclusive of the end points in the time range. For instance, whether 
        it’s 7:00, 7:01, 7:59, or 8:00, or anytime in between, it’s time for breakfast.*/
if(timeInFloat >= breakfastStart && timeInFloat <= breakfastEnd){
    Console.WriteLine("Its breakfast time!!!");
}else if(timeInFloat >= lunchStart && timeInFloat <= lunchEnd){
    Console.WriteLine("Lunch time!!!");
}else if(timeInFloat >= dinnerStart && timeInFloat <= dinnerEnd){
    Console.WriteLine("Dinner time!!!");
}else{Console.WriteLine("Not time to eat!!!");}

    }

     /*Your are to also create a function called convertTime  (that can be called by main) that 
        accepts one parameter, the string time the user entered, and converts time, a string in 
        24-hour format, to the corresponding number of hours as a float. For instance, given a 
        time like "7:30" (i.e., 7 hours and 30 minutes), convert should return 7.5 (i.e., 7.5 hours).*/

    public static float convertTime(time){
    int hours, minutes;

     string[] twoParts = time.Split(':'); 

        hours = int.Parse(twoParts[0]);
        minutes = int.Parse(twoParts[1]);

        return (hours + minutes) / 60.0;
    }
}



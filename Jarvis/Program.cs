using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;


namespace Jarvis
{
    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        static void Main(string[] args)
        {
            List<string> cpuMaxedOutMessages = new List<string>();
            cpuMaxedOutMessages.Add("WARNING: Your CPU is about to catch fire!");
            cpuMaxedOutMessages.Add("WARNING: Oh my god you should not run your CPU that hard");
            cpuMaxedOutMessages.Add("WARNING: Stop downloading it's maxing me out");
            cpuMaxedOutMessages.Add("WARNING: Your CPU is officially chasing squirrels");
            cpuMaxedOutMessages.Add("RED ALERT! RED ALERT! RED ALERT! RED ALERT! RED ALERT!");

            Random rand = new Random();
            
            JerrySpeak("Welcolme to Jarvis version 1 point oh!", VoiceGender.Male);

            PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCPUCounter.NextValue();

            PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");
            perfMemCounter.NextValue();

            PerformanceCounter perfUpTimeCounter = new PerformanceCounter("System", "System Up Time");
            perfUpTimeCounter.NextValue();

            TimeSpan upTimeSpan = TimeSpan.FromSeconds(perfUpTimeCounter.NextValue());
            string systemUpTimeMessage = string.Format("The current system up time is {0} Days {1} Hours {2} Minutes {3} Seconds",
                (int)upTimeSpan.TotalDays,
                (int)upTimeSpan.Hours,
                (int)upTimeSpan.Minutes,
                (int)upTimeSpan.Seconds
                );

            JerrySpeak(systemUpTimeMessage, VoiceGender.Male, 5);

            int speechSpeed = 1;

            bool isChromeOpenedAlready = false;

            while (true)
            {

                int currentCPUPercentage = (int)perfCPUCounter.NextValue();
                int currentAvailableMemory = (int)perfMemCounter.NextValue();

                Console.WriteLine("CPU Load:         {0}%", currentCPUPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);

                if(currentCPUPercentage > 80)
                {
                    if(currentCPUPercentage == 100)
                    { 
                        if(speechSpeed < 5)
                        {
                            speechSpeed++;
                        }

                        string cpuLoadVocalMessage = cpuMaxedOutMessages[rand.Next(5)];

                        if(isChromeOpenedAlready == false)
                        {
                            openWebSite("https://www.facebook.com/");
                            isChromeOpenedAlready = true;
                        }

                        JerrySpeak(cpuLoadVocalMessage, VoiceGender.Female, speechSpeed);

                    }
                    else
                    {
                        string cpuLoadVocalMessage = string.Format("The current CPU Load is {0} percent", currentCPUPercentage);
                        JerrySpeak(cpuLoadVocalMessage, VoiceGender.Female, 5);
                    }
                    
                }
 
                if(currentAvailableMemory < 1024)
                {
                    string memAvailableVocalMessage = string.Format("You currently have {0} Megabytes of memory available", currentAvailableMemory);
                    JerrySpeak(memAvailableVocalMessage, VoiceGender.Male, 10);
                }

                Thread.Sleep(1000);
            }

        }
        /// <summary>
        /// Speaks with a certain voice
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGenger"></param>
        public static void JerrySpeak(string message, VoiceGender voiceGenger)
        {
            synth.SelectVoiceByHints(voiceGenger);
            synth.Speak(message);
        }

        /// <summary>
        /// Speak with the selected voice at the selected speed
        /// </summary>
        /// <param name="message"></param>
        /// <param name="voiceGenger"></param>
        /// <param name="rate"></param>
        public static void JerrySpeak(string message, VoiceGender voiceGenger, int rate)
        {
            synth.Rate = rate;
            JerrySpeak(message, voiceGenger);
        }

        public static void openWebSite(string URL)
        {
            Process p1 = new Process();
            p1.StartInfo.FileName = "chrome.exe";
            p1.StartInfo.Arguments = URL;
            p1.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p1.Start();
        }
    }
}

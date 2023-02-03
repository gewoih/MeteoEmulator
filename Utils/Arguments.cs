using System;

namespace MeteoEmulator.Utils
{
    public class Arguments
    {
        //--url[url] --sleep[sleep interval, seconds] --instance[instance id] --stability[data stability(INT, from 2 to 10)] --with-noise
        public string Url { get; set; }
        public int SleepInterval { get; set; }
        public string InstanceID { get; set; }
        public int DataStability { get; set; }
        public bool WithNoise { get; set; }

        public Arguments()
        {
            SleepInterval = 1;
            InstanceID = "test";
            DataStability = 3;
            WithNoise = false;
        }

        public void FromArgs(string[] args)
        {
            if (args.Length == 0 || args.Length == 1 && args[0] == "--help")
            {
                throw new ArgumentException("Specify arguments to run the emulator:\n" +
                                            "MeteoEmulator.exe --url [url] --sleep [sleep interval, seconds] --instance [instance id] --stability [data stability (INT, from 2 to 10)] --with-noise\n" +
                                            "something like\n" +
                                            "MeteoEmulator.exe --url http://localhost:55355 --sleep 5 --instance meteo1 --stability 3 --with-noise");
            }

            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case "--url":
                        Url = args[i + 1];
                        break;
                    case "--sleep":
                        if (!int.TryParse(args[i + 1], out var sleep))
                        {
                            throw new ArgumentException("Sleep interval must be integer value. Stop");
                        }
                        SleepInterval = sleep;
                        break;
                    case "--instance":
                        InstanceID = args[i + 1];
                        break;
                    case "--stability":
                        if (!int.TryParse(args[i + 1], out var ds) || ds is < 2 or > 10)
                        {
                            throw new ArgumentException("Data stability must be integer value between 2 an 10. Stop");
                        }
                        DataStability = ds;
                        break;
                    case "--with-noise":
                        WithNoise = true;
                        break;
                    default:
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ArgumentException("--url must be set");
            }
        }
    }
}

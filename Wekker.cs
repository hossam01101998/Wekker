using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gebruik_van_delegates
{
    internal class Wekker
    {
        private delegate void Alarmtype();

        private Alarmtype alarmtype;
        private Timer snoozeTimer;
        private TimeSpan snoozeTime = TimeSpan.FromMinutes(1);
        private TimeSpan alarmTime = TimeSpan.Zero;

        private bool running = true;

        public Wekker()
        {
            alarmtype = dummy;
            snoozeTimer = new Timer(OnSnoozeTimerElapsed);
            //CheckAlarm();
        }

        public void Start()
        {
            Console.WriteLine("Wekker");
            char choice;
            //CheckAlarm();

            var checkAlarmThread = new Thread(() =>
            {
                while (running)
                {
                    CheckAlarm();
                    //ALARM VERIFICATIE ELKE SECONDE
                    Thread.Sleep(1000);
                }
            });

            checkAlarmThread.Start();

            do
            {
                Console.WriteLine("Welke optie ga jij kiezen?");
                Console.WriteLine("0 - STOPPEN");
                Console.WriteLine("1 - WEKKER INSTELLINGEN");
                Console.WriteLine("2 - SNOOZE TIJD ZETTEN");
                Console.WriteLine("3 - WEKKER STELLEN");

                Console.Write("Schrijf je keuze: ");
                Console.WriteLine();
                choice = Console.ReadKey().KeyChar;
                Console.ReadLine();

                switch (choice)
                {
                    case '1':
                        ConfigureAlarm();
                        break;

                    case '2':
                        SetSnoozeTime();
                        break;

                    case '3':
                        SetAlarmTime();

                        break;

                    case '0':
                        Console.WriteLine("Tot ziens!");
                        break;

                    default:
                        Console.WriteLine("Ongeldige keuze. Probeer opnieuw.");
                        break;
                }

                CheckAlarm();

            } while (choice != '0');
        }

        private void ConfigureAlarm()
        {
            Console.WriteLine("A - Voeg een tekst toe");
            Console.WriteLine("a - Verwijder de tekst");

            Console.WriteLine("B - Maak lawaai");
            Console.WriteLine("b - Maak geen lawaai");

            Console.WriteLine("C - Knipper de lichten");
            Console.WriteLine("c - Knipper de lichten niet");

            char alarm = Console.ReadKey().KeyChar;
            Console.ReadLine();

            switch (alarm)
            {
                case 'A':
                    alarmtype += SchrijfTekst;
                    Console.WriteLine("Tekst is toegevoegd");
                    break;

                case 'a':
                    alarmtype -= SchrijfTekst;
                    Console.WriteLine("Tekst is verwijderd");
                    break;

                case 'B':
                    alarmtype += MaakLawaai;
                    Console.WriteLine("Lawaai is toegevoegd");
                    break;

                case 'b':
                    alarmtype -= MaakLawaai;
                    Console.WriteLine("Lawaai is verwijderd");
                    break;

                case 'C':
                    alarmtype += KnipperLichten;
                    Console.WriteLine("Lichten zijn toegevoegd");
                    break;

                case 'c':
                    alarmtype -= KnipperLichten;
                    Console.WriteLine("Lichten zijn verwijderd");
                    break;
            }
        }

        private void SetSnoozeTime()
        {
            Console.Write("Schrijf de Snooze tijd (in seconden): ");
            if (int.TryParse(Console.ReadLine(), out int snoozeSeconds))
            {
                snoozeTime = TimeSpan.FromSeconds(snoozeSeconds);
                Console.WriteLine($"Snooze tijd is ingesteld op {snoozeSeconds} seconden.");
            }
            else
            {
                Console.WriteLine("Snooze tijd moet een geldig nummer zijn.");
            }
        }

        private void SetAlarmTime()
        {
            Console.Write("Zet de wekker tijd (HH:MM:SS): ");
            string inputTime = Console.ReadLine();

            if (TryParseTime(inputTime, out alarmTime))
            {
                Console.WriteLine($"Wekker is ingesteld op: {alarmTime}");
            }
            else
            {
                Console.WriteLine("Ongeldige tijdsindeling. Gebruik het formaat HH:MM:SS.");
            }
        }

        private bool alarmActivated = false;
        private void CheckAlarm()
        {


            if (!alarmActivated && Math.Abs((alarmTime - DateTime.Now.TimeOfDay).TotalSeconds) < 1)
            {
                alarmActivated = true;
                alarmtype();

                StartSnoozeTimer();
            }
        }

        private bool TryParseTime(string input, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;
            string[] timeParts = input.Split(':');

            if (timeParts.Length == 3 && int.TryParse(timeParts[0], out int hours) &&
                int.TryParse(timeParts[1], out int minutes) && int.TryParse(timeParts[2], out int seconds))
            {
                timeSpan = new TimeSpan(hours, minutes, seconds);
                return true;
            }

            return false;
        }

        private void StartSnoozeTimer()
        {
            if (snoozeTime > TimeSpan.Zero)
            {
                snoozeTimer.Change(snoozeTime, TimeSpan.Zero);
            }
        }

        private void OnSnoozeTimerElapsed(object state)
        {
            alarmtype();
        }

        private void dummy() { }

        private void SchrijfTekst()
        {
            Console.WriteLine();
            Console.WriteLine("HET IS TIJD!!!");
        }

        private void MaakLawaai()
        {

            for (int i = 0; i < 20; i++)
            {
                Console.Beep();
                Thread.Sleep(500); // Pausa de medio segundo entre cada "beep"

            }
            Console.WriteLine("De alarm is stopt");
        }

        private void KnipperLichten()
        {
            Console.WriteLine();
            Console.WriteLine("***********************************");
        }
    }
}


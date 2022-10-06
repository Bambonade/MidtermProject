using System;
using System.IO;
using NLog.Web;

namespace TicketingSystemWithClasses
{
    class Program
    {
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
            string ticketFilePath1 = Directory.GetCurrentDirectory() + "\\tickets1.csv";
            string ticketFilePath2 = Directory.GetCurrentDirectory() + "\\tickets2.csv";
            string ticketFilePath3 = Directory.GetCurrentDirectory() + "\\tickets3.csv";
            logger.Info("Program started");
            TicketFile ticketFile = new TicketFile(ticketFilePath1,ticketFilePath2,ticketFilePath3);
            string choice;
            do {
                Console.WriteLine("1) Read ticket information");
                Console.WriteLine("2) Add ticket infomation");
                Console.WriteLine("Enter any other key to exit");
                choice = Console.ReadLine();

                if (choice == "1") {
                    Console.WriteLine("Bug/defect Tickets");
                    foreach(Bug bug in ticketFile.Bugs) Console.WriteLine(bug.Entry());
                    Console.WriteLine("Enhancement Tickets");
                    foreach(Enhancement enhancement in ticketFile.Enhancements) Console.WriteLine(enhancement.Read());
                    Console.WriteLine("Task Tickets");
                    foreac(Task task in ticketFile.Tasks) Console.WriteLine(task.Read());
                }
                
                if (choice == "2") {
                    Ticket ticket = new Ticket();
                    ticket.summary = NullCheck("Enter Ticket Summary", "summary");
                    ticket.status = NullCheck("Enter Ticket Status", "status");
                    ticket.priority = NullCheck("Enter Ticket Priority", "priority");
                    ticket.submitter = NullCheck("Enter the Ticket Submitter", "submitter");
                    ticket.assigned = NullCheck("Enter Person Assigned", "assigned");
                    ticket.peopleWatching.Add(NullCheck("Enter Person Watching", "person watching"));
                    string anotherWatcher;
                    do {
                        Console.WriteLine("Enter Another Person Watching Or Just Press 'ENTER' To Continue");
                        anotherWatcher = Console.ReadLine();
                        if (anotherWatcher != "") {
                            ticket.peopleWatching.Add(anotherWatcher);
                        }
                    } while (anotherWatcher != "");
                    ticketFile.AddTicket(ticket);
                }
            } while (choice == "1" || choice == "2");

            logger.Info("Program Ended");
        }

        public static string NullCheck(string question, string errorName) {
            bool continueLoop = true;
            string entry;
            do {
                Console.WriteLine(question);
                entry = Console.ReadLine();
                if (entry == "") {
                    logger.Error("No input for {0} was entered", errorName);
                    continueLoop = true;
                }
                else {
                    continueLoop = false;
                }
            } while (continueLoop == true);

            return entry;
        }
    }
}
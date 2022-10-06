using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog.Web;

namespace TicketingSystemWithClasses {
    public class TicketFile {
        public string filePath {get; set;}
        public List<Ticket> Tickets {get; set;}
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

        public TicketFile(string ticketFilePath) {
            filePath = ticketFilePath;
            Tickets = new List<Ticket>();
            if(File.Exists(filePath))
            {
                StreamReader ticketRead = new StreamReader(filePath);
                while (!ticketRead.EndOfStream)
                {
                    Ticket ticket = new Ticket();
                    string line = ticketRead.ReadLine();
                    string[] ticketSplit = line.Split(',');
                    ticket.ticketID = UInt64.Parse(ticketSplit[0]);
                    ticket.summary = ticketSplit[1];
                    ticket.status = ticketSplit[2];
                    ticket.priority = ticketSplit[3];
                    ticket.submitter = ticketSplit[4];
                    ticket.assigned = ticketSplit[5];
                    ticket.peopleWatching = ticketSplit[6].Split('|').ToList();
                    Tickets.Add(ticket);
                }
                ticketRead.Close();
            }
            else
            {
                logger.Error("File Does Not Exist");
            }
        }

        public void AddTicket(Ticket ticket) {
            if(File.Exists(filePath)) {
                ticket.ticketID = Tickets.Max(t => t.ticketID) + 1;
                File.AppendAllText(filePath, ticket.entry() + "\n");
                logger.Info("Ticket ID {0} added", ticket.ticketID);
            }
            else {
                StreamWriter ticketWrite = new StreamWriter(filePath);
                ticket.ticketID = 1;
                ticketWrite.Write(ticket.entry() + "\n");
                ticketWrite.Close();
                logger.Info("File {0} added and Ticket ID {1} added", filePath, ticket.ticketID);
            }
        }
    }
}
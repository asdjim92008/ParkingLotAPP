using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotAPP.DAL
{
    public class ParkingLot_FTP
    {
        private string ftpServerIP;
        private string ftpUser;
        private string ftpPassword;
        public ParkingLot_FTP(string ftpServerIP, string ftpUser,string ftpPassword)
        {
            this.ftpServerIP = ftpServerIP;
            this.ftpUser = ftpUser;
            this.ftpPassword = ftpPassword;
        }
    }
}

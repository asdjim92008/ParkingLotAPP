using CoreFtp;
using CoreFtp.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        public async Task<string> FileDownloadAsync(string f_name)
        {
            using (var ftpClient = new FtpClient(new FtpClientConfiguration
            {
                Host = ftpServerIP,
                Username = ftpUser,
                Password = ftpPassword
            }))
            {
                await ftpClient.LoginAsync();
                try
                {
                    using (var ftpReadStream = await ftpClient.OpenFileReadStreamAsync("/LinePayPicture/" + f_name + ".jpg"))
                    {
                        Bitmap bmp = new Bitmap(ftpReadStream);
                        MemoryStream ms = new MemoryStream();
                        try
                        {
                            bmp.Save(ms, ImageFormat.Jpeg);
                            return Convert.ToBase64String(ms.ToArray());
                        }
                        catch (Exception)
                        {
                            return null;
                        }
                        finally
                        {
                            bmp.Dispose();
                        }
                    }
                }
                catch (Exception)
                {
                    return "no such file";
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.IO;

namespace EDC
{
    public class UnloadingService : IUnloadingService
    {
        public void ExportToExcel(int upID, string userName)
        {
            var UPR = new Models.Repository.UnloadingProfileRepository();
            var up = UPR.SelectByID(upID);
            if (up == null)
                return;
            up.ReadyToUnloading = false;
            UPR.Update(up);
            UPR.Save();

            try
            {
                Core.Export.ExportToExcel.CreateUnloadingFile(up, userName);
            }
            catch(Exception error)
            {
                string pathToDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Logs";
                if(!Directory.Exists(pathToDirectory))
                {
                    Directory.CreateDirectory(pathToDirectory);
                }
                using(StreamWriter sw = new StreamWriter(pathToDirectory+"\\log.txt",true))
                {
                    sw.WriteLine("_________________________________________");
                    sw.WriteLine(error.Message);
                    sw.WriteLine(error.StackTrace);

                    var st = new System.Diagnostics.StackTrace(error);
                    var frame = st.GetFrame(1);
                    int line = frame.GetFileLineNumber();
                    sw.WriteLine("Строка: "+line);

                    while (error.InnerException != null)
                    {
                        error = error.InnerException;
                        sw.WriteLine(error.Message);
                    }
                    sw.WriteLine("_________________________________________");
                }
                throw new Exception(error.Message, error);
            }

            up.ReadyToUnloading = true;
            UPR.Update(up);
            UPR.Save();
        }
    }
}

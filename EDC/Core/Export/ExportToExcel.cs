using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EDC.Models;
using EDC.Models.Repository;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;

namespace EDC.Core.Export
{
    class ExportToExcel
    {
        public static bool CreateUnloadingFile(EDC.Models.UnloadingProfile up, string userName)
        {
            var ASR = new AppSettingRepository();
            var SR = new SubjectRepository();
            var SIR = new SubjectsItemRepoitory();
            var CIR = new CRFItemRepository();

            var sStudyName = ASR.SelectByID(EDC.Core.Core.STUDY_NAME);
            string studyName = sStudyName == null ? "" : sStudyName.Value;

            var sStudyProtocol = ASR.SelectByID(EDC.Core.Core.STUDY_PROTOCOL);
            string studyProtoloc = sStudyProtocol == null ? "" : sStudyProtocol.Value;

            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory+"Data\\UnloadingFiles\\";

            try
            {
                if (!System.IO.Directory.Exists(currentDirectory))
                    System.IO.Directory.CreateDirectory(currentDirectory);
            }
            catch(Exception error)
            {
                return false;
            }

            string fileName = string.Format("EXCEL_{0}_{1}_{2}.xlsx", up.Name, DateTime.Now.ToString().Replace("/",".").Replace(",","").Replace(':','-').Replace(' ','-'), DateTime.Now.TimeOfDay.TotalSeconds.ToString().Replace(",",""));

            IWorkbook wb = new XSSFWorkbook();
            ISheet ws = wb.CreateSheet();
            int currentRowNumber = 0;
            int i = 0;

            #region infoTable
            var infoTable = GetDataTableInfo(up, studyName, studyProtoloc);
            for (i = 0; i < infoTable.Rows.Count; i++)
            {
                IRow row = ws.CreateRow(currentRowNumber++);
                for (int j = 0; j < infoTable.Columns.Count; j++)
                {
                    ICell cell = row.CreateCell(j);
                    cell.SetCellValue(infoTable.Rows[i][j].ToString());
                }

            }
            #endregion


            List<UnloadingItem> uItems = up.Items.OrderBy(x => x.Event.Position).ThenBy(x=>x.CRFInEvent.Position).ThenBy(x=>x.ItemID).ToList();

            #region Headers
            IRow rowEvents = ws.CreateRow(currentRowNumber++);
            IRow rowCRFs = ws.CreateRow(currentRowNumber++);
            IRow rowItemsDescription = ws.CreateRow(currentRowNumber++);
            IRow rowItemsName = ws.CreateRow(currentRowNumber++);

            ICell cellEvent = rowEvents.CreateCell(2);
            ICell cellCRF = rowCRFs.CreateCell(2);
            ICell cellItemDescription = rowItemsDescription.CreateCell(2);
            ICell cellItemName = rowItemsName.CreateCell(0);
            cellItemName.SetCellValue("Study Subject ID");

            cellItemName = rowItemsName.CreateCell(1);
            cellItemName.SetCellValue("Protocol ID");

            cellEvent.SetCellValue("Event (Occurrence)");
            cellCRF.SetCellValue("CRF - Version");
            cellItemDescription.SetCellValue("Item Description (Occurrence)");
            cellItemName = rowItemsName.CreateCell(2);
            cellItemName.SetCellValue("Item Name");

            int itemIndex = 0;

            int[,] maxCount = new int[uItems.Count+1,2];
            
            i = 0;
            while(i<uItems.Count)
            {
                long itemID = uItems[i].ItemID;
                List<Models.SubjectsItem> sis = SIR.GetManyByFilter(x => x.ItemID == itemID).ToList();
                int maxIndex = 1;
                int countInGroup = 1;

                if (!uItems[i].Item.Ungrouped)
                {
                    //id группы
                    long currentCRFItemGroupID = CIR.SelectByID(itemID).Group.CRF_GroupID;
                    //итемы в группе
                    var currentGroupItems = uItems.Where(x => x.EventID == uItems[i].EventID && x.CRFID == uItems[i].CRFID && x.Item.GroupID == currentCRFItemGroupID).ToList();
                    //итемов в текущей группе
                    int itemsInCurrentGroup = currentGroupItems.Count();

                    var currentItems = sis.Where(x => x.EventID == uItems[i].EventID && x.CRFID == uItems[i].CRFID).OrderBy(x => x.IndexID).ToList();
                   
                    //итемов с текущим ItemID (повторений в группе)
                    maxIndex = currentItems.Count>0 ? currentItems.Max(x => x.IndexID):1;
                    maxIndex = maxIndex <= 0 ? 1 : maxIndex;
                    countInGroup = itemsInCurrentGroup>0?itemsInCurrentGroup:1;

                    maxCount[i, 0] = maxIndex;
                    maxCount[i, 1] = countInGroup;

                    for (int k = 0; k < itemsInCurrentGroup; k++)
                    {
                        for (int j = 0; j < maxIndex; j++)
                        {
                            int sdvig = j * itemsInCurrentGroup;
                            int cellIndex = itemIndex + sdvig + 3;

                            CreateCellWriteInfo(rowEvents, cellIndex, uItems[i].Event.Name);
                            CreateCellWriteInfo(rowCRFs, cellIndex, string.IsNullOrWhiteSpace(uItems[i].CRF.RussianName) ? uItems[i].CRF.Name : uItems[i].CRF.RussianName);
                            CreateCellWriteInfo(rowItemsDescription, cellIndex, uItems[i].Item.DescriptionLabel);
                            CreateCellWriteInfo(rowItemsName, cellIndex, uItems[i].Item.Identifier + "_" + (j + 1).ToString());
                        }
                        i++;
                        itemIndex++;
                    }
                    itemIndex+=(maxIndex-1)*itemsInCurrentGroup;
                }
                else
                {
                    CreateCellWriteInfo(rowEvents, itemIndex + 3, uItems[i].Event.Name);
                    CreateCellWriteInfo(rowCRFs, itemIndex + 3, string.IsNullOrWhiteSpace(uItems[i].CRF.RussianName) ? uItems[i].CRF.Name : uItems[i].CRF.RussianName);
                    CreateCellWriteInfo(rowItemsDescription, itemIndex + 3, uItems[i].Item.DescriptionLabel);
                    CreateCellWriteInfo(rowItemsName, itemIndex + 3, uItems[i].Item.Identifier);

                    i++;
                    itemIndex++;

                    maxCount[i, 0] = maxIndex;
                    maxCount[i, 1] = countInGroup;
                }



            }
            #endregion

            List<Models.Subject> subjects = new List<Subject>();
            up.Centers.Select(x => x.MedicalCenter).ToList().ForEach(x=> subjects.AddRange(x.Subjects));

            //проход по субъектам
            for(i =0;i<subjects.Count;i++)
            {
                //строка для субъекта
                IRow rowSubject = ws.CreateRow(currentRowNumber++);
                //текущая ячейка
                ICell cellSubject = rowSubject.CreateCell(0);
                cellSubject.SetCellValue(subjects[i].Number);

                cellSubject = rowSubject.CreateCell(1);
                cellSubject.SetCellValue(studyProtoloc);

                //id текущего субъекта
                long currentSubjectID = subjects[i].SubjectID;
                //данные субъекта
                List<Models.SubjectsItem> sis = SIR.GetManyByFilter(x => x.SubjectID == currentSubjectID).ToList();

                //индекс ячейки
                int cellIndex = 0;

                //проход по выгружаемым полям
                for (int j = 0; j < uItems.Count; j++)
                {
                    //если не в группе
                    if (uItems[j].Item.Ungrouped)
                    {
                        var currentItems = sis.Where(x => x.EventID == uItems[j].EventID && x.CRFID == uItems[j].CRFID && x.ItemID == uItems[j].ItemID).ToList();
                        switch (up.CrfStatus)
                        {
                            case "All": break;
                            case "IsCheck":
                                {
                                    currentItems = currentItems.Where(x => x.SubjectCRF.IsCheckAll && !x.SubjectCRF.IsApproved).ToList();
                                    break;
                                }
                            case "IsEnd":
                                {
                                    currentItems = currentItems.Where(x => x.SubjectCRF.IsEnd && !x.SubjectCRF.IsCheckAll && !x.SubjectCRF.IsApproved).ToList();
                                    break;
                                }
                            default:
                                {
                                    currentItems = new List<SubjectsItem>();
                                    break;
                                }
                        }
                        //запись информации в ячейку
                        if (currentItems.Count > 0)
                            CreateCellWriteInfo(rowSubject, 3 + cellIndex, currentItems[0].Value);
                        cellIndex++;
                    }
                    else
                    {
                        int startCellIndex = cellIndex;
                        int itemsInGroup = maxCount[j, 1];
                        int maxIndex = maxCount[j, 0];
                        for (int z = 0; z < itemsInGroup; z++,j++,cellIndex++)
                        {
                            var currentItems = sis.Where(x => x.EventID == uItems[j].EventID && x.CRFID == uItems[j].CRFID && x.ItemID == uItems[j].ItemID).ToList();
                            switch (up.CrfStatus)
                            {
                                case "All": break;
                                case "IsCheck":
                                    {
                                        currentItems = currentItems.Where(x => x.SubjectCRF.IsCheckAll && !x.SubjectCRF.IsApproved).ToList();
                                        break;
                                    }
                                case "IsEnd":
                                    {
                                        currentItems = currentItems.Where(x => x.SubjectCRF.IsEnd && !x.SubjectCRF.IsCheckAll && !x.SubjectCRF.IsApproved).ToList();
                                        break;
                                    }
                                default:
                                    {
                                        currentItems = new List<SubjectsItem>();
                                        break;
                                    }
                            }
                            for (int k = 0; k < currentItems.Count; k++)
                            {
                                int cellWriteIndex = cellIndex + 3 + k * itemsInGroup;
                                CreateCellWriteInfo(rowSubject, cellWriteIndex, currentItems[k].Value);
                            }
                        }
                        cellIndex += ((maxIndex - 1) * itemsInGroup);
                        //если после группы стал не групповой элемент
                        if (j< uItems.Count && uItems[j].Item.Ungrouped)
                            j--;
                    }
                }

            }


            #region SaveFile
            string pathToFile = currentDirectory + "\\" + fileName;
            long fileSize;
            using (var sw = new FileStream(pathToFile, FileMode.Create, FileAccess.Write))
            {
                wb.Write(sw);
            }
            FileInfo fi = new FileInfo(pathToFile);
            fileSize = fi.Length;
            
            UnloadingFile uf = new UnloadingFile();
            uf.CreatedBy = userName;
            uf.CreatedDate = DateTime.Now;
            uf.FileName = fileName;
            uf.FileSize = fileSize;
            uf.PathToFile = pathToFile;
            uf.Type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            uf.UnloadingProfileID = up.UnloadingProfileID;

            UnloadingFileRepository UFR = new UnloadingFileRepository();
            UFR.Create(uf);
            UFR.Save();
            #endregion

            return true;
        }

        static void CreateCellWriteInfo(IRow row, int cellIndex,string info)
        {
            ICell cell = row.CreateCell(cellIndex);
            cell.SetCellValue(info);
        }

        static DataTable GetDataTableInfo(EDC.Models.UnloadingProfile up, string studyName, string studyProtocol)
        {
            var dTable = new DataTable();
            dTable.Columns.Add("Info", typeof(string));
            dTable.Columns.Add("Value", typeof(string));

            var row = dTable.NewRow();
            row[0] = "Dataset Name:";
            row[1] = up.Name;
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Dataset Description:";
            row[1] = up.Description;
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Study Name:";
            row[1] = studyName;
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Protocol ID:";
            row[1] = studyProtocol;
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Date:";
            row[1] = DateTime.Now.ToShortDateString();
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Subjects:";
            var mcs = up.Centers.Select(x => x.MedicalCenter).ToList();
            row[1] = mcs.Sum(x => x.Subjects.Count);
            dTable.Rows.Add(row);

            row = dTable.NewRow();
            row[0] = "Study Event Definitions:";
            int eventsCount = up.Items.GroupBy(x => x.EventID).Count();
            row[1] = eventsCount.ToString();
            dTable.Rows.Add(row);

            var orderedItems = up.Items.OrderBy(x => x.Event.Position).ToList();
            long prevEventID = 0;
            int crfCount = 0;
            List<long> itemsInCurrentEvent = new List<long>();
            eventsCount = 0;
            for (int i = 0; i < orderedItems.Count; i++)
            {
                if (orderedItems[i].EventID != prevEventID)
                {
                    itemsInCurrentEvent = new List<long>();
                    eventsCount++;
                    row = dTable.NewRow();
                    row[0] = "Study Event Definition " + eventsCount;
                    row[1] = orderedItems[i].Event.Name;
                    dTable.Rows.Add(row);
                    prevEventID = orderedItems[i].EventID;
                }
                if (!itemsInCurrentEvent.Contains(orderedItems[i].CRFID))
                {
                    crfCount++;
                    row = dTable.NewRow();
                    row[0] = "CRF";
                    row[1] = orderedItems[i].CRF.RussianName == null ? orderedItems[i].CRF.Name : orderedItems[i].CRF.RussianName;
                    dTable.Rows.Add(row);
                    itemsInCurrentEvent.Add(orderedItems[i].CRFID);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                row = dTable.NewRow();
                row[0] = "";
                row[1] = "";
                dTable.Rows.Add(row);
            }

            return dTable;
        }
    }
}
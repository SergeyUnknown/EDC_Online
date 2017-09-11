using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EDC.Models;
using EDC.Models.Repository;
using System.IO;

namespace EDC.Pages.CRF
{
    public partial class AddCRF : BasePage
    {
        CRFRepository crfRepository = new CRFRepository();
        CRFSectionRepository crfSectionRepository = new CRFSectionRepository();
        CRFGroupRepository crfGroupRepository = new CRFGroupRepository();
        CRFItemRepository crfItemRepository = new CRFItemRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole(Core.Roles.Administrator.ToString()))
                Response.Redirect("~/");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            List<CRF_Error> allCrfErrors = new List<CRF_Error>();

            if (!fuAddCRF.HasFiles)
            {
                allCrfErrors.Add(new CRF_Error("", null, null, "Не выбраны файлы для загрузки", ""));
                return;
            }

            string folderPath = Request.PhysicalApplicationPath + "Data/CRFs/";
            if (!Directory.Exists(folderPath))
            {
                try
                {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception error)
                {
                    Response.Write(error.Message);
                    return;
                }
            }

            string[] allowedExtensions = { ".xls", ".xlsx" };

            foreach (var file in fuAddCRF.PostedFiles)
            {
                string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
                string filePath = "";

                //не правильное расширение файла
                if (!allowedExtensions.Contains(fileExtension))
                {
                    continue;
                }

                bool uploaded = false;
                try
                {
                    filePath = folderPath + file.FileName;
                    if (System.IO.File.Exists(filePath))
                    {
                        allCrfErrors.Add(new CRF_Error("",null,null,"Файл уже существует",file.FileName));
                        continue;
                    }

                    file.SaveAs(filePath);
                    uploaded = true;
                }
                catch (Exception ex)
                {
                    allCrfErrors.Add(new CRF_Error("", null, null, "Не удалось загрузить файл. <br/>"+ex.Message, file.FileName));
                    continue;
                }

                //файл не загружен
                if (!uploaded)
                {
                    continue;
                }

                Excel.IExcelDataReader IEDR;
                System.IO.FileStream stream =
                    new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                if (fileExtension.IndexOf("xlsx") > -1)
                {
                    IEDR = Excel.ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    IEDR = Excel.ExcelReaderFactory.CreateBinaryReader(stream);
                }

                IEDR.IsFirstRowAsColumnNames = true; //в первой строке название колонок

                DataSet ds = IEDR.AsDataSet(); //получаем листы в виде таблиц

                List<CRF_Error> CRF_Errors = new List<CRF_Error>();

                EDC.Models.CRF CRFInfo = GetCRFInfo(ds.Tables[0], CRF_Errors, filePath,file.FileName);//Информация о CRF

                if(CRF_Errors.Count>0)
                {
                    System.IO.File.Delete(filePath);
                    //В общий список ошибок добавляем ошибки в текущей ИРК
                    allCrfErrors.AddRange(CRF_Errors);
                    continue;
                }

                List<CRF_Section> CRF_Sections = GetCRFSections(ds.Tables[1], CRF_Errors, file.FileName); //Информация о секциях

                List<CRF_Group> CRF_Groups = GetCRFGroups(ds.Tables[2], CRF_Errors, file.FileName); //Группы

                List<CRF_Item> CRF_Items = GetCRFItems(ds.Tables[3], CRF_Sections, CRF_Groups, CRF_Errors, file.FileName);//Итемы

                IEDR.Close();

                if (CRF_Errors.Count > 0) //вывести ошибки
                {
                    System.IO.File.Delete(filePath);
                    //В общий список ошибок добавляем ошибки в текущей ИРК
                    allCrfErrors.AddRange(CRF_Errors);
                    continue;
                }
                else
                {
                    var crf = crfRepository.Create(CRFInfo); //добавляем crf в БД
                    crfRepository.Save();

                    for (int i = 0; i < CRF_Sections.Count; i++)
                    {
                        CRF_Sections[i].CRFID = crf.CRFID;
                        CRF_Sections[i] = crfSectionRepository.Create(CRF_Sections[i]);
                    }
                    crfSectionRepository.Save();

                    /////////////////Группа UnGrouped/////////////////////
                    CRF_Group ungrouped = new CRF_Group();
                    ungrouped.CRFID = crf.CRFID;
                    ungrouped.Identifier = "UNGROUPED_" + crf.CRFID.ToString().PadLeft(6, '0');
                    ungrouped.RepeatNumber = 1;
                    ungrouped.RepeatMax = 1;
                    ungrouped = crfGroupRepository.Create(ungrouped);
                    /////////////////////////////////////////////////////

                    for (int i = 0; i < CRF_Groups.Count; i++)
                    {
                        CRF_Groups[i].CRFID = crf.CRFID;
                        CRF_Groups[i] = crfGroupRepository.Create(CRF_Groups[i]);
                    }
                    crfGroupRepository.Save();

                    foreach (var item in CRF_Items)
                    {
                        item.CRFID = crf.CRFID;
                        item.SectionID = CRF_Sections.FirstOrDefault(x => x.Label == item.Section.Label).CRF_SectionID;
                        item.Section = null;
                        if (item.Group != null)
                            item.GroupID = CRF_Groups.FirstOrDefault(x => x.Label == item.Group.Label).CRF_GroupID;
                        else
                            item.GroupID = ungrouped.CRF_GroupID;
                        item.Group = null;

                        crfItemRepository.Create(item);
                    }
                    crfItemRepository.Save();
                }

            }
            if(allCrfErrors.Count>0)
            {
                gvErrors.DataSource = allCrfErrors;
                gvErrors.DataBind();
            }
            else
                Response.Redirect("~/CRFs");


        }
        private EDC.Models.CRF GetCRFInfo(DataTable table,List<CRF_Error> CRF_Errors,string filePath,string fileName)
        {
            EDC.Models.CRF newCRF = new Models.CRF();

            List<string> CRFInfo = table.Rows[0].ItemArray.Select(x=>x.ToString()).ToList();

            if (!string.IsNullOrWhiteSpace(CRFInfo[0]))
                newCRF.Name = CRFInfo[0];
            else
                CRF_Errors.Add(new CRF_Error(table.TableName,2,1,"Не указано название CRF",fileName));
            List<Models.CRF> addedCRF = crfRepository.GetManyByFilter(x => x.Name.ToLower() == newCRF.Name.ToLower()).ToList();

            newCRF.Identifier = ClearTrash(newCRF.Name).ToUpper();
            if(newCRF.Identifier.Length>25)
            {
                newCRF.Identifier = newCRF.Identifier.Substring(0, 25);
                var allCRFs = crfRepository.SelectAll().ToList();
                int countIdentificator = allCRFs.Count(x=> x.Identifier.IndexOf(newCRF.Identifier)>0);
                if(countIdentificator>0)
                {
                    newCRF.Identifier = newCRF.Identifier + countIdentificator.ToString().PadLeft(3);
                }
            }

            if (!string.IsNullOrWhiteSpace(CRFInfo[1]))
            {
                double version =0;
                if (double.TryParse(CRFInfo[2].Replace('.', ','), out version) || double.TryParse(CRFInfo[2].Replace(',', '.'), out version))
                {
                    newCRF.RussianName = CRFInfo[1];
                    newCRF.Version = CRFInfo[2];
                }
                else
                {
                    newCRF.Version = CRFInfo[1];
                }
            }
            else
                CRF_Errors.Add(new CRF_Error(table.TableName, 2, 2, "Не указано название CRF",fileName));

            newCRF.CreationDate = DateTime.Now;
            newCRF.CreatedBy = User.Identity.Name;
            if (addedCRF.Any(x => x.Version.ToLower() == newCRF.Version.ToLower()))
            {
                CRF_Errors.Add(new CRF_Error(table.TableName, 2, 3, "CRF с указанным именем и версий уже существует",fileName));
            }
            else
                newCRF.Identifier += ClearTrash(newCRF.Version);
            newCRF.FilePath = filePath;

            return newCRF;
        }
        private List<CRF_Section> GetCRFSections(DataTable table,List<CRF_Error> CRF_Errors,string fileName)
        {
            List<CRF_Section> CRF_Sections = new List<CRF_Section>();

            var CRF_SectionRows = table.Rows;

            for ( int i =0;i<CRF_SectionRows.Count;i++)
            {
                DataRow row = CRF_SectionRows[i];
                List<string> rowItems = row.ItemArray.Select(x=>x.ToString()).ToList();
                CRF_Section newSection = new CRF_Section();
                
                if(string.IsNullOrWhiteSpace(rowItems[0]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i+1, 2, "Не указана метка Секции",fileName));
                else if(CRF_Sections.Any(x=>x.Label == rowItems[0]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 1, 1, "Секция с указанной меткой уже добавлена",fileName));
                else
                    newSection.Label = rowItems[0];

                if(!string.IsNullOrWhiteSpace(rowItems[1]))
                    newSection.Title = rowItems[1];
                else
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 1, 2, "Не указано название Секции",fileName));

                newSection.Subtitle = rowItems[2];
                newSection.Instructions = rowItems[3];

                int pageNumber;
                newSection.PageNumber = (int.TryParse(rowItems[4], out pageNumber) && pageNumber > 0) ? pageNumber : 1;

                string border = rowItems[6].ToLower();
                newSection.Border = border == "1" || border == "true";

                CRF_Sections.Add(newSection);
            }

            return CRF_Sections;
        }
        private List<CRF_Group> GetCRFGroups(DataTable table,List<CRF_Error> CRF_Errors,string fileName)
        {
            List<CRF_Group> CRF_Groups = new List<CRF_Group>();

            var CRF_GroupRows = table.Rows;

            List<CRF_Group> addedGroups = crfGroupRepository.SelectAll().ToList();


            for(int i =0;i< CRF_GroupRows.Count;i++)
            {
                DataRow row = CRF_GroupRows[i];
                List<string> rowItems = row.ItemArray.Select(x=>x.ToString()).ToList();
                CRF_Group newGroup = new CRF_Group();

                if (string.IsNullOrWhiteSpace(rowItems[0]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 1, 1, "Не указано название Группы",fileName));
                else
                {
                    string lowerLabel = rowItems[0].ToLower();
                    if (CRF_Groups.Any(x => x.Label.ToLower() == lowerLabel))
                        CRF_Errors.Add(new CRF_Error(table.TableName, i + 1, 1, "Группа с указанным названием уже добавлена", fileName));
                    else if (addedGroups.Any(x => x.Label!=null && x.Label.ToLower() == lowerLabel))
                        CRF_Errors.Add(new CRF_Error(table.TableName, i + 1, 1, "Группа с указанным названием уже добавлена", fileName));
                    else
                        newGroup.Label = rowItems[0];
                }

                if (CRF_Errors.Count > 0)
                    return new List<CRF_Group>();

                newGroup.Identifier = ClearTrash(newGroup.Label).ToUpper();
                if (newGroup.Identifier.Length > 23)
                {
                    newGroup.Identifier = newGroup.Identifier.Substring(0, 23);
                }

                newGroup.Header = rowItems[1];

                int repeatNumber;
                newGroup.RepeatNumber = (int.TryParse(rowItems[2], out repeatNumber) && repeatNumber > 0) ? repeatNumber : 1;

                int repeatMax;
                newGroup.RepeatMax = (int.TryParse(rowItems[3], out repeatMax) && repeatMax > 0) ? repeatMax : int.MaxValue;

                CRF_Groups.Add(newGroup);
            }

            return CRF_Groups;
        }
        private List<CRF_Item> GetCRFItems(DataTable table,List<CRF_Section> CRFSections,List<CRF_Group> CRFGroups, List<CRF_Error> CRF_Errors,string fileName)
        {
            List<CRF_Item> CRF_Items = new List<CRF_Item>();

            var CRF_ItemRows = table.Rows;

            for (int i=0;i<CRF_ItemRows.Count;i++)
            {
                DataRow row = CRF_ItemRows[i];
                List<string> rowItems = row.ItemArray.Select(x=>x.ToString()).ToList();
                CRF_Item newItem = new CRF_Item();
                string name = rowItems[0].ToUpper();

                List<CRF_Item> addedItems = crfItemRepository.SelectAll().ToList();

                if (string.IsNullOrWhiteSpace(name))
                {
                    bool allNull = true;
                    foreach(var cell in rowItems)
                    {
                        if (!string.IsNullOrWhiteSpace(cell))
                        {
                            allNull = false;
                        }
                        break;
                    }
                    if (allNull)
                        continue;
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 1, "Не указано название параметра", fileName));
                }
                else if (CRF_Items.Any(x => x.Name.ToUpper() == name))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 1, "Параметр с указанным названием уже добавлен", fileName));
                else if (addedItems.Any(x => x.Name == name))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 1, "Параметр с указанным названием уже добавлен", fileName));
                else
                    newItem.Name = name;

                if (CRF_Errors.Count > 0)
                    return addedItems;

                newItem.Identifier = ClearTrash(newItem.Name).ToUpper();
                if (newItem.Identifier.Length > 23)
                {
                    newItem.Identifier = newItem.Identifier.Substring(0, 23);
                }

                if (string.IsNullOrWhiteSpace(rowItems[1]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 2, "Не указано описание параметра", fileName));
                else
                    newItem.DescriptionLabel = rowItems[1];

                newItem.LeftItemText = rowItems[2];
                newItem.Units = rowItems[3];
                newItem.RightItemText = rowItems[4];

                CRF_Section tempSection = CRFSections.FirstOrDefault(x => x.Label == rowItems[5]);
                if (tempSection == null)
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 6, "Отсутствует Секция с указанной меткой", fileName));
                else
                    newItem.Section = tempSection;

                if (string.IsNullOrWhiteSpace(rowItems[6]))
                {
                    newItem.Group = null;
                    newItem.Ungrouped = true;
                }
                else
                {
                    CRF_Group tempGroup = CRFGroups.FirstOrDefault(x => x.Label == rowItems[6]);
                    if (tempGroup == null)
                        CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 7, "Отсутствует Группа с указанной меткой", fileName));
                    else
                    {
                        newItem.Group = tempGroup;
                        newItem.Ungrouped = false;
                    }
                }

                newItem.Header = rowItems[7];
                newItem.Subheader = rowItems[8];


                int columnNumber;
                newItem.ColumnNumber = (int.TryParse(rowItems[10], out columnNumber) && columnNumber > 0) ? columnNumber : 1;

                int pageNumber;
                newItem.PageNumber = (int.TryParse(rowItems[11], out pageNumber) && pageNumber > 0) ? pageNumber : 1;

                int questionNumber;
                newItem.QuestionNumber = (int.TryParse(rowItems[12], out questionNumber) && questionNumber > 0) ? questionNumber : 1;

                if (string.IsNullOrWhiteSpace(rowItems[13]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 14, "Не указан тип ответа", fileName));
                else
                {
                    Core.ResponseType RT = Core.Core.StringToResponseType(rowItems[13]);
                    if(RT == Core.ResponseType.None)
                    {
                        CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 14, "Указан неверный тип ответа", fileName));
                    }
                    else
                        newItem.ResponseType = RT;
                }

                newItem.ResponseLabel = rowItems[14];
                newItem.ResponseOptionText = rowItems[15];
                newItem.ResponseValuesOrCalculation = rowItems[16];
                if(newItem.ResponseOptionText.Replace("\\,"," ").Split(',').Length != newItem.ResponseValuesOrCalculation.Split(',').Length)
                {
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 18, "Количество вариантов ответов и значений не совпадает", fileName));
                }
                newItem.ResponseLayout = rowItems[17];
                newItem.DefaultValue = rowItems[18];

                if (string.IsNullOrWhiteSpace(rowItems[19]))
                    CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 20, "Не указан тип данных", fileName));
                else
                {
                    Core.DataType DT = Core.Core.StringToDataType(rowItems[19]);
                    if(DT == Core.DataType.NONE)
                    {
                        CRF_Errors.Add(new CRF_Error(table.TableName, i + 2, 20, "Указан неверный тип данных", fileName));
                    }
                    else
                        newItem.DataType = (Core.DataType)DT;
                }

                int widthDecimal;
                newItem.WidthDecimal = (int.TryParse(rowItems[20], out widthDecimal) && widthDecimal > 0) ? widthDecimal : 1;

                newItem.Validation = rowItems[21];
                newItem.ValidationErrorMessage = rowItems[22];

                string PHI = rowItems[23].ToLower();
                newItem.PHI = PHI == "1" || PHI == "true";

                string required = rowItems[24].ToLower();
                newItem.Required = required == "1" || required == "true";


                CRF_Items.Add(newItem);
            }
            return CRF_Items;
        }

        char[] cTrash = new char[] { '/', ' ', '.', '-','?','!',',','%','$','&','^','@','#','*','+','=','(',')','{','}',':',';','>','<','|','\\' };
        string ClearTrash(string strWithTrash)
        {
            if (strWithTrash == null)
                return strWithTrash;
            strWithTrash = strWithTrash.Replace(" ", "");
            strWithTrash = strWithTrash.Trim(cTrash);
            foreach(char c in cTrash)
            {
                strWithTrash = strWithTrash.Replace(c, '_');
            }
            return strWithTrash.Replace("__","_");
        }
    }
}
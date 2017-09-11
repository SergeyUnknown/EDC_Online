using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using EDC.Core.Rule;

namespace EDC.Pages.Rule
{
    public partial class AddRule : BasePage
    {
        Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
        Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
        Models.Repository.CRFGroupRepository CRFGR = new Models.Repository.CRFGroupRepository();
        Models.Repository.CRFItemRepository CRFIR = new Models.Repository.CRFItemRepository();
        Models.Repository.RuleRepository RR = new Models.Repository.RuleRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.IsInRole(Core.Roles.Administrator.ToString()))
                Response.Redirect("~/");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            lblStatus.Visible = false;
            List<Rule_Error> errorList = new List<Rule_Error>();
            if (fuAddQuery.HasFile)
            {
                string folderPath = Request.PhysicalApplicationPath + "Data/Rules/";
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
                String fileExtension = System.IO.Path.GetExtension(fuAddQuery.FileName).ToLower();
                string filePath = "";
                if (fileExtension == ".xml")
                {
                    bool uploaded = false;
                    try
                    {
                        filePath = folderPath + fuAddQuery.FileName;
                        if (System.IO.File.Exists(filePath))
                        {
                            errorList.Add(new Rule_Error() { ErrorMessage="Файл уже существует" });
                            LoadErrors(errorList);
                            return;
                        }

                        fuAddQuery.PostedFile.SaveAs(filePath);
                        uploaded = true;
                    }
                    catch (Exception ex)
                    {
                        errorList.Add(new Rule_Error() { ErrorMessage = "Файл не загружен. <br/>" + ex.Message });
                        LoadErrors(errorList);
                        return;
                    }

                    if (uploaded)
                    {
                        XmlDocument xDoc = new XmlDocument();

                        try
                        {
                            xDoc.Load(filePath); //загружаем документ
                        }
                        catch(Exception error)
                        {
                            errorList.Add(new Rule_Error() { ErrorMessage = error.Message });
                            LoadErrors(errorList);
                            File.Delete(filePath);
                            return;
                        }
                        
                        XmlElement xRoot = xDoc.DocumentElement;

                        XmlNodeList ruleAssigments = xRoot.SelectNodes("RuleAssignment"); //получаем список RuleAssignment
                        XmlNodeList ruleDef = xRoot.SelectNodes("RuleDef"); //получаем список RuleDef

                        //проходим по всем RuleAssignment
                        for(int i =0;i<ruleAssigments.Count;i++)
                        {
                            XmlNode item = ruleAssigments[i];
                            //выясняем Таргет
                            string target = item.SelectSingleNode("Target").InnerText;
                            Target t = RuleParse.TargetParse(errorList, target,"",i+1);

                            XmlNodeList ruleRef = item.SelectNodes("RuleRef"); //получаем список RuleRef

                            foreach (XmlNode rule in ruleRef)
                            {
                                var newRule = new Models.Rule();
                                newRule.EventID = t.EventID;
                                newRule.CRFID = t.CRFID;
                                newRule.GroupID = t.GroupID;
                                newRule.ItemID = t.ItemID;
                                newRule.AddedDate = DateTime.Now;
                                string OID = rule.Attributes["OID"].InnerText; //OID

                                List<Models.Rule> thisOIDs = RR.GetManyByFilter(x=> x.OID==OID).ToList();
                                if(thisOIDs.Count>0)
                                {
                                    errorList.Add(new Rule_Error()
                                    {
                                        ErrorMessage = "Уже существуют правила с указанным OID",
                                        RuleAssignmentNumber = i + 1,
                                        OID = OID
                                    });
                                    continue;
                                }

                                XmlNode discrepancyNoteAction = rule.SelectSingleNode("DiscrepancyNoteAction");
                                bool ifExpressionEvaluates = //ifExpressionEvaluates
                                    discrepancyNoteAction.Attributes["IfExpressionEvaluates"].InnerText.ToLower() == "true";
                                string message = discrepancyNoteAction.SelectSingleNode("Message").InnerText; //ErrorMessage

                                newRule.ErrorMessage = message;
                                newRule.Target = target;
                                newRule.OID = OID;
                                newRule.IfExpressionEvaluates = ifExpressionEvaluates;

                                XmlNodeList currentRulesDef = xRoot.SelectNodes(string.Format("RuleDef[@OID='{0}']", OID));
                                string ruleName = "";
                                string expression = "";
                                if (currentRulesDef.Count != 1)
                                {
                                    errorList.Add(new Rule_Error()
                                    {
                                        ErrorMessage = "Не удалось найти RuleDef с указанным OID",
                                        RuleAssignmentNumber = i+1,
                                        OID = OID
                                    });
                                }
                                else
                                {
                                    ruleName = currentRulesDef[0].Attributes["Name"].InnerText;
                                    expression = currentRulesDef[0].SelectSingleNode("Expression").InnerText;
                                    if (errorList.Count > 0)
                                        continue;
                                    if (expression.Length >= 5)
                                    {
                                        expression = Regex.Replace(expression, @"( eq )", " == ", RegexOptions.IgnoreCase);
                                        expression = Regex.Replace(expression, @"( ne )", " != ", RegexOptions.IgnoreCase);
                                        expression = Regex.Replace(expression, @"( lt )", " < ", RegexOptions.IgnoreCase);
                                        expression = Regex.Replace(expression, @"( lte )", " <= ", RegexOptions.IgnoreCase);
                                        expression = Regex.Replace(expression, @"( gt )", " > ", RegexOptions.IgnoreCase);
                                        expression = Regex.Replace(expression, @"( gte )", " >= ", RegexOptions.IgnoreCase);

                                        List<IToken> tokens = RuleParse.GetTokenList(expression);

                                        if(tokens.Count==0)
                                        {
                                            errorList.Add(new Rule_Error()
                                            {
                                                ErrorMessage = "При разборе выражения произошла ошибка. Выражение: " + expression,
                                                RuleAssignmentNumber = i + 1,
                                                OID = OID
                                            });
                                        }

                                        RuleParse.UpdateCrfTokens(tokens,t, errorList,i+1,OID);
                                        if (errorList.Count > 0)
                                            continue;

                                        if(!RuleParse.TestTokens(tokens))
                                        {
                                            errorList.Add(new Rule_Error()
                                            {
                                                ErrorMessage = "Не верно указано выражение для проверки: " + expression,
                                                RuleAssignmentNumber = i + 1,
                                                OID = OID
                                            });
                                        }
                                        newRule.Name = ruleName;
                                        newRule.Expression = expression;
                                        newRule.Tokens = new List<Models.Token>();
                                        foreach (IToken iT in tokens)
                                            newRule.Tokens.Add(new Models.Token() { Type = iT.Type, Value = iT.Value });
                                    }
                                    else
                                    {
                                        errorList.Add(new Rule_Error()
                                        {
                                            ErrorMessage = "Не верно указано выражение для проверки: "+expression,
                                            RuleAssignmentNumber = i+1,
                                            OID = OID
                                        });
                                    }
                                }
                                if(errorList.Count==0)
                                    RR.Create(newRule);
                            }
                            File.Delete(filePath);
                        }
                        if (errorList.Count == 0)
                        {
                            lblStatus.Visible = true;
                            RR.Save();
                        }

                    }
                }
            }
            else
            {
                errorList.Add(new Rule_Error() { ErrorMessage = "Не выбран файл для загрузки" });
            }
            LoadErrors(errorList);

        }

        void LoadErrors(List<Rule_Error> errorList)
        {
            gvErrors.DataSource = errorList;
            gvErrors.DataBind();
        }

    }
}
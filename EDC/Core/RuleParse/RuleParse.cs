using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace EDC.Core.Rule
{
    public class RuleParse
    {
        /// <summary>
        /// Получает ТокенЛист
        /// </summary>
        /// <param name="expression">Исходное выражение</param>
        /// <returns>Лист токенов</returns>
        public static List<IToken> GetTokenList(string expression)
        {
            MatchCollection collection = Regex.Matches(expression, "(\".*?\")|([\\.|\"|_|\\w|\\d])+|NOT|AND|OR|XOR|<=|==|!=|>=|>|<|\\(|\\)", RegexOptions.IgnoreCase);
            Regex variables = new Regex("(\".*?\")|([\\.|\"|_|\\w|\\d])+");
            Regex operations = new Regex(@"NOT|AND|OR|<=|==|!=|>=|>|<", RegexOptions.IgnoreCase);
            Regex brackets = new Regex(@"\(|\)");
            Regex notCrfTarget = new Regex("^\".*\"$");

            string[] priority = { "<=", "==", "!=", ">=", ">", "<", "NOT", "AND", "OR" };

            Stack<string> stack = new Stack<string>();
            List<IToken> tokens = new List<IToken>();
            if(collection.Count>0)
            {
                int bracket = 0;
                foreach (Match m in collection)
                    if (m.Value == ")")
                        bracket++;
                    else if(m.Value == "(")
                        bracket--;
                if (bracket != 0)
                    return tokens;
            }
            foreach (Match match in collection)
            {
                Match temp = variables.Match(match.Value);
                if (temp.Success)
                {
                    if (!priority.Contains(match.Value.ToUpper()))
                    {
                        if (!notCrfTarget.IsMatch(match.Value) && !Regex.IsMatch(match.Value, "^[\\.|\\d]+$"))
                            tokens.Add(new Token(temp.Value, TokenType.Item));
                        else
                        {
                            tokens.Add(new Token(temp.Value.Replace("\"",""), TokenType.Constant));
                        }
                        continue;
                    }
                }
                temp = brackets.Match(match.Value);
                if (temp.Success)
                {
                    if (temp.Value == "(")
                    {
                        stack.Push(temp.Value);
                        continue;
                    }
                    string operation = stack.Pop();
                    while (operation != "(")
                    {
                        tokens.Add(new Token(operation, TokenType.Operation));
                        operation = stack.Pop();
                    }
                    continue;
                }
                temp = operations.Match(match.Value);
                if (temp.Success)
                {
                    if (stack.Count != 0)
                    {
                        while (stack.Count > 0 && Array.IndexOf(priority, temp.Value.ToUpper()) > Array.IndexOf(priority, stack.Peek()))
                        {
                            if (stack.Peek().ToUpper() == "(")
                                break;
                            tokens.Add(new Token(stack.Pop(), TokenType.Operation));
                        }
                    }
                    stack.Push(temp.Value.ToUpper());
                    continue;
                }
            }
            while (stack.Count > 0)
            {
                tokens.Add(new Token(stack.Pop(), TokenType.Operation));
            }
            return tokens;
        }

        /// <summary>
        /// Преобразует Лист Токенов
        /// </summary>
        /// <param name="tokens">Исходный список</param>
        /// <param name="t">Текущая цель</param>
        /// <param name="errorList">Список ошибок</param>
        /// <param name="ruleAssigmentIndex">Индекс правила</param>
        /// <param name="OID">Индекс правила</param>
        public static void UpdateCrfTokens(List<IToken> tokens, Target t, List<Rule_Error> errorList, int ruleAssigmentIndex=-1, string OID="")
        {
            Models.Repository.CRFItemRepository CRFIR = new Models.Repository.CRFItemRepository();
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == TokenType.Item)
                {
                    string temp = tokens[i].Value;
                    string[] elems = temp.Split('.');

                    if (elems.Length == 1)
                    {
                        int oldLength = errorList.Count;

                        Target newT = TargetParse(errorList, elems[0], t,OID,ruleAssigmentIndex);

                        if(oldLength == errorList.Count)
                        {
                            CrfToken ct = new CrfToken(tokens[i], newT.EventID, newT.CRFID, newT.GroupID, newT.ItemID);
                            tokens[i] = ct;
                        }
                    }
                    else
                    {
                        int oldLength = errorList.Count;
                        Target newT = TargetParse(errorList, tokens[i].Value,OID,ruleAssigmentIndex);
                        if (oldLength == errorList.Count)
                        {
                            CrfToken ct = new CrfToken(tokens[i], newT.EventID, newT.CRFID, newT.GroupID, newT.ItemID);
                            tokens[i] = ct;
                        }
                    }
                }
            }
        }

        public static Target TargetParse(List<Rule_Error> errorList, string strTarget, Target currentTarget, string OID="", int ruleAssigmentIndex=-1)
        {
            Models.Repository.CRFItemRepository CRFIR = new Models.Repository.CRFItemRepository();
            List<Models.CRF_Item> tempItems =
                CRFIR.GetManyByFilter(x => x.CRFID == currentTarget.CRFID && x.Identifier == strTarget).ToList();
            if (tempItems.Count == 1)
            {
                return new Target() { CRFID = currentTarget.CRFID, EventID = currentTarget.EventID, GroupID = currentTarget.GroupID, ItemID = tempItems[0].CRF_ItemID };
            }
            else
            {
                errorList.Add(new Rule_Error()
                {
                    ErrorMessage = "Не верно указана цель в выражении: "+strTarget,
                    OID=OID,
                    RuleAssignmentNumber = ruleAssigmentIndex
                });
                return null;
            }
        }

        /// <summary>
        /// Получает цель правила
        /// </summary>
        /// <param name="errorList">Список ошибок</param>
        /// <param name="ruleAssigmentIndex">Индекс правила</param>
        /// <param name="strTarget">Цель в строковом представлении</param>
        /// <returns>Цель правила</returns>
        public static Target TargetParse(List<Rule_Error> errorList, string strTarget, string OID="", int ruleAssigmentIndex=-1)
        {
            Models.Repository.EventRepository ER = new Models.Repository.EventRepository();
            Models.Repository.CRFItemRepository CRFIR = new Models.Repository.CRFItemRepository();
            Models.Repository.CRFRepository CRFR = new Models.Repository.CRFRepository();
            Models.Repository.CRFGroupRepository CRFGR = new Models.Repository.CRFGroupRepository();
            string[] pathToTarget = strTarget.Split('.');
            byte pathIndex = 0;
            Target t = new Target();

            string strEventID = ""; //событие

            if (pathToTarget.Length == 4)
            {
                strEventID = pathToTarget[0];
                pathIndex++;
            }
            else
                if (pathToTarget.Length != 3)
                {
                    errorList.Add(
                        new Rule_Error()
                        {
                            ErrorMessage = "Не верно указана цель в выражении",
                            OID = OID,
                            RuleAssignmentNumber = ruleAssigmentIndex
                        });
                    return new Target();
                }
            string strCrfID = pathToTarget[pathIndex++]; //ИРК
            string strGroupID = pathToTarget[pathIndex++]; //Группа
            string strItemID = pathToTarget[pathIndex++]; //Итем

            long? eventID = null;
            long crfID = 0;
            long groupID = 0;
            long itemID = 0;

            if (strEventID != "")
            {
                var events = ER.GetManyByFilter(x => x.Identifier == strEventID).ToList();
                if (events.Count == 1)
                {
                    eventID = events[0].EventID;
                }
                else
                {
                    errorList.Add(new Rule_Error()
                    {
                        ErrorMessage = "Не удалось найти событие с указанным идентификатором",
                        OID = OID,
                        RuleAssignmentNumber = ruleAssigmentIndex
                    });
                }
            }

            var crfs = CRFR.GetManyByFilter(x => x.Identifier == strCrfID).ToList();
            if (crfs.Count == 1)
            {
                crfID = crfs[0].CRFID;
            }
            else
            {
                errorList.Add(new Rule_Error()
                {
                    ErrorMessage = "Не удалось найти ИРК с указанным идентификатором",
                    OID = OID,
                    RuleAssignmentNumber = ruleAssigmentIndex
                });
            }

            var groups = CRFGR.GetManyByFilter(x => x.Identifier == strGroupID).ToList();
            if (groups.Count == 1)
            {
                groupID = groups[0].CRF_GroupID;
            }
            else
            {
                errorList.Add(new Rule_Error()
                {
                    ErrorMessage = "Не удалось найти группу с указанным идентификатором",
                    OID = OID,
                    RuleAssignmentNumber = ruleAssigmentIndex
                });
            }

            var items = CRFIR.GetManyByFilter(x => x.Identifier == strItemID).ToList();
            if (items.Count == 1)
            {
                itemID = items[0].CRF_ItemID;
            }
            else
            {
                errorList.Add(new Rule_Error()
                {
                    ErrorMessage = "Не удалось найти итем с указанным идентификатором",
                    OID = OID,
                    RuleAssignmentNumber = ruleAssigmentIndex
                });
            }
            if (errorList.Count > 0)
                return new Target();
            else
                return new Target() { CRFID = crfID, EventID = eventID, GroupID = groupID, ItemID = itemID };
        }

        public static bool TestTokens(List<IToken> tokens)
        {
            Stack<IToken> stack = new Stack<IToken>();
            foreach (IToken token in tokens)
            {
                if (token.Type == TokenType.Constant || token.Type == TokenType.Item)
                    stack.Push(token);
                if (token.Type == TokenType.Operation)
                {
                    if (stack.Count < 2)
                        return false;
                    else
                    {
                        stack.Pop();
                        stack.Pop();
                        stack.Push(new EDC.Core.Rule.Token("testValue", TokenType.Constant));
                    }
                }
            }
            if (stack.Count != 1)
                return false;
            return true;
        }
    }
}
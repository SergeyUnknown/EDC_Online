using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Core.Rule
{
    public class Token : IToken
    {
        private string value;
        private TokenType type;
        public string Value { get { return value; } }
        public TokenType Type { get { return type; } }
        public Token(string value,TokenType type)
        {
            this.value = value;
            this.type = type;
        }
    }

    public class CrfToken : Token, IToken
    {
        private long? eventID;
        private long crfID;
        private long groupID;
        private long itemID;

        public long? EventID 
        { 
            get 
            {
                return eventID; 
            } 
            set 
            {
                eventID = value;
            } 
        }
        public long CrfID { get { return crfID; } }
        public long GroupID { get { return groupID; } }
        public long ItemID { get { return itemID; } }

        public CrfToken(IToken token,long? eventID,long crfID,long groupID,long itemID)
            :base(token.Value,token.Type)
        {
            this.eventID = eventID;
            this.crfID = crfID;
            this.groupID = groupID;
            this.itemID = itemID;
        }
    }
}
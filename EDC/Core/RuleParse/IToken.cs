using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDC.Core.Rule
{
    public interface IToken
    {
        string Value { get; }
        TokenType Type { get; }
    }
}

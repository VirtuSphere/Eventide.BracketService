using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketService.Domain.Enums;

public enum MatchStatus
{
    Pending,
    InProgress,
    Completed,
    Disputed
}
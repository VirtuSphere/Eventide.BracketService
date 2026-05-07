using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketService.Domain.Exceptions;

public class ArgumentNullValueException(string paramName)
    : ArgumentNullException(paramName, $"Argument \"{paramName}\" value is null");
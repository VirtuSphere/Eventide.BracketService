using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BracketService.ValueObjects.Exceptions;
public class ValidatorNullException(string paramName)
: ArgumentNullException(paramName, $"Validator \"{paramName}\" must be specified for type.");
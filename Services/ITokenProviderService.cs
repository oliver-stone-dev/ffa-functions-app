using ffa_functions_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ffa_functions_app.Services;

public interface ITokenProviderService
{
    public string Create(Account account);
}

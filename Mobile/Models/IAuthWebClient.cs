using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobile.Models;
public interface IAuthWebClient<T> where T : Auth
{
    Task<string> Token(T t);
}
;

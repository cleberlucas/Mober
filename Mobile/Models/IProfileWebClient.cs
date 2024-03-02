using System.Web.Mvc;

namespace Mobile.Models;

public interface IProfileWebClient<T> where T : Profile
{
    Task<T> View();
}



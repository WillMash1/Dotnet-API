using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Services
{
    public class ServiceResponse<T>
    {
        public T? Data {get; set;}
        public bool Success {get; set;} = true;
        public string message {get; set;} = string.Empty;
    }
}
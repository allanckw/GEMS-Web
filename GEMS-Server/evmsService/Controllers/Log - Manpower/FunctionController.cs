using System.Collections.Generic;
using System.Linq;
using evmsService.DataAccess;
using evmsService.entities;

namespace evmsService.Controllers
{
    public class FunctionController
    {
        public static List<Function> ViewFunction()
        {

            DAL dalDataContext = new DAL();

            List<Function> Functions = (from func in dalDataContext.functions

                                        select func).ToList<Function>();

            return Functions.OrderBy(func => func.Grouping).ToList<Function>();

        }
    }
}
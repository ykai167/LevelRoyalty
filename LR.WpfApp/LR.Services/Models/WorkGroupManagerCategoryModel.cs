using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LR.Services;

namespace LR.Models
{
    public class WorkGroupManagerCategoryModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }

        public static WorkGroupManagerCategoryModel[] WorkGroupManagerCategories
        {
            get
            {
                return LR.Tools.DIHelper.GetInstance<IWorkGroupManagerCategoryService>().List().Select(item => new WorkGroupManagerCategoryModel
                {
                    ID = item.ID,
                    Name = item.Name
                }).ToArray();
            }
        }
    }
}

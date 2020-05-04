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

        public readonly static WorkGroupManagerCategoryModel[] WorkGroupManagerCategories;
        static WorkGroupManagerCategoryModel()
        {
            WorkGroupManagerCategories = LR.Tools.DIHelper.GetInstance<IWorkGroupManagerCategoryService>().All().Select(item => new WorkGroupManagerCategoryModel
            {
                ID = item.ID,
                Name = item.Name
            }).ToArray();
        }
    }

}

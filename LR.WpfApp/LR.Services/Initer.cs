using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Services
{
    public class Initer
    {
        public readonly string DataPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.data");
        public static async Task<KeyValuePair<string, string>> Init()
        {
            return await Task.Run<KeyValuePair<string, string>>(() =>
             {
                 var context = new LR.Repositories.DataContext(true);
                 var result = new KeyValuePair<string, string>("admin", new Random().Next(100000, 999999).ToString());
                 context.Admins.Insert(new Entity.Admin
                 {
                     ID = Guid.NewGuid(),
                     UserName = result.Key,
                     Name = "超级管理员",
                     CreateDate = DateTime.Now,
                     ModifyDate = DateTime.Now,
                     OperatorID = Guid.Empty,
                     State = LR.Entity.DataState.Normal,
                     Type = (int)AdminType.Super,
                     Password = Administrator.PsString(result.Value)
                 });
                 return result;
             });
        }

        public static bool IsInit
        {
            get
            {
                return LR.Repositories.DataContext.Exist;
            }
        }
    }
}

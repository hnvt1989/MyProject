using MyProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyProject.Models.ViewModels.ContentManagement
{
    public abstract class ResourceModel
    {
        protected string SelectedCulture { get; set; }
        protected Dictionary<string, string> Resources = new Dictionary<string, string>(); 
        protected string ResourceContext { get; set; }

        public virtual void InitializeResources()
        {
            //initialize resources
            using (var context = new ShoppingCartContext())
            {
                var selectedCultureId = 0;

                var selectedCultureAppSetting = context.AppSettings.SingleOrDefault(a => a.Code == "SelectedCulture").Value;
                if (selectedCultureAppSetting != null)
                {
                    var selectedCulture = context.Cultures.SingleOrDefault(c => c.Code == selectedCultureAppSetting);
                    if (selectedCulture == null)
                        throw new Exception("Missing Culture [" + selectedCultureAppSetting + "]");
                    else
                        selectedCultureId = selectedCulture.Id;
                }
                else
                {
                    //look for default culture
                    selectedCultureId = context.Cultures.Where(c => c.Default).First().Id;
                }


                var resourceKeys = context.ResourceKeys.Where(rk => rk.Context == ResourceContext).Select(a => a.Id).ToList();

                context.ResourceValues
                    .Where(r => r.CultureId == selectedCultureId && resourceKeys.Contains(r.ResourceKeyId))
                    .ToList()
                    .ForEach(
                        res =>
                        {
                            var key = res.ResourceKey.Context + "." + res.ResourceKey.Name + "." + res.ResourceKey.Property;
                            if (!Resources.ContainsKey(key))
                                Resources.Add(key, res.Value);
                        }
                    );
            }
        } 
    }
}